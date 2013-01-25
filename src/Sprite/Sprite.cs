using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy
{
    public class Sprite
    {
		public Sprite()
		{
			Scale = 1f;
			Color = Color.White;
		}

        public Vector2 Location;
        public float Rotation;
        public float Scale;
        public Texture2D Texture;
        public Vector2 Origin;
        public Color Color;
        
        public CollisionDataType CollisionType;
        public object CollisionData;

        public virtual void Update (GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw (Texture, Location, null, Color.White);//, Rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
