using ProjectDwarf.Data.Objects.Blocks;
using ProjectDwarf.Enums;
using ProjectDwarf.ScriptableObjects.Objects.Blocks;
using System;

namespace ProjectDwarf.Objects.Blocks
{
    public abstract class BaseBlock
    {
        //Callbacks
        public Action OnDestroy;

        //Settings
        protected EnumResources block;
        protected float curHitPoints;

        protected BlockData data;


        //Constructor
        public BaseBlock(EnumResources _block, BlocksDatabase _database)
        {
            block = _block;

            LoadSettings(_database);
        }


        //State methods
        public virtual void Update()
        { 
        
        }

        public virtual void Destroy()
        {
            OnDestroy?.Invoke();
        }


        //Other
        protected void LoadSettings(BlocksDatabase _database)
        {
            data = _database.blocks.Find(x => x.block == block);
        }
    }
}
