using UnityEngine;

namespace ProjectDwarf.WorldGeneration.Biomes
{
    public class DirtBiomeGenerator : BiomeGenerator
    {
        public DirtBiomeGenerator(int[,] _world) : base(_world) { }


        public int[] GenerateDirtTopLayer(Vector2 _minTopRange, Vector2 _maxTopAdditionalRange)
        {
            int groundLevelMin = StaticRandom.Next((int)(worldHeight * _minTopRange.x), (int)(worldHeight * _minTopRange.y));
            int groundLevelMax = groundLevelMin + StaticRandom.Next((int)(worldHeight * _maxTopAdditionalRange.x), (int)(worldHeight * _maxTopAdditionalRange.y));

            int[] arr = new int[worldWidth];
            for (int i = 0; i < worldWidth; i++)
            {
                int dir = StaticRandom.Next(0, 2) == 1 ? 1 : -1;

                if (i > 0)
                {
                    if (arr[i - 1] + dir < groundLevelMin || arr[i - 1] + dir > groundLevelMax)
                        dir = -dir;

                    arr[i] = arr[i - 1] + dir;
                }
                else
                {
                    arr[i] = StaticRandom.Next(groundLevelMin, groundLevelMax);
                }
            }

            arr = GetSmoothing(arr, 4);

            return arr;
        }

        public int[] GenerateDirtBottomLayer(Vector2 _bottomRange)
        {
            int[] arr = new int[worldWidth];
            for (int i = 0; i < worldWidth; i++)
            {
                var minValue = (int)(worldHeight * _bottomRange.x);
                var maxValue = (int)(worldHeight * _bottomRange.y);
                arr[i] = StaticRandom.Next(minValue, maxValue);
            }

            arr = GetSmoothing(arr, 3);

            return arr;
        }
    }
}