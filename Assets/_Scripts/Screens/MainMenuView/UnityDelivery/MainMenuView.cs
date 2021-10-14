using UnityEngine.UI;
using UnityEngine;

public class MainMenuView : MonoBehaviour, IMainMenuView
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    MainMenuPresenter presenter;

    MainMenuPresenter Presenter() => new MainMenuPresenter(this, 
                                     DependencyProvider.GetDependency<SimpleScreenNavigator>());

    public void Initialize()
    {
        presenter = Presenter();

        playButton.onClick.AddListener(() => presenter.OnPlayButtonClick());
        exitButton.onClick.AddListener(() => Application.Quit());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
