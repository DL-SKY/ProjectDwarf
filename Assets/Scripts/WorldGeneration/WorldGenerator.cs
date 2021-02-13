using ProjectDwarf.Enums;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectDwarf.WorldGeneration
{
    public class WorldGenerator : MonoBehaviour
    {
        public const int WORLD_WIDTH = 100;        
        public const int WORLD_HEIGHT = 50;

        //Для Шума
        public float NOISE_SCALE = 18.0f;
        public int minNoiseVisible = 25;
        public int maxNoiseVisible = 75;

        //Диапазон верхней границы почвы
        public readonly Vector2 DIRT_TOP_MIN_RANGE = new Vector2(0.6f, 0.7f);
        public readonly Vector2 DIRT_TOP_MAX_ADD_RANGE = new Vector2(0.1f, 0.2f);

        //Диапазон нижней границы почвы
        public readonly Vector2 DIRT_BOTTOM_RANGE = new Vector2(0.4f, 0.55f);

        //Диапазон нижней границы камня
        public readonly Vector2 STONE_BOTTOM_RANGE = new Vector2(0.2f, 0.35f);


        //Точка начала роста травы и деревьев
        public const float MIN_HEIGHT_GRASSPOINT = 0.45f;


        //Модификаторы
        public int maxWaterInNoise = 30;                //Водоемы
        public int minCaveInNoise = 68;                 //Пещеры
        

        

        [Header("Settings")]
        [SerializeField] private int seed = -1;      //1146381508
        [SerializeField] private TilePreset tilePreset;

        [Header("Links")]
        [SerializeField] private Tilemap tilemap;

        private int[,] world;

        [Space()]
        [SerializeField] private WorldNoiseGenerator worldNoise;


        private void Start()
        {
            tilePreset = Instantiate(tilePreset);
            tilePreset.Initialize();

            Generate(seed);
            GenerationTest01();
        }


        private void OnDrawGizmos()
        {
            worldNoise?.OnDrawGizmos();
        }

        [ContextMenu("ReGeneration")]
        private void ReGeneration()
        {
            Generate(seed);
            GenerationTest01();
        }


        public void GenerationTest01()
        {
            tilemap.SetTile(new Vector3Int(0, 0, 0), tilePreset.GetTile(EnumResources.Water));
            tilemap.SetTile(new Vector3Int(WORLD_WIDTH-1, 0, 0), tilePreset.GetTile(EnumResources.Water));
            tilemap.SetTile(new Vector3Int(0, WORLD_HEIGHT-1, 0), tilePreset.GetTile(EnumResources.Water));
            tilemap.SetTile(new Vector3Int(WORLD_WIDTH-1, WORLD_HEIGHT-1, 0), tilePreset.GetTile(EnumResources.Water));
        }

        public void Generate(int _seed)
        {
            StaticRandom.Initialize(_seed);

            world = new int[WORLD_WIDTH, WORLD_HEIGHT];
            
            var newNoiseData = new WorldNoiseData()
            {
                width = WORLD_WIDTH,
                height = WORLD_HEIGHT,

                noiseScale = NOISE_SCALE,
                minNoiseVisible = minNoiseVisible,
                maxNoiseVisible = maxNoiseVisible,

                noiseOffsetX = StaticRandom.Next(1, 100),
                noiseOffsetY = StaticRandom.Next(1, 100),
            };
            worldNoise = new WorldNoiseGenerator(newNoiseData);

            //Генерация верхнего уровня почвы
            int[] arrDirtTop = GetDirtTopLayer();
            //Генерация нижнего уровня почвы
            int[] arrDirtBottom = GetDirtBottomLayer();
            //Генерация нижнего слоя камня
            int[] arrStoneBottom = GetStoneBottomLayer();

            //Собираем карту
            for (int x = 0; x < WORLD_WIDTH; x++)
            {
                //Dirt
                var maxY = arrDirtTop[x];
                var minY = arrDirtBottom[x];
                for (int dirtY = minY; dirtY <= maxY; dirtY++)
                    world[x, dirtY] = (int)EnumResources.Dirt;

                //Stone
                minY = arrStoneBottom[x];
                for (int stoneY = minY; stoneY < WORLD_HEIGHT; stoneY++)
                {
                    if (world[x, stoneY] > 0)
                        break;
                    else
                        world[x, stoneY] = (int)EnumResources.Stone;
                }                
            }

            //Модификаторы

            //Вода на поверхности
            for (int x = 0; x < WORLD_WIDTH; x++)
                for (int y = 0; y < WORLD_HEIGHT; y++)
                {
                    if (worldNoise.GetNoiseInt100(x, y) <= maxWaterInNoise && CheckWaterZone(x, y))
                        world[x, y] = (int)EnumResources.Water;
                }

            //Пещеры
            for (int x = 0; x < WORLD_WIDTH; x++)
                for (int y = 0; y < WORLD_HEIGHT; y++)
                {
                    if (worldNoise.GetNoiseInt100(x, y) >= minCaveInNoise)
                        world[x, y] = (int)EnumResources.Void;
                }



            ShowMap(world);
        }


        private bool CheckWaterZone(int _x, int _y)
        {
            var checkMatrix = new int[WORLD_WIDTH, WORLD_HEIGHT];
            var isVoidUp = CheckVoidUpWaterZone(_x, _y, checkMatrix);

            checkMatrix = new int[WORLD_WIDTH, WORLD_HEIGHT];
            var isWaterOrDirt = CheckWaterOrDirtWaterZone(_x, _y, checkMatrix);

            return isWaterOrDirt && isVoidUp;
        }

        private bool CheckVoidUpWaterZone(int _x, int _y, int[,] _checkMatrix)
        {
            _checkMatrix[_x, _y] = 1;

            if (world.GetLength(0) > _x && world.GetLength(1) > _y + 1 && world[_x, _y + 1] == (int)EnumResources.Void)
            {
                return true;
            }
            else
            {
                //вверх
                if (world.GetLength(0) > _x && world.GetLength(1) > _y + 1
                    && worldNoise.GetNoiseInt100(_x, _y + 1) <= maxWaterInNoise
                    && _checkMatrix[_x, _y + 1] < 1)
                {
                    if (CheckVoidUpWaterZone(_x, _y + 1, _checkMatrix))
                        return true;
                }

                //направо
                if (world.GetLength(0) > _x + 1 && world.GetLength(1) > _y
                    && worldNoise.GetNoiseInt100(_x + 1, _y) <= maxWaterInNoise
                    && _checkMatrix[_x + 1, _y] < 1)
                {
                    if (CheckVoidUpWaterZone(_x + 1, _y, _checkMatrix))
                        return true;
                }

                //вниз
                if (world.GetLength(0) > _x && _y - 1 >= 0
                    && worldNoise.GetNoiseInt100(_x, _y - 1) <= maxWaterInNoise
                    && _checkMatrix[_x, _y - 1] < 1)
                {
                    if (CheckVoidUpWaterZone(_x, _y - 1, _checkMatrix))
                        return true;
                }

                //налево
                if (_x - 1 >= 0 && world.GetLength(1) > _y
                    && worldNoise.GetNoiseInt100(_x - 1, _y) <= maxWaterInNoise
                    && _checkMatrix[_x - 1, _y] < 1)
                {
                    if (CheckVoidUpWaterZone(_x - 1, _y, _checkMatrix))
                        return true;
                }
            }

            return false;
        }

        private bool CheckWaterOrDirtWaterZone(int _x, int _y, int[,] _checkMatrix)
        {
            _checkMatrix[_x, _y] = 1;

            //вверх
            if (world.GetLength(0) > _x && world.GetLength(1) > _y + 1)
            {
                if (world[_x, _y + 1] == (int)EnumResources.Dirt || world[_x, _y + 1] == (int)EnumResources.Water)
                    return true;
                else if (worldNoise.GetNoiseInt100(_x, _y + 1) <= maxWaterInNoise
                    && _checkMatrix[_x, _y + 1] < 1)
                    if (CheckWaterOrDirtWaterZone(_x, _y + 1, _checkMatrix))
                        return true;
            }

            //направо
            if (world.GetLength(0) > _x + 1 && world.GetLength(1) > _y)
            { 
                if (world[_x + 1, _y] == (int)EnumResources.Dirt || world[_x + 1, _y] == (int)EnumResources.Water)
                    return true;
                else if (worldNoise.GetNoiseInt100(_x + 1, _y) <= maxWaterInNoise
                    && _checkMatrix[_x + 1, _y] < 1)
                    if (CheckWaterOrDirtWaterZone(_x + 1, _y, _checkMatrix))
                        return true;
            }

            //вниз
            if (world.GetLength(0) > _x && _y - 1 >= 0)
            {
                if (world[_x, _y - 1] == (int)EnumResources.Dirt || world[_x, _y - 1] == (int)EnumResources.Water)
                    return true;
                else if (worldNoise.GetNoiseInt100(_x, _y - 1) <= maxWaterInNoise
                    && _checkMatrix[_x, _y - 1] < 1)
                    if (CheckWaterOrDirtWaterZone(_x, _y - 1, _checkMatrix))
                        return true;
            }

            //налево
            if (_x - 1 >= 0 && world.GetLength(1) > _y)
            {
                if (world[_x - 1, _y] == (int)EnumResources.Dirt || world[_x - 1, _y] == (int)EnumResources.Water)
                    return true;
                else if (worldNoise.GetNoiseInt100(_x - 1, _y) <= maxWaterInNoise
                    && _checkMatrix[_x - 1, _y] < 1)
                    if (CheckWaterOrDirtWaterZone(_x - 1, _y, _checkMatrix))
                        return true;
            }

            return false;
        }






        private int[] GetDirtTopLayer()
        {
            int groundLevelMin = StaticRandom.Next((int)(WORLD_HEIGHT * DIRT_TOP_MIN_RANGE.x), (int)(WORLD_HEIGHT * DIRT_TOP_MIN_RANGE.y));
            int groundLevelMax = groundLevelMin + StaticRandom.Next((int)(WORLD_HEIGHT * DIRT_TOP_MAX_ADD_RANGE.x), (int)(WORLD_HEIGHT * DIRT_TOP_MAX_ADD_RANGE.y));

            int[] arr = new int[WORLD_WIDTH];
            for (int i = 0; i < WORLD_WIDTH; i++)
            {
                int dir = StaticRandom.Next(0, 2) == 1 ? 1 : -1;

                if (i > 0)
                {
                    if (arr[i - 1] + dir < groundLevelMin || arr[i - 1] + dir > groundLevelMax)
                        dir = -dir;

                    arr[i] = arr[i - 1] + dir;
                }
                else
                {
                    arr[i] = StaticRandom.Next(groundLevelMin, groundLevelMax);
                }
            }

            arr = GetSmoothing(arr, 4);

            return arr;
        }

        private int[] GetDirtBottomLayer()
        {
            int[] arr = new int[WORLD_WIDTH];
            for (int i = 0; i < WORLD_WIDTH; i++)
            {
                var minValue = (int)(WORLD_HEIGHT * DIRT_BOTTOM_RANGE.x);
                var maxValue = (int)(WORLD_HEIGHT * DIRT_BOTTOM_RANGE.y);
                arr[i] = StaticRandom.Next(minValue, maxValue);
            }

            arr = GetSmoothing(arr, 3);

            return arr;
        }
        
        private int[] GetStoneBottomLayer()
        {
            int[] arr = new int[WORLD_WIDTH];
            for (int i = 0; i < WORLD_WIDTH; i++)
            {
                var minValue = (int)(WORLD_HEIGHT * STONE_BOTTOM_RANGE.x);
                var maxValue = (int)(WORLD_HEIGHT * STONE_BOTTOM_RANGE.y);
                arr[i] = StaticRandom.Next(minValue, maxValue);
            }

            arr = GetSmoothing(arr, 1);

            return arr;
        }

        private void ShowMap(int[,] _map)
        {
            tilemap.ClearAllTiles();

            for (int x = 0; x < _map.GetLength(0); x++)
                for (int y = 0; y < _map.GetLength(1); y++)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tilePreset.GetTile((EnumResources)_map[x, y]));
        }

        private int[] GetSmoothing(int[] _origin, int _avrLenght)
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
