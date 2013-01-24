using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jammy.Helpers
{
    public static class GeomHelpers
    {
        public static Vector2 ToVector2(this Point self)
        {
            return new Vector2(self.X, self.Y);
        }

        public static Vector2 SafelyNormalize(this Vector2 self)
        {
            if (self.Length() <= 0)
                return self;

            return Vector2.Normalize(self);
        }

        public static Vector2 NegateY (this Vector2 self)
        {
            self.Y *= -1;
            return self;
        }

        public static Rectangle CreateCenteredRectangle(Vector2 location, int width, int height)
        {
            Rectangle rect = new Rectangle((int)location.X, (int)location.Y, 0, 0);
            rect.Inflate(width, height);

            return rect;
        }

        public static void CenterRectangle (Rectangle rect, Vector2 v)
        {
            rect.Location = new Point (
                (int) (v.X - (rect.Width/2)),
                (int) (v.Y - (rect.Height/2)));
        }
    }
}
