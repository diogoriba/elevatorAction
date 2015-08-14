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
            bullets.RemoveAll(bullet => bullet.Dead);
        }
    }

    public class Bullet : Entity
    {
        private Texture2D bulletTexture;
        public Entity Owner { get; private set; }

        public Bullet(Entity owner, Vector2 initialPosition) 
            : base(initialPosition, Map.Instance.CellSize * new Vector2(0.5f, 0.25f))
        {
            Owner = owner;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, new Rectangle(Body.Position.ToPoint().X, Body.Position.ToPoint().Y, Body.Size.ToPoint().X, Body.Size.ToPoint().Y), Color.White);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            List<Entity> collidesWith = MoveTo(Body.Position).Where(entity => entity.GetType() != Owner.GetType()).ToList();
            if (collidesWith.Count > 0)
            {
                if (!ElevatorAction.Voxels)
                {
                    collidesWith = collidesWith.Where(entity => entity is Player || entity is Enemy).ToList();
                }
                collidesWith.ForEach(entity => entity.Dead = true);
                Dead = true;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 tentativePosition = Body.Position;
            tentativePosition += Body.Orientation * deltaTime * Map.Instance.CellSize * new Vector2(12f, 0f);
            MoveTo(tentativePosition);
        }

        private List<Entity> MoveTo(Vector2 tentativePosition)
        {
            Body.Position = tentativePosition;
            var collidable = Map.Instance.Camera.Entities.Where(entity => entity is Wall || entity is Player || entity is Enemy);
            var collidesWith = collidable.Where(entity => entity.Body.Collides(Body)).ToList();
            return collidesWith;
        }

        public override void Initialize(Microsoft.Xna.Framework.Game game)
        {
            base.Initialize(game);
            Body.Orientation = Owner.Body.LastActiveOrientation;
            bulletTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            bulletTexture.SetData(new Color[] { Color.Yellow });
        }
    }
}
