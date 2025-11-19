using wpf_snake.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace wpf_snake.Persistence
{
    public class FileManagement
    {
        List<MapSize> mapSizeList = new List<MapSize>();
        public FileManagement() 
        {
            
        }
        public MapSize loadMapSize(int x)
        {
            StreamReader sr = new StreamReader("mapsize.txt");
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parts = line.Split(';');
                MapSize ms= new MapSize(int.Parse(parts[0]), int.Parse(parts[1]));
                mapSizeList.Add(ms);
            }
            return mapSizeList[x];
        }
        public List<Scores> loadScores()
        {
            List<Scores> scoresList = new List<Scores>();
            StreamReader sr = new StreamReader("scores.txt");
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] temp = line.Split(";");
                if (int.TryParse(temp[1], out int s))
                {
                    scoresList.Add(new Scores(temp[0], s)); 
                }
            }
            sr.Close();
            return scoresList;
        }
        public void saveScore(int s,string n)
        {
            if(n == "")
            {
                n = "Anonymous";
            }
            List<Scores> slist = loadScores();
            slist.Add(new Scores(n,s));
            slist = slist.OrderByDescending(x => x.score).ToList();
            StreamWriter sw = new StreamWriter("scores.txt");
            for(int i = 0; i< slist.Count; i++)
            {
                sw.WriteLine(slist[i].name + ";" + slist[i].score);
            }
            sw.Flush();
            sw.Close();

        }
    }
}
