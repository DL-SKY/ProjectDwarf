using ProjectDwarf.Enums;
using UnityEngine;

namespace ProjectDwarf.WorldGeneration.Biomes
{
    public class SandBiomeGenerator : BiomeGenerator
    {
        private WorldNoiseGenerator worldNoise;

        private Vector2Int sandWithSandInNoise;


        public SandBiomeGenerator(int[,] _world, WorldNoiseGenerator _worldNoise) : base(_world) 
        {
            worldNoise = _worldNoise;
        }


        public int[,] GenerateSandInTop(int[,] _world, Vector2Int _sandInTopInNoise, int _minHeight)
        {
            for (int x = 0; x < worldWidth; x++)
                for (int y = _minHeight; y < worldHeight; y++)
                {
                    var noise = worldNoise.GetNoiseInt100(x, y);
                    if (noise >= _sandInTopInNoise.x && noise <= _sandInTopInNoise.y)
                        if (_world[x, y] >= (int)EnumResources.Dirt)
                            if (CheckWater(_world, x, y))
                            _world[x, y] = GetSandlikeBlock(_world[x, y]);
                }                    

            return _world;
        }

        public int[,] GenerateSandInNoise(int[,] _world, Vector2Int _sandInNoise)
        {
            for (int x = 0; x < worldWidth; x++)
                for (int y = 0; y < worldHeight; y++)
                {
                    var noise = worldNoise.GetNoiseInt100(x, y);
                    if (noise >= _sandInNoise.x && noise <= _sandInNoise.y)
                        if (_world[x, y] >= (int)EnumResources.Dirt)
                            _world[x, y] = GetSandlikeBlock(_world[x, y]);
                }

            return _world;
        }

        public int[,] GenerateSandWithSandInNoise(int[,] _world, Vector2Int _sandWithSandInNoise)
        {
            sandWithSandInNoise = _sandWithSandInNoise;

            for (int x = 0; x < worldWidth; x++)
                for (int y = 0; y < worldHeight; y++)
                {
                    var noise = worldNoise.GetNoiseInt100(x, y);
                    if (noise >= sandWithSandInNoise.x && noise <= sandWithSandInNoise.y)
                        if (_world[x, y] >= (int)EnumResources.Dirt)
                            if (CheckSand(_world, x, y))
                                _world[x, y] = GetSandlikeBlock(_world[x, y]);
                }

            return _world;
        }


        private bool CheckWater(int[,] _world, int _x, int _y)
        {
            var arrDirections = new Vector2Int[] {  Vector2Int.up, new Vector2Int(1, 1), 
                                                    Vector2Int.right,
                                                    Vector2Int.left, new Vector2Int(-1, 1) };

            foreach (var dir in arrDirections)
                if (CheckValidMatrixElement(_world, _x + dir.x, _y + dir.y))
                    if (_world[_x + dir.x, _y + dir.y] == (int)EnumResources.Water)
                        return true;

            return false;
        }

        private int GetSandlikeBlock(int _resource)
        {
            var random = StaticRandom.Next(0, 101);
            var maxValueForSand = 80;

            switch ((EnumResources)_resource)
            {
                case EnumResources.Stone:
                    maxValueForSand = 40;
                    break;
                case EnumResources.VolcanicDirt:
                case EnumResources.VolcanicStone:
                    maxValueForSand = 10;
                    break;
            }

            return random <= maxValueForSand ? (int)EnumResources.Sand : (int)EnumResources.SandStone;
        }

        private bool CheckSand(int[,] _world, int _x, int _y)
        {
            var arrDirections = new Vector2Int[] {  Vector2Int.up, new Vector2Int(1, 1),
                                                    Vector2Int.right, new Vector2Int(1, -1),
                                                    Vector2Int.down, new Vector2Int(-1, -1),
                                                    Vector2Int.left, new Vector2Int(-1, 1) };

            foreach (var dir in arrDirections)
            {
                var x = _x + dir.x;
                var y = _y + dir.y;
                var noise = worldNoise.GetNoiseInt100(x, y);

                if (CheckValidMatrixElement(_world, x, y))
                    if (_world[x, y] == (int)EnumResources.Sand
                        || _world[x, y] == (int)EnumResources.SandStone
                        || (noise >= sandWithSandInNoise.x && noise <= sandWithSandInNoise.y))
                        return true;
            }

            return false;
        }
    }
}
