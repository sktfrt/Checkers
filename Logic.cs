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
        int currentPlayer;

        List<Button> simpleSteps = new List<Button>();

        int countEatSteps = 0;
        Button prevButton;
        Button pressedButton;
        bool isContinue = false;

        bool isMoving;

        int[,] map = new int[GameConstants.mapSize, GameConstants.mapSize];

        Button[,] buttons = new Button[GameConstants.mapSize, GameConstants.mapSize];

        Image whiteFigure;
        Image blackFigure;

        public void Init()
        {
            currentPlayer = 1;
            isMoving = false;
            prevButton = null;

            map = GameConstants.InitializeBoard();

            CreateMap();
        }

        public void ResetGame()
        {
            bool player1 = false;
            bool player2 = false;

            for (int i = 0; i < GameConstants.mapSize; i++)
            {
                for (int j = 0; j < GameConstants.mapSize; j++)
                {
                    if (map[i, j] == 1)
                        player1 = true;
                    if (map[i, j] == 2)
                        player2 = true;
                }
            }
            if (!player1 || !player2)
            {
                this.Controls.Clear();
                Init();
            }
        }

        public void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
            ResetGame();
        }

        public void OnFigurePress(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = GetPrevButtonColor(prevButton);

            pressedButton = sender as Button;

            if(map[pressedButton.Location.Y/GameConstants.cellSize, pressedButton.Location.X/GameConstants.cellSize] != 0 && map[pressedButton.Location.Y / GameConstants.cellSize, pressedButton.Location.X / GameConstants.cellSize] == currentPlayer)
            {
                CloseSteps();
                pressedButton.BackColor = Color.Red;
                DeactivateAllButtons();
                pressedButton.Enabled = true;
                countEatSteps = 0;
                if(pressedButton.Text == GameConstants.kingIcon)
                ShowSteps(pressedButton.Location.Y / GameConstants.cellSize, pressedButton.Location.X / GameConstants.cellSize,false);
                else ShowSteps(pressedButton.Location.Y / GameConstants.cellSize, pressedButton.Location.X / GameConstants.cellSize);

                if (isMoving)
                {
                    CloseSteps();
                    pressedButton.BackColor = GetPrevButtonColor(pressedButton);
                    ShowPossibleSteps();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }
            else
            {
                if (isMoving)
                {
                    isContinue = false;
                      if (Math.Abs(pressedButton.Location.X / GameConstants.cellSize - prevButton.Location.X/GameConstants.cellSize) > 1)
                    {
                        isContinue = true;
                        DeleteEaten(pressedButton, prevButton);                        
                    }
                    int temp = map[pressedButton.Location.Y / GameConstants.cellSize, pressedButton.Location.X / GameConstants.cellSize];
                    map[pressedButton.Location.Y / GameConstants.cellSize, pressedButton.Location.X / GameConstants.cellSize] = map[prevButton.Location.Y / GameConstants.cellSize, prevButton.Location.X / GameConstants.cellSize];
                    map[prevButton.Location.Y / GameConstants.cellSize, prevButton.Location.X / GameConstants.cellSize] = temp;
                    pressedButton.Image = prevButton.Image;
                    prevButton.Image = null;
                    pressedButton.Text = prevButton.Text;
                    prevButton.Text = "";
                    SwitchButtonToKing(pressedButton);
                    countEatSteps = 0;
                    isMoving = false;                    
                    CloseSteps();
                    DeactivateAllButtons();
                    if (pressedButton.Text == GameConstants.kingIcon)
                        ShowSteps(pressedButton.Location.Y / GameConstants.cellSize, pressedButton.Location.X / GameConstants.cellSize, false);
                    else ShowSteps(pressedButton.Location.Y / GameConstants.cellSize, pressedButton.Location.X / GameConstants.cellSize);
                    if (countEatSteps == 0 || !isContinue)
                    {
                        CloseSteps();
                        SwitchPlayer();
                        ShowPossibleSteps();
                        isContinue = false;
                    }else if(isContinue)
                    {
                        pressedButton.BackColor = Color.Red;
                        pressedButton.Enabled = true;
                        isMoving = true;
                    }
                }
            }

            prevButton = pressedButton;
        }

        public void SwitchButtonToKing(Button button)
        {
            if (map[button.Location.Y / GameConstants.cellSize, button.Location.X / GameConstants.cellSize] == 1 && button.Location.Y / GameConstants.cellSize == GameConstants.mapSize - 1) 
            {
                button.Text = GameConstants.kingIcon;
                
            }
            if (map[button.Location.Y / GameConstants.cellSize, button.Location.X / GameConstants.cellSize] == 2 && button.Location.Y / GameConstants.cellSize == 0)
            {
                button.Text = GameConstants.kingIcon;
            }
        }

        public void DeleteEaten(Button endButton, Button startButton)
        {
            int count = Math.Abs(endButton.Location.Y / GameConstants.cellSize - startButton.Location.Y / GameConstants.cellSize);
            int startIndexX = endButton.Location.Y / GameConstants.cellSize - startButton.Location.Y / GameConstants.cellSize;
            int startIndexY = endButton.Location.X / GameConstants.cellSize - startButton.Location.X / GameConstants.cellSize;
            startIndexX = startIndexX < 0 ? -1 : 1;
            startIndexY = startIndexY < 0 ? -1 : 1;
            int currCount = 0;
            int i = startButton.Location.Y / GameConstants.cellSize + startIndexX;
            int j = startButton.Location.X / GameConstants.cellSize + startIndexY;
            while (currCount < count-1)
            {
                map[i, j] = 0;
                buttons[i, j].Image = null;
                buttons[i, j].Text = "";
                i += startIndexX;
                j += startIndexY;
                currCount++;
            }

        }
        
        public bool DeterminePath(int ti,int tj)
        {
            
            if (map[ti, tj] == 0 && !isContinue)
            {
                buttons[ti, tj].BackColor = Color.Yellow;
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]);
            }else
            {
                
                if (map[ti, tj] != currentPlayer)
                {
                    if (pressedButton.Text == GameConstants.kingIcon)
                        ShowProceduralEat(ti, tj, false);
                    else ShowProceduralEat(ti, tj);
                }

                return false;
            }
            return true;
        }

        public void CloseSimpleSteps(List<Button> simpleSteps)
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetPrevButtonColor(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }

        public bool IsButtonHasEatStep(int IcurrFigure, int JcurrFigure, bool isOneStep, int[] dir)
        {
            bool eatStep = false;
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (dir[0] == 1 && dir[1] == -1 && !isOneStep)break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j + 1))
                            eatStep = false;
                        else if (map[i - 1, j + 1] != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j < (GameConstants.mapSize - 1))
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (dir[0] == 1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j - 1))
                            eatStep = false;
                        else if (map[i - 1, j - 1] != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < GameConstants.mapSize; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (dir[0] == -1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j - 1))
                            eatStep = false;
                        else if (map[i + 1, j - 1] != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < GameConstants.mapSize; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (dir[0] == -1 && dir[1] == -1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j + 1))
                            eatStep = false;
                        else if (map[i + 1, j + 1] != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j < (GameConstants.mapSize - 1))
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
            return eatStep;
        }

        public void CloseSteps()
        {
            for (int i = 0; i < GameConstants.mapSize; i++)
            {
                for (int j = 0; j < GameConstants.mapSize; j++)
                {
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                }
            }
        }

        public bool IsInsideBorders(int ti,int tj)
        {
            if(ti >= GameConstants.mapSize || tj >= GameConstants.mapSize || ti < 0 || tj < 0)
            {
                return false;
            }
            return true;
        }

        public void ActivateAllButtons()
        {
            for (int i = 0; i < GameConstants.mapSize; i++)
            {
                for (int j = 0; j < GameConstants.mapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }

        public void DeactivateAllButtons()
        {
            for (int i = 0; i < GameConstants.mapSize; i++)
            {
                for (int j = 0; j < GameConstants.mapSize; j++)
                {
                    buttons[i, j].Enabled = false;
                }
            }
        }
    }
}