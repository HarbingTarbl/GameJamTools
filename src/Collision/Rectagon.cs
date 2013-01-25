using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jammy.Collision
{
	public class Rectagon
		: Polygon
	{
		public Rectagon(float x, float y, float width, float height)
			: base(
			new	Vector2(0, 0), 
			new Vector2(width, 0),
			new Vector2(width, height), 
			new Vector2(0, height))
		{
			Location = new Vector2(x, y);
		}

		public Rectagon(Vector2 topLeft, float width, float height)
			: this(topLeft.X, topLeft.Y, width, height)
		{
			
		}

		public Rectagon(float width, float height)
			: this(0, 0, width, height)
		{
			
		}

	}
}
