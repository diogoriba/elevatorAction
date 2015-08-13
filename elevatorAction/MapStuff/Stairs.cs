using elevatorAction.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.MapElements
{
    public class Stairs : Entity
    {
        private Texture2D debugTexture;
        public string Name { get; private set; }
        private string _to;
        public Stairs(string name, string to, Vector2 initialPosition = default(Vector2), Vector2 initialSize = default(Vector2))
            : base(initialPosition, initialSize)
        {
            Name = name;
            _to = to;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(debugTexture, new Rectangle((int)Body.Position.X, (int)Body.Position.Y, (int)Body.Size.X, (int)Body.Size.Y), Color.White);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public void Go(Body body)
        {
            Stairs destination = Map.Instance.Camera.Entities.FindAll(entity => entity is Stairs).Select(entity => entity as Stairs).Where(stairs => stairs.Name == _to).First();
            body.Position = destination.Body.Position;
        }

        public override void Initialize(Microsoft.Xna.Framework.Game game)
        {
            base.Initialize(game);
            Body = new NonSolidBody(_initialPosition, _initialSize);

            debugTexture = new Texture2D(game.GraphicsDevice, (int)Map.Instance.CellSize.X, (int)Map.Instance.CellSize.Y);
            debugTexture.SetData(Enumerable.Repeat(Color.Orange, (int)Map.Instance.CellSize.X * (int)Map.Instance.CellSize.Y).ToArray());
        }
    }
}
