using ProjectDwarf.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectDwarf.WorldGeneration
{
    [Serializable]
    public class ResourceTile
    {
        public EnumResources key;
        public TileBase value;

        public ResourceTile(EnumResources _key, TileBase _value)
        {
            key = _key;
            value = _value;
        }
    }

    [CreateAssetMenu(fileName = "TilePreset_00", menuName = "Data/World/Tile Preset")]
    public class TilePreset : ScriptableObject
    {
        public List<ResourceTile> tiles = new List<ResourceTile>();
        private Dictionary<EnumResources, TileBase> tilesDic = new Dictionary<EnumResources, TileBase>();
        private bool isInit = false;


        public void Initialize()
        {
            tilesDic.Clear();
            CreateDictionary();

            isInit = true;
        }

        public TileBase GetTile(EnumResources _resource)
        {
            if (!isInit)
                Initialize();

            if (tilesDic.ContainsKey(_resource))
                return tilesDic[_resource];
            else
                return null;
        }


        private void CreateDictionary()
        {
            foreach (var resTile in tiles)
                tilesDic.Add(resTile.key, resTile.value);
        }
    }
}
