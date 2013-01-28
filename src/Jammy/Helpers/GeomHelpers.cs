using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jammy.Helpers
{
    public static class GeomHelpers
    {
	    public static Vector2[] SinCosLookupTable;
	    public const int LookupTableSize = 64;

		static GeomHelpers()
		{
			SinCosLookupTable = new Vector2[LookupTableSize];
			for (var i = 0; i < LookupTableSize; i++)
			{
				var n = (((float) i)/LookupTableSize)*MathHelper.TwoPi;
				SinCosLookupTable[i] = new Vector2((float) Math.Cos(n), (float) Math.Sin(n));

			}
		}

		public static Vector2 LookupAngle(float radians)
		{
			var n = (radians%MathHelper.TwoPi)/MathHelper.TwoPi*LookupTableSize;
			return Vector2.SmoothStep(SinCosLookupTable[(int) Math.Floor(n)], SinCosLookupTable[(int) Math.Ceiling(n)], n);
		}


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

		public static Rectangle Clip(this Rectangle self, Rectangle clipping)
		{
			var left = self.Left > clipping.Left ? self.Left : clipping.Left;
			var right = self.Right < clipping.Right ? self.Right : clipping.Right;
			var top = self.Top > clipping.Top ? self.Top : clipping.Top;
			var bottom = self.Bottom < clipping.Bottom ? self.Bottom : clipping.Bottom;
			return new Rectangle(left, top, right - left, bottom - top);
		}


		public static List<Rectangle> Subtract(this Rectangle self, Rectangle clipping)
		{
			var list = new List<Rectangle>();

			if (self.Left < clipping.Left) //Something on the left
			{
				list.Add(
				         self.Clip(new Rectangle(self.Left, clipping.Top, clipping.Left - self.Left,
												clipping.Bottom - clipping.Top)));
			}

			if (self.Right > clipping.Right) //Something on the right
			{
				list.Add(
				         self.Clip(new Rectangle(self.Right, clipping.Top, self.Right - clipping.Right,
				                                 clipping.Bottom - clipping.Top)));
			}

			if (self.Top < clipping.Top) //Zzz something on top
			{
				list.Add(
				         self.Clip(new Rectangle(self.Left < clipping.Left ? self.Left : clipping.Left, self.Top,
				                                 (self.Right > clipping.Right ? self.Right : clipping.Right)
				                                 - (self.Left < clipping.Left ? self.Left : clipping.Left),
				                                 clipping.Top - self.Top)));
			}

			if (self.Bottom > clipping.Bottom)
			{
				list.Add(
				         self.Clip(new Rectangle(self.Left < clipping.Left ? self.Left : clipping.Left, clipping.Bottom,
				                                 (self.Right > clipping.Right ? self.Right : clipping.Right)
				                                 - (self.Left < clipping.Left ? self.Left : clipping.Left),
				                                 clipping.Bottom - self.Bottom)));
			}

			return list;
		}

        public static void CenterRectangle (ref Rectangle rect, Vector2 v)
        {
            rect.Location = new Point (
                (int) (v.X - (rect.Width/2)),
                (int) (v.Y - (rect.Height/2)));
        }
    }
}
