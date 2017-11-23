using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
* Class: Maze
* Author: Brendan Kelly
* Date: 10/11/2017
* Description: Class for generating and handling a Maze generation and solving
*/


namespace kellybs1Maze
{
    public class Maze
    {
        private int cellSize;
        private Random rand;
        public int CellCount { get; set; }
        public List<MazeCell> Cells { get; set; }
        private int gridWallSize;
        private MazeCell[,] grid;
        public Maze(int startCellCount, int inCellSize)
        {
            cellSize = inCellSize;
            rand = new Random();
            CellCount = startCellCount;
            Cells = new List<MazeCell>();
            gridWallSize = (int)Math.Sqrt(CellCount);
            grid = new MazeCell[gridWallSize, gridWallSize];
            generateCells();
            generateMaze();

        }
     

        //initiliases maze solving
        public void SolveMaze(Graphics canvas)
        {
            //reset visited
            foreach (MazeCell cell in Cells)
                cell.Visited = false;

            //brushes for fillings
            Brush solutionBrush = new SolidBrush(Constants.SOLUTION_FILL);
            Brush travBrush = new SolidBrush(Constants.TRAVERSAL_FILL);
            Pen stroke = new Pen(Constants.WALL_COLOUR);

            //Make the initial cell the current cell and mark it as visited
            int startR = 0;
            int startC = 0;
            Stack<MazeCell> visitedStack = new Stack<MazeCell>();           
            MazeCell startCell = grid[startR, startC];
            //set the target (bottom right cell)
            MazeCell targetCell = grid[gridWallSize - 1, gridWallSize - 1]; //bottom left corner
            visitedStack.Push(startCell);
            //now solve
            mazeSolve(canvas, travBrush, solutionBrush, stroke, startCell, targetCell, visitedStack);

        }

        //solves the maze
        private void mazeSolve(Graphics canvas, Brush travBrush, Brush solBrush, Pen stroke, 
            MazeCell currentCell, MazeCell targetCell, Stack<MazeCell> visitedStack)
        {

            while (currentCell != targetCell) //while we haven't found the end
            {
                if (currentCell.HasUnvisitedOpen()) //if the current cell has unvisitied neighbours with a path to them
                {
                    int nextIndex = -1; //index will break everything if it isn't correctly set 
                    //Choose randomly one of the unvisited neighbours
                    if (currentCell.Walls[Constants.SOUTH] == false && currentCell.Neighbours[Constants.SOUTH].Visited == false) //if we can go south or east let's do that
                        nextIndex = Constants.SOUTH;
                    else if (currentCell.Walls[Constants.EAST] == false && currentCell.Neighbours[Constants.EAST].Visited == false)
                        nextIndex = Constants.EAST;                    
                    else
                        nextIndex = currentCell.GetRandomOpenWallIndex();

                    MazeCell nextCell = currentCell.Neighbours[nextIndex];
                    //Push the current cell to the stack
                    visitedStack.Push(currentCell);
                    drawCell(canvas, solBrush, stroke, currentCell);
                    //Make the chosen cell the current cell and mark it as visited
                    nextCell.Visited = true;
                    currentCell = nextCell;
                }
                else //Else if stack is not empty 
                {
                    //Pop a cell from the stack
                    MazeCell nextCell = visitedStack.Pop();
                    drawCell(canvas, travBrush, stroke, currentCell);
                    //Make it the current cell
                    currentCell = nextCell;
                }
            }
            //draw the end
            drawCell(canvas, solBrush, stroke, currentCell);
        }


        //draws a cell with the given colours
        private void drawCell(Graphics canvas, Brush fill, Pen stroke, MazeCell cell)
        {
            //find cell's location on screen
            int xPos = cell.Col * cellSize;
            int yPos = cell.Row * cellSize;
            //fill and outline
            canvas.FillRectangle(fill, xPos, yPos, cellSize, cellSize);
            canvas.DrawRectangle(stroke, xPos, yPos, cellSize, cellSize);
            //wait because the solution is so darned quick 
            Thread.Sleep(Constants.DRAWWAIT);
        }


        //build the maze out of the grid
        private void generateMaze()
        {
            //Make the initial cell the current cell and mark it as visited
            int startR = 0;
            int startC = 0;
            Stack<MazeCell> visitedStack = new Stack<MazeCell>();
            MazeCell startCell = grid[startR, startC];
            startCell.Visited = true;
            visitedStack.Push(startCell);   
            //init maze generation
            mazeGen(startCell, visitedStack);          
        }


        //recursive backtrack maze generation
        private void mazeGen(MazeCell currentCell, Stack<MazeCell> visitedStack)
        {

            //While there are unvisited cells
            while (visitedStack.Count > 0)
            {
                //If the current cell has unvisited neighbours
                if (currentCell.HasUnvisited())
                {
                    //Choose random unvisited neighbour
                    int nextIndex = currentCell.GetRandomNeighbourIndex();
                    MazeCell nextCell = currentCell.Neighbours[nextIndex];
                    //Push the current on stack
                    visitedStack.Push(currentCell);
                    //Remove the wall between the two
                    tearDownAWall(currentCell, nextIndex);
                    //Visit the new cell and make it the currentCell
                    nextCell.Visited = true;
                    currentCell = nextCell;
                }
                else //Else if stack is not empty 
                {
                    //Pop a cell from the stack
                    MazeCell nextCell = visitedStack.Pop();
                    //Make it the current cell
                    currentCell = nextCell;
                }
            }

            //leave start and finished gaps in first and last cells because it looks cool
            Cells[0].Walls[Constants.NORTH] = false;
            Cells[0].Walls[Constants.WEST] = false;
            Cells[CellCount - 1].Walls[Constants.EAST] = false;
            Cells[CellCount - 1].Walls[Constants.SOUTH] = false;

        }

        //puts a gap in the wap between a cell and its neighbours based on index
        private void tearDownAWall(MazeCell cell, int neighbourIndex)
        {
            //turn the index into a direction
            int direction = neighbourIndex;
            //get the opposite direction for neighbour's wall
            int backDirection = getOppositeDirection(direction);
            //get rid of walls
            cell.Walls[direction] = false;
            cell.Neighbours[neighbourIndex].Walls[backDirection] = false;

        }


        //gets opposite direction value for tearing down both walls
        private int getOppositeDirection(int direction)
        {
            switch (direction)
            {
                case Constants.NORTH:
                    return Constants.SOUTH;

                case Constants.EAST:
                    return Constants.WEST;

                case Constants.SOUTH:
                    return Constants.NORTH;

                case Constants.WEST:
                    return Constants.EAST;

                default:
                    return -1;
            }
        }

        //add nodes to graph node list
        private void generateCells()
        {
            //add nodes to main list
            for (int i = 0; i < CellCount; i++)
            {
                MazeCell mCell = new MazeCell(rand);
                Cells.Add(mCell);
            }

            int index = 0;
            //build the grid from same nodes as node list
            for (int row = 0; row < gridWallSize; row++)
            {
                for (int col = 0; col < gridWallSize; col++)
                {
                    //apply node's row/column
                    Cells[index].Row = row;
                    Cells[index].Col = col;
                    //apply node to grid
                    grid[row, col] = Cells[index];
                    index++; //next node
                }
            }

            //build neighbours for every node
            for (int row = 0; row < gridWallSize; row++)
            {
                for (int col = 0; col < gridWallSize; col++)
                {

                    //calculate ajacent nodes
                    int west = col - 1;
                    int east = col + 1;
                    int north = row - 1;
                    int south = row + 1;

                    //if statements stop adding neighbours outside edges of grid
                    //north
                    if (north >= 0 && north < gridWallSize)
                        grid[row, col].Neighbours[Constants.NORTH] = grid[north, col];

                    //east
                    if (east >= 0 && east < gridWallSize)
                        grid[row, col].Neighbours[Constants.EAST] = grid[row, east];

                    //south
                    if (south >= 0 && south < gridWallSize)
                        grid[row, col].Neighbours[Constants.SOUTH] = grid[south, col];

                    //west
                    if (west >= 0 && west < gridWallSize)
                        grid[row, col].Neighbours[Constants.WEST] = grid[row, west];


                }
            }
        }
    }
}
