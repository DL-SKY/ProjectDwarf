using ProjectDwarf.Services;
using ProjectDwarf.UI.Windows.MainMenu;
using ProjectDwarf.UI.WindowsManager;
using UnityEngine;

namespace ProjectDwarf.Scenes.MainMenu
{
    public class MainMenuSceneController : MonoBehaviour
    {
        private void Awake()
        {
            var windowsManager = ComponentLocator.Resolve<WindowsManager>();
            windowsManager?.CreateWindow<MainMenuWindow>(MainMenuWindow.prefabPath, Enums.EnumWindowsLayer.Main);
        }
    }
}
