using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public class Game: Control
    {
        public int fieldSize;
        public int cellSize;
        private Snake snake;
        public Timer updateTimer;
        public Game()
        {
            this.DoubleBuffered = true;

            fieldSize = 15;
            cellSize = 50;
            //cellSize = 500 / fieldSize;

            Restart();
            snake.OnGameOver += () => Restart();
            
            updateTimer = new Timer();
            updateTimer.Interval = 150;
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Enabled = true;
        }

        private void Restart()
        {
            snake = new Snake(fieldSize);
            snake.OnGameOver += () => { Restart(); };
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            snake.Moving();
            Invalidate();
        }

        public void KeyboardListener(object sender, KeyEventArgs e)
        {
            snake.KeyboardMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            bool dark = true;
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    e.Graphics.FillRectangle(new SolidBrush((dark ? Color.DarkGoldenrod : Color.Goldenrod)), new Rectangle(
                            new Point(
                                    cellSize * j,
                                    cellSize * i
                                ), 
                            new Size(
                                    cellSize,
                                    cellSize
                                )
                        ));
                    dark = !dark;
                }
                
            }
            //for (int i = 0; i < fieldSize; i++)
            //{
            //    e.Graphics.DrawLine(new Pen(new SolidBrush(Color.White), 2), cellSize * i, 0, cellSize * i, cellSize * fieldSize);
            //}
            //for (int i = 0; i < fieldSize; i++)
            //{
            //    e.Graphics.DrawLine(new Pen(new SolidBrush(Color.White), 2), 0, cellSize * i, cellSize * fieldSize, cellSize * i);
            //}
            //DrawBerry(e, cellSize * snake.apple.coordinates);
            e.Graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(
                        new Point(
                                cellSize * snake.apple.coordinates.X + 7,
                                cellSize * snake.apple.coordinates.Y + 7
                            ),
                        new Size(
                                cellSize - 14,
                                cellSize - 14
                            )
                    ));
            if (snake.TailCells.Count > 0)
            {
                int maxSize = cellSize - cellSize / 3;
                int minSize = cellSize / 8;
                int oneCellSize = (maxSize - minSize) / snake.TailCells.Count;
                int currentSize = maxSize;
                for (int i = 0; i < snake.TailCells.Count; i++)
                {
                    e.Graphics.DrawLine(
                    new Pen(
                        new SolidBrush(Color.DarkGreen),
                        currentSize
                        ),
                    (i != 0 ? snake.TailCells[i - 1].X * cellSize + cellSize / 2 : snake.HeadCell.X * cellSize + cellSize / 2),
                    (i != 0 ? snake.TailCells[i - 1].Y * cellSize + cellSize / 2 : snake.HeadCell.Y * cellSize + cellSize / 2),
                    snake.TailCells[i].X * cellSize + cellSize / 2,
                    snake.TailCells[i].Y * cellSize + cellSize / 2
                    );
                    if (oneCellSize != 0)
                        currentSize -= oneCellSize;
                }
            }

            int xOffset = -5;
            int yOffset = -5;
            int betweenEyes = 6;
            int farEyes = 3;
            e.Graphics.FillRectangle(new SolidBrush(Color.DarkGreen), new Rectangle(
                        new Point(
                                cellSize * snake.HeadCell.X + cellSize / 10,
                                cellSize * snake.HeadCell.Y + cellSize / 10
                            ),
                        new Size(
                                cellSize - cellSize / 10 * 2,
                                cellSize - cellSize / 10 * 2
                            )
                    ));
            Point head = new Point(cellSize * snake.HeadCell.X /*+ cellSize / 2*/, cellSize * snake.HeadCell.Y /*+ cellSize / 2*/);
            switch (snake.direction)
            {
                case 1:
                    DrawEye(e, new Point(
                        head.X + (cellSize / betweenEyes) + xOffset,
                        head.Y + (cellSize / farEyes) - yOffset
                        ));
                    DrawEye(e, new Point(
                        head.X + (cellSize - (cellSize / betweenEyes)) + xOffset,
                        head.Y + (cellSize / farEyes) - yOffset
                        ));
                    break;
                case 2:
                    DrawEye(e, new Point(
                        head.X + (cellSize / farEyes) + xOffset,
                        head.Y + (cellSize / betweenEyes) + yOffset
                        ));
                    DrawEye(e, new Point(
                        head.X + (cellSize / farEyes) + xOffset,
                        head.Y + (cellSize - (cellSize / betweenEyes)) + yOffset
                        ));
                    break;
                case 3:
                    DrawEye(e, new Point(
                        head.X + (cellSize / betweenEyes) + xOffset,
                        head.Y + (cellSize / farEyes) + yOffset
                        ));
                    DrawEye(e, new Point(
                        head.X + (cellSize - (cellSize / betweenEyes)) + xOffset,
                        head.Y + (cellSize / farEyes) + yOffset
                        ));
                    break;
                case 4:
                    DrawEye(e, new Point(
                        head.X + (cellSize / farEyes) - xOffset,
                        head.Y + (cellSize / betweenEyes) + yOffset
                        ));
                    DrawEye(e, new Point(
                        head.X + (cellSize / farEyes) - xOffset,
                        head.Y + (cellSize - (cellSize / betweenEyes)) + yOffset
                        ));
                    break;
            }
        }

        public void DrawBerry(PaintEventArgs e, Point point)
        {
            e.Graphics.FillEllipse(new SolidBrush(Color.Red), new Rectangle(
                        new Point(
                                cellSize * snake.apple.coordinates.X + 7,
                                cellSize * snake.apple.coordinates.Y + 7
                            ),
                        new Size(
                                cellSize - 14,
                                cellSize - 14
                            )
                    ));
        }
        
        public void DrawEye(PaintEventArgs e, Point point)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(
                        new Point(
                                point.X,
                                point.Y
                            ),
                        new Size(
                                10,
                                10
                            )
                    ));
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(
                        new Point(
                                point.X + 1,
                                point.Y + 1
                            ),
                        new Size(
                                8,
                                8
                            )
                    ));
        }
    }

    public class Snake
    {
        public delegate void GameEventHandler();
        public event GameEventHandler OnGameOver;

        public Point HeadCell { get; set; }
        public List<Point> TailCells { get; set; }

        public int direction;

        private int fieldSize;
        private bool sizeIncreases;
        private bool canChangeDirection;

        private Point preHeadCell;
        public Apple apple { get; }

        public Snake(int fieldSize)
        {
            this.fieldSize = fieldSize;
            direction = 2;
            TailCells = new List<Point>();
            HeadCell = new Point(fieldSize / 2, fieldSize / 2);
            preHeadCell = new Point(fieldSize / 2 - 1, fieldSize / 2);
            apple = new Apple(fieldSize);
            apple.OnAppleEaten += LengthIncrease;
        }

        public void LengthIncrease()
        {
            TailCells.Add((TailCells.Count > 1 ? TailCells[TailCells.Count- 1] : HeadCell));
            sizeIncreases = true;
        }

        private void CheckApple()
        {
            if (HeadCell.X == apple.coordinates.X & HeadCell.Y == apple.coordinates.Y)            {
                apple.SendAppleEaten();
            }
        }

        private void CheckCollisions()
        {
            if (TailCells.Contains(HeadCell))            {
                OnGameOver.Invoke();
            }
        }

        public void Moving()
        {
            CheckCollisions();
            CheckApple();
            preHeadCell = HeadCell;
            switch (direction)
            {
                case 1:
                    HeadCell = new Point(HeadCell.X, HeadCell.Y - 1);
                  break;
                case 2:
                    HeadCell = new Point(HeadCell.X + 1, HeadCell.Y);
                   break;
                case 3:
                    HeadCell = new Point(HeadCell.X, HeadCell.Y + 1);
                  break;
                case 4:
                    HeadCell = new Point(HeadCell.X - 1, HeadCell.Y);
                   break;
            }

            if (HeadCell.X >= fieldSize) OnGameOver.Invoke();;
            if (HeadCell.X < 0) OnGameOver.Invoke(); ;
            if (HeadCell.Y >= fieldSize) OnGameOver.Invoke(); ;
            if (HeadCell.Y < 0) OnGameOver.Invoke(); ;

            if (TailCells.Count > 0)
                MoveTail();
            canChangeDirection = true;
        }

        private void MoveTail()
        {
            if (TailCells.Count == 1)
            {
                TailCells[0] = preHeadCell;
                return;
            }
            Point bufCell = preHeadCell;
            for (int i = 0; i < TailCells.Count; i++)
            {
               if (i == TailCells.Count - 1 && sizeIncreases)
               {
                    sizeIncreases = false;
                    break;
                }
                Point currentCell = bufCell;
                bufCell = TailCells[i];
                TailCells[i] = new Point(currentCell.X,currentCell.Y);
            }
        }

        public void KeyboardMove(KeyEventArgs e)
        {
            if (!canChangeDirection) return;

            if (e.KeyCode == Keys.W && direction != 3)
                direction = 1;
            if (e.KeyCode == Keys.D && direction != 4)
                direction = 2;
            if (e.KeyCode == Keys.S && direction != 1)
                direction = 3;
            if (e.KeyCode == Keys.A && direction != 2)
                direction = 4;

            canChangeDirection = false;
        }
    }

    public class Apple
    {
        public delegate void AppleEventHandler();
        public event AppleEventHandler OnAppleEaten;

        public Point coordinates { get; set; }
        private int fieldSize;

        public Apple(int fieldSize)
        {
            this.fieldSize = fieldSize;
            OnAppleEaten += Spawn;
            Spawn();
        }

        public void SendAppleEaten()
        {
            OnAppleEaten.Invoke();
        }

        private void Spawn()
        {
            var rand = new Random();
            coordinates = new Point(rand.Next(0, fieldSize), rand.Next(0, fieldSize));
        }
    }
}
