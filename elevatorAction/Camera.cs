using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction
{
    public class Camera : Entity
    {
        private Map _map;
        public List<Entity> Entities;
        public Camera()
            : base(new Vector2(0, 0),
                new Vector2(800, 600))
        {
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Entities = new List<Entity>();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gametime)
        {
            Entities.Clear();
            Entities = Map.Instance.Entities.FindAll(entity => entity.Body.Collides(Body)).ToList();
            //TODO: ADD CAMERA MOVE
        }
    }
}
