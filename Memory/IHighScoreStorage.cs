using System.Collections.Generic;

namespace Memory
{
    public interface IHighScoreStorage
    {
        void AddEntryToDatabase(int Tiles, double totalMilliseconds, string PlayerName);
        List<Score> ReadHighscoreFromDatabase(int Tiles);
    }
}
