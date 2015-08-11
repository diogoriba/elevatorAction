using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace elevatorAction
{
    public static class Input
    {
        public static KeyboardState previousKeyboardState { get; set; }
        public static KeyboardState currentKeyboardState { get; set; }

        static Input()
        {
            currentKeyboardState = new KeyboardState();
            previousKeyboardState = new KeyboardState();
        }

        public static bool KeyWasPressed(Keys key)
        {
            return previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }
    }
}
