﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.MapElements
{
    public class Floor : Entity
    {
        public Floor(Vector2 initialPosition) : base(initialPosition, Map.Instance.CellSize)
        {
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            Body.AdjustMask = new Vector2(0, 1);
        }
    }
}
