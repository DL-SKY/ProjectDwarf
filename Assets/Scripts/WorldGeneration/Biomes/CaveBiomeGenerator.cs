using ProjectDwarf.Enums;

namespace ProjectDwarf.WorldGeneration.Biomes
{
    public class CaveBiomeGenerator : BiomeGenerator
    {
        private WorldNoiseGenerator worldNoise;


        public CaveBiomeGenerator(int[,] _world, WorldNoiseGenerator _worldNoise) : base(_world) 
        {
            worldNoise = _worldNoise;
        }


        public int[,] GenerateCaves(int[,] _world, int _minCaveInNoise)
        {
            for (int x = 0; x < worldWidth; x++)
                for (int y = 0; y < worldHeight; y++)
                {
                    if (worldNoise.GetNoiseInt100(x, y) >= _minCaveInNoise)
                        _world[x, y] = (int)EnumResources.Void;
                }

            return _world;
        }
    }
}
