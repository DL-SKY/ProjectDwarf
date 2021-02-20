using ProjectDwarf.Enums;
using UnityEngine;

namespace ProjectDwarf.WorldGeneration.Biomes
{
    public class StoneBiomeGenerator : BiomeGenerator
    {
        private WorldNoiseGenerator worldNoise;


        public StoneBiomeGenerator(int[,] _world, WorldNoiseGenerator _worldNoise) : base(_world)
        {
            worldNoise = _worldNoise;
        }


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

        public int[,] GenerateStoneNoise(int[,] _world, Vector2Int _stoneInNoise)
        {
            for (int x = 0; x < worldWidth; x++)
                for (int y = 0; y < worldHeight; y++)
                {
                    var noise = worldNoise.GetNoiseInt100(x, y);
                    if (noise >= _stoneInNoise.x && noise <= _stoneInNoise.y)
                    {
                        var cell = (EnumResources)_world[x, y];
                        switch (cell)
                        {
                            case EnumResources.Dirt:
                                _world[x, y] = (int)EnumResources.Stone;
                                break;
                            case EnumResources.Stone:
                                _world[x, y] = (int)EnumResources.Dirt;
                                break;
                        }
                    }
                }

            return _world;
        }
    }
}
