using System;
using System.Collections.Generic;
using System.Drawing;

namespace wpf_snake.Models
{
    public class GameState
    {
        public int n { get; }
        private readonly Random r = new Random();

        public Snake snake { get; }
        public Food food { get; private set; }
        public int score { get; private set; }
        public bool isGameOver { get; private set; }

        
        public IReadOnlyList<Point> SnakeSegments => snake.body;

        public GameState(int n)
        {
            this.n = n;
            Point startPosition = new Point(n / 2, n / 2);
            snake = new Snake(startPosition);
            food = new Food(new Point(r.Next(0, n), r.Next(0, n)));
            score = 0;
            isGameOver = false;
        }

        public void SetDirection(Direction d) => snake.direction = d;

        public void Update()
        {
            if (isGameOver) return;

            bool grow = snake.body[0].Equals(food.position);
            snake.Move(grow);

            if (grow)
            {
                score++;
                food.Respawn(n, snake.body);
            }

            if (snake.IsCollisionWithSelf() || IsCollisionWithWall())
            {
                isGameOver = true;
            }
        }

        private bool IsCollisionWithWall()
        {
            Point head = snake.body[0];
            return head.X < 0 || head.X >= n || head.Y < 0 || head.Y >= n;
        }
    }
}
