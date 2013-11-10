using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TheLegendOfZigmundREVAMP.Entities;
using TheLegendOfZigmundREVAMP.GameWorld.Objects;
using TheLegendOfZigmundREVAMP.Utilities;
using System.Diagnostics;
using GameHelperLibrary;
using GameHelperLibrary.Controls;

namespace TheLegendOfZigmundREVAMP.GameWorld
{
    public class World
    {
        #region Fields
        private Texture2D grassTile, sandTile,
            iceTile, lavaTile, waterTile;
        private Random random;
        private int[,] world;
        private List<Tile> tiles;
        private Rock[,] rockArr;
        private Player player;
        private int width;
        private int height;
        private int realWidth;
        private int realHeight;
        private int tileWidth = 60;
        private int tileHeight = 60;
        private SmoothPerlinNoise noiseGenerator;
        #endregion

        #region Properties
        /// <summary>
        /// Width of the map in tiles
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// Height of the map in tiles
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Width of the map in pixels
        /// </summary>
        public int RealWidth
        {
            get { return realWidth; }
        }

        /// <summary>
        /// Height of the map in pixels
        /// </summary>
        public int RealHeight
        {
            get { return realHeight; }
        }

        /// <summary>
        /// Width of tiles on the map
        /// </summary>
        public int TileWidth
        {
            get { return tileWidth; }
        }

        /// <summary>
        /// Height of tiles on the map
        /// </summary>
        public int TileHeight
        {
            get { return tileHeight; }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Creates a world object
        /// </summary>
        /// <param name="width">Width of the map in tiles</param>
        /// <param name="height">Height of the map in tiles</param>
        public World(int width, int height)
        {
            this.random = new Random();
            this.width = width;
            this.height = height;
            this.world = new int[height, width];
            this.tiles = new List<Tile>();
            this.realWidth = width * TileWidth;
            this.realHeight = height * TileHeight;
            noiseGenerator = new SmoothPerlinNoise(width, height);
            Load();
        }

        /// <summary>
        /// Loads the world
        /// </summary>
        public void Load()
        {
            InitializeWorld();
           
            PlaceRocks();
            PlacePlayer();
        }

        /// <summary>
        /// Initializes the world array
        /// </summary>
        private void InitializeWorld()
        {
            var content = TheLegendOfZigmund.CONTENT;

            // Initialize the tile images
            grassTile = content.Load<Texture2D>("Tiles\\GrassTile");
            sandTile = content.Load<Texture2D>("Tiles\\SandTile");
            iceTile = content.Load<Texture2D>("Tiles\\icetile");
            lavaTile = content.Load<Texture2D>("Tiles\\lavaTile");
            waterTile = content.Load<Texture2D>("Tiles\\water\\IMG00001");

            // Initialize the water animation
            Texture2D[] waterImages = new Texture2D[4];
            for (int i = 0; i < waterImages.Length; i++)
                waterImages[i] = content.Load<Texture2D>("Tiles\\water\\IMG000" + (i >= 10 ? "" + i : "0" + i * 2));
            var waterAnim = new Animation(waterImages, 7000f);

            //Deletes tiles content
            tiles.Clear();

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    float value = noiseGenerator.GetNoise(x, y);
                    if (value < .51) world[y, x] = 0;
                    else if (value < .6) world[y, x] = 2;
                    else world[y, x] = 1;
                }
            }

            SmoothPerlinNoise lavaGenerator = new SmoothPerlinNoise(width, height);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    float value = lavaGenerator.GetNoise(x, y);
                    if (value < .28) if (world[y, x] == 0) world[y, x] = 4; 
                }
            }

            //Loads the tiles for the map
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Point pos = new Point(x, y);
                    int tileNum = world[y, x];
                    if (tileNum == 0) tiles.Add(new Tile(pos, TileType.GRASS, true, false, grassTile));
                    else if (tileNum == 1) tiles.Add(new Tile(pos, TileType.WATER, false, false, waterAnim));
                    else if (tileNum == 2) tiles.Add(new Tile(pos, TileType.SAND, true, false, sandTile));
                    else if (tileNum == 3) tiles.Add(new Tile(pos, TileType.ICE, true, false, iceTile));
                    else if (tileNum == 4) tiles.Add(new Tile(pos, TileType.LAVA, false, true, lavaTile));
                }
            }

        }

        /// <summary>
        /// Places the player in the world
        /// </summary>
        private void PlacePlayer()
        {
            const int MAX_ITERATIONS = 3000;

            // Create a new player object
            player = new Player(this);

            // Number of times the loop has iterated
            int loopIterations = 0;

            // If the point is a valid point in the world
            bool validPoint = false;

            do
            {
                Point point = new Point(random.Next(0, width), random.Next(0, height));
                if (GetTile(point).CanWalk)
                {
                    validPoint = true;
                    player.Move(point);
                }

                // If the loop loops more times than allotted, stop the loop
                if (++loopIterations > MAX_ITERATIONS)
                {
                    // Insert a square of land and place the player on it
                    PlaceLandBlob(point);
                    validPoint = true;
                    player.Move(point);
                    
                    // Remove the possible rock
                    rockArr[point.Y, point.X] = null;
                }
            } while (!validPoint);

            Camera.Move(player.Position);
        }

        /// <summary>
        /// Places the rocks in the world
        /// </summary>
        private void PlaceRocks()
        {
            rockArr = new Rock[height, width];

            // Initialize the borders of the array
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    rockArr[y, x] = ((x + 1) % width == 0 
                        || (y + 1) % height == 0 
                        || x == 0 
                        || y == 0) && CanMove(new Point(x, y))
                        ? new Rock(new Point(x, y), this) 
                        : null;
                }
            }

            SmoothPerlinNoise rockGenerator = new SmoothPerlinNoise(width, height, 7, .5f);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    float value = rockGenerator.GetNoise(x, y);
                    if (value < .35) if (world[y, x] == 0) rockArr[y, x] = new Rock(new Point(x, y), this);
                }
            }

        }
        #endregion

        #region Update/Draw
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            // Camera's start position translated to a point
            var cameraStart = NearestPoint(Camera.Pos);
            // Camera's end position translated to a point
            var cameraEnd = new Point((int)(cameraStart.X + ((TheLegendOfZigmund.GAMEWIDTH / Camera.Zoom) / TileWidth) + 3),
                (int)(cameraStart.Y + ((TheLegendOfZigmund.GAMEHEIGHT / Camera.Zoom) / TileHeight) + 3));

            // Draws the game world starting at the camera's view
            for (int y = cameraStart.Y; y < cameraEnd.Y; y++)
            {
                // If the camera's end point is not in the map, don't draw it
                if (y >= height) continue;
                for (int x = cameraStart.X; x < cameraEnd.X; x++)
                {
                    // If the camera's end point is not in the map, don't draw it
                    if (x >= width) continue;

                    Tile tile = GetTile(x, y);

                    if (tile.TilePic != null)
                        batch.Draw(tile.TilePic, new Vector2(x * tileWidth, y * tileHeight), Color.White);
                    else
                    {
                        if (Camera.Zoom >= 0.25)
                            tile.TileAnim.Draw(batch, gameTime, tile.GridPos.X * TileWidth, tile.GridPos.Y * TileHeight);
                        else
                            batch.Draw(waterTile, new Vector2(x * tileWidth, y * tileHeight), Color.White);
                    }

                    if (rockArr[y, x] != null)
                    {
                        if (rockArr[y, x].IsDestroyed)
                        {
                            rockArr[y, x] = null;
                            continue;
                        }
                        rockArr[y, x].Draw(batch, gameTime);
                    }
                }
            }

            player.Draw(batch, gameTime);

            batch.End();
            batch.Begin();
            batch.DrawString(ControlManager.SpriteFont, "[" + player.GridPosition.X + ", " + player.GridPosition.Y + "]", new Vector2(10, 10), Color.White);
            batch.End();
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.Default, RasterizerState.CullNone, null, Camera.GetTransform());
        }
        #endregion

        #region Helper Methods
        
        /// <summary>
        /// If a point in the grid is in the map
        /// </summary>
        /// <param name="p">The point to test</param>
        /// <returns>True or false if a point is in a map</returns>
        public bool IsInBounds(Point p)
        {
            return p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height;
        }


        /// <summary>
        /// Gets the tile at a specific position
        /// </summary>
        public Tile GetTile(Point position)
        {
            return tiles[position.Y * width + position.X];
        }

        /// <summary>
        /// Gets the tile at a specific position
        /// </summary>
        public Tile GetTile(int x, int y)
        {
            return GetTile(new Point(x, y));
        }

        /// <summary>
        /// Checks wether the position is within bounds AND canWalk boolean of the tile is set to true
        /// </summary>
        public bool CanMove(Point position)
        {
            return IsInBounds(position) && GetTile(position).CanWalk && rockArr[position.Y, position.X] == null;
        }

        /// <summary>
        /// Converts a grid point to a real point
        /// </summary>
        /// <param name="p">Point to convert</param>
        /// <returns>The point converted to a Vector</returns>
        public Vector2 PointToTile(Point p)
        {
            return new Vector2(p.X * 60, p.Y * 60);
        }

        /// <summary>
        /// Gets the nearest point to a Vector position
        /// </summary>
        /// <param name="p">The vector position</param>
        /// <returns>The nearest point</returns>
        public Point NearestPoint(Vector2 p)
        {
            var x = (int)p.X / 60;
            var y = (int)p.Y / 60;

            if (x > width - 1) x = width - 1;
            if (y > height - 1) y = height - 1;
            if (x < 0) x = 0;
            if (y < 0) y = 0;

            return new Point(x, y);
        }

        /// <summary>
        /// Gets a rock at a specified point
        /// </summary>
        /// <param name="point">Point to get the rock at</param>
        /// <returns>The rock at the point or null</returns>
        public Rock GetRock(Point point)
        {
            if (point.X < 0 || point.X > width - 1 || point.Y < 0 || point.Y > height - 1)
                return null;

            return rockArr[point.Y, point.X];
        }

        /// <summary>
        /// Only use in EMERGENCIES 
        ///   i.e. When we can't place the player
        /// </summary>
        /// <param name="p">The point of the player</param>
        private void PlaceLandBlob(Point p)
        {
            int widthOfBlob = width / 4;
            int heightOfBlob = height / 4;
            for (int x = p.X; x < (p.X + widthOfBlob > width ? width : p.X + widthOfBlob); x++)
            {
                for (int y = p.Y; y < (p.Y + heightOfBlob > height ? height : p.Y + heightOfBlob); y++)
                {
                    tiles[y * width + x] = new Tile(new Point(x, y), TileType.GRASS, true, false, grassTile);
                }
            }
        }
        #endregion
    }
}
