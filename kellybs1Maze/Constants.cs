using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
* Class: Constants
* Author: Brendan Kelly
* Date: 29/10/2017
* Description: Static class to hold constants for Maze generation and solving project
*/

namespace kellybs1Maze
{


    public static class Constants
    {
        public const int NWALLS = 4;

        public const int PANELSIZE = 512;

        public const int DRAWWAIT = 5;

        public static Color TRAVERSAL_FILL = Color.FromArgb(198, 196, 196);
        public static Color SOLUTION_FILL = Color.FromArgb(161, 244, 159);
        public static Color WALL_COLOUR = Color.Black;

        //all directions indexes are clockwise 
        public const int NORTH = 0;
        public const int EAST = 1;
        public const int SOUTH = 2;
        public const int WEST = 3;

        public const int LINE_START = 0;
        public const int LINE_END = 1;

    }
}
