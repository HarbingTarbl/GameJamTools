using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.Helpers
{
    public static class MiscHelpers
    {
        public static Texture2D GetPixelTexture (GraphicsDevice d)
        {
            if (pixel == null) {
                pixel = new Texture2D (d, 1, 1, false, SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
            }
            return pixel;
        }

        private static Texture2D pixel;
    }
}
