using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    public partial class Form1 : Form
    {

        private List<circle> snake = new List<circle>();
        private circle food = new circle();

        public Form1()
        {
            InitializeComponent();

            // Set settings to default 
            new Settings();

            // Set game speed and timer 
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            // Start a new game
            StartGame();
        }

        private void StartGame()
        {

            lblGameOver.Visible = false;

            new Settings();

            snake.Clear();
            circle head = new circle();
            head.x = 10;
            head.y = 5;
            snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        // Place random food object on the screen 
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new circle();
            food.x = random.Next(0, maxXPos);
            food.y = random.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            // Check for a game over
            if(Settings.GameOver == true)
            {
                // Check for enter key
                if(Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if(Settings.GameOver == false)
            {
                // Set snake color
                Brush snakeColor;

                // Draw the snake
                for(int i = 0; i < snake.Count; i++)
                {

                    if (i == 0)
                        snakeColor = Brushes.Black;
                    else
                        snakeColor = Brushes.Blue;

                    canvas.FillEllipse(snakeColor,
                        new Rectangle(
                            snake[i].x * Settings.Width,
                            snake[i].y * Settings.Height,
                            Settings.Width,
                            Settings.Height
                            ));

                    // Draw the food 
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(
                            food.x * Settings.Width,
                            food.y * Settings.Height,
                            Settings.Width,
                            Settings.Height
                            ));

                }
            }
            else
            {
                string gameOver = "Game Over \n The final score is:" + Settings.Score + "\n Press Enter to play again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void MovePlayer()
        {
            for(int i = snake.Count - 1; i >= 0; i--)
            {
                // Move the head
                if(i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            snake[i].x++;
                            break;
                        case Direction.Left:
                            snake[i].x--;
                            break;
                        case Direction.Up:
                            snake[i].y--;
                            break;
                        case Direction.Down:
                            snake[i].y++;
                            break;
                    }

                    // Get params for map
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    // Detect collision with borders
                    if (snake[i].x < 0 || snake[i].y < 0
                        || snake[i].x >= maxXPos || snake[i].y >= maxYPos)
                    {
                        Die();
                    }

                    // Detect collision with body 
                    for (int j = 1; j < snake.Count; j++)
                    {
                        if(snake[i].x == snake[j].x &&
                            snake[i].y == snake[j].y)
                        {
                            Die();
                        }
                    }

                    // Detect collision with food peice
                    if (snake[0].x == food.x && snake[0].y == food.y)
                    {
                        Eat();
                    }

                }
                else
                {
                    //Move the body
                    snake[i].x = snake[i - 1].x;
                    snake[i].y = snake[i - 1].y;
                }
            }
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Eat()
        {
            // Add circle to body
            circle food = new circle();
            food.x = snake[snake.Count - 1].x;
            food.y = snake[snake.Count - 1].y;

            snake.Add(food);

            // Update Score
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
