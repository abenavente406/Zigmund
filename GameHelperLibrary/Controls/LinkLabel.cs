using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameHelperLibrary.Controls
{
    public class LinkLabel : Control
    {
        #region Fields and Properties

        public event EventHandler OnMouseIn;

        Color selectedColor = Color.CadetBlue;
        int index = 0;

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public int Index
        {
            get { return index; }
        }

        #endregion

        #region Constructor Region

        public LinkLabel(int index)
        {
            TabStop = true;
            HasFocus = false;
            Position = Vector2.Zero;
            this.index = index;
        }

        #endregion

        #region Abstract Methods

        public override void Update(GameTime gameTime)
        {
            if (Bounds.Contains(InputHandler.MousePos))
            {
                if (OnMouseIn != null)
                    OnMouseIn(this, null);

                if (InputHandler.MouseButtonPressed(MouseButton.LeftButton))
                    OnSelected(null);
            }

            if (hasFocus)
                if (InputHandler.ButtonPressed(Buttons.A, PlayerIndex.One) ||
                    InputHandler.ButtonPressed(Buttons.Start, PlayerIndex.One))
                    OnSelected(null);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gametime)
        {
            if (hasFocus)
            {
                switch (Effect)
                {
                    case ControlEffect.NONE:
                        {
                            spriteBatch.DrawString(SpriteFont, Text, Position, selectedColor);
                            break;
                        }
                    case ControlEffect.FLASH:
                        {
                            if (gametime.TotalGameTime.Milliseconds % flashDuration > flashDuration / 2)
                                spriteBatch.DrawString(SpriteFont, text, Position, selectedColor);
                            break;
                        }
                    case ControlEffect.GLOW:
                        {
                            if (!flipped)
                            {
                                spriteBatch.DrawString(SpriteFont, text, Position, selectedColor);
                                overlay.A += glowSpeed;
                                spriteBatch.DrawString(SpriteFont, text, position, Overlay);

                            }
                            else
                            {
                                spriteBatch.DrawString(SpriteFont, text, Position, selectedColor);
                                overlay.A -= glowSpeed;
                                spriteBatch.DrawString(SpriteFont, text, position, Overlay);

                            }

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
                            pulseFadeSpeed = (float)gametime.ElapsedGameTime.TotalSeconds;

                            if (pulseUp)
                                selectionFade = (float)Math.Min(selectionFade + pulseFadeSpeed, 1);
                            else
                                selectionFade = (float)Math.Max(selectionFade - pulseFadeSpeed, 0);

                            if (pulseUp && selectionFade == 1)
                                pulseUp = false;
                            else if (!pulseUp && selectionFade == 0)
                                pulseUp = true;

                            double time = gametime.ElapsedGameTime.TotalSeconds;
                            float pulse = (float)Math.Sin(time * 6) + 1;
                            float scale = 1 + pulse * pulseRate * selectionFade;

                            var origin = new Vector2(0, 0);
                            spriteBatch.DrawString(spriteFont, text, Position, SelectedColor, 0, 
                                origin, scale, SpriteEffects.None, 0);

                            break;
                        }
                }
            }
            else
            {
                spriteBatch.DrawString(SpriteFont, Text, Position, Color);
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (HasFocus &&
                bounds.Contains(InputHandler.MousePos) &&
                InputHandler.MouseButtonPressed(MouseButton.LeftButton))
                base.OnSelected(null);

            if (InputHandler.KeyReleased(Keys.Enter) ||
                InputHandler.ButtonReleased(Buttons.A, playerIndex))
                base.OnSelected(null);
        }

        #endregion
    }
}
