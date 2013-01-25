using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jammy.Collision
{
	public class Polygon
	{
		public readonly List<Vector2> Vertices;
		public readonly Vector2 RelativeCenter;
		


		public Vector2 Location;

		

		public Vector2 AbsoluteCenter
		{
			get { return RelativeCenter + Location; }
		}

		public float Rotation
		{
			get { return _rotation; }
			set
			{
				RotateAbsoluteAboutPoint(value, RelativeCenter);
			}
		}

		public void RotateRelativeAboutPoint(float rotation, Vector2 point)
		{
			RotateAbsoluteAboutPoint(rotation - _rotation, point);
		}

		public void RotateAbsoluteAboutPoint(float rotation, Vector2 point)
		{
			var sin = (float) Math.Sin(rotation);
			var cos = (float) Math.Cos(rotation);
			for (var i = 0; i < Vertices.Count; i++)
			{
				var x = Vertices[i].X - point.X;
				var y = Vertices[i].Y - point.Y;
				Vertices[i] = new Vector2(cos*x - sin*y + point.X, sin*x + cos*y + point.Y);
			}
			_rotation = rotation;
		}

		public Polygon()
		{
			Vertices = new List<Vector2>();
		}

		public Polygon(params Vector2[] verts)
		{
			Vertices = new List<Vector2>(verts);
			RelativeCenter = new Vector2();
			for (var i = 0; i < Vertices.Count; i++)
			{
				RelativeCenter += Vertices[i];
			}
			RelativeCenter /= Vertices.Count;
		}

		public bool PolygonsCollide(Polygon collider, out Vector2 penetrator)
		{
			// Iterate through every verticy on polygon #1
			for (int i = 0; i < Vertices.Count; i++)
			{
				var a = Vertices[i];
				var b = (i + 1 == Vertices.Count ? Vertices[0] : Vertices[i + 1]);

				Vector2 edge = b - a;
				Vector2 normal = new Vector2(-edge.Y, edge.X);
				normal.Normalize();

				float threshhold = Vector2.Dot(a, normal);
				float minProjected = float.PositiveInfinity;

				// Now compare it against every verticy on polygon #2
				for (int j = 0; j < collider.Vertices.Count; j++)
				{
					float projected = Vector2.Dot(collider.Vertices[j], normal);
					minProjected = MathHelper.Min(minProjected, projected);
				}


			}


			penetrator = Vector2.Zero;
			return false;
		}


		private float _rotation;
		private float _needsUpdate;
	}
}
