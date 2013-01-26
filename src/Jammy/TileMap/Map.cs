using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.TileMap
{
	public class Tile
	{
		public Texture2D Texture;
		public Rectangle SourceRectangle;
		public SpriteEffects SpriteEffects;
	}

	public class Layer
	{
		public int Width;
		public int Height;
		public Tile[] Tiles;
	}

	public class ObjectLayer
	{
		public int Width;
		public int Height;
		public List<Polygon> Polygons;
	}

	public class Map
	{
		public int TileWidth;
		public int TileHeight;

		public List<Layer> Layers = new List<Layer> ();
		public List<ObjectLayer> ObjectLayers = new List<ObjectLayer>();

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var l in Layers)
			{
				for (int y = 0; y < l.Height; y++)
				{
					for (int x = 0; x < l.Width; x++)
					{
						Tile t = l.Tiles[y * l.Width + x];
						spriteBatch.Draw (
							t.Texture,
							new Rectangle (x * TileWidth, y * TileHeight, TileWidth, TileHeight),
							t.SourceRectangle,
							Color.White,
							0,
							Vector2.Zero,
							t.SpriteEffects,
							0);
					}
				}
			}
		}
	}
}
