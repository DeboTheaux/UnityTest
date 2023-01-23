using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine;
using UniRx;

namespace UT.GameLogic
{
    public class DifficultyItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button button;


        public void ToConfigure(StringReactiveProperty name, Difficulty difficulty, Action<Difficulty> OnButtonClick)
        {
            name.Subscribe(Display);

            button.onClick.AddListener(() => OnButtonClick(difficulty));
        }

        private void Display(string dificultyName)
        {
            nameText.text = dificultyName;
        }
    }
}