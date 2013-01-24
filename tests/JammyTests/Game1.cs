using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Jammy.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JammyTests
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		private SpriteFont font;
		SpriteBatch spriteBatch;
		private Texture2D test;


		private Vector2[] locations =
		{
			new Vector2(100, 0), //Rectangles
			new Vector2(210, 0),

			new Vector2(100, 110), //Polygons
			new Vector2(210, 110),

			new Vector2(100, 300), //Circles
			new Vector2(300, 300)
		};

		private Rectangle[] rectangles = //Location + 0
		{
			new Rectangle(0, 0, 100, 100),
			new Rectangle(0, 0, 50, 75)
		};

		private Polygon[] polys = //Location + 2
		{
			new Polygon( //Square
				new Vector2(0, 0),
				new Vector2(0, 100),
				new Vector2(100, 100),
				new Vector2(100, 0)),

			new Polygon( //Triangle
				new Vector2(0, 0),
				new Vector2(0, 100),
				new Vector2(50,50))
		};

		private float[] circles = // Location + 4
		{
			50f, 90f
		};

		private Polygon testPoly;
		private CollisionRenderer collisionRender;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			collisionRender = new CollisionRenderer(graphics.GraphicsDevice);
			IsMouseVisible = true;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("font");

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			var mouseState = Mouse.GetState();
			var deltaX = mouseState.X - oldState.X;
			var deltaY = mouseState.Y - oldState.Y;
			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				if (CollisionChecker.PointToRect(new Vector2(mouseState.X, mouseState.Y), rectangles[0]))
				{
					
					locations[0].X += deltaX;
					locations[0].Y += deltaY;
					
				}
				else
				if (CollisionChecker.PointToCircle(new Vector2(mouseState.X, mouseState.Y), locations[4],circles[0]))
				{
					locations[4].X += deltaX;
					locations[4].Y += deltaY;
				}

			}
			else
			if (mouseState.RightButton == ButtonState.Pressed)
			{
				if (CollisionChecker.PointToRect(new Vector2(mouseState.X, mouseState.Y), rectangles[1]))
				{
					locations[1].X += deltaX;
					locations[1].Y += deltaY;
					
				}
				else
				if (CollisionChecker.PointToCircle(new Vector2(mouseState.X, mouseState.Y), locations[5], circles[1]))
				{
					locations[5].X += deltaX;
					locations[5].Y += deltaY;

				}
			}

			rectangles[0].X = (int)locations[0].X;
			rectangles[0].Y = (int)locations[0].Y;
			rectangles[1].X = (int)locations[1].X;
			rectangles[1].Y = (int)locations[1].Y;
			




			// TODO: Add your update logic here
			oldState = mouseState;
			base.Update(gameTime);
		}

		public MouseState oldState;

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			if (CollisionChecker.RectToRect(rectangles[0], rectangles[1]))
			{
				spriteBatch.DrawString(font, "Rectangle - Rectangle Collision!", new Vector2(200, 200), Color.Black);
			}

			if (CollisionChecker.RadiusToRadius(locations[4], circles[0], locations[5], circles[1]))
			{
				spriteBatch.DrawString(font, "Circle - Circle Collision!", new Vector2(200, 250), Color.Black);
			}

			for (var i = 0; i < locations.Length; i++)
			{
				spriteBatch.DrawString(font, string.Format("{0} : {1},{2}", i + 1, locations[i].X, locations[i].Y),
				                       new Vector2(600, 25*i), Color.Black);
			}

			spriteBatch.End();

			collisionRender.Begin();
		
			for (var i = 0; i < rectangles.Length; i++)
			{
				collisionRender.DrawRectangle(rectangles[i]);
			}

			for (var i = 0; i < polys.Length; i++)
			{
				collisionRender.DrawPolygon(locations[i + 2], polys[i]);
			}

			for (var i = 0; i < circles.Length; i++)
			{
				collisionRender.DrawCircle(locations[i + 4], circles[i]);
			}

			collisionRender.Stop();
			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
