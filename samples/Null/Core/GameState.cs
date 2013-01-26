using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy;
using Jammy.Collision;
using Jammy.Helpers;
using Jammy.Parallax;
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
		CollisionRenderer debug;
		ParallaxManager parallax;

		public override void Load()
		{
			debug = new CollisionRenderer (Game.Graphics.GraphicsDevice);

			map = Game.ContentLoader.Load<Map> ("Map");
			camera = new CameraSingle (Game.ScreenWidth, Game.ScreenHeight);
			
			player = new Player();
			player.Texture = Game.ContentLoader.Load<Texture2D> ("Player");

			Rock = new Sprite();
			Rock.Texture = Game.ContentLoader.Load<Texture2D> ("Rock");

			// TODO: Holy hell this API is brutal, and it doesn't work
			parallax = new ParallaxManager();
			var p1 = new ParallaxLayer();
			var p2 = new ParallaxLayer ();
			var p3 = new ParallaxLayer ();
			var p4 = new ParallaxLayer ();
			p1.Sprites.Add (new ParallaxSprite {Texture = Game.ContentLoader.Load<Texture2D> ("parallax1")});
			p2.Sprites.Add (new ParallaxSprite {Texture = Game.ContentLoader.Load<Texture2D> ("parallax2")});
			p3.Sprites.Add (new ParallaxSprite {Texture = Game.ContentLoader.Load<Texture2D> ("parallax3")});
			p4.Sprites.Add (new ParallaxSprite {Texture = Game.ContentLoader.Load<Texture2D> ("parallax4")});
			parallax.AddLayer (p1);
			parallax.AddLayer (p2);
			parallax.AddLayer (p3);
			parallax.AddLayer (p4);
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
			camera.CenterOnPoint (player.Location);//Game.ScreenWidth/2, Game.ScreenHeight/2);
		}

		public override void Draw (SpriteBatch batch)
		{
			parallax.Draw (batch, camera.Transformation);

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
			batch.End ();

			// Display debug information
			debug.Begin ();
			foreach (var l in map.ObjectLayers)
			{
				foreach (var p in l.Polygons)
				{
					//TODO: I have to transform the polygon
					// manually to adhere to the camera transformation
					// I think it would be better if I pass in the current
					// camera transformation into debug.Begin
					p.Location.X = -camera.Location.X;
					p.Location.Y = -camera.Location.Y;
					debug.DrawPolygon (p);
				}
			}
			debug.Stop ();
		}
	}
}
