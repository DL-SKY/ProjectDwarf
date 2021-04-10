using UnityEngine;

namespace ProjectDwarf.Tools.Arrays
{
    public static class ToolArrays
    {
        /// <summary>
        /// Возвращает сквозной индекс для элемента массива
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <param name="_xCount"></param>
        /// <returns></returns>
        public static int GetSingleIndex(int _x, int _y, int _xCount)
        {
            return _x + _y * _xCount;
        }

        /// <summary>
        /// Возвращает сквозной индекс для элемента массива
        /// </summary>
        /// <param name="_coord"></param>
        /// <param name="_xCount"></param>
        /// <returns></returns>
        public static int GetSingleIndex(Vector2Int _coord, int _xCount)
        {
            return _coord.x + _coord.y * _xCount;
        }
    }
}
