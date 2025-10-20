using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersGame
{
    public partial class BoardForm : Form
    {
        public BoardForm()
        {
            InitializeComponent();

            whiteFigure = CreateCheckerFigure(Color.White, Color.Black);
            blackFigure = CreateCheckerFigure(Color.Black, Color.White);

            this.Text = "Checkers";

            Init();
        }

        private Bitmap CreateCheckerFigure(Color mainColor, Color borderColor)
        {
            int size = GameConstants.cellSize - 10;
            var bitmap = new Bitmap(size, size);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (var mainBrush = new SolidBrush(mainColor))
                {
                    g.FillEllipse(mainBrush, 2, 2, size - 4, size - 4);
                }

                using (var outerPen = new Pen(Color.Black, 2))
                {
                    g.DrawEllipse(outerPen, 2, 2, size - 4, size - 4);
                }

                using (var innerPen = new Pen(borderColor, 1))
                {
                    g.DrawEllipse(innerPen, 4, 4, size - 8, size - 8);
                }
            }

            return bitmap;
        }

        public void CreateMap()
        {
            this.Width = (GameConstants.mapSize + 1) * GameConstants.cellSize;
            this.Height = (GameConstants.mapSize + 1) * GameConstants.cellSize;

            for (int i = 0; i < GameConstants.mapSize; i++)
            {
                for (int j = 0; j < GameConstants.mapSize; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * GameConstants.cellSize, i * GameConstants.cellSize);
                    button.Size = new Size(GameConstants.cellSize, GameConstants.cellSize);
                    button.Click += new EventHandler(OnFigurePress);
                    if (map[i, j] == 1)
                        button.Image = whiteFigure;
                    else if (map[i, j] == 2) button.Image = blackFigure;

                    button.BackColor = GetPrevButtonColor(button);
                    button.ForeColor = Color.Red;

                    buttons[i, j] = button;

                    this.Controls.Add(button);
                }
            }
        }
        public Color GetPrevButtonColor(Button prevButton)
        {
            if ((prevButton.Location.Y/GameConstants.cellSize % 2) != 0)
            {
                if ((prevButton.Location.X / GameConstants.cellSize % 2) == 0)
                {
                    return Color.Gray;
                }
            }
            if ((prevButton.Location.Y / GameConstants.cellSize) % 2 == 0)
            {
                if ((prevButton.Location.X / GameConstants.cellSize) % 2 != 0)
                {
                    return Color.Gray;
                }
            }
            return Color.White;
        }
    }

}