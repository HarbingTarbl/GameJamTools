using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy;
using Jammy.Collision;
using Jammy.Helpers;
using Jammy.Sprites;
using Jammy.StateManager;
using Jammy.TileMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SampleJammy.Core;

namespace SampleJammy
{
	public class GameState : BaseGameState
	{
		public GameState()
			: base ("Game")
		{
		}

		Player player;
		Sprite Rock;
		CameraSingle camera;
		Map map;
		Polygon[] mapSurfaces;
		private CollisionRenderer t;

		public override void Load()
		{
			map = Game.ContentLoader.Load<Map> ("Map");
			camera = new CameraSingle (Game.ScreenWidth, Game.ScreenHeight);
			
			player = new Player();
			player.Texture = Game.ContentLoader.Load<Texture2D> ("Player");

			Rock = new Sprite();
			Rock.Texture = Game.ContentLoader.Load<Texture2D> ("Rock");

			mapSurfaces = new Polygon[map.Layers.Count];
			var i = 0;
			foreach (var layer in map.Layers)
			{
				mapSurfaces[i] = new Rectagon(0, 0, layer.Width*map.TileWidth, layer.Height*map.TileHeight);
				break;
			}

		}

		public override void Update(GameTime gameTime)
		{
			var keyState = Keyboard.GetState();

			if (keyState.IsKeyDown (Keys.A)) {
				player.Location.X -= 5f;
			} else if (keyState.IsKeyDown (Keys.D)) {
				player.Location.X += 5f;
			}
			if (keyState.IsKeyDown (Keys.W)) {
				player.Location.Y -= 5f;
			} else if (keyState.IsKeyDown (Keys.S)) {
				player.Location.Y += 5f;
			}

			player.Update (gameTime);	
			camera.CenterOnPoint  (player.Location);
		}

		public override void Draw (SpriteBatch batch)
		{
			batch.Begin (SpriteSortMode.Immediate,
				BlendState.NonPremultiplied,
				SamplerState.PointClamp,
				DepthStencilState.Default,
				RasterizerState.CullCounterClockwise,
				null,
				camera.Transformation);

			map.Draw (batch);
			Rock.Draw (batch);
			player.Draw (batch);

			batch.End();

			Game.CollisionRenderer.Begin(camera.Transformation);

			Game.CollisionRenderer.DrawPolygon(mapSurfaces[0], Color.Black);
			Game.CollisionRenderer.Stop();


		}
	}
}
