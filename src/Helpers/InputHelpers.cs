﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Jammy.Helpers
{
    public static class InputHelpers
    {
        public static bool WasButtonPressed(this ButtonState currentState, ButtonState lastState)
        {
            return currentState == ButtonState.Pressed && lastState == ButtonState.Released;
        }
    }
}
