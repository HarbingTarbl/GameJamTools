using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
						return PolyToPoly ( a, b);
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
			for (int i = 0, j = poly.Vertices.Count - 1; i < poly.Vertices.Count; i++)
			{
				if (poly.Vertices[i].Y < point.Y
				    && poly.Vertices[j].Y < point.Y)
				{
					continue;
				}

				if (poly.Vertices[i].Y >= point.Y
				    && poly.Vertices[j].Y >= point.Y)
				{
					continue;
				}

				var deno = (poly.Vertices[i].X - poly.Vertices[j].X)*point.Y - (poly.Vertices[i].Y - poly.Vertices[j].Y)*point.Y;
				if (deno > 0f)
					inside = !inside;
				//if (((poly.Vertices[i].Y > point.Y) != (poly.Vertices[j].Y > point.Y))
				//	&& (point.X
				//		< (poly.Vertices[j].X - poly.Vertices[i].X)*(point.Y - poly.Vertices[i].Y)
				//		/(poly.Vertices[j].Y - poly.Vertices[i].Y) + poly.Vertices[i].X))
				//{
				//	inside = !inside;
				//}
				j = i;
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
		public static bool PolyToPoly (Sprite a, Sprite b)
		{
			var ap = (Polygon) a.CollisionData;
			var bp = (Polygon) b.CollisionData;
			Vector2 penetrator;
			return ap.PolygonsCollide (bp, out penetrator);
		}
    }

    public enum CollisionDataType
    {
        Rectangle,
        Radius,
        Polygon
    }
}
