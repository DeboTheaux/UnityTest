using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameResultPopUpView : MonoBehaviour, IGameResultPopUpView
{
    [SerializeField] private Button buttonReplay;
    [SerializeField] private Button buttonExit;
    [SerializeField] private TextMeshProUGUI resultText;

    public void Initialize()
    {
        buttonReplay.onClick.AddListener(() => NavigateToGameDifficultySelection());
        buttonExit.onClick.AddListener(() => Application.Quit());
    }

    private void NavigateToGameDifficultySelection()
    {
        DependencyProvider.GetDependency<SimpleScreenNavigator>().ShowScreen<IDifficultySelectorView>();
    }

    public void ShowPopUpWithResult(string result)
    {
        resultText.text = result;
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
