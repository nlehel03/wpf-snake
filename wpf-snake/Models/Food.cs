using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace wpf_snake.Models
{
    public class Food
    {
        Random r = new Random();
        public Point position { get; private set; }
        
        public Food(Point p)
        {
            position = p;
        }
        public void Respawn(int n, List<Point> snakeBody)
        {
            
            Point newPosition;
            do
            {
                newPosition = new Point(r.Next(0, n), r.Next(0, n));
            } while (snakeBody.Contains(newPosition));
            position = newPosition;
        }
    }
}
