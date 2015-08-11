using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.MapElements
{
    public class Wall : Entity
    {
        private Texture2D wallTexture;
        public Wall(Vector2 initialPosition) : base(initialPosition, Map.Instance.CellSize)
        {
        }

        public Wall(Vector2 initialPosition, Vector2 initialSize)
            : base(initialPosition, initialSize)
        {
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(wallTexture, new Rectangle(Body.Position.ToPoint(), Body.Size.ToPoint()), Color.White);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Body.AdjustMask = new Vector2(1, 0);

            wallTexture = new Texture2D(game.GraphicsDevice, (int)Map.Instance.CellSize.X, (int)Map.Instance.CellSize.Y);
            wallTexture.SetData(Enumerable.Repeat(Color.Navy, (int)Map.Instance.CellSize.X * (int)Map.Instance.CellSize.Y).ToArray());
        }
    }
}
