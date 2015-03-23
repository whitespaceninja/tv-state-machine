using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVStates
{
    
    public class TV
    {
        // possible TV states
        public const int STATE_OFF = 0;
        public const int STATE_ON = 1;

        // total height of the TV in lines
        const int TV_HEIGHT = 25;

        const string TV_STATIC_LINE = "0000000000000000000000000000000000000000000000000000000000000000000000000000000";

        // the current state of the TV
        private int _state;

        public TV()
        {
            // initialize a TV as OFF
            _state = STATE_OFF;
        }

        public void HandleInput()
        {
            // if the user presses the spacebar, toggle the power on the tv
            ConsoleKeyInfo info = Console.ReadKey();
            if (info.Key == ConsoleKey.Spacebar)
            {
                TogglePower();
            }
        }

        public void TogglePower()
        {
            // flip the power states based on our current state
            if (_state == STATE_ON)
            {
                _state = STATE_OFF;
            }
            else
            {
                _state = STATE_ON;
            }
        }

        public void Draw()
        {
            Console.Clear();

            if (_state == STATE_OFF)
            {
                // draw nothing. TV is OFF
            }
            else if (_state == STATE_ON)
            {
                // draw static lines for our entire height
                for (int i = 0; i < TV_HEIGHT; i++)
                {
                    Console.WriteLine(TV_STATIC_LINE);
                }
            }
        }
    }
}
