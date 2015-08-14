using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction.Graphics
{
    public static class AnimationFactory
    {
        public static Animation TestAnimation()
        {
            Animation resp = new Animation();

            //resp.States.Add(new AnimationState() { Texture = TextureNames.TEST_1, Time = 1 });
            //resp.States.Add(new AnimationState() { Texture = TextureNames.TEST_2, Time = 1 });
            //resp.States.Add(new AnimationState() { Texture = TextureNames.TEST_3, Time = 1 });
            //resp.States.Add(new AnimationState() { Texture = TextureNames.TEST_4, Time = 1 });

            return resp;
        }

        public static Animation StandAnimation()
        {
            Animation resp = new Animation();

            AddState(resp, TextureNames.PLAYER_S, 1);

            return resp;
        }

        public static Animation WalkAnimation()
        {
            Animation resp = new Animation();

            AddState(resp, TextureNames.PLAYER_S, 0.15f);
            AddState(resp, TextureNames.PLAYER_W, 0.15f);

            return resp;
        }

        public static Animation CrouchAnimation()
        {
            Animation resp = new Animation();

            AddState(resp, TextureNames.PLAYER_C, 1f);

            return resp;
        }

        public static Animation JumpingAnimation()
        {
            Animation resp = new Animation();

            AddState(resp, TextureNames.PLAYER_J, 1f);

            return resp;
        }

        private static void AddState(Animation a, string texture, float time)
        {
            a.States.Add(new AnimationState() { Texture = texture, Time = time });
        }
    }
}
