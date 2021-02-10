using ProjectDwarf.Constants;
using ProjectDwarf.Services;
using ProjectDwarf.UI.WindowsManager;
using ProjectDwarf.UI.Windows.Loading;
using ProjectDwarf.UI.Windows.MainMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDwarf.Scenes.MainMenu
{
    public class MainMenuSceneController : MonoBehaviour
    {
        private void Awake()
        {
            var windowsManager = ComponentLocator.Resolve<WindowsManager>();
            windowsManager.CreateWindow<MainMenuWindow>(MainMenuWindow.prefabPath, Enums.EnumWindowsLayer.Main);
        }
    }
}
