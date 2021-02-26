using ProjectDwarf.Enums;
using ProjectDwarf.WorldGeneration.Biomes;
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
        public readonly Vector2 DIRT_BOTTOM_RANGE = new Vector2(0.4f, 0.6f);

        //Диапазон нижней границы камня
        public readonly Vector2 STONE_BOTTOM_RANGE = new Vector2(0.2f, 0.35f);

        //Диапазон нижней границы каменой почвы
        public readonly Vector2 VOLCANICDIRT_BOTTOM_RANGE = new Vector2(0.1f, 0.25f);

        //Диапазон нижней границы каменой почвы
        public readonly Vector2 VOLCANICSTONE_BOTTOM_RANGE = new Vector2(0.05f, 0.15f);



        //Точка начала роста травы и деревьев
        public const float MIN_HEIGHT_GRASSPOINT = 0.45f;


        //Модификаторы
        [Space()]
        public int maxWaterInNoise = 25;                                        //Водоемы       30
        public int minCaveInNoise = 75;                                         //Пещеры        68
        public Vector2Int sandInTopInNoise = new Vector2Int(26, 35);            //Песок на поверхности по алгоритму воды
        public Vector2Int sandInNoise = new Vector2Int(71, 74);                 //Песок по всей карте (примерно около пещер по диапазону шума)
        public Vector2Int sandWithSandInNoise = new Vector2Int(27, 31);         //Песок, если соседние клетки песок/песчаник
        public Vector2Int stoneInNoise = new Vector2Int(60, 70);                //Песок, если соседние клетки песок/песчаник




        [Header("Settings")]
        [SerializeField] private int seed = -1;      //1146381508
        [SerializeField] private TilePreset tilePreset;

        [Header("Links")]
        [SerializeField] private Tilemap tilemap;

        private int[,] world;

        [Space()]
        [SerializeField] private WorldNoiseGenerator worldNoise;

        //Генераторы биомов
        private DirtBiomeGenerator dirtBiome;
        private StoneBiomeGenerator stoneBiome;
        private VolcanicDirtBiomeGenerator volcDirtBiome;
        private VolcanicStoneBiomeGenerator volcStoneBiome;

        private CaveBiomeGenerator caveBiome;
        private WaterBiomeGenerator waterBiome;
        private SandBiomeGenerator sandBiome;

        private BorderBiomeGenerator borderBiome;


        private void Start()
        {
            tilePreset = Instantiate(tilePreset);
            tilePreset.Initialize();

            Generate(seed);
        }


        private void OnDrawGizmos()
        {
            worldNoise?.OnDrawGizmos();
        }

        //TODO TEST
        [ContextMenu("ReGeneration")]
        private void ReGeneration()
        {
            Generate(seed);
        }


        //public void GenerationTest01()
        //{
        //    tilemap.SetTile(new Vector3Int(0, 0, 0), tilePreset.GetTile(EnumResources.Water));
        //    tilemap.SetTile(new Vector3Int(WORLD_WIDTH-1, 0, 0), tilePreset.GetTile(EnumResources.Water));
        //    tilemap.SetTile(new Vector3Int(0, WORLD_HEIGHT-1, 0), tilePreset.GetTile(EnumResources.Water));
        //    tilemap.SetTile(new Vector3Int(WORLD_WIDTH-1, WORLD_HEIGHT-1, 0), tilePreset.GetTile(EnumResources.Water));
        //}

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
            dirtBiome = new DirtBiomeGenerator(world);
            int[] arrDirtTop = dirtBiome.GenerateDirtTopLayer(DIRT_TOP_MIN_RANGE, DIRT_TOP_MAX_ADD_RANGE);
            //Генерация нижнего уровня почвы
            int[] arrDirtBottom = dirtBiome.GenerateDirtBottomLayer(DIRT_BOTTOM_RANGE);
            //Генерация нижнего слоя камня
            stoneBiome = new StoneBiomeGenerator(world, worldNoise);
            int[] arrStoneBottom = stoneBiome.GenerateStoneBottomLayer(STONE_BOTTOM_RANGE);
            //Генерация нижнего слоя каменистой почвы
            volcDirtBiome = new VolcanicDirtBiomeGenerator(world);
            int[] arrVolcanicDirtBottom = volcDirtBiome.GenerateVolcanicDirtBottomLayer(VOLCANICDIRT_BOTTOM_RANGE);
            //Генерация нижнего слоя каменистого камня
            volcStoneBiome = new VolcanicStoneBiomeGenerator(world);
            int[] arrVolcanicStoneBottom = volcStoneBiome.GenerateVolcanicStoneBottomLayer(VOLCANICSTONE_BOTTOM_RANGE);

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

                //VolcanicDirt
                minY = arrVolcanicDirtBottom[x];
                for (int volcanicDirtY = minY; volcanicDirtY < WORLD_HEIGHT; volcanicDirtY++)
                {
                    if (world[x, volcanicDirtY] > 0)
                        break;
                    else
                        world[x, volcanicDirtY] = (int)EnumResources.VolcanicDirt;
                }

                //VolcanicStone
                minY = arrVolcanicStoneBottom[x];
                for (int volcanicDirtY = minY; volcanicDirtY < WORLD_HEIGHT; volcanicDirtY++)
                {
                    if (world[x, volcanicDirtY] > 0)
                        break;
                    else
                        world[x, volcanicDirtY] = (int)EnumResources.VolcanicStone;
                }
            }

            //Модификаторы

            //Пещеры
            caveBiome = new CaveBiomeGenerator(world, worldNoise);
            world = caveBiome.GenerateCaves(world, minCaveInNoise);            

            //Вода на поверхности
            waterBiome = new WaterBiomeGenerator(world, worldNoise);
            world = waterBiome.GenerateWaterInTop(world, maxWaterInNoise);

            //Песок
            //На поверхности
            sandBiome = new SandBiomeGenerator(world, worldNoise);
            world = sandBiome.GenerateSandInTop(world, sandInTopInNoise, (int)(WORLD_HEIGHT * DIRT_BOTTOM_RANGE.x));
            //Везде по шуму
            world = sandBiome.GenerateSandInNoise(world, sandInNoise);
            //Везде, где рядом песок
            world = sandBiome.GenerateSandWithSandInNoise(world, sandWithSandInNoise);

            //Земля/камень шумом
            world = stoneBiome.GenerateStoneNoise(world, stoneInNoise);

            //Границы мира
            borderBiome = new BorderBiomeGenerator(world);
            world = borderBiome.GenerateBorder(world);



            ShowMap(world);
        }
        

        //DOTO: TEST - после теста оставить только функционал генерации. Отображение убрать!
        private void ShowMap(int[,] _map)
        {
            tilemap.ClearAllTiles();

            for (int x = 0; x < _map.GetLength(0); x++)
                for (int y = 0; y < _map.GetLength(1); y++)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tilePreset.GetTile((EnumResources)_map[x, y]));
        }       
    }
}
