using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Characters
{
    public class Enemy : Entity
    {
        private readonly Vector2 ENEMY_SIZE_BASE = new Vector2() { X = 2, Y = 3 };
        private Texture2D _enemyTexture;

        public Enemy(Vector2 initialPosition)
            : base(initialPosition)
        {

        }

        public override void Initialize(Game game)
        {
            _initialSize = ENEMY_SIZE_BASE * Map.Instance.CellSize;
            base.Initialize(game);
            _enemyTexture = new Texture2D(game.GraphicsDevice, (int)Body.Size.X, (int)Body.Size.Y);
            _enemyTexture.SetData(Enumerable.Repeat(Color.Black, (int)Body.Size.X * (int)Body.Size.Y).ToArray());
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_enemyTexture, Body.Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            var bullets = Map.Instance.Entities.Where(entity => entity is Bullet).Select(entity => entity as Bullet).ToList();
            var collidingBullets = bullets.Where(bullet => bullet.Body.Collides(Body) && bullet.Owner.GetType() != this.GetType()).ToList();
            if (collidingBullets.Count > 0)
            {
                Dead = true;
            }
        }
    }
}
