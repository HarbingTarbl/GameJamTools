using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.Parallax
{
	public class ParallaxManager
	{

		public void AddLayer(ParallaxLayer layer)
		{
			Layers.Add(layer);
			Layers.Sort();
		}

		public void RemoveLayer(ParallaxLayer layer)
		{
			Layers.Remove(layer);
		}

		public void Draw(SpriteBatch batch, Matrix matrix)
		{
			
		}

		public void Update(GameTime time)
		{
			foreach (var layer in Layers)
			{
				//layer. // JERK!
			}
		}

		public Vector2 Location;
		public bool IsScrolling;
		public List<ParallaxLayer> Layers = new List<ParallaxLayer>();
	}
}
