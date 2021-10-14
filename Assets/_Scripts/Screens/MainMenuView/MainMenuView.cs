using UnityEngine.UI;
using UnityEngine;

public class MainMenuView : MonoBehaviour, IMainMenuView
{
    [SerializeField] private Button playButton;

    MainMenuPresenter presenter;

    MainMenuPresenter Presenter() => new MainMenuPresenter(this, 
                                     DependencyProvider.GetDependency<SimpleScreenNavigator>());

    public void Initialize()
    {
        presenter = Presenter();

        playButton.onClick.AddListener(() => presenter.OnPlayButtonClick());
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
