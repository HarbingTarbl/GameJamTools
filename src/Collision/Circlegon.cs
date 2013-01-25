using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy.Helpers;
using Microsoft.Xna.Framework;

namespace Jammy.Collision
{
	public class Circlegon
		: Polygon
	{
		public Circlegon(float x, float y , float radius, int divisions = 16)
		{
			divisions = Math.Min(GeomHelpers.LookupTableSize, divisions);
			for (var i = 0; i < divisions; i++)
			{
				var n = GeomHelpers.LookupTableSize/divisions*i;
				Vertices.Add(GeomHelpers.SinCosLookupTable[n] * radius);
			}

			Location = new Vector2(x, y);
		}

		public Circlegon(Vector2 center, float radius, int divisions = 16)
			: this(center.X, center.Y, radius, divisions)
		{
			
		}

		public Circlegon(float radius, int divisions = 16)
			: this(0, 0, radius, divisions)
		{
			
		}

	}
}
