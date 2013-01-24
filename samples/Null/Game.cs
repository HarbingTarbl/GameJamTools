using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jammy.StateManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SampleJammy
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game : Microsoft.Xna.Framework.Game
	{
		public static GraphicsDeviceManager Graphics;
		public static StateManager States;
		public static ContentManager ContentLoader;

		SpriteBatch spriteBatch;

		public Game()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			ContentLoader = Content;

			States = new StateManager();
			States.Load (Assembly.GetExecutingAssembly());
			States.Set ("Game");
		}

		protected override void UnloadContent()
		{
			
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			States.Update (gameTime);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			States.Draw (spriteBatch);
			base.Draw(gameTime);
		}
	}
}
