using elevatorAction.Characters;
using elevatorAction.Framework;
using elevatorAction.Graphics;
using elevatorAction.MapElements;
using elevatorAction.MapStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace elevatorAction
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ElevatorAction : Game
    {
        public static bool Jiggle = true;
        public static bool Kerbal = false;
        public static bool Voxels = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;

        public ElevatorAction()
        {
            graphics = new GraphicsDeviceManager(this);
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
            //*
            TexturePool.Instance.Game = this;
            TexturePool.Instance.LoadAll();

            map = Map.Instance;
            map.Entities.Clear();

            Vector2 startPosition = new Vector2(5, 2);
            //startPosition = new Vector2(14, 151);

            Player player = new Player(startPosition * map.CellSize);
            Enemy enemy = new Enemy(new Vector2(18, 4) * map.CellSize);
            map.Camera = new Camera();
            map.Player = player;

            
            map.Entities.Add(player);
            map.Entities.Add(map.Camera);
            map.Entities.Add(enemy);

            //DEBUG ELEVATOR
            //map.Entities.Add(new Elevator(new Point(2, 7), 2));

            map.Entities.Add(new Elevator(new Point(14, 1), 11));

            map.Entities.Add(new Elevator(new Point(14, 79), 2));

            map.Entities.Add(new Elevator(new Point(2, 91), 2));

            map.Entities.Add(new Elevator(new Point(26, 103), 6));

            map.Entities.Add(new Elevator(new Point(14, 109), 2));

            map.Entities.Add(new Elevator(new Point(2, 115), 4));

            map.Entities.Add(new Elevator(new Point(10, 139), 6));
            map.Entities.Add(new Elevator(new Point(18, 139), 6));
            map.Entities.Add(new Elevator(new Point(22, 139), 7));

            map.Entities.Add(new Elevator(new Point(6, 145), 5));

            map.Entities.Add(new Elevator(new Point(14, 151), 4));
            

            map.Initialize(this);

            //player.Body.Position = new Vector2(412, 1968);

            base.Initialize();
            //*/
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here
            Input.Update();
            map.Update(gameTime);

            if (Input.KeyWasPressed(Keys.D1))
            {
                Initialize();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Map.Instance.Camera.TransformMatrix);

            map.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
