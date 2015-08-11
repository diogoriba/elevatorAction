using elevatorAction.MapElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Characters
{
    public class BulletPool : ICanUpdate
    {
        public int MaxElements { get; protected set; }
        public Entity Owner { get; protected set; }
        private List<Bullet> bullets;
        private Game game;
        public BulletPool(Game game, Entity owner, int maxElements)
        {
            this.game = game;
            bullets = new List<Bullet>();
            Owner = owner;
            MaxElements = maxElements;
        }

        public void CreateBullet()
        {
            if (bullets.Count < MaxElements)
            {
                Bullet newBullet = new Bullet(Owner, Owner.Body.CollisionRectangle.Center.ToVector2());
                newBullet.Initialize(game);
                bullets.Add(newBullet);
                Map.Instance.Entities.Add(newBullet);
            }
        }

        public void Update(GameTime gametime)
        {
            var deadBullets = bullets.Where(bullet => bullet.Dead);
            bullets.RemoveAll(bullet => deadBullets.Contains(bullet));
        }
    }

    public class Bullet : Entity
    {
        public bool Dead { get; private set; }

        private Texture2D bulletTexture;
        private Entity owner;

        public Bullet(Entity owner, Vector2 initialPosition) 
            : base(initialPosition, Map.Instance.CellSize * new Vector2(0.5f, 0.25f))
        {
            this.owner = owner;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, new Rectangle(Body.Position.ToPoint(), Body.Size.ToPoint()), Color.White);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 tentativePosition = Body.Position;
            tentativePosition += Body.Orientation * deltaTime * Map.Instance.CellSize * new Vector2(12f, 0f);
            List<Entity> collidesWith = MoveTo(tentativePosition).Where(entity => entity != owner).ToList();
            if (collidesWith.Count > 0)
            {
                Dead = true;
                Map.Instance.Entities.Remove(this);
            }
        }

        private List<Entity> MoveTo(Vector2 tentativePosition)
        {
            Body.Position = tentativePosition;
            var collidable = Map.Instance.Entities.Where(entity => entity is Wall || entity is Player || entity is Enemy);
            var collidesWith = collidable.Where(entity => entity.Body.Collides(Body)).ToList();
            return collidesWith;
        }

        public override void Initialize(Microsoft.Xna.Framework.Game game)
        {
            base.Initialize(game);
            Body.Orientation = owner.Body.LastActiveOrientation;
            bulletTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            bulletTexture.SetData(new Color[] { Color.Yellow });
        }
    }
}
