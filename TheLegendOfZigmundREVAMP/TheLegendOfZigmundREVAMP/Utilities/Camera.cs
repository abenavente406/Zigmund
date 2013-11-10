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

namespace TheLegendOfZigmundREVAMP.Utilities
{
    public class Camera
    {
        #region Fields
        private static Vector2 pos;
        private static float zoom;
        private static Vector2 CONST_MAX_CLAMP;
        private static Vector2 maxClamp;
        private static Rectangle CONST_BOUNDS;
        private static Rectangle bounds;
        #endregion

        #region Properties

        /// <summary>
        /// The position of the camera
        /// </summary>
        public static Vector2 Pos
        {
            get { return pos; }
            set
            {
                pos = Vector2.Clamp(value, Vector2.Zero, maxClamp);

                // Update the bounds too
                bounds.X = (int)pos.X;
                bounds.Y = (int)pos.Y;
            }
        }

        /// <summary>
        /// The zoom of the camera
        /// </summary>
        public static float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = MathHelper.Clamp(value, 0.05f, 20f);
                // Update bounds
                bounds.Width = (int)(CONST_BOUNDS.Width / zoom);
                bounds.Height = (int)(CONST_BOUNDS.Height / zoom);
                // Update max clamp
                Camera.maxClamp = new Vector2(CONST_MAX_CLAMP.X, CONST_MAX_CLAMP.Y)
                 - new Vector2((TheLegendOfZigmund.GAMEWIDTH / Camera.zoom),
                 (TheLegendOfZigmund.GAMEHEIGHT / Camera.zoom));
            }
        }

        /// <summary>
        /// The furthest the camera can go
        /// </summary>
        public static Vector2 MaxClamp
        {
            get { return maxClamp; }
            set { maxClamp = value; }
        }

        /// <summary>
        /// The viewing area of the camera
        /// </summary>
        public static Rectangle ViewArea
        {
            get { return bounds; }
        }
        
        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the Camera given a maxClamp (<em>generally the size of the screen</em>)
        /// </summary>
        /// <param name="maxClamp"></param>
        public static void Initialize(Vector2 maxClamp)
        {
            Camera.zoom = 0.5f;
            Camera.pos = Vector2.Zero;
            Camera.CONST_BOUNDS = new Rectangle(0, 0, TheLegendOfZigmund.GAMEWIDTH, TheLegendOfZigmund.GAMEHEIGHT);
            Camera.bounds = new Rectangle(0, 0, TheLegendOfZigmund.GAMEWIDTH, TheLegendOfZigmund.GAMEHEIGHT);
            Camera.CONST_MAX_CLAMP = maxClamp;
            Camera.maxClamp = new Vector2(maxClamp.X, maxClamp.Y) 
                - new Vector2((TheLegendOfZigmund.GAMEWIDTH / Camera.zoom), 
                (TheLegendOfZigmund.GAMEHEIGHT / Camera.zoom));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the cameras transformation matrix
        /// </summary>
        /// <returns>The transformation matrix for the camera</returns>
        public static Matrix GetTransform()
        {
            return Matrix.CreateTranslation(-pos.X, -pos.Y, 0) *
                Matrix.CreateScale(zoom);
        }

        /// <summary>
        /// Moves the camera by a specified amount
        /// </summary>
        /// <param name="amount">Amount to move the camera</param>
        public static void Move(Vector2 amount)
        {
            Pos = Pos + amount;
        }

        /// <summary>
        /// Changes the position of the camera
        /// </summary>
        /// <param name="pos">The new position</param>
        public static void SetPos(Vector2 pos)
        {
            Pos = pos;
        }

        /// <summary>
        /// The a rectangle is in the viewport of the camera
        /// </summary>
        /// <param name="bounds">The bounding box of the object to test</param>
        /// <returns>If the object is in view</returns>
        public static bool IsInView(Rectangle bounds)
        {
            return Camera.bounds.Contains(bounds);
        }

        #endregion
    }
}
