using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/*
 * REGARDING CODE QUALITY:
 * ABANDON ALL HOPE YE WHO ENTERS HERE
 */
namespace MovePrototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        enum State // player
        {
            Walking,
            Jumping,
            Falling,
            FallingHole
        }

        enum Direction // orientable
        {
            Left = -1,
            Right = 1
        }
        GraphicsDeviceManager graphics; // game
        SpriteBatch spriteBatch; // game
        Texture2D charTexture; // player
        Texture2D floorTexture; // camera
        Texture2D holeTexture; // camera
        Texture2D wallTexture; // camera
        Texture2D bulletTexture; // bullet
        Vector2 playerPosition; // player
        Vector2 cellSize; // map
        Vector2 boundingBoxSize; // player
        Vector2?[] playerBullets = new Vector2?[3]; // player
        Direction?[] playerBulletsDirections = new Direction?[3]; // bullet
        Direction playerDirection = Direction.Right; // orientable
        KeyboardState previousKeyboardState; // game
        State currentState = State.Walking; // player
        Vector2 inputWhenJumpStarted; // player
        float elapsedJumpTime = 0f; // player
        int scale; // body
        bool kerbal = true; // player
        enum BoardElements // map
        {
            Empty,
            Floor,
            Hole,
            Wall
        }

        BoardElements[,] board; // map

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            scale = 3;
            currentState = State.Falling;
            cellSize = new Vector2(8, 8) * scale;
            playerPosition = new Vector2(2, 0) * cellSize;
            boundingBoxSize = new Vector2(2, 3) * cellSize;
            board = new BoardElements[,] { 
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole,      BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole,      BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole,      BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole,      BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole,      BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole, BoardElements.Hole,      BoardElements.Floor,  BoardElements.Floor,  BoardElements.Floor,  BoardElements.Floor, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty, BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty,  BoardElements.Empty, BoardElements.Wall, BoardElements.Wall },
                { BoardElements.Wall, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor,  BoardElements.Floor,  BoardElements.Floor,  BoardElements.Floor,  BoardElements.Floor, BoardElements.Wall, BoardElements.Wall },
            };

            base.Initialize();                                        
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            charTexture = new Texture2D(GraphicsDevice, (int)cellSize.X, (int)cellSize.Y);
            charTexture.SetData(Enumerable.Repeat(Color.White, (int)cellSize.X * (int)cellSize.Y).ToArray());
            floorTexture = new Texture2D(GraphicsDevice, (int)cellSize.X, (int)cellSize.Y);
            Color[] floorColors = Enumerable.Repeat(Color.Blue, (int)cellSize.X * (int)cellSize.Y).ToArray();
            floorTexture.SetData(floorColors);
            holeTexture = new Texture2D(GraphicsDevice, (int)cellSize.X, (int)cellSize.Y);
            holeTexture.SetData(Enumerable.Repeat(Color.Gray, (int)cellSize.X * (int)cellSize.Y).ToArray());
            wallTexture = new Texture2D(GraphicsDevice, (int)cellSize.X, (int)cellSize.Y);
            wallTexture.SetData(Enumerable.Repeat(Color.Navy, (int)cellSize.X * (int)cellSize.Y).ToArray());
            bulletTexture = new Texture2D(GraphicsDevice, 1, 1);
            bulletTexture.SetData(new Color[] { Color.Yellow });
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                playerPosition = new Vector2(16, 0) * scale;
                currentState = State.Falling;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 input = Vector2.Zero;
            Vector2 tentativePosition = playerPosition;
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
            {
                for (int i = 0; i < playerBullets.Length; i++)
                {
                    if (playerBullets[i] == null)
                    {
                        playerBullets[i] = playerPosition + boundingBoxSize / 2;
                        playerBulletsDirections[i] = playerDirection;
                        break;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up) && (currentState == State.Walking || kerbal))
            {
                currentState = State.Jumping;
                inputWhenJumpStarted = new Vector2();
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputWhenJumpStarted.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputWhenJumpStarted.X -= 1;
                elapsedJumpTime = 0f;
            }

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
                    tentativePosition += input * deltaTime * cellSize * new Vector2(8, 8);
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

            for (int i = 0; i < playerBullets.Length; i++)
            {
                if (playerBullets[i] != null)
                {
                    playerBullets[i] = new Vector2(playerBullets[i].Value.X + ((int)playerBulletsDirections[i].Value * cellSize.X * 12 * deltaTime), playerBullets[i].Value.Y);
                    bool collides = CollisionWall2(new Rectangle((int)playerBullets[i].Value.X, (int)playerBullets[i].Value.Y, 6, 2));
                    if (collides)
                    {
                        playerBullets[i] = null;
                    }
                }
            }

            base.Update(gameTime);
            previousKeyboardState = Keyboard.GetState();
        }

        private Vector2 AdjustPosition2(Vector2 point, BoardElements element)
        {
            Vector2 boardPosition = playerPosition / cellSize;
            Vector2 tileTopLeft = boardPosition.ToPoint().ToVector2() * cellSize;
            Vector2 tileCenter = tileTopLeft + (cellSize / 2);
            Vector2 difference = tileCenter - point;
            difference.Normalize();

            if(element == BoardElements.Floor)
            {
                return new Vector2(point.X, tileCenter.Y + (cellSize.Y / 2) * Math.Sign(difference.Y) * -1);
            }
            if(element == BoardElements.Wall)
            {
                return new Vector2(tileCenter.X + (cellSize.X / 2) * Math.Sign(difference.X) * -1, point.Y);
            }

            return point;
        }

        private bool CollisionWall2(Rectangle obj)
        {
            List<Point> points = new List<Point>();
            Point topLeft = new Point(obj.X, obj.Y);
            Point topRight = new Point(obj.X + obj.Width, obj.Y);
            points.Add(topLeft);
            points.Add(topRight);

            bool collidesWall = points.Select(x => CollisionPoint(x)).Any(x => x == BoardElements.Wall);

            return collidesWall;
                
        }

        private bool CollisionFloor2(Rectangle obj)
        {
            Point bottomLeft = new Point(obj.X, obj.Y + obj.Height);
            Point bottomRight = new Point(obj.X + obj.Width, obj.Y + obj.Height);
            bool collided = CollisionPoint(bottomLeft) == BoardElements.Floor || CollisionPoint(bottomRight) == BoardElements.Floor;
            return collided;
        }

        private BoardElements CollisionPoint(Point obj)
        {
            Point boardPosition = new Point(obj.X / (int)cellSize.X, obj.Y / (int)cellSize.Y);
            BoardElements elementAt = board[boardPosition.Y, boardPosition.X];
            return elementAt;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            for (int line = 0; line < board.GetLength(0); line++)
            {
                for (int column = 0; column < board.GetLength(1); column++)
                {
                    Texture2D textureToDraw = null;
                    bool floor = false;
                    switch (board[line, column])
                    {
                        case BoardElements.Floor:
                            textureToDraw = wallTexture;
                            floor = true;
                            break;
                        case BoardElements.Empty:
                            textureToDraw = floorTexture;
                            floor = false;
                            break;
                        case BoardElements.Hole:
                            textureToDraw = holeTexture;
                            break;
                        case BoardElements.Wall:
                            textureToDraw = wallTexture;
                            break;
                        default:
                            break;
                    }
                    spriteBatch.Draw(textureToDraw, new Rectangle(column * ((int)cellSize.X), line * (int)cellSize.Y, (int)cellSize.X, (int)cellSize.Y), Color.White);
                    if (floor)
                        spriteBatch.Draw(wallTexture, new Rectangle(column * ((int)cellSize.X), line * (int)cellSize.Y + ((int)cellSize.Y - 3), (int)cellSize.X, 3), Color.White);
                }
            }

            for (int i = 0; i < playerBullets.Length; i++)
            {
                if (playerBullets[i] != null) 
                {
                    spriteBatch.Draw(bulletTexture, new Rectangle((int)playerBullets[i].Value.X - 3, (int)playerBullets[i].Value.Y - 1, 6, 2), Color.White);
                }
            }

                spriteBatch.Draw(charTexture, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, (int)boundingBoxSize.X, (int)boundingBoxSize.Y), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
