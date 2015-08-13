using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.MapElements
{
    public class FloorBody : Body
    {
        public override void Adjust(Body body)
        {
            Rectangle tileCollisionRectangle = CollisionRectangle;
            Rectangle collisionBox = Rectangle.Intersect(tileCollisionRectangle, body.CollisionRectangle);
            bool flatAngle = collisionBox.Width >= collisionBox.Height;
            if (flatAngle)
            {
                AdjustMask = Vector2.UnitY;
            }
            else
            {
                AdjustMask = Vector2.UnitX;
            }
            base.Adjust(body);
        }
    }

    public class Floor : Entity
    {
        private Texture2D floorTexture;

        public Floor(Vector2 initialPosition)
            : base(initialPosition, Map.Instance.CellSize)
        {
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(floorTexture, new Rectangle(Body.Position.ToPoint().X, Body.Position.ToPoint().Y, Body.Size.ToPoint().X, Body.Size.ToPoint().Y), Color.White);
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
