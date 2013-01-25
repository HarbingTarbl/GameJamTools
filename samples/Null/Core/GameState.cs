using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy;
using Jammy.Helpers;
using Jammy.StateManager;
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
		KeyboardState oldState;

		public override void Load()
		{
			camera = new CameraSingle (Game.ScreenWidth, Game.ScreenHeight);
			
			player = new Player();
			player.Texture = Game.ContentLoader.Load<Texture2D> ("Player");

			Rock = new Sprite();
			Rock.Texture = Game.ContentLoader.Load<Texture2D> ("Rock");
		}

		public override void Update (GameTime gameTime)
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
			batch.Begin (SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
				DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.Transformation);

			Rock.Draw (batch);
			player.Draw (batch);

			batch.End();
		}
	}
}
