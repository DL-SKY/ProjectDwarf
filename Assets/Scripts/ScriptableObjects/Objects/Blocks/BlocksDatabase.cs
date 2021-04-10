using ProjectDwarf.Data.Objects.Blocks;
using ProjectDwarf.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDwarf.ScriptableObjects.Objects.Blocks
{
    [CreateAssetMenu(fileName = "BlocksDatabase", menuName = "Data/Objects/Blocks/Blocks Database")]
    public class BlocksDatabase : ScriptableObject
    {
        public List<BlockData> blocks;


        [ContextMenu("Get full list")]
        private void GetFullList()
        {
            blocks.Clear();

            foreach (EnumResources res in Enum.GetValues(typeof(EnumResources)))
                blocks.Add(new BlockData() { block = res });
        }
    }
}

