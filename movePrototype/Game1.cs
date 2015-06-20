using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;

namespace MovePrototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D charTexture;
        Texture2D floorTexture;
        Texture2D holeTexture;
        Texture2D wallTexture;
        Vector2 playerPosition;
        Vector2 cellSize;
        Vector2 boundingBoxSize;
        int scale;
        bool walljump = false;
        bool jiggle = false;
        enum BoardElements
        {
            Floor,
            Hole,
            Wall
        }

        BoardElements[,] board;

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
            scale = 2;
            playerPosition = new Vector2(24, 0) * scale;
            cellSize = new Vector2(12, 48) * scale;
            boundingBoxSize = new Vector2(12, 24) * scale;
            board = new BoardElements[,] { 
                { BoardElements.Wall, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Hole, BoardElements.Hole, BoardElements.Floor },
                { BoardElements.Wall, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Floor, BoardElements.Wall }
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

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 input = Vector2.Zero;
            Vector2 tentativePosition = playerPosition;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                input.X += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                input.X -= 1;
            input.Y += 1;

            float mag = input.Length();
            if (mag > 0)
                input /= mag;

            bool caindo = !CollisionFloor(tentativePosition + new Vector2(boundingBoxSize.X / 2, boundingBoxSize.Y));

            tentativePosition += input * deltaTime * cellSize.X * 5;
            tentativePosition = Collision(tentativePosition) ? AdjustPosition(tentativePosition) : tentativePosition;
            tentativePosition = Collision(tentativePosition + boundingBoxSize) ? AdjustPosition(tentativePosition) : tentativePosition;
            tentativePosition = CollisionFloor(tentativePosition + new Vector2(boundingBoxSize.X / 2, boundingBoxSize.Y)) ? AdjustPositionFloor(tentativePosition) : tentativePosition;
            playerPosition = tentativePosition;

            base.Update(gameTime);
        }

        private bool CollisionFloor(Vector2 point)
        {
            Vector2 boardPosition = playerPosition / cellSize;
            if (board[(int)boardPosition.Y, (int)boardPosition.X] == BoardElements.Floor)
            {
                if (point.Y > ((int)boardPosition.Y * cellSize.Y) + cellSize.Y - 3)
                {
                    return true;
                }
            }
            return false;
        }

        private Vector2 AdjustPositionFloor(Vector2 point)
        {
            int bounce = 3;
            if (jiggle) bounce += 3;
            Vector2 boardPosition = playerPosition / cellSize;
            return new Vector2(point.X, ((int)boardPosition.Y * cellSize.Y) + cellSize.Y - boundingBoxSize.Y - bounce);
        }

        private bool Collision(Vector2 point)
        {
            Vector2 boardPosition = point / cellSize;
            if(!CollisionFloor(point + new Vector2(boundingBoxSize.X / 2, boundingBoxSize.Y)) && !jiggle && !walljump)
            {
                //int posy = ((int)boardPosition.Y - 1 < 0) ? (int)boardPosition.Y : (int)boardPosition.Y - 1;

                //if (board[posy, (int)boardPosition.X] != BoardElements.Hole)
                //{
                //    return true;
                //}
                return true;
            }
            if (board[(int)boardPosition.Y, (int)boardPosition.X] == BoardElements.Wall)
            {
                return true;
            }
            return false;
        }

        private Vector2 AdjustPosition(Vector2 point)
        {
            Vector2 pointToUse;
            if (walljump)
            {
                pointToUse = playerPosition;
            }
            else
            {
                pointToUse = point;
            }
            Vector2 boardPosition = (playerPosition + cellSize/2) / cellSize;
            return new Vector2(((int)boardPosition.X) * cellSize.X, pointToUse.Y);
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
                            textureToDraw = floorTexture;
                            floor = true;
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
                    spriteBatch.Draw(textureToDraw, new Rectangle(column * (int)cellSize.X, line * (int)cellSize.Y, (int)cellSize.X, (int)cellSize.Y), Color.White);
                    if (floor)
                        spriteBatch.Draw(wallTexture, new Rectangle(column * (int)cellSize.X, line * (int)cellSize.Y + ((int)cellSize.Y - 3), (int)cellSize.X, 3), Color.White);
                }
            }

            spriteBatch.Draw(charTexture, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, (int)boundingBoxSize.X, (int)boundingBoxSize.Y), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
