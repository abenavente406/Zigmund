using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameHelperLibrary.Controls
{
    public class Label : Control
    {
        #region Constructor Region

        public Label()
        {
            tabStop = false;
        }

        #endregion

        #region Abstract Methods

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (Effect)
            {
                case ControlEffect.NONE:
                    {
                        spriteBatch.DrawString(SpriteFont, Text, Position, Color);
                        break;
                    }
                case ControlEffect.FLASH:
                    {
                        if (gameTime.TotalGameTime.Milliseconds % flashDuration > flashDuration / 2)
                            spriteBatch.DrawString(spriteFont, Text, Position, Color);
                        break;
                    }
                case ControlEffect.GLOW:
                    {
                        spriteBatch.DrawString(SpriteFont, text, Position, Color);
                        spriteBatch.DrawString(SpriteFont, text, position, Overlay);

                        if (!flipped)
                            overlay.A += glowSpeed;
                        else
                            overlay.A -= glowSpeed;

                        if (overlay.A > 250)
                        {
                            overlay.A = 250;
                            flipped = true;
                        }
                        else if (overlay.A < glowSpeed)
                        {
                            overlay.A = glowSpeed;
                            flipped = false;
                        }

                        break;
                    }
                case ControlEffect.PULSE:
                    {
                        pulseFadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (pulseUp)
                            selectionFade = Math.Min(selectionFade + pulseFadeSpeed, 1);
                        else
                            selectionFade = Math.Max(selectionFade - pulseFadeSpeed, 0);

                        if (pulseUp && selectionFade == 1)
                            pulseUp = false;
                        else if (!pulseUp && selectionFade == 0)
                            pulseUp = true;

                        double time = gameTime.ElapsedGameTime.TotalSeconds;
                        float pulse = (float)Math.Sin(time * 6) + 1;
                        float scale = 1 + pulse * pulseRate * selectionFade;

                        var origin = new Vector2(0, spriteFont.LineSpacing / 2);

                        spriteBatch.DrawString(spriteFont, text, position, Color, 0,
                            origin, scale, SpriteEffects.None, 0);
                        break;
                    }
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
        }

        #endregion
    }
}
