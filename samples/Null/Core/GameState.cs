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


			mapSurfaces = new Polygon[map.Layers[0].Tiles.Length];
			for (var x = 0; x < map.Layers[0].Width; x++)
			{
				for (var y = 0; y < map.Layers[0].Height; y++)
				{
					mapSurfaces[y*map.Layers[0].Width + x] = new Rectagon(x*map.TileWidth, y*map.TileHeight, map.TileWidth,
					                                                      map.TileHeight);
				}
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
			for (var i = 0; i < mapSurfaces.Length; i++)
			{
				Game.CollisionRenderer.DrawPolygon(mapSurfaces[i], Color.Lime);
			}
			Game.CollisionRenderer.Stop();


		}
	}
}
