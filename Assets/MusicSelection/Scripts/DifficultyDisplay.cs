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

        // テキストの更新はサムネイルに任せる．サムネイルは難易度から譜面レベルを知ることが出来るため．
        // [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _arrowLeft;
        [SerializeField] private GameObject _arrowRight;

        private void Awake()
        {
            _instanceGameObj = gameObject;
            DifficultySelection.SetActive(true);
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