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
using GameHelperLibrary.Controls;

namespace TheLegendOfZigmundREVAMP.States
{
    public class MenuState : BaseGameState
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Initialization
        public MenuState(Game game, GameStateManager manager)
            : base(game, manager) { }

        protected override void LoadContent()
        {
            Label instructions = new Label()
            {
                Text = "Press [ENTER] to play.\n\n" +
                       "Press [UP|DOWN|LEFT|RIGHT] to move.\n" +
                       "Press [+|-] to adjust the camera zoom.\n" +
                       "Press [SPACE] to destroy rocks."
            };
            instructions.Position = new Vector2(TheLegendOfZigmund.GAMEWIDTH / 2 - instructions.Width / 2,
                TheLegendOfZigmund.GAMEHEIGHT / 2 - instructions.Height / 2);
            controls.Add(instructions);

            //base.LoadContent();
        }
        #endregion

        #region Update/Draw
        public override void Update(GameTime gameTime)
        {
            if (InputHandler.KeyPressed(Keys.Enter)) StateManager.ChangeState(new PlayingState(mainGame, StateManager));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var batch = mainGame.SpriteBatch;
            batch.Begin();

            batch.End();
            base.Draw(gameTime);
        }
        #endregion


    }
}
