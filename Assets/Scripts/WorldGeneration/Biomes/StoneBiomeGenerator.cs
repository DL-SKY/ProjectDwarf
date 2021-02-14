using UnityEngine;

namespace ProjectDwarf.WorldGeneration.Biomes
{
    public class StoneBiomeGenerator : BiomeGenerator
    {
        public StoneBiomeGenerator(int[,] _world) : base(_world) { }


        public int[] GenerateStoneBottomLayer(Vector2 _bottomRange)
        {
            int[] arr = new int[worldWidth];
            for (int i = 0; i < worldWidth; i++)
            {
                var minValue = (int)(worldHeight * _bottomRange.x);
                var maxValue = (int)(worldHeight * _bottomRange.y);
                arr[i] = StaticRandom.Next(minValue, maxValue);
            }

            arr = GetSmoothing(arr, 1);

            return arr;
        }
    }
}
