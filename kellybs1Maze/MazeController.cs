using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
* Class: MazeController
* Author: Brendan Kelly
* Date: 29/10/2017
* Description: Controls running maze operations such as creation and drawing to form
*/

namespace kellybs1Maze
{

    public class MazeController
    {
        private int cellSize;
        private Maze maze;
        private Graphics canvas;
        private int canvasSize;
        private int nCells;
        private Pen linePen;
        private Point[][][] wallPoints; //stores arrays of n/e/s/w wall points for each node
                                        //[NodeIndex][WallLocationIndex 0-3][0 - StartPoint or 1 - EndPoint]

        public MazeController(Graphics inCanvas, int inCellSize)
        {
            cellSize = inCellSize;
            linePen = new Pen(Constants.WALL_COLOUR);
            canvas = inCanvas;
            canvasSize = Constants.PANELSIZE;
            nCells = (int)Math.Pow((canvasSize / cellSize),2);
            
            wallPoints = new Point[nCells][][]; //storage for drawing points - only needs to be run once
            maze = new Maze(nCells, cellSize); //make the maze
            generateWallPoints(); //run after maze init
        }


        //solves the maze
        public void SolveMaze()
        {
            maze.SolveMaze(canvas);
        }

        //draws the maze
        public void DrawMaze()
        {
            for (int index = 0; index < nCells; index++)
            {
                //pull the walls to be drawn from the node
                MazeCell currentCell = maze.Cells[index];
                bool[] currentWalls = currentCell.Walls;
                //generate the current drawing points for walls
                //check all walls 
                for (int i = 0; i < Constants.NWALLS; i++)
                {
                    //if the current wall is up
                    bool currentWall = currentWalls[i];
                    if (currentWall)
                    {
                        //pull its corresponding start and end points using corresponding index
                        Point[] points = wallPoints[index][i];
                        Point start = points[Constants.LINE_START];
                        Point end = points[Constants.LINE_END];
                        //draw the wall
                        canvas.DrawLine(linePen, start.X, start.Y, end.X, end.Y);
                    }
                }
            } 
        }

        //generates jagged array of start and end point for wall drawing based on current x and y positions
        private void generateWallPoints()
        {
            int sideSize = canvasSize / cellSize;
            int index = 0; //index of the color in the chromosome to be drawn
            for (int y = 0; y < sideSize; y++)
            {
                //generate ypos
                int yPos = y * cellSize;
                for (int x = 0; x < sideSize; x++)
                {
                    //generate xpos
                    int xPos = x * cellSize;
                    //north
                    int nWallStartX = xPos;
                    int nWallEndX = xPos + cellSize;
                    int nWallStartY = yPos;
                    int nWallEndY = yPos;
                    //east
                    int eWallStartX = xPos + cellSize;
                    int eWallEndX = xPos + cellSize;
                    int eWallStartY = yPos;
                    int eWallEndY = yPos + cellSize;
                    //south
                    int sWallStartX = xPos;
                    int sWallEndX = xPos + cellSize;
                    int sWallStartY = yPos + cellSize;
                    int sWallEndY = yPos + cellSize;
                    //west
                    int wWallStartX = xPos;
                    int wWallEndX = xPos;
                    int wWallStartY = yPos;
                    int wWallEndY = yPos + cellSize;

                    //init new jagged array for this index
                    wallPoints[index] = new Point[Constants.NWALLS][]; //new 2D array

                    //generate points
                    //north
                    Point startN = new Point(nWallStartX, nWallStartY);
                    Point endN = new Point(nWallEndX, nWallEndY);
                    Point[] nPoints = new Point[] { startN, endN };
                    //add to main array                  
                    wallPoints[index][Constants.NORTH] = nPoints;

                    //east
                    Point startE = new Point(eWallStartX, eWallStartY);
                    Point endE = new Point(eWallEndX, eWallEndY);
                    Point[] ePoints = { startE, endE };
                    wallPoints[index][Constants.EAST] = ePoints;

                    //south
                    Point startS = new Point(sWallStartX, sWallStartY);
                    Point endS = new Point(sWallEndX, sWallEndY);
                    Point[] sPoints = { startS, endS };
                    wallPoints[index][Constants.SOUTH] = sPoints;

                    //west
                    Point startW = new Point(wWallStartX, wWallStartY);
                    Point endW = new Point(wWallEndX, wWallEndY);
                    Point[] wPoints = { startW, endW };
                    wallPoints[index][Constants.WEST] = wPoints;

                    index++; //go to next index
                }
            }
        }
    }


    
}
