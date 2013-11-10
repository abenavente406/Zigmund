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
using GameHelperLibrary;
using TheLegendOfZigmundREVAMP.States;
using TheLegendOfZigmundREVAMP.Utilities;
using System.IO;

namespace TheLegendOfZigmundREVAMP
{
    public class TheLegendOfZigmund : Microsoft.Xna.Framework.Game
    {
        public const int GAMEWIDTH = 800;
        public const int GAMEHEIGHT = 480;
        public static SpriteFont DEFAULT_FONT;
        public static ContentManager CONTENT;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameStateManager stateManager;

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public TheLegendOfZigmund()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth = GAMEWIDTH;
            graphics.PreferredBackBufferHeight = GAMEHEIGHT;
            graphics.ApplyChanges();

            CONTENT = Content;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.Components.Add(new InputHandler(this));
            stateManager = new GameStateManager(this);
            stateManager.ChangeState(new MenuState(this, stateManager));
            this.Components.Add(stateManager);

            /* Generates a perlin noise height map to test
             * 
            SmoothPerlinNoise testGenerator = new SmoothPerlinNoise(500, 500);
            var noise = testGenerator.GeneratePerlinNoise(500, 500, 7);
            Stream output = File.Open("..\\test.jpeg", FileMode.Create);
            Texture2D texture = new Texture2D(GraphicsDevice, 500, 500);
            Color[] data = new Color[500 * 500];
            for (int y = 0; y < 500; y++)
            {
                for (int x = 0; x < 500; x++)
                {
                    data[(y * 500) + x] = GetColor(testGenerator.GetNoise(x, y));
                }
            }
            texture.SetData<Color>(data);
            texture.SaveAsJpeg(output, 500, 500);
            output.Close();
             */
        }

        Color GetColor(float val)
        {
            int grayScale = (int)(val * 255);
            return new Color(grayScale, grayScale, grayScale);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
