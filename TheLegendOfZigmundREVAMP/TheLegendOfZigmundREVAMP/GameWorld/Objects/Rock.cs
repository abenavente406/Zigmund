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
using TheLegendOfZigmundREVAMP.Entities;

namespace TheLegendOfZigmundREVAMP.GameWorld.Objects
{
    public class Rock : Entity
    {
        #region Fields
        private int health = 100;
        private Texture2D[] images;
        private int index = 0;
        #endregion

        #region Properties
        /// <summary>
        /// If the rock is destroyed
        /// </summary>
        public bool IsDestroyed { get { return health <= 0; } }
        #endregion

        #region Initialization
        /// <summary>
        /// Creates a rock object
        /// </summary>
        /// <param name="pos">Position of the rock</param>
        /// <param name="world">World the rock resides in</param>
        public Rock(Point pos, World world)
            : base(pos, world)
        {
        }

        /// <summary>
        /// Sets the rock texture
        /// </summary>
        public override void SetTexture()
        {
            var content = TheLegendOfZigmund.CONTENT;
            images = new Texture2D[5];
            images[0] = content.Load<Texture2D>("RockTile");
            for (int i = 1; i < 5; i++) images[i] = content.Load<Texture2D>("RockTile" + i.ToString());
        }
        #endregion

        #region Update/Draw
        public override void Update(GameTime gameTime)
        {
            // Do nothing to update the rock
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Only draw if the rock is not destroyed
            if (IsDestroyed) 
                return;

            spriteBatch.Draw(images[index], Position, Color.White);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Damages the rock
        /// </summary>
        /// <param name="amt"></param>
        public void Damage(int amt)
        {
            health -= amt;
            index++;
        }
        #endregion
    }
}
