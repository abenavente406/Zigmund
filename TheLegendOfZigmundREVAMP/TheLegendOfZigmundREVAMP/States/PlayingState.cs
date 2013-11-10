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
using TheLegendOfZigmundREVAMP.GameWorld;
using System.Diagnostics;
using TheLegendOfZigmundREVAMP.Utilities;

namespace TheLegendOfZigmundREVAMP.States
{
    public class PlayingState : BaseGameState
    {
        #region Fields
        private World world;
        #endregion

        #region Initialization
        public PlayingState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            Random rand = new Random();
            world = new World(350, 350);
            Camera.Initialize(new Vector2(350 * 60, 350 * 60));
        }

        protected override void LoadContent()
        {
        }

        /// <summary>
        /// Used to load a lot of content. <em>Called from another state.</em>
        /// </summary>
        /// <param name="sender"></param>
        public override void LargeLoadContent(object sender)
        {
            base.LargeLoadContent(sender);
        }
        #endregion

        #region Update/Draw
        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);

            if (InputHandler.KeyDown(Keys.OemPlus) || InputHandler.KeyDown(Keys.Add)) Camera.Zoom = Camera.Zoom + .01f;
            if (InputHandler.KeyDown(Keys.OemMinus) || InputHandler.KeyDown(Keys.Subtract)) Camera.Zoom = Camera.Zoom - .01f;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var batch = mainGame.SpriteBatch;

            // Draw using the camera's transform matrix
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.Default, RasterizerState.CullNone, null, Camera.GetTransform());
            world.Draw(batch, gameTime);
            batch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
