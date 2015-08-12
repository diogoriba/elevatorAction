using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Framework
{
    public class ElevatorBody : Body
    {
        private Body _elevatorTop, _elevatorBottom;

        public ElevatorBody(Vector2 position, Vector2 size)
        {
            var cellSize = Map.Instance.CellSize;

            _elevatorTop = new Body(size: size);
            _elevatorBottom = new Body(size: size);
            Position = _position;
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                if (_elevatorTop != null)
                    _elevatorTop.Position = value;
                if (_elevatorBottom != null)
                    _elevatorBottom.Position = value + Map.Instance.CellSize;

                base.Position = value;
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
    }
}
