using elevatorAction.Framework;
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
        public enum EnemyState
        {
            Wandering,
            Attacking,
            AttackRecovery
        }
        private readonly Vector2 ENEMY_SIZE_BASE = new Vector2() { X = 2, Y = 3 };
        private Texture2D _enemyTexture;
        private BulletPool _bulletPool;
        private float _attackTimer;
        private const float _timeToAttack = 0.5f;
        private const float _timeToAttackRecovery = 1f;
        private const float _timeToAttackCooldown = 1f;

        private readonly Vector2 SIGHT_RECTANGLE_SIZE = new Vector2(10, 5);

        public NonSolidBody SightRectangle { get; private set; }
        public EnemyState CurrentState { get; private set; }

        public Enemy(Vector2 initialPosition)
            : base(initialPosition)
        {

        }

        public override void Initialize(Game game)
        {
            _initialSize = ENEMY_SIZE_BASE * Map.Instance.CellSize;
            base.Initialize(game);
            BeginWandering();
            _enemyTexture = new Texture2D(game.GraphicsDevice, (int)Body.Size.X, (int)Body.Size.Y);
            _enemyTexture.SetData(Enumerable.Repeat(Color.Black, (int)Body.Size.X * (int)Body.Size.Y).ToArray());
            _bulletPool = new BulletPool(game, this, 1);
            Body.Orientation = Vector2.UnitX * -1;
            SightRectangle = new NonSolidBody(Vector2.Zero, SIGHT_RECTANGLE_SIZE * Map.Instance.CellSize);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_enemyTexture, Body.Position, Color.White);
            spriteBatch.Draw(_enemyTexture, new Rectangle((int)SightRectangle.Position.X, (int)SightRectangle.Position.Y, (int)SightRectangle.Size.X, (int)SightRectangle.Size.Y), Color.White * 0.5f);
        }

        public override void Update(GameTime gameTime)
        {
            _bulletPool.Update(gameTime);
            CheckIfShot();
            switch (CurrentState)
            {
                case EnemyState.Wandering:
                    Wandering(gameTime);
                    break;
                case EnemyState.Attacking:
                    Attacking(gameTime);
                    break;
                case EnemyState.AttackRecovery:
                    AttackRecovery(gameTime);
                    break;
                default:
                    break;
            }
        }

        private void UpdateSightRectangle()
        {
            Vector2 eyeLevel = Body.Position + ((Body.Size * Vector2.UnitX) / 2) + ((Map.Instance.CellSize * Vector2.UnitY) / 2);
            SightRectangle.Position = new Vector2(eyeLevel.X, eyeLevel.Y - (SightRectangle.Size.Y / 2));
            if (Body.Orientation.X < 0)
            {
                SightRectangle.Position -= SightRectangle.Size * Vector2.UnitX;
            }
        }

        private void BeginWandering()
        {
            _attackTimer = 0;
            CurrentState = EnemyState.Wandering;
        }

        private void Wandering(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateSightRectangle();
            bool seesPlayer = Map.Instance.Player.Body.Collides(SightRectangle);
            if (seesPlayer && _attackTimer >= _timeToAttackCooldown)
            {
                BeginAttacking();
            }
            _attackTimer = Math.Min(_attackTimer + deltaTime, _timeToAttackCooldown);
        }

        private void BeginAttacking()
        {
            _attackTimer = 0;
            CurrentState = EnemyState.Attacking;
        }

        private void Attacking(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_attackTimer >= _timeToAttack)
            {
                _bulletPool.CreateBullet();
                BeginAttackRecovery();
            }
            _attackTimer += deltaTime;
        }

        private void BeginAttackRecovery()
        {
            _attackTimer = 0;
            CurrentState = EnemyState.AttackRecovery;
        }

        private void AttackRecovery(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_attackTimer >= _timeToAttackRecovery)
            {
                BeginWandering();
            }
            _attackTimer += deltaTime;
        }

        private void CheckIfShot()
        {
            var bullets = Map.Instance.Camera.Entities.Where(entity => entity is Bullet).Select(entity => entity as Bullet).ToList();
            var collidingBullets = bullets.Where(bullet => bullet.Body.Collides(Body) && bullet.Owner.GetType() != this.GetType()).ToList();
            if (collidingBullets.Count > 0)
            {
                Dead = true;
            }
        }
    }
}
