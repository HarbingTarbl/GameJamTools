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
                    case CollisionDataType.Radius:      return RadiusToRadius (a, b);
                    case CollisionDataType.Rectangle:   return RectToRect (a, b);
                    case CollisionDataType.Polygon:     return PolyToPoly ( a, b);
                }
            }

            //TODO: RectToRadius, RectToPoly, RadiusToRect, RadiusToPoly
            throw new NotSupportedException();
        }

		public static bool RectToRect(Sprite a, Sprite b)
        {
            var ar = (Rectangle) a.CollisionData;
            var br = (Rectangle) b.CollisionData;
            return ar.Intersects (br);
        }

		public static bool RadiusToRadius(Sprite a, Sprite b)
        {
            var af = (float) a.CollisionData;
            var bf = (float) b.CollisionData;
            return Vector2.Distance (a.Location, b.Location) - af - bf < 0;
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
