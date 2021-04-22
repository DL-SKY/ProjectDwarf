using System;

namespace ProjectDwarf.WorldGeneration
{
    public static class StaticRandom
    {
        public static int Seed { private set; get; }
        public static bool IsInit { private set; get; }

        private static System.Random random;


        public static void Initialize(int _seed = -1)
        {
            Seed = _seed < 0 ? (int)DateTime.Now.Ticks : _seed;
            random = new System.Random(Seed);

            IsInit = true;
        }

        public static int Next(int _minValueIn, int _maxValueEx)
        {
            CheckInit();
            return random.Next(_minValueIn, _maxValueEx);
        }

        public static float Next01()
        {
            CheckInit();
            return random.Next(0, 10000) / 10000.0f;
        }

        public static int Next()
        {
            CheckInit();
            return random.Next();
        }


        private static bool CheckInit()
        {
            if (!IsInit)
            {
                Initialize();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
