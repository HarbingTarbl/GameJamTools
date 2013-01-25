using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy
{
    public class CameraCoop
    {
        public List<Sprite> ViewTargets = new List<Sprite>();
        public bool UseBounds;
        public Size MinimumSize;
        public int InflateAmount = 100;
        public Rectangle PlayerFrustrum;
        public Rectangle ViewFrustrum;

        public Vector2 ScreenToWorld (Vector2 screenVector)
        {
            return new Vector2 (
                (screenVector.X / zoom) + position.X,
                (screenVector.Y / zoom) + position.Y);
        }

        public Vector2 WorldToScreen(Vector2 WorldPos)
        {
            return Vector2.Transform (WorldPos, GetTransformation());
        }


        public Matrix GetTransformation()
        {
            return Matrix.CreateTranslation (new Vector3 (-position.X, -position.Y, 0)) *
                   Matrix.CreateScale (new Vector3 (zoom, zoom, 0));
        }

        public void Update(GraphicsDevice d)
        {
            var worldAtZero = ScreenToWorld(Vector2.Zero);
            var worldAtView = ScreenToWorld(new Vector2(d.Viewport.Width, d.Viewport.Height));
            
            ViewFrustrum = new Rectangle(
                (int)Math.Floor(worldAtZero.X),
                (int)Math.Floor(worldAtZero.Y),
                (int)Math.Ceiling(worldAtView.X - worldAtZero.X),
                (int)Math.Ceiling(worldAtView.Y - worldAtZero.Y));

            ViewFrustrum.Inflate(ZOOM_PADDING, ZOOM_PADDING);

            if (ViewTargets.Count > 0)
            {
                Vector2 min = ViewTargets[0].Location;
                Vector2 max = ViewTargets[0].Location;

                for (int i = 1; i < ViewTargets.Count; i++)
                {
                    if (ViewTargets[i].Location.X < min.X) min.X = ViewTargets[i].Location.X;
                    else if (ViewTargets[i].Location.X > max.X) max.X = ViewTargets[i].Location.X;
                    if (ViewTargets[i].Location.Y < min.Y) min.Y = ViewTargets[i].Location.Y;
                    else if (ViewTargets[i].Location.Y > max.Y) max.Y = ViewTargets[i].Location.Y;
                }

                Rectangle rect = new Rectangle((int)min.X, (int)min.Y,
                    (int)(max.X - min.X), (int)(max.Y - min.Y));

                if (UseBounds)
                {
                    if (rect.Width < MinimumSize.Width)
                        rect.Inflate((int)(MinimumSize.Width - rect.Width) / 2, 0);

                    if (rect.Height < MinimumSize.Height)
                        rect.Inflate(0, (int)(MinimumSize.Height - rect.Height) / 2);
                }

                rect.Inflate(InflateAmount, InflateAmount);
                PlayerFrustrum = rect;

                positionDesired = new Vector2(rect.X, rect.Y);

                float widthdiff = ((float)d.Viewport.Width) / ((float)rect.Width);
                float heightdiff = ((float)d.Viewport.Height) / ((float)rect.Height);
                zoomDesired = Math.Min(widthdiff, heightdiff);

            }

            position = Vector2.Lerp(position, positionDesired, 0.1f);
            zoom = MathHelper.Lerp(zoom, zoomDesired, 0.1f);
        }

        private Vector2 position = Vector2.Zero;
        private Vector2 positionDesired = Vector2.Zero;
        private float zoom = 1;
        private float zoomDesired = 1;
        private const int ZOOM_PADDING = 150;
    }
}
