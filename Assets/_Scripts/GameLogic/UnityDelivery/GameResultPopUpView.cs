using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
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
            DependencyProvider.GetDependency<FrameNavigator>().OpenFrameByType<IDifficultySelectorView>();
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
}
