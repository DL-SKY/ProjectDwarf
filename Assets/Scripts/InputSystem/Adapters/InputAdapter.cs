namespace ProjectDwarf.InputSystem.Adapters
{
    public abstract class InputAdapter
    {
        public bool Enabled { get; private set; }


        public InputAdapter(bool _enabled) 
        { 
            Enabled = _enabled; 
        }


        public void SetEnable(bool _state)
        {
            Enabled = _state;
        }

        public virtual void Update()
        {
            if (Enabled)
                CustomUpdate();
        }


        protected abstract void CustomUpdate();
    }
}
