using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jammy.Helpers;

namespace Jammy.Parallax
{
	public class ParallaxLayer
		: IComparable<ParallaxLayer>
	{
		public void Draw(SpriteBatch batch, Matrix matrix)
		{
			foreach (var sprite in Sprites)
			{
				var clippedDraw = sprite.DrawRectangle.Clip(Bounds);
				if (clippedDraw.Width == 0 || clippedDraw.Height == 0)
					continue;

				var xScale = (float)sprite.DrawRectangle.Width/sprite.SourceRectangle.Value.Width;
				var yScale = (float)sprite.DrawRectangle.Height/sprite.SourceRectangle.Value.Height;

				var xOff = (sprite.DrawRectangle.Left - clippedDraw.Left)*xScale;
				var yOff = (sprite.DrawRectangle.Top - clippedDraw.Top)*yScale;

				var clippedSource = new Rectangle((int)(xOff + sprite.SourceRectangle.Value.X),
				                                  (int)(yOff + sprite.SourceRectangle.Value.Y),
				                                  (int)((sprite.DrawRectangle.Right - clippedDraw.Right)*xOff),
				                                  (int)((sprite.DrawRectangle.Bottom - clippedDraw.Bottom)*yOff));

				clippedDraw.X += Bounds.X;
				clippedDraw.Y += Bounds.Y;


				batch.Draw(sprite.Texture, clippedDraw, clippedSource, sprite.Color);
			}
		}

		public void Update(GameTime time)
		{
			Bounds.X += (int) ((float) time.ElapsedGameTime.TotalMilliseconds*Speed*ScrollDirection.X);
			Bounds.Y += (int) ((float) time.ElapsedGameTime.TotalMilliseconds*Speed*ScrollDirection.Y);
			Bounds.X %= Bounds.Width;
			Bounds.Y %= Bounds.Height;

		}

		public void SrollPercentage(float percent)
		{
			Bounds.X = (int)(Bounds.Width*percent);
		}

		public ParallaxLayer()
		{
			
		}


		

		public List<ParallaxSprite> Sprites = new List<ParallaxSprite>();
		public bool IsScrolling;
		public bool IsRepeating;

		public Vector2 Location;
		public float Speed; //Units per ms
		public Vector2 ScrollDirection;
		public Rectangle Bounds; //Width/Height of the parallax layer. 
		public int Order; //0 - Lowest, Rendered in descending order

		private Vector2 _scrollOffset;

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
