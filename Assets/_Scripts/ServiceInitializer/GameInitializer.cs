using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Transform gameCanvas;
        [SerializeField] private List<FramePath> framePaths;

        private FrameNavigator _frameNavigator;

        private void Awake()
        {
            InjectDependencies();
            InitializeSimpleNavigator();
            NavigateToFirstScreen();
        }

        private void InjectDependencies()
        {
            DependencyProvider.RegisterDependency<Canvas>(gameCanvas.GetComponent<Canvas>());
            DependencyProvider.RegisterDependency<AudioManager>(audioManager);
        }

        private void InitializeSimpleNavigator()
        {
            _frameNavigator = new FrameNavigator(framePaths);
            DependencyProvider.RegisterDependency<FrameNavigator>(_frameNavigator);
            _frameNavigator.InitializeFrames();
        }

        private void NavigateToFirstScreen()
        {
            _frameNavigator.OpenFrameByType<IMainMenuView>();
        }
    }
}

