using System;

namespace wpf_snake.Models
{
    public class Scores
    {
        public string name { get; set; }
        public int score { get; set; }

        public Scores()
        {
            name = "";
            score = 0;
        }

        public Scores(string n, int s)
        {
            name = n;
            score = s;
        }
    }
}
