using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVStates.Advanced
{
    public class TV
    {
        // possible TV states
        const int STATE_OFF = 0;
        const int STATE_ON = 1;
        const int STATE_TURNING_OFF = 2;

        // 'turning off' sub states
        const int SUBSTATE_TURNOFF_VERTICAL_SHRINK = 0;
        const int SUBSTATE_TURNOFF_HORIZONTAL_SHRINK = 1;
        const int SUBSTATE_TURNOFF_WAIT_AS_DOT = 2;
        const int NUM_TURN_OFF_SUBSTATES = 3;

        // different strings to draw to represent the TV
        const string TV_STATIC_LINE = "0000000000000000000000000000000000000000000000000000000000000000000000000000000";
        const string TV_CLOSING_LINE = "_______________________________________________________________________________";
        const string TV_DOT1 = "                                       o                                       ";
        const string TV_DOT2 = "                                       .                                       ";

        // total height of the TV in lines
        const int TV_HEIGHT = 25;

        // amount of '_' characters we chop off in our HORIZONTAL_SHRINK substate before switching substates
        const int TV_LINE_SHRING_MIN_WIDTH = 60;

        // current state of the TV
        private int _state;

        // current substate of the TV
        private int _substate;

        // keeps track of time (kind of) in each substate
        private int _stateProgress;

        // the string we are drawing in our WAIT_AS_DOT substate
        private string _dotString;

        public TV()
        {
            // initialize a TV as OFF
            _state = STATE_OFF;
        }

        public void HandleInput()
        {
            // If we are turning off, wait for the animation to end
            if (_state == STATE_TURNING_OFF)
            {
                return;
            }

            // Toggle the power when the user hits spacebar
            ConsoleKeyInfo info = Console.ReadKey();
            if (info.Key == ConsoleKey.Spacebar)
            {
                TogglePower();
            }
        }

        public void Update()
        {
            if (_state != STATE_TURNING_OFF)
            {
                // we don't update anything except for TURNING_OFF state
                return;
            }

            // check the substate
            switch (_substate)
            {
                case SUBSTATE_TURNOFF_VERTICAL_SHRINK:
                    {
                        _stateProgress++;

                        // _stateProgress is the number of lines we are removing from top/bottom so
                        // once it gets down to half the TV height then it has shrunk the whole thing down
                        // to a single line like this: ______________________________________________
                        if (_stateProgress >= TV_HEIGHT / 2)
                        {
                            _substate = SUBSTATE_TURNOFF_HORIZONTAL_SHRINK;
                            _stateProgress = 0;
                        }
                    }
                    break;
                case SUBSTATE_TURNOFF_HORIZONTAL_SHRINK:
                    {
                        _stateProgress += 5;

                        // _stateProgress is the number of characters we are shrinking horizonatally like this:
                        // _________________________________________________________________________
                        //       _____________________________________________________________
                        //              _______________________________________________
                        //                              etc.
                        //
                        // Once we have shrunken to the MIN_WIDTH then we switch states
                        if (_stateProgress >= TV_LINE_SHRING_MIN_WIDTH)
                        {
                            _substate = SUBSTATE_TURNOFF_WAIT_AS_DOT;
                            _stateProgress = 0;
                        }
                    }
                    break;
                case SUBSTATE_TURNOFF_WAIT_AS_DOT:
                    {
                        _stateProgress++;

                        // _state progress is the length of time that we are waiting for our last dot 
                        // to stay on the screen
                        if (_stateProgress >= 10)
                        {
                            _state = STATE_OFF;
                        }
                        else if (_stateProgress >= 5)
                        {
                            // this is a smaller dot to simulate shrinking
                            _dotString = TV_DOT2;
                        }
                        else
                        {
                            // start with a bigger dot
                            _dotString = TV_DOT1;
                        }
                    }
                    break;
            }
        }

        public void TogglePower()
        {
            switch (_state)
            {
                case STATE_ON:
                    {
                        // If we were on, start turning off
                        _state = STATE_TURNING_OFF;
                        _substate = SUBSTATE_TURNOFF_VERTICAL_SHRINK;
                        _stateProgress = 0;
                    }
                    break;
                case STATE_OFF:
                    {
                        // if we were off, turn on
                        _state = STATE_ON;
                    }
                    break;
            }
        }

        public void Draw()
        {
            Console.Clear();

            switch (_state)
            {
                case STATE_OFF:
                    {
                        DrawStateOff();
                    }
                    break;
                case STATE_ON:
                    {
                        DrawStateOn();
                    }
                    break;
                case STATE_TURNING_OFF:
                    {
                        DrawStateTurningOff();
                    }
                    break;
            }
        }

        private void DrawStateOff()
        {
            // TV is off, don't draw anything
        }

        private void DrawStateOn()
        {
            // draw a bunch of static
            for (int i = 0; i < TV_HEIGHT; i++)
            {
                Console.WriteLine(TV_STATIC_LINE);
            }
        }

        private void DrawStateTurningOff()
        {
            // check the substate
            switch (_substate)
            {
                case SUBSTATE_TURNOFF_VERTICAL_SHRINK:
                    {
                        // draw blank lines at the top
                        for (int i = 0; i < _stateProgress; i++)
                        {
                            Console.WriteLine();
                        }

                        // one closing line at the top...
                        Console.WriteLine(TV_CLOSING_LINE);

                        // draw a bunch of static. Subtract 2 for the closing lines.
                        for (int i = 0; i < TV_HEIGHT - (_stateProgress * 2) - 2; i++)
                        {
                            Console.WriteLine(TV_STATIC_LINE);
                        }

                        //...and one closing line at the bottom
                        Console.WriteLine(TV_CLOSING_LINE);
                    }
                    break;
                case SUBSTATE_TURNOFF_HORIZONTAL_SHRINK:
                    {
                        // make sure our final line is in the middle of the TV
                        for (int i = 0; i < TV_HEIGHT / 2; i++)
                        {
                            Console.WriteLine();
                        }

                        // Draw spaces equal to half our progress to shrink the bar
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < _stateProgress / 2; i++)
                        {
                            sb.Append(' ');
                        }

                        // Shrink the bar by our progress
                        sb.Append(TV_CLOSING_LINE.Substring(_stateProgress));
                        Console.WriteLine(sb.ToString());
                    }
                    break;
                case SUBSTATE_TURNOFF_WAIT_AS_DOT:
                    {
                        // make sure our final dot is in the middle of the TV
                        for (int i = 0; i < TV_HEIGHT / 2; i++)
                        {
                            Console.WriteLine();
                        }

                        // draw our dot
                        Console.WriteLine(_dotString);
                    }
                    break;
            }
        }
    }
}
