using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.Parallax
{
	public class StaticParallax
	{
		public StaticParallax(Texture2D texture, Rectangle? bounds)
		{
			Texture = texture;

			_bounds = bounds.HasValue ? bounds.Value : texture.Bounds;
		}

		public void Draw(SpriteBatch batch)
		{
			var drawRectangle = new Rectangle((int)Location.X, (int)Location.Y, _bounds.Width, _bounds.Height);
			batch.Draw(Texture, drawRectangle, _bounds, Color.White);
		}

		public void ScrollPixels(int amountx, int amounty)
		{
			_bounds.X += amountx;
			_bounds.Y += amounty;
		}

		public void ScrollSet(Vector2 scroll)
		{
			_bounds.X = (int) scroll.X;
			_bounds.Y = (int) scroll.Y;
		}

		public void ScrollPercent(float percent)
		{
			_bounds.X += (int)(_bounds.Width*percent);
		}

		public int CurrentScroll
		{
			get { return _bounds.X; }
			set { _bounds.X = value; }
		}

		public Vector2 Location;
		private Rectangle _bounds;
		public Texture2D Texture;
	}
}
