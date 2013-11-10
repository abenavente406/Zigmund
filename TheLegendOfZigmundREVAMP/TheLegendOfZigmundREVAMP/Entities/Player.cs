using System;
using GameHelperLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheLegendOfZigmundREVAMP.GameWorld;
using TheLegendOfZigmundREVAMP.GameWorld.Objects;
using TheLegendOfZigmundREVAMP.Utilities;

namespace TheLegendOfZigmundREVAMP.Entities
{
    public class Player : Entity
    {
        #region Fields
        private bool isHitting = false;
        private Animation hittingUp, hittingDown,
            hittingLeft, hittingRight;
        private float hitTicks;
        private bool canHit;
        private int steps = 0;
        #endregion

        #region Properties
        /// <summary>
        /// If the player is hitting
        /// </summary>
        public bool IsHitting
        {
            get { return isHitting; }
            set { isHitting = value; }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Creates a player object
        /// </summary>
        /// <param name="world">The world that the player resides in</param>
        public Player(World world)
            : base(new Point(0, 0), world)
        {
            bool worked = false;
            Random rand = new Random();
            do
            {
                // Generate a random point
                Point p = new Point(rand.Next(world.Width), rand.Next(world.Height));

                // The player can only be assigned a position if he is able to move 
                if (world.CanMove(p))
                {
                    gridPos = p;
                    pos = new Vector2(p.X * world.TileWidth, p.Y * world.TileHeight);
                    worked = true;
                }
                
                // Keep looping until the player gets assigned a valid point
            } while (!worked);
             
        }

        public override void SetTexture()
        {
            var contentManager = TheLegendOfZigmund.CONTENT;

            avatarUp = new Image(contentManager.Load<Texture2D>("Player\\ZigmundUp1"));
            avatarDown = new Image(contentManager.Load<Texture2D>("Player\\ZigmundDown1"));
            avatarLeft = new Image(contentManager.Load<Texture2D>("Player\\ZigmundLeft1"));
            avatarRight = new Image(contentManager.Load<Texture2D>("Player\\ZigmundRight1"));

            Texture2D[] up = new Texture2D[2], down = new Texture2D[2],
                left = new Texture2D[2], right = new Texture2D[2];
            
            for (int i = 0; i < 2; i++)
            {
                up[i] = contentManager.Load<Texture2D>("Player\\ZigmundUp" + (i+1));
                down[i] = contentManager.Load<Texture2D>("Player\\ZigmundDown" + (i+1));
                left[i] = contentManager.Load<Texture2D>("Player\\ZigmundLeft" + (i+1));
                right[i] = contentManager.Load<Texture2D>("Player\\ZigmundRight" + (i+1));
            }

            moveUp = new Animation(up);
            moveDown = new Animation(down);
            moveLeft = new Animation(left);
            moveRight = new Animation(right);

            Texture2D[] imgsUp = new Texture2D[] 
            {
                contentManager.Load<Texture2D>("Player\\plungerupbreak"),
                contentManager.Load<Texture2D>("Player\\plungerupstraight"),
            };
            Texture2D[] imgsDown = new Texture2D[] 
            {
                contentManager.Load<Texture2D>("Player\\plungerdownbreak"),
                contentManager.Load<Texture2D>("Player\\plungerdownstraight"),
            };
            Texture2D[] imgsLeft = new Texture2D[] 
            {
                contentManager.Load<Texture2D>("Player\\plungerleftbreak"),
                contentManager.Load<Texture2D>("Player\\plungerleftstraight"),
            };
            Texture2D[] imgsRight = new Texture2D[] 
            {
                contentManager.Load<Texture2D>("Player\\plungerrightbreak"),
                contentManager.Load<Texture2D>("Player\\plungerrightstraight"),
            };


            hittingUp = new Animation(imgsUp, 2000f);
            hittingDown = new Animation(imgsDown, 2000f);
            hittingLeft = new Animation(imgsLeft, 2000f);
            hittingRight = new Animation(imgsRight, 2000f);
        }
        #endregion

        #region Update/Draw
        public override void Update(GameTime gameTime)
        {
            if (hitTicks > 0)
                hitTicks--;
            else
                canHit = true;
            
            HandleInput();
  
        }

        /// <summary>
        /// Handles the input from the player
        /// </summary>
        private void HandleInput()
        {
            // --- Key Map ---------------------------------
            // ---------------------------------------------
            //  [LEFT]  - Moves the player left
            //  [RIGHT] - Moves the player right
            //  [UP]    - Moves the player up
            //  [DOWN]  - Moves the player down
            //  [SPACE] - Breaks a rock if one exists there
            // ---------------------------------------------
            // ---------------------------------------------

            int dirX = 0, dirY = 0;

            if (InputHandler.KeyPressed(Keys.Up)) dirY--;
            if (InputHandler.KeyPressed(Keys.Down)) dirY++;
            if (InputHandler.KeyPressed(Keys.Left)) dirX--;
            if (InputHandler.KeyPressed(Keys.Right)) dirX++;

            dir = (dirY < 0 ? 0 : dirY > 0 ? 1 : dirX < 0 ? 2 : dirX > 0 ? 3 : dir);

            // Handles the player hitting a rock
            if (InputHandler.KeyPressed(Keys.Space) && canHit)
            {
                var rockAtDir = GetRockAtDir();
                if (rockAtDir != null)
                    rockAtDir.Damage(100 / 5);
            }

            // Attempt to set the player's position if a key was pressed
            if (dirX != 0 || dirY != 0)
            {
                ++steps;
                steps %= 1000;
                var newPos = new Point(GridPosition.X + dirX, GridPosition.Y + dirY);
                Move(newPos);
            }

            // Sets the camera's position
            Camera.SetPos(new Vector2((Position.X - (TheLegendOfZigmund.GAMEWIDTH / Camera.Zoom) / 2),
                (Position.Y - (TheLegendOfZigmund.GAMEHEIGHT / Camera.Zoom) / 2)));
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!isHitting)
            {
                if (dir == 0) batch.Draw(moveUp.Images[steps % 2], pos, Color.White);
                else if (dir == 1) batch.Draw(moveDown.Images[steps % 2], pos, Color.White);
                else if (dir == 2) batch.Draw(moveLeft.Images[steps % 2], pos, Color.White);
                else if (dir == 3) batch.Draw(moveRight.Images[steps % 2], pos, Color.White);
            }
            else
            {
                if (dir == 0) hittingUp.Draw(batch, gameTime, pos);
                if (dir == 1) hittingDown.Draw(batch, gameTime, pos);
                if (dir == 2) hittingLeft.Draw(batch, gameTime, pos);
                if (dir == 3) hittingRight.Draw(batch, gameTime, pos);
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Gets a rock at the direction that the player is facing
        /// </summary>
        /// <returns>A rock in the direction the player is facing</returns>
        private Rock GetRockAtDir()
        {
            Point N = new Point(GridPosition.X, GridPosition.Y - 1);
            Point S = new Point(GridPosition.X, GridPosition.Y + 1);
            Point E = new Point(GridPosition.X - 1, GridPosition.Y);
            Point W = new Point(GridPosition.X + 1, GridPosition.Y);

            return world.GetRock(dir == 0 ? N : dir == 1 ? S : dir == 2 ? E : W);
        }
        #endregion
    }
}
