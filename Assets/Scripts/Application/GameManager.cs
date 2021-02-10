using ProjectDwarf.Constants;
using ProjectDwarf.Patterns;
using ProjectDwarf.Services;
using ProjectDwarf.UI.WindowsManager;
using ProjectDwarf.UI.Windows.Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDwarf.Application
{
    public class GameManager : Singleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }


        //TODO
        private void Initialize()
        {
            var windowsManager = ComponentLocator.Resolve<WindowsManager>();
            windowsManager.CreateWindow<GameLoadingWindow>(GameLoadingWindow.prefabPath, Enums.EnumWindowsLayer.Loading, ConstantScenes.MAIN_MENU);
        }
    }
}
