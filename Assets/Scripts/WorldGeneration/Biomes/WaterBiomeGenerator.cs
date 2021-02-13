using ProjectDwarf.Enums;
using System;
using UnityEngine;

namespace ProjectDwarf.WorldGeneration.Biomes
{
    public class WaterBiomeGenerator
    {
        private int worldWidth;
        private int worldHeight;

        private int maxWaterInNoise;
        private WorldNoiseGenerator worldNoise;


        public int[,] GenerateWaterInTop(int[,] _world, int _maxWaterInNoise, WorldNoiseGenerator _worldNoise)
        {
            var start = DateTime.Now;

            worldWidth = _world.GetLength(0);
            worldHeight = _world.GetLength(1);

            maxWaterInNoise = _maxWaterInNoise;
            worldNoise = _worldNoise;

            for (int x = 0; x < worldWidth; x++)
                for (int y = 0; y < worldHeight; y++)
                    if (_worldNoise.GetNoiseInt100(x, y) <= _maxWaterInNoise && CheckWaterZoneInDirt(_world, x, y))
                        _world[x, y] = (int)EnumResources.Water;

            UnityEngine.Debug.Log("Water gen: " + (DateTime.Now - start).TotalMilliseconds);

            return _world;
        }


        private bool CheckWaterZoneInDirt(int[,] _world, int _x, int _y)
        {
            var checkMatrix = new int[worldWidth, worldHeight];
            var isVoidUp = CheckVoidUpWaterZone(_world, _x, _y, checkMatrix);

            checkMatrix = new int[worldWidth, worldHeight];
            var isWaterOrDirt = CheckWaterOrDirtWaterZone(_world, _x, _y, checkMatrix);

            return isWaterOrDirt && isVoidUp;
        }

        Func<int[,], int, int, bool> CheckValidMatrixElement = (_world, _x, _y) => { return _x >= 0 && _world.GetLength(0) > _x && _y >= 0 && _world.GetLength(1) > _y; };

        private bool CheckVoidUpWaterZone(int[,] _world, int _x, int _y, int[,] _checkMatrix)
        {
            _checkMatrix[_x, _y] = 1;            

            if (CheckValidMatrixElement(_world, _x, _y + 1) && _world[_x, _y + 1] == (int)EnumResources.Void)
            {
                return true;
            }
            else
            {
                var arrDirections = new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left};

                foreach (var dir in arrDirections)
                    if (CheckValidMatrixElement(_world, _x + dir.x, _y + dir.y)
                        && worldNoise.GetNoiseInt100(_x + dir.x, _y + dir.y) <= maxWaterInNoise
                        && _checkMatrix[_x + dir.x, _y + dir.y] < 1)
                        if (CheckVoidUpWaterZone(_world, _x + dir.x, _y + dir.y, _checkMatrix))
                            return true;                
            }

            return false;
        }

        Func<int[,], int, int, bool> CheckWaterOrDirt = (_world, _x, _y) => { return _world[_x, _y] == (int)EnumResources.Water || _world[_x, _y] == (int)EnumResources.Dirt; };

        private bool CheckWaterOrDirtWaterZone(int[,] _world, int _x, int _y, int[,] _checkMatrix)
        {
            _checkMatrix[_x, _y] = 1;

            var arrDirections = new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

            foreach (var dir in arrDirections)
                if (CheckValidMatrixElement(_world, _x + dir.x, _y + dir.y))
                {
                    if (CheckWaterOrDirt(_world, _x + dir.x, _y + dir.y))
                        return true;
                    else if (worldNoise.GetNoiseInt100(_x + dir.x, _y + dir.y) <= maxWaterInNoise
                        && _checkMatrix[_x + dir.x, _y + dir.y] < 1)
                        if (CheckWaterOrDirtWaterZone(_world, _x + dir.x, _y + dir.y, _checkMatrix))
                            return true;
                }               

            return false;
        }
    }
}
