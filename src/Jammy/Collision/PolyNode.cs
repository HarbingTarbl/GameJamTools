using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jammy.Collision
{
	public class PolyNode
	{
		public PolyNode (Polygon poly, params PolyLink[] links)
		{
			this.Poly = poly;
			this.Links = links;
		}

		public Polygon Poly;
		public PolyLink[] Links;
	}

	public class PolyLink
	{
		public PolyLink(int vertIndex, PolyNode parent, PolyLink target)
		{
			this.VertexIndex = vertIndex;
			this.Parent = parent;
			this.Target = target;
		}

		public int VertexIndex;
		public PolyNode Parent;
		public PolyLink Target;
	}
}