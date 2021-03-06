﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.StateManager
{
    public class ColorTransition
        : ITransition
    {
        public ColorTransition(GraphicsDevice d, int width, int height,
			float durationSeconds, Color startColor, Color endColor)
			: this (d, width, height, new TimeSpan(0, 0, 0, 0, (int)(durationSeconds * 1000f)), startColor, endColor)
        {
        }

        public ColorTransition(GraphicsDevice d, int width, int height,
            TimeSpan duration, Color startColor, Color endColor)
        {
            totalDuration = duration;
            remainingDuration = duration;

            this.startColor = startColor;
            this.endColor = endColor;

            texture = MiscHelpers.GetPixelTexture(d);
            texture.SetData(new[] { Color.White });
            screenArea = new Rectangle(0, 0, width, height);
        }

        public event EventHandler OnTransitionStart;
        public event EventHandler OnTransitionEnd;
	    public Action Finished;

        public TimeSpan TimeRemaining
        {
            get
            {
                return remainingDuration;
            }
        }

        public Rectangle ScreenArea
        {
            get
            {
                return screenArea;
            }
        }

        public bool IsRunning
        {
            get { return running; }
        }

        public bool HasStarted
        {
            get { return started; }
        }

        public bool HasFinished
        {
            get { return finished; }
        }

        public void Cancel()
        {
            running = false;
        }

	    public void Reset()
	    {
		    displayColor = startColor;
		    remainingDuration = totalDuration;
			finished = false;
			started = false;
			running = false;
	    }

	    public virtual void Update(GameTime gameTime)
        {
            if (!started || !running || finished)
                return;

            remainingDuration -= gameTime.ElapsedGameTime;
            displayColor = Color.Lerp(endColor, startColor,
                (float)(remainingDuration.TotalMilliseconds / totalDuration.TotalMilliseconds));

            if (remainingDuration.TotalMilliseconds <= 0)
            {
                displayColor = endColor;
                TriggerEnd();
            }
        }

        public void TriggerStart()
        {
			displayColor = startColor;
			remainingDuration = totalDuration;
			finished = false;
			started = false;
			running = false;

            if (!running)
            {
                if (OnTransitionStart != null)
                    OnTransitionStart(this, null);

                running = true;
                started = true;
            }
        }

        public void TriggerEnd()
        {
            if (running)
            {
				if (Finished != null)
					Finished ();

                if (OnTransitionEnd != null)
                    OnTransitionEnd(this, null);

                running = false;
                finished = true;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, screenArea, displayColor);
            spriteBatch.End();
        }

        private Texture2D texture;
        private Color startColor, endColor, displayColor;
        private Rectangle screenArea;
        private TimeSpan totalDuration, remainingDuration;
        private bool running = false, started = false, finished = false;
    }
}
