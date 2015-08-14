using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Graphics
{
    public class Animation
    {
        public List<AnimationState> States { get; set; }
        private int CurrentIndex { get; set; }

        private AnimationState CurrentState
        {
            get
            {
                if (CurrentIndex < 0) return null;
                return States[CurrentIndex];
            }
        }

        private float Timer { get; set; }
        
        public Animation()
        {
            States = new List<AnimationState>();
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentState == null) return;

            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(Timer >= CurrentState.Time)
            {
                Step();
            }
        }

        public Texture2D Texture
        {
            get
            {
                return TexturePool.Instance.Textures[CurrentState.Texture];
            }
        }
        public Color Color
        {
            get
            {
                return Color.White;
            }
        }

        //public Rectangle Destination { get; set; } //spritesheet
        //public Rectangle Origin { get; set; } //spritesheet

        public void Step()
        {
            GoToState(CurrentIndex + 1);
        }

        public void Reset()
        {
            GoToState(0);
        }

        public void GoToState(int stateNr)
        {
            Timer = 0;
            int qnt = States.Count;
            if (qnt == 0)
            {
                CurrentIndex = -1;
                return;
            }

            CurrentIndex = stateNr % qnt;
        }
    }
}
