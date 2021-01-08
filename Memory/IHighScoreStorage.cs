using System.Collections.Generic;

namespace Memory
{
    public interface IHighScoreStorage
    {
        void AddEntry(int Tiles, double totalMilliseconds, string PlayerName);
        List<Score> ReadHighscore(int Tiles);
    }
}
