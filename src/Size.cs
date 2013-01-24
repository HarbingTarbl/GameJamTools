using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jammy
{
    public class Size
    {
        public Size() {}

        public Size (float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public readonly float Width;
        public readonly float Height;

        public readonly static Size Zero = new Size(0, 0);
    }
}
