using ProjectDwarf.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDwarf.WorldGeneration.Biomes
{
    public class BorderBiomeGenerator : BiomeGenerator
    {
        public BorderBiomeGenerator(int[,] _world) : base(_world) { }


        public int[,] GenerateBorder(int[,] _world)
        {
            //Лава
            _world = GenerateLava(_world);

            //Граничная порода
            _world = GenerateFrontier(_world);

            return _world;
        }


        private int[,] GenerateLava(int[,] _world)
        {
            var y = 0;
            for (int x = 0; x < worldWidth; x++)
                _world[x, y] = (int)EnumResources.Lava;

            return _world;
        }

        private int[,] GenerateFrontier(int[,] _world)
        {
            var xL = 0;
            var xR = worldWidth - 1;
            var topY = 0;

            for (int y = worldHeight - 1; y >= 0; y--)
            {
                if (_world[xL, y] > 0 && _world[xR, y] > 0 && topY <= 0)
                    topY = y;
                else if (topY <= 0)
                    continue;

                _world[xL, y] = (int)EnumResources.FrontierStone;
                _world[xR, y] = (int)EnumResources.FrontierStone;
            }

            return _world;
        }
    }
}
