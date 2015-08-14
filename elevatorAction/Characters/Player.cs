using elevatorAction.MapElements;
using elevatorAction.MapStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Characters
{
    public class Player : Entity
    {
        public enum PlayerState // player
        {
            Walking,
            Jumping,
            Crouching,
            Falling,
            FallingHole,
            EnteringStairs,
            LeavingStairs
        }

        private readonly Vector2 PLAYER_SIZE_BASE = new Vector2(2, 3);
        private readonly Vector2 PLAYER_SIZE_CROUCHING = new Vector2(2, 2);
        private Texture2D _playerTexture;
        private BulletPool bulletPool;
        private PlayerState _currentState;

        public PlayerState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        public Player(Vector2 startPosition)
            : base(startPosition)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 position = Body.Position;

            spriteBatch.Draw(_playerTexture, position, Color.White * _opacity);
        }

        public override void Update(GameTime gameTime)
        {
            bulletPool.Update(gameTime);
            CheckIfShot();
            switch (_currentState)
            {
                case PlayerState.Walking:
                    Walking(gameTime);
                    Shoot(gameTime);
                    break;
                case PlayerState.Jumping:
                    Jumping(gameTime);
                    Shoot(gameTime);
                    break;
                case PlayerState.Falling:
                    Falling(gameTime);
                    Shoot(gameTime);
                    break;
                case PlayerState.FallingHole:
                    break;
                case PlayerState.EnteringStairs:
                    EnterStairs(gameTime);
                    break;
                case PlayerState.LeavingStairs:
                    LeaveStairs(gameTime);
                    break;
                default:
                    break;
            }
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

        private void BeginWalking()
        {
            _currentState = PlayerState.Walking;
        }

        private float _timeToVanish = 1f;
        private float _stairsTimer;
        private float _opacity = 1f;
        private Stairs _stairsToGo;
        private void BeginEnterStairs(Stairs stairs)
        {
            _stairsToGo = stairs;
            _stairsTimer = 0;
            _currentState = PlayerState.EnteringStairs;
        }

        private void EnterStairs(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _opacity = MathHelper.Lerp(1, 0, _stairsTimer / _timeToVanish);
            _stairsTimer += deltaTime;
            if (_stairsTimer >= _timeToVanish)
            {
                BeginLeaveStairs();
            }
        }

        private void BeginLeaveStairs()
        {
            _stairsTimer = 0;
            _currentState = PlayerState.LeavingStairs;
            _stairsToGo.Go(Body);
        }

        private void LeaveStairs(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _opacity = MathHelper.Lerp(0, 1, _stairsTimer / _timeToVanish);
            _stairsTimer += deltaTime;
            if (_stairsTimer >= _timeToVanish)
            {
                BeginWalking();
            }
        }

        private void Walking(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Input.IsKeyDown(Keys.Down) || Input.IsKeyDown(Keys.Up))
            {
                var collidesWith = MoveTo(Body.Position + Vector2.UnitY * 8f);
                Elevator elevator = collidesWith.FirstOrDefault(entity => entity is Elevator) as Elevator;
                if (elevator != null && collidesWith.Count == 1)
                {
                    if (Input.IsKeyDown(Keys.Down))
                    {
                        elevator.MoveDown();
                    }
                    else
                    {
                        elevator.MoveUp();
                    }
                }
            }
            else if (Input.KeyWasPressed(Keys.Up))
            {
                var collidesWith = MoveTo(Body.Position + Vector2.UnitY * 8f);
                Stairs stairs = collidesWith.FirstOrDefault(entity => entity is Stairs) as Stairs;
                if (stairs != null)
                {
                    BeginEnterStairs(stairs);
                }
                else
                {
                    BeginJumping(gameTime);
                }
            }
            else
            {
                Body.Orientation = Vector2.Zero;

                if (Input.IsKeyDown(Keys.Right))
                {
                    Body.Orientation += Vector2.UnitX;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Body.Orientation -= Vector2.UnitX;
                }
                Body.Orientation += Vector2.UnitY; // gravity

                Vector2 tentativePosition = Body.Position;
                tentativePosition += Body.Orientation * deltaTime * Map.Instance.CellSize * new Vector2(8f, 8f);
                List<Entity> collidesWith = MoveTo(tentativePosition);
                // TODO: this could totally work with separate elevator entities and collision boxes
                //var collided = collidesWith.Where(entity => entity is Floor || entity is Elevator).ToList();
                //var collisionVectors = collided.Select(entity => entity.Body.CollisionRectangle.Center.ToVector2() - Body.CollisionRectangle.Center.ToVector2()).ToList();
                //var collisionVectorSigns = collisionVectors.Select(vector => Math.Sign(vector.Y)).ToList();
                //bool squished = collisionVectorSigns.Distinct().ToList().Count > 1;
                //if (squished)
                //{
                //    Dead = true;
                //}
            }
        }

        private float _elapsedJumpTime;
        private void BeginJumping(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _currentState = PlayerState.Jumping;
            _elapsedJumpTime = 0;
            Body.Orientation = (Body.Orientation * Vector2.UnitX) + new Vector2(0, -1);
            Jumping(gameTime, true);
            _elapsedJumpTime -= deltaTime; // do not count first frame
        }

        private void Jumping(GameTime gameTime, bool started = false)
        {
            if (Input.KeyWasPressed(Keys.Up) && ElevatorAction.Kerbal && !started)
            {
                BeginJumping(gameTime);
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 tentativePosition = Body.Position;
            tentativePosition += Body.Orientation * deltaTime * Map.Instance.CellSize * new Vector2(3f, 2.4f);
            var collided = MoveTo(tentativePosition);
            if (collided.Any(entity => entity is Wall))
            {
                Body.Orientation *= Vector2.UnitY;
            }
            _elapsedJumpTime += deltaTime;
            if (_elapsedJumpTime >= 0.5)
            {
                BeginFalling(gameTime);
            }
        }

        private void BeginFalling(GameTime gameTime)
        {
            _currentState = PlayerState.Falling;
            Body.Orientation = (Body.Orientation * Vector2.UnitX) + new Vector2(0, 1);
            Falling(gameTime);
        }

        private void Falling(GameTime gameTime)
        {
            if (Input.KeyWasPressed(Keys.Up) && ElevatorAction.Kerbal)
            {
                BeginJumping(gameTime);
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 tentativePosition = Body.Position;
            tentativePosition += Body.Orientation * deltaTime * Map.Instance.CellSize * new Vector2(3f, 2.4f);
            List<Entity> collidedWith = MoveTo(tentativePosition);
            bool grounded = collidedWith.Any(entity => entity is Floor || entity is Elevator);
            bool walled = collidedWith.Any(entity => entity is Wall);
            if (grounded)
            {
                BeginWalking();
            }
            if (walled)
            {
                Body.Orientation *= Vector2.UnitY;
            }
        }

        private void Shoot(GameTime gameTime)
        {
            if (Input.KeyWasPressed(Keys.Space) && Body.LastActiveOrientation.X != 0)
            {
                bulletPool.CreateBullet();
            }
        }

        private List<Entity> MoveTo(Vector2 tentativePosition)
        {
            Body.Position = tentativePosition;
            var wallsAndFloors = Map.Instance.Camera.Entities.Where(entity => entity is Wall || entity is Floor || entity is Elevator || entity is Stairs);
            var collidesWith = wallsAndFloors.Where(entity => entity.Body.Collides(Body)).ToList();
            var collidingBodies = collidesWith.Select(e => e.Body).ToList();

            collidingBodies.ForEach(e => e.Adjust(Body));
            return collidesWith.ToList();
        }

        public override void Initialize(Game game)
        {
            _initialSize = PLAYER_SIZE_BASE * Map.Instance.CellSize;
            base.Initialize(game);
            BeginWalking();
            _playerTexture = new Texture2D(game.GraphicsDevice, (int)Body.Size.X, (int)Body.Size.Y);
            _playerTexture.SetData(Enumerable.Repeat(Color.White, (int)Body.Size.X * (int)Body.Size.Y).ToArray());
            bulletPool = new BulletPool(game, this, 3);
            Body.Orientation = Vector2.One;
        }
    }
}
