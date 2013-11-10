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

namespace TheLegendOfZigmundREVAMP.Entities
{
    public abstract class Entity
    {
        #region Fields
        protected World world;
        protected Vector2 pos;
        protected Point gridPos;
        protected bool isMoving = false;
        protected int dir = 1;
        protected Image avatarUp;
        protected Image avatarDown;
        protected Image avatarLeft;
        protected Image avatarRight;
        protected Animation moveUp;
        protected Animation moveDown;
        protected Animation moveLeft;
        protected Animation moveRight;
        #endregion

        #region Properties
        /// <summary>
        /// Direction the entity is facing
        /// </summary>
        public int Direction
        {
            get { return dir; }
        }

        /// <summary>
        /// Real position of the entity (not by grid)
        /// </summary>
        public Vector2 Position
        {
            get { return pos; }
        }

        /// <summary>
        /// Grid position of the entity
        /// </summary>
        public Point GridPosition
        {
            get { return gridPos; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Creates an Entity object
        /// </summary>
        /// <param name="gridPos">Position of the world via grid</param>
        /// <param name="world">World the entity resides in</param>
        public Entity(Point gridPos, World world)
        {
            this.gridPos = gridPos;
            this.pos = new Vector2(gridPos.X * 60, gridPos.Y * 60);
            this.world = world;
            SetTexture();
        }

        /// <summary>
        ///  Use this to set the entities images/animations
        /// </summary>
        public abstract void SetTexture();
        #endregion

        #region Update/Draw
        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch batch, GameTime gameTime)
        {
            switch (dir)
            {
                case 0:
                    if (isMoving)
                        moveUp.Draw(batch, gameTime, pos);
                    else
                        avatarUp.Draw(batch, pos);
                    break;
                case 1:
                    if (isMoving)
                        moveDown.Draw(batch, gameTime, pos);
                    else
                        avatarDown.Draw(batch, pos);
                    break;
                case 2:
                    if (isMoving)
                        moveLeft.Draw(batch, gameTime, pos);
                    else
                        avatarLeft.Draw(batch, pos);
                    break;
                case 3:
                    if (isMoving)
                        moveRight.Draw(batch, gameTime, pos);
                    else
                        avatarRight.Draw(batch, pos);
                    break;
            }
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// Moves the entity by the grid
        /// </summary>
        /// <param name="newPos">Moves the entity by the given amount of grid spaces</param>
        public void Move(Point newPos)
        {
            Move(new Vector2(newPos.X * world.TileWidth, newPos.Y * world.TileHeight));
        }

        /// <summary>
        /// Moves the entity by a variable amount
        /// </summary>
        /// <param name="newPos">Amount to move the entity</param>
        public void Move(Vector2 newPos)
        {
            if (world.CanMove(world.NearestPoint(newPos)))
            {
                pos = newPos;
                gridPos = world.NearestPoint(newPos);
            }
        }
        #endregion
    }
}
