using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jammy.Collision
{
	public class Polygon
	{
		public List<Vector2> Vertices;
		public readonly Vector2 Center;


		public Polygon()
		{
			Vertices = new List<Vector2>();
		}

		public Polygon(params Vector2[] verts)
		{
			Vertices = new List<Vector2>(verts);
			Center = new Vector2();
			for (var i = 0; i < Vertices.Count; i++)
			{
				Center += Vertices[i];
			}
			Center /= Vertices.Count;
		}

		//Source: http://pastebin.com/wKCFV2kk
		public bool PolygonsCollide (Polygon collider, out Vector2 penetrator)
		{
			var contacts = new List<Vector2>();

			// Iterate through every verticy on polygon #1
			for (int i = 0; i < Vertices.Count; i++)
			{
				var a = Vertices[i];
				var b = (i + 1 == Vertices.Count ? Vertices[0] : Vertices[i + 1]);

				Vector2 edge = b - a;
				Vector2 normal = new Vector2 (-edge.Y, edge.X);
				normal.Normalize();

				float threshhold = Vector2.Dot(a, normal);
				float minProjected = float.PositiveInfinity;

				// Now compare it against every verticy on polygon #2
				for (int j = 0; j < collider.Vertices.Count; j++)
				{
					float projected = Vector2.Dot(collider.Vertices[j], normal);
					minProjected = MathHelper.Min (minProjected, projected);
				}

				if (minProjected < threshhold)
					contacts.Add(normal * -minProjected);
			}

			if (contacts.Count == Vertices.Count)
			{
				penetrator = contacts.Min();
				return true;
			}

			penetrator = Vector2.Zero;
			return false;
		}
	}
}
