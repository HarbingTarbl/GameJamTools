using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.Helpers
{
	public static class SpriteHelper
	{
		public static void BeginDraw (this SpriteBatch self,
			BlendState state, ref Matrix transformation)
		{
			self.Begin (
				SpriteSortMode.Deferred,
				state,
				SamplerState.PointClamp,
				DepthStencilState.Default,
				RasterizerState.CullCounterClockwise,
				null,
				transformation);
		}
	}
}
