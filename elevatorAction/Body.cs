using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction
{
    public class Body
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Orientation { get; set; }
        public Vector2 AdjustMask { get; set; }

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
        }

        public virtual bool Collides(Body body)
        {
            return this.CollisionRectangle.Intersects(body.CollisionRectangle);
        }

        public virtual void Adjust(Body body)
        {
            Rectangle tileCollisionRectangle = CollisionRectangle;
            Rectangle collisionBox = Rectangle.Intersect(tileCollisionRectangle, body.CollisionRectangle);
            Vector2 collisionBoxCenter = collisionBox.Center.ToVector2();
            Vector2 tileCenter = tileCollisionRectangle.Center.ToVector2();
            Vector2 direction = collisionBoxCenter - tileCenter;
            direction = new Vector2(Math.Sign(direction.X), Math.Sign(direction.Y));
            Vector2 magnitude = new Vector2(collisionBox.Width, collisionBox.Height);
            Vector2 force = direction * magnitude;
            body.Position += force * AdjustMask;
            body.Position = body.Position.ToPoint().ToVector2(); // remove this line to add jiggling
        }
    }
}
