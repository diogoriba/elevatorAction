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
            Vector2 boardPosition = body.Position / Map.Instance.CellSize;
            Vector2 tileTopLeft = this.Position;
            Vector2 tileCenter = tileTopLeft + (this.Size / 2);
            Vector2 difference = tileCenter - body.Position;
            difference.Normalize();

            Vector2 reverse = new Vector2(AdjustMask.Y, AdjustMask.X);
            Vector2 adjustedPosition = new Vector2(tileCenter.X + (Map.Instance.CellSize.X / 2) * Math.Sign(difference.X) * -1, tileCenter.Y + (Map.Instance.CellSize.Y / 2) * Math.Sign(difference.Y) * -1);
            Vector2 movement = adjustedPosition * AdjustMask + body.Position * reverse;
            body.Position = movement;
            // this is actually snapping the top-left coordinate of the body being adjusted to this position
            // we need to change this into a force that is applied back in the body, maybe
        }
    }
}
