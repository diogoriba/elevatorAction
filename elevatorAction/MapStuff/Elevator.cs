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

        private readonly int FLOOR_SIZE = 6;

        private Point _shaftTop;
        private int _shaftSize;

        private int _positionNow;
        
        private Point PointNow
        {
            get
            {
                return _shaftTop + new Point(0, _positionNow);
            }
        }
        
        private int _elevatorStartIndex;

        private readonly Vector2 ELEVATOR_SIZE = new Vector2(3, 1);

        private float _timeStoped = 0f;
        private float _timeLimit = 2f;
        private bool _moving = false;
        private bool _movingUp = false;
        private int _target;

        public Elevator(Point shaftTop, int shaftSize)
            : base()
        {
            FLOOR_SIZE = 6 * (int)Map.Instance.CellSize.Y;
            shaftSize *= FLOOR_SIZE;
            _shaftTop = shaftTop;

            if (shaftSize <= FLOOR_SIZE) shaftSize = 2 * FLOOR_SIZE; //MIN VALUE //LARGE VALUE = BOOM

            _elevatorStartIndex = 0;
            _shaftSize = shaftSize;
            _positionNow = _elevatorStartIndex;

            //var cellSize = Map.Instance.CellSize;
            //Body = new ElevatorBody(PointNow.ToVector2() * cellSize, ELEVATOR_SIZE * cellSize);
            //Body.Position = PointNow.ToVector2() * cellSize;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_elevatorTexture, Body.Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (_moving)
            {
                ExecuteMove(gameTime);
            }
            else
            {
                _timeStoped += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            return _positionNow < _shaftSize - FLOOR_SIZE;
        }

        private bool CanMoveUp()
        {
            return _positionNow > 0;
        }

        private bool MovingUp()
        {
            return _movingUp;
        }

        private void ExecuteMove(GameTime gameTime)
        {
            Vector2 positionSelf = Body.Position;
            Vector2 positionTarget = (_shaftTop + new Point(0, _target)).ToVector2();

            if (positionSelf.Y == positionTarget.Y)
            {
                _moving = false;
                _positionNow = _target;
                return;
            }

            float limitUp = 0;
            float limitDown = 0;
            float direction = 1;

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

            float temp = positionSelf.Y + (direction * (float)gameTime.ElapsedGameTime.TotalSeconds);
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
            _target = MathHelper.Clamp(x, 0, _shaftSize - FLOOR_SIZE);
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            //_elevatorTexture = game.Content.Load<Texture2D>("elevator");
            Vector2 size = ELEVATOR_SIZE * Map.Instance.CellSize;
            _elevatorTexture = new Texture2D(game.GraphicsDevice, (int)size.X, (int)size.Y);
            _elevatorTexture.SetData(Enumerable.Repeat(Color.Red, (int)size.X * (int)size.Y).ToArray());
            Body = new ElevatorBody(PointNow.ToVector2() * Map.Instance.CellSize, ELEVATOR_SIZE * Map.Instance.CellSize);
        }
    }
}
