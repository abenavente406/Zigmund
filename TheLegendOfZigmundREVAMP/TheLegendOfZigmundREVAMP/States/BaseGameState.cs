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
    public partial class BaseGameState : GameState
    {
        #region Fields
        protected TheLegendOfZigmund mainGame;
        protected ControlManager controls;
        protected PlayerIndex playerIndexInControl;
        #endregion

        #region Properties
        public PlayerIndex PlayerIndexInControl
        {
            get { return playerIndexInControl; }
        }
        #endregion

        #region Initialization
        public BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            mainGame = (TheLegendOfZigmund)game;
            playerIndexInControl = PlayerIndex.One;

            SpriteFont font = game.Content.Load<SpriteFont>("font");
            controls = new ControlManager(game, font);

            LoadContent();
        }

        public override void LargeLoadContent(object sender)
        {
            FinishedLoading = true;
        }
        #endregion

        #region Update/Draw
        public override void Update(GameTime gameTime)
        {
            controls.Update(gameTime, playerIndexInControl);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var batch = mainGame.SpriteBatch;
            batch.Begin();
            controls.Draw(batch, gameTime);
            batch.End();

            base.Draw(gameTime);
        }

        protected void SwitchStateWithFade(GameState targetState)
        {
            this.IsExiting = true;
            StateManager.TargetState = targetState;
        }
        #endregion

    }
}
