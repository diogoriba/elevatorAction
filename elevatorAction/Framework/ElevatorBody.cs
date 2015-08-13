using elevatorAction.MapElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Framework
{
    public class ElevatorBody : Body
    {
        private Body _elevatorTop, _elevatorBottom;

        private Texture2D _debugTexture;

        private float _distance; 

        public ElevatorBody(Vector2 position, Vector2 size, float distance)
        {
            var cellSize = Map.Instance.CellSize;

            _elevatorTop = new Body() { AdjustMask = Vector2.UnitY, Size = size };
            _elevatorBottom = new Body() { AdjustMask = Vector2.UnitY, Size = size };
            _distance = distance;
            Position = position;

            _debugTexture = new Texture2D(Map.Instance.game.GraphicsDevice, 1, 1);
            _debugTexture.SetData<Color>(new Color[] { Color.White });
        }

        public override Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (_elevatorTop != null)
                    _elevatorTop.Position = value;
                if (_elevatorBottom != null)
                    _elevatorBottom.Position = value + (_distance * Vector2.UnitY);

                _position = value;
            }
        }

        public override bool Collides(Body body)
        {
            if (_elevatorBottom.Collides(body))
                return true;

            return _elevatorTop.Collides(body);
        }

        public override void Adjust(Body body)
        {
            if (_elevatorBottom.Collides(body))
            {
                _elevatorBottom.Adjust(body);
            }
            if (_elevatorTop.Collides(body))
            {
                _elevatorTop.Adjust(body);
            }
        }

        internal void DebugDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_debugTexture, _elevatorTop.CollisionRectangle, Color.Black);
            spriteBatch.Draw(_debugTexture, _elevatorBottom.CollisionRectangle, Color.Black);
        }
    }
}
