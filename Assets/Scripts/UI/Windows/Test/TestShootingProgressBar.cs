using ProjectDwarf.UI.Components;

namespace ProjectDwarf.UI.Windows.Test
{
    public class TestShootingProgressBar : ProgressBar
    {
        public void AddFillAmount(float _delta)
        {
            FillAmount += _delta;
        }
    }
}
