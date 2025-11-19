using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace wpf_snake.Models
{
    public class Snake
    {
        public List<Point> body { get; private set; }
        public Direction direction { get; set; }

        public Snake(Point startPosition)
        {
            body = new List<Point> { startPosition };
            var random = new Random();
            direction = (Direction)Enum.GetValues(typeof(Direction)).GetValue(random.Next(4));
            if(direction == Direction.Up)
            {
                for(int i = 0; i < 4; i++)
                {
                    body.Add(new Point(startPosition.X, startPosition.Y + i));
                }
            }
            else if(direction == Direction.Down)
            {
                for (int i = 0; i < 4; i++)
                {
                    body.Add(new Point(startPosition.X, startPosition.Y - i));
                }
            }
            else if(direction == Direction.Left)
            {
                for (int i = 0; i < 4; i++)
                {
                    body.Add(new Point(startPosition.X+i, startPosition.Y));
                }
            }
            else if(direction == Direction.Right)
            {
                for (int i = 0; i < 4; i++)
                {
                    body.Add(new Point(startPosition.X-i, startPosition.Y));
                }
            }
        }
        public void Move( bool grow = false)
        {
            Point head = body[0];
            Point newHead = head;
            switch (direction)
            {
                case Direction.Up: newHead.Y-=1; break;
                case Direction.Down: newHead.Y += 1; break;
                case Direction.Left: newHead.X -= 1; break;
                case Direction.Right: newHead.X += 1; break;
            }
            Debug.WriteLine($"Moving {direction}, new head at {newHead}");
            body.Insert(0, newHead);
            if(!grow)
            {
                body.RemoveAt(body.Count - 1);
            }
        }
        public bool IsCollisionWithSelf()
        {
            Point head = body[0];
            return body.Skip(1).Any(segment => segment.X == head.X && segment.Y == head.Y);
        }

    }
}
