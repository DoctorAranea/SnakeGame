using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            game1.updateTimer.Tick += Game1_OnGameSizeChanged;
        }

        private void Game1_OnGameSizeChanged(object sender, EventArgs e)
        {
            game1.Width = game1.cellSize * game1.fieldSize;
            Width = game1.cellSize * game1.fieldSize + 20;

            game1.Height = game1.cellSize * game1.fieldSize;
            Height = game1.cellSize * game1.fieldSize + 42;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            game1.KeyboardListener(sender, e);
        }
    }
}
