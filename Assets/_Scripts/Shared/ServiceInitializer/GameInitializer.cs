using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameInitializer : MonoBehaviour, IGameInitializer
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Transform gameCanvas;
    [SerializeField] private Transform gameViewsContainer;

    private SimpleScreenNavigator simpleNavigator;

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
        var viewList = gameViewsContainer.GetComponentsInChildren<IHideable>(true).ToList();
        viewList.AddRange(gameCanvas.GetComponentsInChildren<IHideable>(true).ToList());

        simpleNavigator = new SimpleScreenNavigator(viewList);
        DependencyProvider.RegisterDependency<SimpleScreenNavigator>(simpleNavigator);
        simpleNavigator.InitializeViews();
    }

    private void NavigateToFirstScreen()
    {
        simpleNavigator.ShowScreen<IMainMenuView>();
    }
}

//para testing
public interface IGameInitializer
{

}