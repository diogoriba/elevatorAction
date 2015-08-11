using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.MapElements
{
    public class Floor : Entity
    {
        private class FloorBody : Body
        {
            public override bool Collides(Body body)
            {
                bool collides = base.Collides(body);
                Rectangle collisionBox = Rectangle.Intersect(body.CollisionRectangle, CollisionRectangle);
                bool flatAngle = collisionBox.Width >= collisionBox.Height;
                Vector2 collisionBoxCenter = new Vector2(collisionBox.Left + collisionBox.Width / 2, collisionBox.Top + collisionBox.Height / 2);
                Vector2 tileCenter = new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
                Vector2 direction = collisionBoxCenter - tileCenter;
                direction = new Vector2(Math.Sign(direction.X), Math.Sign(direction.Y));
                bool fromAbove = direction.Y < 0;
                return collides && flatAngle && fromAbove;
            }
        }

        private Texture2D floorTexture;

        public Floor(Vector2 initialPosition) : base(initialPosition, Map.Instance.CellSize)
        {
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(floorTexture, new Rectangle(Body.Position.ToPoint(), Body.Size.ToPoint()), Color.White);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        public override void Initialize(Game game)
        {
            Body = new FloorBody();
            Body.Position = _initialPosition;
            Body.Size = _initialSize;
            Body.AdjustMask = new Vector2(0, 1);

            floorTexture = new Texture2D(game.GraphicsDevice, (int)Map.Instance.CellSize.X, (int)Map.Instance.CellSize.Y);
            floorTexture.SetData(Enumerable.Repeat(Color.Navy, (int)Map.Instance.CellSize.X * (int)Map.Instance.CellSize.Y).ToArray());
        }
    }
}
