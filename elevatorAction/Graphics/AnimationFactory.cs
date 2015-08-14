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
    }
}
