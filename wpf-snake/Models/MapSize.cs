using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_snake.Models
{
    public class MapSize
    {
        public int cellSize;
        public int n;
        public MapSize(int c, int n)
        {
            cellSize = c;
            this.n = n;
        }
    }
}
