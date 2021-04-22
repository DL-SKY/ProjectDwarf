using System;

namespace ProjectDwarf.WorldGeneration.Biomes
{
    public abstract class BiomeGenerator
    {
        protected int worldWidth;
        protected int worldHeight;


        protected Func<int[,], int, int, bool> CheckValidMatrixElement = (_world, _x, _y) => { return _x >= 0 && _world.GetLength(0) > _x && _y >= 0 && _world.GetLength(1) > _y; };


        public BiomeGenerator(int[,] _world) 
        {
            worldWidth = _world.GetLength(0);
            worldHeight = _world.GetLength(1);
        }


        protected int[] GetSmoothing(int[] _origin, int _avrLenght)
        {
            for (int i = 1; i < _origin.Length - 1; i++)
            {
                float sum = _origin[i];
                int count = 1;
                for (int k = 1; k <= _avrLenght; k++)
                {
                    int i1 = i - k;
                    int i2 = i + k;

                    if (i1 > 0)
                    {
                        sum += _origin[i1];
                        count++;
                    }

                    if (i2 < _origin.Length)
                    {
                        sum += _origin[i2];
                        count++;
                    }
                }

                _origin[i] = (int)(sum / count);
            }

            return _origin;
        }
    }
}