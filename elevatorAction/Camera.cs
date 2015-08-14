using elevatorAction.Characters;
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
        public List<Entity> Entities;

        private Vector2 scrollSpeed = new Vector2(0, 8);
        public Camera()
            : base(new Vector2(0, 0),
                new Vector2(800, 600))
        {
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Entities = new List<Entity>();

            Body.Position = new Vector2(Body.Position.X, Map.Instance.Player.Body.Position.Y);
        }

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(Body.Position, 0) * -1);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gametime)
        {
            Entities.Clear();
            Entities = Map.Instance.Entities.FindAll(entity => entity.Body.Collides(Body)).ToList();
            //TODO: ADD CAMERA MOVE
            float deltaTime = (float)gametime.ElapsedGameTime.TotalSeconds;
            Player player = Map.Instance.Player;
            Player.PlayerState state = player.CurrentState;
            if ((state == Player.PlayerState.Walking || state == Player.PlayerState.FallingHole) &&
                Body.CollisionRectangle.Center.Y != player.Body.CollisionRectangle.Center.Y)
            {
                Vector2 distance = player.Body.CollisionRectangle.Center.ToVector2() - Body.CollisionRectangle.Center.ToVector2();
                int direction = Math.Sign(distance.Y);
                Vector2 movement = (direction * scrollSpeed * Map.Instance.CellSize * deltaTime);
                Vector2 tentativePos = Body.Position + movement;
                tentativePos.Y = MathHelper.Clamp(tentativePos.Y, 0, Map.Instance.Height - Body.Size.Y);
                Vector2 newDistance = player.Body.CollisionRectangle.Center.ToVector2() - (tentativePos + Body.Size/2);
                if (Math.Abs(newDistance.Y) > Math.Abs(distance.Y))
                {
                    tentativePos = Body.Position + distance * Vector2.UnitY;
                }
                Body.Position = tentativePos;
            }
        }
    }
}
