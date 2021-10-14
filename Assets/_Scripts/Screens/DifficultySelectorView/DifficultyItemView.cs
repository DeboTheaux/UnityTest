using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine;

public class DifficultyItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    private string dificultyName;

    public void ToConfigure(string name, Difficulty difficulty, Action<Difficulty> OnButtonClick)
    {
        this.dificultyName = name;

        button.onClick.AddListener(() => OnButtonClick(difficulty));

        Display();
    }

    private void Display()
    {
        nameText.text = dificultyName;
    }
}
