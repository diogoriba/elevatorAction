using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction
{
    public abstract class Entity : ICanDraw, ICanUpdate
    {
        //public int Order;
        protected Vector2 _initialPosition;
        protected Vector2 _initialSize;
        public Entity(Vector2 initialPosition = default(Vector2), Vector2 initialSize = default(Vector2))
        {
            _initialPosition = initialPosition;
            _initialSize = initialSize;
        }

        public Body Body { get; protected set; }
        public bool Active { get; protected set; }
        public bool Visible { get; protected set; }
        public bool Dead { get; protected set; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public virtual void Initialize(Game game) {
            Body = new Body(_initialPosition, _initialSize);
        }
    }
}
