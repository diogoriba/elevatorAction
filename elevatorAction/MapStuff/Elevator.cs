using elevatorAction.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.MapStuff
{
    public class Elevator : Entity
    {
        private Texture2D _elevatorTexture;

        private int FLOOR_SIZE = 6;

        private Point _shaftTop;

        private int _shaftSize;

        private bool _turbo = false;


        private Vector2 _elevatorStartPosition;
        private int _positionNow;

        //private int _elevatorStartIndex;

        private readonly Vector2 ELEVATOR_SIZE = new Vector2(3, 1);

        private float _timeStoped = 0f;
        private float _timeLimit = 2f;
        private bool _moving = false;
        private bool _movingUp = false;
        private int _target;

        public Elevator(Point shaftTop, int numberOfFloors)
            : base()
        {
            FLOOR_SIZE = 6 * (int)Map.Instance.CellSize.Y;
            if (numberOfFloors <= 1) numberOfFloors = 2; //MIN VALUE //LARGE VALUE = BOOM

            _shaftTop = shaftTop;
            _elevatorStartPosition = _shaftTop.ToVector2() * Map.Instance.CellSize;
            _shaftSize = numberOfFloors * FLOOR_SIZE;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_elevatorTexture, position: Body.Position, color: Color.White, scale: new Vector2(Map.SCALE, Map.SCALE));
            //(Body as ElevatorBody).DebugDraw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (_moving)
            {
                ExecuteMove(gameTime);
            }
            else
            {
                _timeStoped += (float)gameTime.ElapsedGameTime.TotalSeconds * ((_turbo) ? 3f : 1f);
                if (_timeStoped >= _timeLimit)
                {
                    //timeStoped = 0f;
                    if (MovingUp())
                    {
                        if (CanMoveUp())
                        {
                            MoveUp();
                        }
                        else
                        {
                            //reverse
                            MoveDown();
                        }
                    }
                    else
                    {
                        if (CanMoveDown())
                        {
                            MoveDown();
                        }
                        else
                        {
                            //reverse
                            MoveUp();
                        }
                    }
                }
            }
        }

        private bool CanMoveDown()
        {
            return _positionNow < _elevatorStartPosition.Y + _shaftSize;
        }

        private bool CanMoveUp()
        {
            return _positionNow > _elevatorStartPosition.Y;
        }

        private bool MovingUp()
        {
            return _movingUp;
        }

        private void ExecuteMove(GameTime gameTime)
        {
            Vector2 positionSelf = Body.Position;
            Vector2 positionTarget = new Vector2(_elevatorStartPosition.X, _target);

            if (positionSelf.Y == positionTarget.Y)
            {
                _moving = false;
                _positionNow = _target;
                return;
            }

            float limitUp = 0;
            float limitDown = 0;
            float direction = 3;

            if (positionSelf.Y > positionTarget.Y)
            {
                limitUp = positionSelf.Y;
                limitDown = positionTarget.Y;
                direction *= -1;
            }
            else
            {
                limitUp = positionTarget.Y;
                limitDown = positionSelf.Y;
            }

            float temp = positionSelf.Y + (direction * (float)gameTime.ElapsedGameTime.TotalSeconds * Map.Instance.CellSize.Y * ((_turbo) ? 3f : 1f));
            temp = MathHelper.Clamp(temp, limitDown, limitUp);
            positionSelf.Y = temp;

            Body.Position = positionSelf;
        }

        public bool MoveUp()
        {
            if (!CanMoveUp()) return false;
            _movingUp = true;
            _timeStoped = 0f;
            _moving = true;
            SetTarget(_positionNow - FLOOR_SIZE);

            return true;
        }

        public bool MoveDown()
        {
            if (!CanMoveDown()) return false;
            _movingUp = false;
            _timeStoped = 0f;
            _moving = true;
            SetTarget(_positionNow + FLOOR_SIZE);

            return true;
        }

        private void SetTarget(int x)
        {
            _target = MathHelper.Clamp(x, (int)_elevatorStartPosition.Y, (int)_elevatorStartPosition.Y + (int)_shaftSize);
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            _elevatorTexture = game.Content.Load<Texture2D>("elevator");
            /* 
            Vector2 size = ELEVATOR_SIZE * Map.Instance.CellSize;
            _elevatorTexture = new Texture2D(game.GraphicsDevice, (int)size.X, (int)size.Y);
            _elevatorTexture.SetData(Enumerable.Repeat(Color.Red, (int)size.X * (int)size.Y).ToArray());
            //*/
            Body = new ElevatorBody(_elevatorStartPosition, ELEVATOR_SIZE * Map.Instance.CellSize, FLOOR_SIZE);
            _positionNow = (int)_elevatorStartPosition.Y;
        }
    }
}
