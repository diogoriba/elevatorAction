using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction
{
    public interface ICanDraw
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
