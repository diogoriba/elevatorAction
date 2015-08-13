using elevatorAction.MapElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace elevatorAction
{
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

        public List<Entity> Entities { get; private set; }
        public Vector2 CellSize { get; private set; }

        public Game game;

        private Map()
        {
            CellSize = new Vector2(CELL_SIZE * SCALE, CELL_SIZE * SCALE);
            Entities = new List<Entity>();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Entities.ForEach(x => x.Draw(gameTime, spriteBatch));
        }

        public void Initialize(Game game)
        {
            this.game = game;
            TmxMap map = new TmxMap("Content/elevatorActionMap.tmx");
            for (var i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                int gid = map.Layers[0].Tiles[i].Gid;
                if (gid != 0)
                {
                    TmxTileset tileset = map.Tilesets.First(set => set.FirstGid == gid);
                    float x = (i % map.Width);
                    float y = (float)Math.Floor(i / (double)map.Width);
                    Vector2 position = new Vector2(x, y) * CellSize;

                    switch (tileset.Name)
                    {
                        case "wall":
                            Entities.Add(new Wall(position));
                            break;
                        case "floor":
                            Entities.Add(new Floor(position));
                            break;
                        default:
                            break;
                    }
                }
            }   

            Entities.ForEach(x => x.Initialize(game));
        }

        public void Update(GameTime gametime)
        {
            Entities.ForEach(x => x.Update(gametime));

            Entities.RemoveAll(x => x.Dead);
        }
    }
}
