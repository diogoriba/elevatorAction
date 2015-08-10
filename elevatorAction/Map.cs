using elevatorAction.MapElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction
{
    public enum MapElement
    {
        Empty,
        Floor,
        Hole,
        Wall
    }

    public class Map : ICanDraw, ICanUpdate
    {
        private static Map instance = new Map();
        public static Map Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Map();
                }
                return instance;
            }
        }

        public const int CELL_SIZE = 8;
        public const int SCALE = 3;

        public MapElement[,] Grid { get; private set; }
        public List<Entity> Entities { get; private set; }
        public Vector2 CellSize { get; private set; }

        private Texture2D wallTexture;
        private Texture2D floorTexture;
        private Texture2D holeTexture;
        private Game game;

        private Map()
        {
            CellSize = new Vector2(CELL_SIZE * SCALE, CELL_SIZE * SCALE);
            Entities = new List<Entity>();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int line = 0; line < this.Grid.GetLength(0); line++)
            {
                for (int column = 0; column < this.Grid.GetLength(1); column++)
                {
                    Texture2D textureToDraw = null;
                    bool floor = false;
                    switch (this.Grid[line, column])
                    {
                        case MapElement.Floor:
                            textureToDraw = wallTexture;
                            floor = true;
                            break;
                        case MapElement.Empty:
                            textureToDraw = floorTexture;
                            floor = false;
                            break;
                        case MapElement.Hole:
                            textureToDraw = holeTexture;
                            break;
                        case MapElement.Wall:
                            textureToDraw = wallTexture;
                            break;
                        default:
                            break;
                    }
                    spriteBatch.Draw(textureToDraw, new Rectangle(column * ((int)this.CellSize.X), line * (int)this.CellSize.Y, (int)this.CellSize.X, (int)this.CellSize.Y), Color.White);
                    if (floor)
                        spriteBatch.Draw(wallTexture, new Rectangle(column * ((int)this.CellSize.X), line * (int)this.CellSize.Y + ((int)this.CellSize.Y - 3), (int)this.CellSize.X, 3), Color.White);
                }
            }

            Entities.ForEach(x => x.Draw(gameTime, spriteBatch));
        }

        public void Initialize(Game game)
        {
            this.game = game;
            Grid = new MapElement[,] { 
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Hole, MapElement.Hole, MapElement.Hole, MapElement.Hole,      MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Hole, MapElement.Hole, MapElement.Hole, MapElement.Hole,      MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Hole, MapElement.Hole, MapElement.Hole, MapElement.Hole,      MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Hole, MapElement.Hole, MapElement.Hole, MapElement.Hole,      MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Hole, MapElement.Hole, MapElement.Hole, MapElement.Hole,      MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Hole, MapElement.Hole, MapElement.Hole, MapElement.Hole,      MapElement.Floor,  MapElement.Floor,  MapElement.Floor,  MapElement.Floor, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty, MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty,  MapElement.Empty, MapElement.Wall, MapElement.Wall },
                { MapElement.Wall, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor, MapElement.Floor,  MapElement.Floor,  MapElement.Floor,  MapElement.Floor,  MapElement.Floor, MapElement.Wall, MapElement.Wall },
            };
            for (int line = 0; line < this.Grid.GetLength(0); line++)
            {
                for (int column = 0; column < this.Grid.GetLength(1); column++)
                {
                    Entity entityToAdd = null;
                    Vector2 position = new Vector2(column * CellSize.X, line * CellSize.Y);
                    switch (Grid[line, column])
                    {
                        case MapElement.Wall:
                            entityToAdd = new Wall(position);
                            break;
                        case MapElement.Floor:
                            entityToAdd = new Floor(position);
                            break;
                    }
                    if (entityToAdd != null)
                    {
                        Entities.Add(entityToAdd);
                    }
                }
            }

            floorTexture = new Texture2D(game.GraphicsDevice, (int)this.CellSize.X, (int)this.CellSize.Y);
            Color[] floorColors = Enumerable.Repeat(Color.Blue, (int)this.CellSize.X * (int)this.CellSize.Y).ToArray();
            floorTexture.SetData(floorColors);
            holeTexture = new Texture2D(game.GraphicsDevice, (int)this.CellSize.X, (int)this.CellSize.Y);
            holeTexture.SetData(Enumerable.Repeat(Color.Gray, (int)this.CellSize.X * (int)this.CellSize.Y).ToArray());
            wallTexture = new Texture2D(game.GraphicsDevice, (int)this.CellSize.X, (int)this.CellSize.Y);
            wallTexture.SetData(Enumerable.Repeat(Color.Navy, (int)this.CellSize.X * (int)this.CellSize.Y).ToArray());

            Entities.ForEach(x => x.Initialize(game));
        }

        public void Update(GameTime gametime)
        {
            Entities.ForEach(x => x.Update(gametime));
        }
    }
}
