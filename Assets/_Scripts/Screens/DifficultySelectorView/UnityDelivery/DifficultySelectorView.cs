using System;
using UnityEngine;

public class DifficultySelectorView : MonoBehaviour, IDifficultySelectorView
{
    [SerializeField] private GameSettings settings;
    [SerializeField] private DifficultyItemView difficultyItem;
    [SerializeField] private RectTransform content;

    DifficultySelectorPresenter presenter;

    private DifficultySelectorPresenter Present() => 
        new DifficultySelectorPresenter(this, settings, DependencyProvider.GetDependency<SimpleScreenNavigator>());

    public void Initialize()
    {
        presenter = Present();
    }

    public void ShowGameDifficulty(Difficulty difficulty, Action<Difficulty> OnButtonClick)
    {
        var prefab = Instantiate(difficultyItem, content);
        prefab.ToConfigure(difficulty.name, difficulty, OnButtonClick);
    }

    public void Show()
    {
        ClearAllItems();
        presenter.Present();
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
