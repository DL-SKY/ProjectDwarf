using ProjectDwarf.Constants;
using ProjectDwarf.Services;
using ProjectDwarf.UI.Windows.Loading;
using ProjectDwarf.UI.Windows.Test;
using UnityEngine;

namespace ProjectDwarf.UI.Windows.MainMenu
{
    public class MainMenuWindow : WindowBase
    {
        public const string prefabPath = @"Prefabs\UI\Windows\MainMenu\MainMenuWindow";


        private void OnEnable()
        {
            Debug.LogError("OnEnable");
        }


        public void OnClickTest()
        {
            //var windowsManager = ComponentLocator.Resolve<WindowsManager.WindowsManager>();
            //windowsManager.CreateWindow<GameLoadingWindow>(GameLoadingWindow.prefabPath, Enums.EnumWindowsLayer.Loading, ConstantScenes.TEST_SCENE);

            //Close();

            var windowsManager = ComponentLocator.Resolve<WindowsManager.WindowsManager>();
            windowsManager.CreateWindow<TestShootingWindow>(TestShootingWindow.prefabPath, Enums.EnumWindowsLayer.Main);

            Close();
        }        
    }
}
