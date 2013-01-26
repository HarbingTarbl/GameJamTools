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
		public PolyLink(int vertIndex)
		{
			this.VertexIndex = vertIndex;
		}

		public int VertexIndex;
		public PolyNode Parent;
		public PolyLink Target;

		//TODO: remove these later
		public int cost;
		public int score;

		public Vector2 GetVertex()
		{
			if (Parent == null)
				throw new InvalidOperationException();

			return Parent.Poly.Vertices[VertexIndex];
		}

		public static void AttachLinks (int indexA, int indexB,
			ref PolyNode na, ref PolyNode nb)
		{
			var a = new PolyLink (indexA);
			var b = new PolyLink (indexB);
			a.Target = b;
			b.Target = a;
			na.AddLink (a);
			nb.AddLink (b);
		}
	}
}