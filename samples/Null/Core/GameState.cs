using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy;
using Jammy.StateManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		CameraSingle camera;

		public override void Load()
		{
			player = new Player();
			player.Texture = Game.ContentLoader.Load<Texture2D> ("Player");
		}

		public override void Update (GameTime gameTime)
		{
			player.Update (gameTime);	
		}

		public override void Draw (SpriteBatch batch)
		{
			player.Draw (batch);
		}
	}
}
