namespace CheckersGame
{
    public static class GameConstants
    {
        public const int mapSize = 8;
        public const int cellSize = 50;
        public const string kingIcon = "♚";
        
        public static int[,] InitializeBoard()
        {
            int[,] map = new int[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if ((i + j) % 2 != 0 && i < mapSize / 2 - 1) map[i, j] = 1;
                    else if ((i + j) % 2 != 0 && i > mapSize / 2) map[i, j] = 2;
                    else map[i, j] = 0;
                }
            }

            return map;
        }
    }
}