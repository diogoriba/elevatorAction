using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Graphics
{
    public class TestAnimation :  Entity
    {
        private Animation _animation;

        public TestAnimation()
            :base(new Vector2(100,100))
        {
            _animation = AnimationFactory.TestAnimation();
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture, Body.Position, _animation.Color);
        }

        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
        }
    }
}
