using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace UT.GameLogic
{
    public class DifficultyItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button button;

        private string _dificultyName;

        public void ToConfigure(string name, Difficulty difficulty, Action<Difficulty> OnButtonClick)
        {
            this._dificultyName = name;

            button.onClick.AddListener(() => OnButtonClick(difficulty));

            Display();
        }

        private void Display()
        {
            nameText.text = _dificultyName;
        }
    }
}