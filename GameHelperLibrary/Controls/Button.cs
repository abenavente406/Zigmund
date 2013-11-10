using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameHelperLibrary;
using GameHelperLibrary.Shapes;
using GameHelperLibrary.Controls;

namespace GameHelperLibrary.Controls
{
    public class Button : Control
    {
        public event EventHandler OnClick;
        public event EventHandler OnMouseEnter;
        public event EventHandler OnMouseLeave;

        private SpriteFont font;

        private Texture2D highlightOverlay;
        private Texture2D border;
        private Texture2D background;

        private float overlayOpacity = 0.0f;

        private int marginLeft = 4;
        private int marginRight = 4;
        private int marginTop = 4;
        private int marginBottom = 4;

        public Texture2D BackgroundImage
        {
            get { return background; }
            set { background = value; }
        }

        public int MarginLeft
        {
            get { return marginLeft; }
            set { marginLeft = value; }
        }

        public int MarginRight
        {
            get { return marginRight; }
            set { marginRight = value; }
        }

        public int MarginTop
        {
            get { return marginTop; }
            set { marginTop = value; }
        }

        public int MarginBottom
        {
            get { return marginBottom; }
            set { marginBottom = value; }
        }

        public Button() : this(ControlManager.SpriteFont) { }

        public Button(SpriteFont font)
        {
            this.font = font;

            highlightOverlay = new DrawableRectangle(ControlManager.Parent.GraphicsDevice,
                new Vector2(width, height), Color.White, true).Texture;
            border = new DrawableRectangle(ControlManager.Parent.GraphicsDevice,
                new Vector2(width, height), Color.Black, false).Texture;

            OnMouseEnter += HighlightButton;
            OnMouseLeave += UnHighlighButton;
            OnClick += (delegate(object sender, EventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("OnClick does not have a function associated with it.");
            });
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            return;
        }

        public override void Update(GameTime gameTime)
        {
            if (Bounds.Contains(InputHandler.MousePos))
            {
                OnMouseEnter(this, null);
                if (InputHandler.MouseButtonPressed(MouseButton.LeftButton))
                    OnClick(this, null);
            }
            else
            {
                OnMouseLeave(this, null);
            }
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            switch (Effect)
            {
                case ControlEffect.NONE:
                    DrawButton(batch);
                    break;
                case ControlEffect.FLASH:
                    if (gameTime.TotalGameTime.Milliseconds % flashDuration > flashDuration / 2)
                    {
                        DrawButton(batch);
                    }
                    break;
            }
        }

        private void DrawButton(SpriteBatch batch)
        {
            SpriteEffects flip = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if (background != null)
                batch.Draw(background, Bounds, null, Color.White, 0f, Vector2.Zero, flip, 1f);
            batch.Draw(highlightOverlay, Bounds, Color.White * overlayOpacity);

            batch.DrawString(font, text, new Vector2(Position.X + marginLeft, Position.Y + marginTop), Color.Black);
        }

        private void HighlightButton(object sender, EventArgs e)
        {
            overlayOpacity = 0.3f;
        }

        private void UnHighlighButton(object sender, EventArgs e)
        {
            overlayOpacity = 0.0f;
        }
    }
}