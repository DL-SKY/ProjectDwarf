using ProjectDwarf.Enums;
using System;

namespace ProjectDwarf.Data.Objects.Blocks
{
    [Serializable]
    public class BlockData
    {
        public EnumResources block;

        public float maxHitPoints;
        
        public EnumResources[] destroyedFromBlocks;        
    }
}
