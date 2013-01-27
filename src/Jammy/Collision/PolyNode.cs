using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Jammy.Collision
{
	public class PolyNode
	{
		public PolyNode (Polygon poly)
		{
			this.Poly = poly;
		}

		public void AddLink (PolyLink link)
		{
			link.Parent = this;
			Links.Add (link);
		}

		public Polygon Poly;
		public List<PolyLink> Links = new List<PolyLink>();
	}

	public class PolyLink
	{
		public PolyLink (float x, float y)
		{
			this.location = new Vector2 (x, y);
		}

		public PolyNode Parent;
		public PolyLink Target;
		private Vector2 location;

		public Vector2 GetVertex()
		{
			return location;
		}

		public static void AttachLinks (float x, float y,
			ref PolyNode na, ref PolyNode nb)
		{
			var a = new PolyLink (x, y);
			var b = new PolyLink (x, y);
			a.Target = b;
			b.Target = a;
			na.AddLink (a);
			nb.AddLink (b);
		}
	}
}