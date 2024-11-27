using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Settings
{
	public class TabLabel : MonoBehaviour, ISelectHandler, IDeselectHandler
	{
		[SerializeField] private GameObject _tabArea;

		private Color32 _nomalColor = new Color32(r: 128, g: 128, b: 128, a: 255);
		private Color32 _selectedColor = new Color32(r: 255, g: 243, b: 228, a: 255);

		private Image _image;

		private void Start()
		{
			_image = GetComponent<Image>();
		}

		public void OnSelect(BaseEventData eventData)
		{
			_image.color = _selectedColor;
			_tabArea.SetActive(true);
		}

		public void OnDeselect(BaseEventData eventData)
		{
			if (eventData.selectedObject.GetComponent<TabLabel>())
			{
				_image.color = _nomalColor;
				_tabArea.SetActive(false);
			}
		}
	}
}