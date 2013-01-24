using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
				Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateOrthographicOffCenter(0, Device.Viewport.Width, Device.Viewport.Height, 0, 0, 1),
				World = Matrix.Identity,
				View = Matrix.Identity,
			};

			CirclePreComputes = new float[2][];
			CirclePreComputes[0] = new float[CircleSubDivides];
			CirclePreComputes[1] = new float[CircleSubDivides];
			for (var i = 0; i < CircleSubDivides; i++)
			{
				CirclePreComputes[0][i] = (float)Math.Cos((i*2f*Math.PI) / CircleSubDivides);
			}

			for (var i = 0; i < CircleSubDivides; i++)
			{
				CirclePreComputes[1][i] = (float)Math.Sin((i * 2f * Math.PI) / CircleSubDivides);
			}

		}

		public void Begin()
		{
			hasBegun = true;
			oldBlend = Device.BlendState;
			oldRaster = Device.RasterizerState;
			oldStencil = Device.DepthStencilState;
			Device.RasterizerState = new RasterizerState()
			{
				CullMode = CullMode.None
			};
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

		public void Draw(Sprite sprite)
		{
			switch (sprite.CollisionType)
			{
				case CollisionDataType.Polygon:
					DrawPolygon(sprite.Location, (Polygon)sprite.CollisionData);
					break;

				case CollisionDataType.Radius:
					DrawCircle(sprite.Location, (float) sprite.CollisionData);
					break;

				case CollisionDataType.Rectangle:
					DrawRectangle((Rectangle) sprite.CollisionData);
					break;
				default:
					throw new NotImplementedException(string.Format("Unable to draw debug lines for {0} colliders!",
					                                  Enum.GetName(typeof (CollisionDataType), sprite.CollisionType)));

			}
		}



		public void DrawRectangle(Rectangle rectangle)
		{
			vertsToDraw.AddRange(
				new []
				{
					new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Top, 0), Color.Red),
					new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Top, 0), Color.Red),
					new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Top, 0), Color.Red),
					new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Bottom, 0), Color.Red),
					new VertexPositionColor(new Vector3(rectangle.Right, rectangle.Bottom, 0), Color.Red),
					new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Bottom, 0), Color.Red),
					new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Bottom, 0), Color.Red),
					new VertexPositionColor(new Vector3(rectangle.Left, rectangle.Top, 0), Color.Red),
				});
		}

		public void DrawPolygon(Vector2 location, Polygon polygon)
		{
			var i = 0;
			for (; i < polygon.Vertices.Count - 1; i ++)
			{
				vertsToDraw.Add(new VertexPositionColor(new Vector3(polygon.Vertices[i] + location, 0), Color.LimeGreen));
				vertsToDraw.Add(new VertexPositionColor(new Vector3(polygon.Vertices[i + 1] + location, 0), Color.LimeGreen));
			}

			vertsToDraw.Add(new VertexPositionColor(new Vector3(polygon.Vertices[i] + location, 0), Color.LimeGreen));
			vertsToDraw.Add(new VertexPositionColor(new Vector3(polygon.Vertices[0] + location, 0), Color.LimeGreen));
		}

		public void DrawCircle(Vector2 location, float radius)
		{
			var i = 0;
			for (; i < CircleSubDivides - 1; i++)
			{
				vertsToDraw.Add(
								new VertexPositionColor(
									new Vector3(CirclePreComputes[0][i], CirclePreComputes[1][i], 0) * radius + new Vector3(location, 0), Color.Black));
				vertsToDraw.Add(
								new VertexPositionColor(
									new Vector3(CirclePreComputes[0][i + 1], CirclePreComputes[1][i + 1], 0) * radius + new Vector3(location, 0), Color.Black));
			}

			vertsToDraw.Add(
					new VertexPositionColor(
						new Vector3(CirclePreComputes[0][i], CirclePreComputes[1][i], 0) * radius + new Vector3(location, 0), Color.Black));
			vertsToDraw.Add(
							new VertexPositionColor(
								new Vector3(CirclePreComputes[0][0], CirclePreComputes[1][0], 0) * radius + new Vector3(location, 0), Color.Black));
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
