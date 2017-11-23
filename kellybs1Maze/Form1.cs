using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kellybs1Maze
{
    public partial class MazeForm : Form
    {
        private Graphics canvas;
        private MazeController mazeCon;
        private int cellSize;
        public MazeForm()
        {
            InitializeComponent();
        }

        private void MazeForm_Load(object sender, EventArgs e)
        {
            buttonSolve.Enabled = false;
            canvas = panelDraw.CreateGraphics();
            cellSize = (int)numericUpDown1.Value;
            mazeCon = new MazeController(canvas, cellSize);
        }

        //runs the maze generation
        private void buttonGo_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            canvas.Clear(Color.White);
            //get cellsize and make new maze
            cellSize = (int)numericUpDown1.Value;
            mazeCon = new MazeController(canvas, cellSize);

            mazeCon.DrawMaze();

            Cursor = Cursors.Default;
            //allow solving now
            buttonSolve.Enabled = true;
        }

        //runs the maze solving
        private void buttonSolve_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            mazeCon.SolveMaze();

            Cursor = Cursors.Default;
        }
    }
}
