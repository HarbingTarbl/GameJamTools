using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.Parallax
{
	public class ParallaxLayer
		: IComparable<ParallaxLayer>
	{
		public void Draw(SpriteBatch batch, Matrix matrix)
		{
			foreach (var sprite in Sprites)
			{
				var destination = sprite.DrawRectangle;

				batch.Draw(sprite.Texture, sprite.DrawRectangle, sprite.SourceRectangle, sprite.Color);
			}
		}

		public ParallaxLayer()
		{
			
		}


		

		public List<ParallaxSprite> Sprites = new List<ParallaxSprite>();
		public bool IsScrolling;
		public float Speed; //Units per ms
		public Rectangle Bounds;
		public int Order; //0 - Lowest, Rendered in descending order

		public int CompareTo(ParallaxLayer other)
		{
			return Order.CompareTo(other.Order);
		}
	}

	public class ParallaxSprite
	{
		public Texture2D Texture;
		public Rectangle DrawRectangle;
		public Rectangle? SourceRectangle;
		public Color Color;
	}
}
