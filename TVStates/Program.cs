﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TVStates
{
    class Program
    {
        static void Main(string[] args)
        {
            TV tv = new TV();

            // run forever in a loop
            while (true)
            {
                tv.Draw();
                tv.HandleInput();

                Thread.Sleep(20);
            }
        }
    }
}
