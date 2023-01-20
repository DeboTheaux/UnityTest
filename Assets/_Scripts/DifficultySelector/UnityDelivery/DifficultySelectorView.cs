using System;
using UnityEngine;
using UT.Shared;

namespace UT.GameLogic
{
    public class DifficultySelectorView : MonoBehaviour, IDifficultySelectorView
    {
        [SerializeField] private GameSettings settings;
        [SerializeField] private DifficultyItemView difficultyItem;
        [SerializeField] private RectTransform content;

        DifficultySelectorPresenter _presenter;

        private DifficultySelectorPresenter Present() =>
            new DifficultySelectorPresenter(this, settings, DependencyProvider.GetDependency<FrameNavigator>());

        public void Initialize()
        {
            _presenter = Present();
        }

        public void ShowGameDifficulty(Difficulty difficulty, Action<Difficulty> OnButtonClick)
        {
            var prefab = Instantiate(difficultyItem, content);
            prefab.ToConfigure(difficulty.name, difficulty, OnButtonClick);
        }

        public void Show()
        {
            ClearAllItems();
            _presenter.Present();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ClearAllItems()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
