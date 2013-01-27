using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.Sprites
{
	public class AnimatedSprite
		: Sprite
	{
		public AnimatedSprite(Texture2D texture, IEnumerable<Animation> animationsList)
			: base()
		{
			AnimationManager = new AnimationManager(texture, animationsList);
			Texture = texture;
		}

		public override void Update(GameTime gameTime)
		{
			AnimationManager.Update(gameTime);
			base.Update(gameTime);
			_IHateRectangles.X = (int) Location.X;
			_IHateRectangles.Y = (int) Location.Y;
		}

		public override void Draw(SpriteBatch batch)
		{
			batch.Draw(Texture, _IHateRectangles, AnimationManager.Bounding, Color.White, Rotation, Origin, SpriteEffects.None, 0f);
		}

		public AnimationManager AnimationManager;

		protected Rectangle _IHateRectangles = new Rectangle(0, 0, 250, 500);

	}

	public class Animation
	{
		private static readonly TimeSpan DefaultTimeSpan = new TimeSpan(0, 0, 0, 0, 150);

		public readonly bool Looping = true;
		public readonly TimeSpan FrameRate;
		public readonly string Name;
		public readonly Rectangle[] Sources;

		public string NextAnim;
		public int CurrentSource;

		public Animation(string name, IEnumerable<Rectangle> sources, TimeSpan frameRate = default(TimeSpan), bool Looping = true)
		{
			Name = name;
			Sources = sources.ToArray();
			FrameRate = frameRate == default(TimeSpan) ? DefaultTimeSpan : frameRate;
			this.Looping = Looping;
		}
	}

	public class AnimationManager
	{
		public AnimationManager(Texture2D texture2D, Rectangle lRectangle, IEnumerable<Animation> animationsList = null)
		{
			animations = animationsList != null ? new Dictionary<string, Animation>(animationsList.ToDictionary((ani) => ani.Name)) : new Dictionary<string, Animation>();
		}

		public AnimationManager(Texture2D texture2D, IEnumerable<Animation> animationsList = null)
		{
			animations = animationsList != null ? new Dictionary<string, Animation>(animationsList.ToDictionary((ani) => ani.Name)) : new Dictionary<string, Animation>();

		}

		public Animation CurrentAnimation
		{
			get { return currentAnimation; }
		}

		public void AddAnimation(Animation animation)
		{
			animations[animation.Name] = animation;
		}

		public bool SetAnimation(string Name)
		{
			Animation animation;
			if (!animations.TryGetValue(Name, out animation))
				return false;

			currentAnimation = animation;
			currentAnimation.CurrentSource = 0;
			Bounding = currentAnimation.Sources[0];
			return true;
		}

		public void Update(GameTime gameTime)
		{
			if (currentAnimation != null)
				playAnimation(gameTime);
		}

		private void playAnimation(GameTime gameTime)
		{
			lastUpdate += gameTime.ElapsedGameTime;
			if (lastUpdate < currentAnimation.FrameRate)
			{

				return;
			}


			if (currentAnimation.Looping)
				currentAnimation.CurrentSource = ((currentAnimation.CurrentSource + 1) % currentAnimation.Sources.Length);
			else
			{
				if (currentAnimation.CurrentSource < currentAnimation.Sources.Length - 1)
					currentAnimation.CurrentSource++;
				else if (currentAnimation.NextAnim != null)
					SetAnimation(currentAnimation.NextAnim);
			}
			Bounding = currentAnimation.Sources[currentAnimation.CurrentSource];
			lastUpdate = TimeSpan.Zero;
		}

		public Rectangle Bounding;

		private TimeSpan lastUpdate = TimeSpan.Zero;
		private Animation currentAnimation;
		private readonly Dictionary<string, Animation> animations;
	}
}
