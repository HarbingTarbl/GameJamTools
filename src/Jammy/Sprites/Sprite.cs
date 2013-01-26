using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammy.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jammy.Sprites
{
    public class Sprite
    {
		public Sprite()
		{
			IsVisible = true;
			_scale = 1f;
			Color = Color.White;
			CollisionData = new Polygon();
		}

        public Vector2 Location;
        public Texture2D Texture;
        public Vector2 Origin;
        public Color Color;
	    public bool IsVisible;
       
        public CollisionDataType CollisionType;
        public Polygon CollisionData;
		
	    public float Scale
	    {
		    get { return _scale; }
		    set
		    {
			    CollisionData.Scale = value;
			    _scale = value;
		    }
	    }

	    public float Rotation
	    {
		    get { return _rotation;  }
			set
			{
				CollisionData.Rotation = value;
				_rotation = value;
			} 
		}

	    public virtual void Update (GameTime gameTime)
        {
	        CollisionData.Location = Location;
        }

        public virtual void Draw(SpriteBatch batch)
        {
	        if (!IsVisible) return;

            batch.Draw (Texture, Location, null, Color.White, Rotation,
				Origin, 1f, SpriteEffects.None, 0);
        }

	    private float _scale;
	    private float _rotation;
    }
}
