using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Gallery
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(PlayerInput))]
    public class Description : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        [SerializeField] private DotIndicator _dotIndicator;

        private void Awake()
        {
            _text = gameObject.GetComponent<TextMeshProUGUI>();
        }

        public void Set(string descriptionText)
        {
            _text.text = descriptionText;
            _text.pageToDisplay = 1;

            // ForceMeshUpdate()を挟むことでtextInfoが更新される
            _text.ForceMeshUpdate();
            _dotIndicator.Init(_text.textInfo.pageCount);
        }

        public void OnNavigateHorizontal(InputValue inputValue)
        {
            var inputHorizontal = inputValue.Get<Vector2>().x;

            switch (inputHorizontal)
            {
                case > 0f:
                    ShowNextPage();
                    break;
                case < 0f:
                    ShowPreviousPage();
                    break;
            }
        }

        private void ShowNextPage()
        {
            var pageIsLast = _text.pageToDisplay >= _text.textInfo.pageCount;
            if (pageIsLast) return;

            SystemSoundEffect.PlaySelect();
            _text.pageToDisplay++;
            _dotIndicator.Indicate(_text.pageToDisplay);
        }

        private void ShowPreviousPage()
        {
            // pageは1-indexed
            var pageIsFirst = _text.pageToDisplay <= 1;
            if (pageIsFirst) return;

            SystemSoundEffect.PlaySelect();
            _text.pageToDisplay--;
            _dotIndicator.Indicate(_text.pageToDisplay);
        }
    }
}