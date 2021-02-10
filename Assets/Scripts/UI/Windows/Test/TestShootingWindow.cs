using ProjectDwarf.Services;
using ProjectDwarf.UI.Windows.MainMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDwarf.UI.Windows.Test
{
    public class TestShootingWindow : WindowBase
    {
        public const string prefabPath = @"Prefabs\UI\Windows\Test\TestShootingWindow";

        [Space()]
        [SerializeField] private TestShootingTouchController touchController;


        public override void Initialize(object _data)
        {
            base.Initialize(_data);
            touchController.ResetProgress();
        }

        public override void Close(bool _result = false)
        {
            base.Close(_result);

            var windowsManager = ComponentLocator.Resolve<WindowsManager.WindowsManager>();
            windowsManager.CreateWindow<MainMenuWindow>(MainMenuWindow.prefabPath, Enums.EnumWindowsLayer.Main);
        }
    }
}
