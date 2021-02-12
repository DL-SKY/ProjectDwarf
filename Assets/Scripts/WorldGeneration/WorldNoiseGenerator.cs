using System;
using UnityEngine;

namespace ProjectDwarf.WorldGeneration
{
    [Serializable]
    public class WorldNoiseGenerator
    {
        public bool IsInit { private set; get; }

        [SerializeField] private WorldNoiseData data;
        private int[,] noise;


        public WorldNoiseGenerator(WorldNoiseData _data)
        {
            Initialize(_data);
        }


        public void OnDrawGizmos()
        {
            if (noise == null || data == null)
                return;

            var offset = 0.5f;

            for (int x = 0; x < noise.GetLength(0); x++)
                for (int y = 0; y < noise.GetLength(1); y++)
                {
                    var value = noise[x, y];

                    // =1=
                    //if (value > minNoiseUnVisible && value < 49)
                    //    continue;
                    //if (value > 52 && value < maxNoiseUnVisible)
                    //    continue;
                    //
                    //if (value <= minNoiseUnVisible)
                    //    Gizmos.color = Color.red;
                    //else if (value >= maxNoiseUnVisible)
                    //    Gizmos.color = Color.green;
                    //else
                    //    Gizmos.color = Color.yellow;

                    // =2=
                    if (value < data.minNoiseVisible || value > data.maxNoiseVisible)
                        continue;
                    Gizmos.color = Color.red;

                    Gizmos.DrawCube(new Vector3(x + offset, y + offset, 0), Vector3.one);
                }
        }


        public void Initialize(WorldNoiseData _data)
        {
            data = _data;
            noise = GenarateNoiseMatrix();            

            IsInit = true;
        }

        public float GetNoise(int x, int y)
        {
            var noiseX = (float)x / data.width * data.noiseScale + data.noiseOffsetX;
            var noiseY = (float)y / data.height * data.noiseScale + data.noiseOffsetY;
            return Mathf.PerlinNoise(noiseX, noiseY);
        }

        public int GetNoiseInt100(int _x, int _y)
        {
            return Mathf.RoundToInt(GetNoise(_x, _y) * 100);
        }


        private int[,] GenarateNoiseMatrix()
        {
            var matrix = new int[data.width, data.height];

            for (int x = 0; x < data.width; x++)
                for (int y = 0; y < data.height; y++)
                    matrix[x, y] = GetNoiseInt100(x, y);

            return matrix;
        }
    }
}
