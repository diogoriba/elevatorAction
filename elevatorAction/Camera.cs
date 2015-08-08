using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction
{
    public class Camera : Entity
    {
        private Map _map;

        public Camera()
            : base(new Vector2(0, 0),
                new Vector2(800, 600))
        {
        }

        public override void Initialize(Game game)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gametime)
        {
            //TODO: ADD CAMERA MOVE
        }
    }
}
