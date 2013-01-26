using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.Collision
{
	public class CollisionRenderer
	{
		public CollisionRenderer(GraphicsDevice device)
		{
			Device = device;
			vertsToDraw = new List<VertexPositionColor>();
			effect = new BasicEffect(device)
			{
				VertexColorEnabled = true,
				World = Matrix.Identity,
				View = Matrix.Identity,
				Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) *Matrix.CreateOrthographicOffCenter(0,  device.Viewport.Width, device.Viewport.Height, 0, 0, 1)

			};
		}

		public void Begin(Matrix? projection)
		{
			hasBegun = true;
			oldBlend = Device.BlendState;
			oldRaster = Device.RasterizerState;
			oldStencil = Device.DepthStencilState;
			Device.RasterizerState = new RasterizerState()
			{
				CullMode = CullMode.None
			};

			effect.World = projection.Value;
			//effect.Projection = projection.Value;
			//effect.View = projection.Value;
		}

		public void Stop()
		{
			effect.CurrentTechnique.Passes[0].Apply();
			Device.DrawUserPrimitives(PrimitiveType.LineList, vertsToDraw.ToArray(), 0, vertsToDraw.Count / 2, VertexPositionColor.VertexDeclaration);
			vertsToDraw.Clear();
			hasBegun = false;
			Device.BlendState = oldBlend;
			Device.RasterizerState = oldRaster;
			Device.DepthStencilState = oldStencil;
		}

		public void Draw(Sprite sprite, Color? color = null)
		{
			DrawPolygon((Polygon) sprite.CollisionData, color.GetValueOrDefault());
		}

		public void DrawRectangle(Rectangle rectangle, Color color)
		{
			DrawPolygon(new Rectagon(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), color);
		}

		public void DrawPolygon(Polygon polygon, Color color)
		{
			if (polygon.Vertices.Count == 0)
				return;

			var i = 0;
			for (; i < polygon.Vertices.Count - 1; i ++)
			{
				vertsToDraw.Add(new VertexPositionColor(new Vector3(polygon.Vertices[i] + polygon.Location, 0), color));
				vertsToDraw.Add(new VertexPositionColor(new Vector3(polygon.Vertices[i + 1] + polygon.Location, 0), color));
			}

			vertsToDraw.Add(new VertexPositionColor(new Vector3(polygon.Vertices[i] + polygon.Location, 0), color));
			vertsToDraw.Add(new VertexPositionColor(new Vector3(polygon.Vertices[0] + polygon.Location, 0), color));
		}

		public void DrawCircle(Vector2 location, float radius, Color color)
		{
			DrawPolygon(new Circlegon(location, radius), color);
		}

		public readonly GraphicsDevice Device;


		private bool hasBegun;
		private BasicEffect effect;
		private List<VertexPositionColor> vertsToDraw;

		private const int CircleSubDivides = 32;

		private float[][] CirclePreComputes;

		private BlendState oldBlend;
		private RasterizerState oldRaster;
		private DepthStencilState oldStencil;
	}
}
