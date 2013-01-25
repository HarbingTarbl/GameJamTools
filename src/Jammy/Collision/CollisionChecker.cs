using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy.Sprites;
using Microsoft.Xna.Framework;

namespace Jammy.Collision
{
    public static class CollisionChecker
    {
        public static bool Colliding (Sprite a, Sprite b)
        {
            if (a.CollisionType == b.CollisionType)
            {
                switch (a.CollisionType)
                {
                    case CollisionDataType.Radius:
		                return RadiusToRadius(a.Location, (float) a.CollisionData, b.Location, (float) b.CollisionData);
                    case CollisionDataType.Rectangle:   
						return RectToRect ((Rectangle)a.CollisionData, (Rectangle)b.CollisionData);
                    case CollisionDataType.Polygon:     
						return PolyToPoly ( (Polygon)a.CollisionData, (Polygon)b.CollisionData);
                }
            }

            //TODO: RectToRadius, RectToPoly, RadiusToRect, RadiusToPoly
            throw new NotSupportedException();
        }

		public static bool PointToSprite(Vector2 point, Sprite a)
		{
			return true;
		}
		
		public static bool PointToPoly(Vector2 point, Polygon poly)
		{
			var inside = false;
			for (int i = 0, j = poly.Vertices.Count - 1; i < poly.Vertices.Count; j = i++)
			{
				var xi = poly.Vertices[i].X + poly.Location.X;
				var xj = poly.Vertices[j].X + poly.Location.X;
				var yi = poly.Vertices[i].Y + poly.Location.Y;
				var yj = poly.Vertices[j].Y + poly.Location.Y;

				if (
					((yi > point.Y) != (yj > point.Y))
					&&
					(point.X < (xj - xi)*(point.Y - yi)/(yj - yi) + xi)
					)
				{
					inside = !inside;
				}
			}
			return inside;
		}

		public static bool PointToRect(Vector2 point, Rectangle rect)
		{
			return rect.Contains((int)point.X, (int)point.Y);
		}

		public static bool PointToCircle(Vector2 point, Vector2 circCenter, float r)
		{
			return Vector2.Distance(point, circCenter) < r;
		}

		public static bool RectToRect(Rectangle a, Rectangle b)
        {
            return a.Intersects (b);
        }

		public static bool RadiusToRadius(Vector2 al, float af, Vector2 bl, float bf)
        {
            return Vector2.Distance (al, bl) < af + bf;
        }

		//TODO: add an out for the penetrator vertex?
		public static bool PolyToPoly(Polygon a, Polygon b)
		{
			for (var i = 0; i < a.Vertices.Count; i++)
			{
				if (PointToPoly(a.Vertices[i] + a.Location, b))
					return true;
			}

			for (var i = 0; i < b.Vertices.Count; i++)
			{
				if (PointToPoly(b.Vertices[i] + b.Location, a))
					return true;
			}

			return false;
		}
    }

    public enum CollisionDataType
    {
        Rectangle,
        Radius,
        Polygon
    }
}
