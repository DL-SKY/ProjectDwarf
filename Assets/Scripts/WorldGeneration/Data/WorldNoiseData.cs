using System;

namespace ProjectDwarf.WorldGeneration
{
    [Serializable]
    public class WorldNoiseData
    {
        public int width;
        public int height;

        public float noiseScale;
        public int minNoiseVisible;
        public int maxNoiseVisible;

        public float noiseOffsetX;
        public float noiseOffsetY;

        public WorldNoiseData()
        { 
        
        }
    }
}
