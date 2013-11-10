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

namespace TheLegendOfZigmundREVAMP.GameWorld
{
    public enum TileType
    {
        GRASS, WATER, SAND, ICE, LAVA
    }

    public class Tile
    {
        #region Fields
        private TileType type = TileType.GRASS;
        private bool canWalk;
        private bool causesDamage;
        private Texture2D tilePic;
        private Point gridPos;
        private Animation tileAnim;
        #endregion

        #region Properties
        /// <summary>
        /// Type of tile
        /// </summary>
        public TileType Type { get { return type; } }

        /// <summary>
        /// If the player can walk on the tile
        /// </summary>
        public bool CanWalk
        {
            get { return canWalk; }
            set { canWalk = value; }
        }

        /// <summary>
        /// If the tile causes damage to the player
        /// </summary>
        public bool CausesDamage { get { return causesDamage; } }

        /// <summary>
        /// Image of the tile
        /// </summary>
        public Texture2D TilePic { get { return tilePic; } }

        /// <summary>
        /// Animation of the tile
        /// </summary>
        public Animation TileAnim { get { return tileAnim; } }

        /// <summary>
        /// Position of the tile on the map
        /// </summary>
        public Point GridPos { get { return gridPos; } }
        #endregion

        #region Initialization
        /// <summary>
        /// Create a tile object based off a texture
        /// </summary>
        /// <param name="gridPos">Position of the tile</param>
        /// <param name="type">Type of tile</param>
        /// <param name="canWalk">If entities can walk on the tile</param>
        /// <param name="causesDamage">If the tile hurts entities</param>
        /// <param name="tilePic">The image to use to draw the tile</param>
        public Tile(Point gridPos, TileType type, bool canWalk, bool causesDamage, Texture2D tilePic)
        {
            this.gridPos = gridPos;
            this.type = type;
            this.canWalk = canWalk;
            this.causesDamage = causesDamage;
            this.tilePic = tilePic;
        }

        /// <summary>
        /// Creates a tile object from an animation
        /// </summary>
        /// <param name="gridPos">Position of the tile</param>
        /// <param name="type">Type of tile</param>
        /// <param name="canWalk">If entities can walk on the tile</param>
        /// <param name="causesDamage">If the tile hurts entities</param>
        /// <param name="tileAnim">The animation to use to draw the tile</param>
        public Tile(Point gridPos, TileType type, bool canWalk, bool causesDamage, Animation tileAnim)
        {
            this.gridPos = gridPos;
            this.type = type;
            this.canWalk = canWalk;
            this.causesDamage = causesDamage;
            this.tileAnim = tileAnim;
        }
        #endregion
    }
}
