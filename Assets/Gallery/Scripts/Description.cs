using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gallery
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(PlayerInput))]
    public class Description : MonoBehaviour
    {
        private TextMeshProUGUI _descriptionText;

        private void Awake()
        {
            _descriptionText = GetComponent<TextMeshProUGUI>();
        }

        public void Set(string description)
        {
            _text.text = description;
            _text.pageToDisplay = 1;

            // ForceMeshUpdate()を挟むことでtextInfoが更新される
            _text.ForceMeshUpdate();
            _dotIndicator.Init(_text.textInfo.pageCount);
        }

        public void OnNavigateHorizontal(InputValue inputValue)
        {
            var inputHorizontal = inputValue.Get<Vector2>().x;
            Debug.Log("horizontal");

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
            var pageIsLast = _descriptionText.pageToDisplay >= _descriptionText.textInfo.pageCount;
            if (pageIsLast) return;
            
            _descriptionText.pageToDisplay++;
        }

        private void ShowPreviousPage()
        {
            // pageは1-indexed
            var pageIsFirst = _descriptionText.pageToDisplay <= 1;
            if (pageIsFirst) return;
            
            _descriptionText.pageToDisplay--;
        }
    }
}