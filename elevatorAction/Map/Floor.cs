using Microsoft.Xna.Framework;
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
            protected override Vector2 GetAdjustMask(Rectangle collisionBox)
            {
                Vector2 adjustMask = Vector2.Zero;
                if (collisionBox.Width >= collisionBox.Height)
                {
                    adjustMask = Vector2.UnitY;
                }
                else
                {
                    adjustMask = Vector2.UnitX;
                }
                return adjustMask;
            }
        }

        public Floor(Vector2 initialPosition) : base(initialPosition, Map.Instance.CellSize)
        {
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        public override void Initialize(Game game)
        {
            Body = new FloorBody();
            Body.Position = _initialPosition;
            Body.Size = _initialSize;
        }
    }
}
