using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Framework
{
    public class NonSolidBody : Body
    {
        public NonSolidBody(Vector2 initialPosition = default(Vector2), Vector2 initialSize = default(Vector2))
            : base(initialPosition, initialSize)
        {
        }

        public override void Adjust(Body body)
        {
        }

        public override bool Collides(Body body)
        {
            return base.Collides(body);
        }
    }
}
