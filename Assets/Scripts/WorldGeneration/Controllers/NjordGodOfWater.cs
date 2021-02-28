using ProjectDwarf.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectDwarf.WorldGeneration.Controllers
{
    public class NjordGodOfWater
    {
        private int worldWidth;
        private int worldHeight;


        protected Func<int, int, int, int, bool> CheckValidMatrixElement = (_x, _y, _maxX, _maxY) => { return _x >= 0 && _maxX > _x && _y >= 0 && _maxY > _y; };


        public NjordGodOfWater(int[,] _world)
        {
            worldWidth = _world.GetLength(0);
            worldHeight = _world.GetLength(1);
        }


        public int[,] GetTickWaterSimulate(int[,] _world)
        {
            for (int x = 0; x < worldWidth; x++)
                for (int y = 0; y < worldHeight; y++)
                    if (_world[x, y] == (int)EnumResources.Water)
                        Simulate(x, y, _world);

            return _world;
        }


        private void Simulate(int _x, int _y, int[,] _world)
        {
            var arrDirections = new Vector2Int[] { Vector2Int.down };
            //var arrDirections = new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left};

            foreach (var dir in arrDirections)
            {
                var newX = _x + dir.x;
                var newY = _y + dir.y;

                if (CheckValidMatrixElement(newX, newY, worldWidth, worldHeight))
                {
                    if (_world[newX, newY] <= (int)EnumResources.Void)
                    {
                        _world[newX, newY] = _world[_x, _y];
                        _world[_x, _y] = (int)EnumResources.Void;
                    }
                }                
            }
        }
    }
}
