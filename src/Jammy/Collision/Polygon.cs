using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ClipperLib;

namespace Jammy.Collision
{
	public class Polygon
	{
		public readonly List<Vector2> Vertices;
		public readonly Vector2 RelativeCenter;

		public Vector2 Location;

		#region "Properties"
		public float Scale
		{
			get { return _scale; }
			set 
			{ 
				ScaleAbsolute(value);
			}
		}

		public float Width
		{
			get
			{ 
				UpdateBounds();
				return Right - Left;
			}
		}

		public float Height
		{
			get 
			{ 
				UpdateBounds();
				return Bottom - Top;
			}
		}

		public float Left
		{
			get
			{
				UpdateBounds();
				return _topLeft.X;
			}
		}

		public float Right
		{
			get
			{
				UpdateBounds();
				return _bottomRight.X;
			}
		}

		public float Top
		{
			get
			{
				UpdateBounds();
				return _topLeft.Y;
			}
		}

		public float Bottom
		{
			get
			{
				UpdateBounds();
				return _bottomRight.Y;
			}
		}

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
		#endregion

		public void RotateRelativeAboutPoint(float rotation, Vector2 point)
		{
			RotateAbsoluteAboutPoint(rotation - _rotation, point);
		}

		public void RotateAbsoluteAboutPoint(float rotation, Vector2 point)
		{
			_needsUpdate = true;
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

		public static List<Polygon> Clip(Polygon source, Polygon clipper)
		{
			var c = new Clipper();
			var s = new List<IntPoint>();
			var r = new List<IntPoint>();
			for (var i = 0; i < source.Vertices.Count; i++)
			{
				s.Add(new IntPoint((int)(source.Vertices[i].X + source.Location.X), (int)(source.Vertices[i].Y + source.Location.Y)));
			}
			for (var i = 0; i < clipper.Vertices.Count; i++)
			{
				r.Add(new IntPoint((int)(clipper.Vertices[i].X + clipper.Location.X), (int)(clipper.Vertices[i].Y + clipper.Location.Y)));
			}



			c.AddPolygon(s, PolyType.ptSubject);
			c.AddPolygon(r, PolyType.ptClip);
			var sol = new List<List<IntPoint>>();
			c.Execute(ClipType.ctDifference, sol);
			var ret = new List<Polygon>();
			for (var i = 0; i < sol.Count; i++)
			{
				var poly = new Polygon();
				for (var k = 0; k < sol[i].Count; k++)
				{
					poly.Vertices.Add(new Vector2(sol[i][k].X, sol[i][k].Y));
				}
				ret.Add(poly);
			}
			return ret;
		}

		public void InflateAbsolute(float width, float height)
		{
			_needsUpdate = true;
			var newInfalte = new Vector2(width, height);
			for (var i = 0; i < Vertices.Count; i++)
			{
				Vertices[i] = Vertices[i] + newInfalte - _inflateValues;
			}
			_inflateValues = newInfalte;
		}

		public void InflateRelative(float width, float height)
		{
			InflateAbsolute(width + _inflateValues.X, height + _inflateValues.Y);
		}

		public void ScaleRelative(float scale)
		{
			ScaleAbsolute(scale * _scale);
		}

		public void ScaleAbsolute(float scale)
		{
			_needsUpdate = true;
			for (var i = 0; i < Vertices.Count; i++)
			{
				Vertices[i] = Vertices[i]*scale/_scale;
			}
			_scale = scale;
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

		

		private void UpdateBounds()
		{
			if (!_needsUpdate)
				return;

			_topLeft = _bottomRight = Vertices[0];
			for (var i = 1; i < Vertices.Count; i++)
			{
				_topLeft = Vector2.Min(_topLeft, Vertices[i]);
				_bottomRight = Vector2.Max(_bottomRight, Vertices[i]);
			}

			_needsUpdate = false;
		}

		private float _rotation = 0f;
		private float _scale = 1f;

		private bool _needsUpdate = true;

		private Vector2 _inflateValues = Vector2.Zero;
		private Vector2 _topLeft;
		private Vector2 _bottomRight;

	}
}
