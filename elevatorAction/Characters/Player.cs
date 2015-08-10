using elevatorAction.MapElements;
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
        enum State // player
        {
            Walking,
            Jumping,
            Falling,
            FallingHole
        }

        private readonly Vector2 PLAYER_SIZE_BASE = new Vector2() { X = 2, Y = 3 };
        private Texture2D _playerTexture;
        private Texture2D _charTexture; // player
        private Vector2?[] _playerBullets = new Vector2?[3]; // player
        private State _currentState = State.Walking; // player
        private Vector2 _inputWhenJumpStarted; // player

        public Player(Vector2 startPosition)
            : base(startPosition)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 position = Body.Position;

            spriteBatch.Draw(_playerTexture, position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {

            /*
            Vector2 input = Vector2.Zero;
            Vector2 tentativePosition = playerPosition;
            switch (currentState)
            {
                case State.Walking:
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        playerDirection = Direction.Right;
                        input.X += 1;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        playerDirection = Direction.Left;
                        input.X -= 1;
                    }
                    input.Y += 1;
                    float mag = input.Length();
                    if (mag > 0)
                        input /= mag;
                    tentativePosition += input * deltaTime * cellSize * new Vector2(8, 0);
                    break;
                case State.Jumping:
                    input.X += inputWhenJumpStarted.X;
                    input.Y -= 1;
                    tentativePosition += input * deltaTime * cellSize * new Vector2(3f, 2.4f);
                    elapsedJumpTime += deltaTime;
                    if (elapsedJumpTime > 0.5)
                    {
                        currentState = State.Falling;
                    }
                    break;
                case State.FallingHole:
                    // add snap to hole
                    input.Y += 1;
                    tentativePosition += input * deltaTime * cellSize * new Vector2(3f, 2.4f);
                    elapsedJumpTime += deltaTime;
                    break;
                case State.Falling:
                    input.X += inputWhenJumpStarted.X;
                    input.Y += 1;
                    tentativePosition += input * deltaTime * cellSize * new Vector2(3f, 2.4f);
                    elapsedJumpTime += deltaTime;
                    break;
                default:
                    break;
            }
            Rectangle tentativeRect = new Rectangle(
                (int)tentativePosition.X,
                (int)tentativePosition.Y,
                (int)boundingBoxSize.X,
                (int)boundingBoxSize.Y
            );

            tentativePosition = CollisionWall2(tentativeRect) ? AdjustPosition2(tentativePosition, BoardElements.Wall) : tentativePosition;
            //tentativePosition = CollisionWall2(tentativeRect) ? AdjustPosition2(tentativePosition, BoardElements.Wall) : tentativePosition;
            tentativePosition = CollisionFloor2(tentativeRect) ? AdjustPosition2(tentativePosition, BoardElements.Floor) : tentativePosition;
            playerPosition = tentativePosition;

            bool grounded = CollisionFloor2(tentativeRect);
            if ((currentState == State.Falling || currentState == State.FallingHole) && grounded)
            {
                currentState = State.Walking;
            }
            if (currentState == State.Walking && !grounded)
            {
                inputWhenJumpStarted = new Vector2();
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputWhenJumpStarted.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputWhenJumpStarted.X -= 1;
                currentState = State.FallingHole;
            }
            //*/

            switch (_currentState)
            {
                case State.Walking:
                    Walking(gameTime);
                    break;
                case State.Jumping:
                    Jumping(gameTime);
                    break;
                case State.Falling:
                    break;
                case State.FallingHole:
                    break;
                default:
                    break;
            }
        }

        private void Walking(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //input
            if (Input.KeyWasPressed(Keys.Up))
            {
                BeginJumping(gameTime);
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
                tentativePosition += Body.Orientation * deltaTime * Map.Instance.CellSize * new Vector2(8, 8);
                MoveTo(tentativePosition);
            }
        }

        private void MoveTo(Vector2 tentativePosition)
        {
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(0,0));

            List<Entity> entities = Map.Instance.Entities;
            var collidesWith = entities.Where(x => x.Body.Collides(this.Body));
            var onlyWallsAndFloors = collidesWith.Where(x => x is Wall || x is Floor);
            var collidingBodies = onlyWallsAndFloors.Select(e => e.Body).ToList();
                
            collidingBodies.ForEach(e => e.Adjust(this.Body));
            
            points = points.Select(x => tentativePosition + (x * Map.Instance.CellSize)).ToList();

            Body.Position = tentativePosition;
        }

        private void BeginJumping(GameTime gameTime)
        {
            _currentState = State.Jumping;
        }

        private void Jumping(GameTime gameTime)
        {

        }

        public override void Initialize(Game game)
        {
            _initialSize = PLAYER_SIZE_BASE * Map.Instance.CellSize;
            base.Initialize(game);
            _playerTexture = new Texture2D(game.GraphicsDevice, (int)Body.Size.X, (int)Body.Size.Y);
            _playerTexture.SetData(Enumerable.Repeat(Color.White, (int)Body.Size.X * (int)Body.Size.Y).ToArray());
        }
    }
}
