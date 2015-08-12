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
        private BulletPool bulletPool;

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
            bulletPool = new BulletPool(game, this, 1);
            Body.Orientation = Vector2.UnitX * -1;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_enemyTexture, Body.Position, Color.White);
        }

        float elapsedSeconds = 0;
        public override void Update(GameTime gameTime)
        {
            bulletPool.Update(gameTime);
            var bullets = Map.Instance.Entities.Where(entity => entity is Bullet).Select(entity => entity as Bullet).ToList();
            var collidingBullets = bullets.Where(bullet => bullet.Body.Collides(Body) && bullet.Owner.GetType() != this.GetType()).ToList();
            if (collidingBullets.Count > 0)
            {
                Dead = true;
            }

            elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedSeconds > 2)
            {
                elapsedSeconds = 0;
                bulletPool.CreateBullet();
            }
        }
    }
}
