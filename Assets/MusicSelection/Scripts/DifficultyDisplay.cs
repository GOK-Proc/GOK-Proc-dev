using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Rhythm;

namespace MusicSelection
{
    public class DifficultyDisplay : MonoBehaviour
    {
        private static GameObject _instanceGameObj;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _arrowLeft;
        [SerializeField] private GameObject _arrowRight;

        private void Awake()
        {
            _instanceGameObj = gameObject;
        }

        public static void Hide()
        {
            _instanceGameObj.gameObject.SetActive(false);
        }

        public static void Show()
        {
            _instanceGameObj.gameObject.SetActive(true);
        }

        public void Set()
        {
            // Enum.ToString()は実行速度が遅いため，あまりきれいではないがswitchによる記述．
            // 以下の式と結果は同じはず
            // _text.text = DifficultySelection.Current.ToString().ToUpper();
            _text.text = DifficultySelection.Current switch
            {
                Difficulty.Easy => "EASY",
                Difficulty.Hard => "HARD",
                Difficulty.Expert => "EXPERT",
                _ => throw new InvalidEnumArgumentException()
            };

            _image.color = DifficultySelection.CurrentColor;

            if (DifficultySelection.IsEasiest)
            {
                _arrowLeft.SetActive(false);
                _arrowRight.SetActive(true);
            }
            else if (DifficultySelection.IsHardest)
            {
                _arrowLeft.SetActive(true);
                _arrowRight.SetActive(false);
            }
            else
            {
                _arrowLeft.SetActive(true);
                _arrowRight.SetActive(true);
            }
        }
    }
}