﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameHelperLibrary.Controls
{
    public enum ControlEffect
    {
        NONE,
        FLASH,
        GLOW,
        PULSE
    }

    public abstract class Control
    {
        #region Field Region
        protected ControlEffect effect = ControlEffect.NONE;
        protected string name;
        protected string text = "";
        protected int width;
        protected int height;
        protected Vector2 position;
        protected object value;
        protected bool hasFocus;
        protected bool enabled;
        protected bool visible;
        protected bool tabStop;
        protected SpriteFont spriteFont;
        protected Color color;
        protected string type;
        protected int flashDuration = 350;
        protected byte glowSpeed = 4;
        protected float scale = 1.0f;
        protected bool flipped = false;
        protected Color overlay = Color.Black;
        protected Rectangle bounds;
        protected bool autoSize = true;
        protected float pulseRate = .15f;
        protected float pulseFadeSpeed = 4f;
        protected bool pulseUp = true;
        protected float selectionFade;
        #endregion

        #region Event Region
        public event EventHandler Selected;
        #endregion

        #region Property Region

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public virtual String Text
        {
            get { return text; }
            set
            {
                text = value;

                if (autoSize)
                {
                    width = (int)spriteFont.MeasureString(text).X;
                    height = (int)spriteFont.MeasureString(text).Y;
                }
            }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                position.Y = (int)position.Y;
            }
        }

        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public virtual bool HasFocus
        {
            get { return hasFocus; }
            set { hasFocus = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool TabStop
        {
            get { return tabStop; }
            set { tabStop = value; }
        }

        public SpriteFont SpriteFont
        {
            get { return spriteFont; }
            set { spriteFont = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public ControlEffect Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        public int FlashDuration
        {
            get { return flashDuration; }
            set { flashDuration = value; }
        }

        public byte GlowSpeed
        {
            get { return glowSpeed; }
            set { glowSpeed = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = MathHelper.Clamp(value, 0.1f, 50.0f); }
        }

        public Color Overlay
        {
            get { return overlay; }
            set { overlay = value; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        public bool AutoSize
        {
            get { return autoSize; }
            set { autoSize = value; }
        }

        public bool Flipped
        {
            get { return flipped; }
            set { flipped = value; }
        }
        #endregion

        #region Constructor Region

        public Control()
        {
            Color = Color.White;
            Enabled = true;
            Visible = true;
            SpriteFont = ControlManager.SpriteFont;

            Width = 1;
            Height = 1;
        }

        #endregion

        #region Abstract Methods

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void HandleInput(PlayerIndex playerIndex);

        #endregion

        #region Virtual Methods

        protected virtual void OnSelected(EventArgs e)
        {
            if (Selected != null)
            {
                Selected(this, e);
            }
        }

        #endregion
    }
}
