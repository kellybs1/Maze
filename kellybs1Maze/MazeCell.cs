using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kellybs1Maze
{
    /*
   * Class: MazeCell
   * Author: Brendan Kelly
   * Date: 10/11/2017
   * Description: Child GraphNode that contains information on a single Maze Cell,its walls, and neighbours
   */

    public class MazeCell
    {      
        private Random rand;
        public bool[] Walls { get; set; }
        public bool Visited { get; set; }

        public int Row { get; set; }
        public int Col { get; set; }

        public MazeCell[] Neighbours { get; set; }
        public MazeCell(Random inRand)
        {
            Row = -1;
            Col = -1;
            Visited = false;
            rand = inRand;
            Neighbours = new MazeCell[Constants.NWALLS];
            Walls = new bool[Constants.NWALLS];

            //initialise all walls up
            Walls[Constants.NORTH] = true;
            Walls[Constants.EAST] = true;
            Walls[Constants.SOUTH] = true;
            Walls[Constants.WEST] = true;
        }


        //selects one of the cells neighbours at random
        public int GetRandomNeighbourIndex()
        {
            int selected;
            //select a random neighbour by index
            do
            {
                selected = rand.Next(Constants.NWALLS);
            } while (Neighbours[selected] == null || Neighbours[selected].Visited == true); //make sure cell not already visited

            return selected;
        }


        //selects a random neighbour with a pathway
        public int GetRandomOpenWallIndex()
        {
            int selected;
            //select a random neighbour by index
            do
            {
                selected = rand.Next(Constants.NWALLS);
            } while (Neighbours[selected] == null || 
                          Walls[selected] == true || 
             Neighbours[selected].Visited == true); //make sure cell not already visited and wall down

            return selected;
        }


        //check if current cell has any unvisited neighbours
        public bool HasUnvisited()
        {
            bool unvisited = false;

            for (int i = 0; i < Constants.NWALLS; i++)
                if (Neighbours[i] != null) //not all array slots are populated
                    if (Neighbours[i].Visited == false)
                        return true;

            return unvisited;
        }


        //check if current cell has any unvisited neighbours with an open path
        public bool HasUnvisitedOpen()
        {
            bool unvisited = false;

            for (int i = 0; i < Constants.NWALLS; i++)
                if (Neighbours[i] != null) //not all array slots are populated
                    if (Neighbours[i].Visited == false && Walls[i] == false)
                        return true;

            return unvisited;
        }
    }
}
