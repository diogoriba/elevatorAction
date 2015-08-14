using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction
{

    public class Body
    {
        protected Vector2 _position;
        public virtual Vector2 Position 
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        private Vector2 _size;
        public Vector2 Size 
        {
            get 
            { 
                return _size; 
            }
            set 
            { 
                _size = value; 
            }
        }
        private Vector2 orientation;
        public Vector2 Orientation 
        {
            get { return orientation; }
            set
            {
                if (orientation.X != 0)
                {
                    LastActiveOrientation = orientation;
                }
                else if (value.X != 0)
                {
                    LastActiveOrientation = value;
                }

                orientation = value;
            }
        }
        public Vector2 LastActiveOrientation { get; private set; }
        public Vector2 AdjustMask { get; set; }
        public bool CollisionsEnabled { get; set; }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public Body(Vector2 position = default(Vector2), Vector2 size = default(Vector2))
        {
            Position = position;
            Size = size;
            Orientation = Vector2.Zero;
            CollisionsEnabled = true;
        }

        public virtual bool Collides(Body body)
        {
            return this.CollisionRectangle.Intersects(body.CollisionRectangle) && CollisionsEnabled;
        }

        protected virtual Vector2 GetAdjustMask(Rectangle collisionBox)
        {
            return AdjustMask;
        }

        public virtual void Adjust(Body body)
        {
            Rectangle tileCollisionRectangle = CollisionRectangle;
            Rectangle collisionBox = Rectangle.Intersect(tileCollisionRectangle, body.CollisionRectangle);
            Vector2 collisionBoxCenter = new Vector2(collisionBox.Left + collisionBox.Width / 2, collisionBox.Top + collisionBox.Height / 2);
            Vector2 tileCenter = new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
            Vector2 direction = collisionBoxCenter - tileCenter;
            direction = new Vector2(Math.Sign(direction.X), Math.Sign(direction.Y));
            Vector2 magnitude = new Vector2(collisionBox.Width, collisionBox.Height);
            Vector2 force = direction * magnitude;
            body.Position += force * GetAdjustMask(collisionBox);
            if (!ElevatorAction.Jiggle)
            {
                body.Position = new Vector2((float)Math.Round(body.Position.X), (float)Math.Round(body.Position.Y)); // remove this line to add jiggling
            }
        }
    }
}
