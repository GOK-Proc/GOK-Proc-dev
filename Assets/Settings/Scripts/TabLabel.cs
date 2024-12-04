using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Settings
{
	public class TabLabel : MonoBehaviour, ISelectHandler, IDeselectHandler
	{
		[SerializeField] private GameObject _tabArea;
		[SerializeField] private TabLabel[] _otherTabLabels;
		[SerializeField] private GameObject[] _otherTabAreas;

		private static readonly Color32 _nomalColor = new Color32(r: 128, g: 128, b: 128, a: 255);
		private static readonly Color32 _selectedColor = new Color32(r: 255, g: 243, b: 228, a: 255);

		private Image _image;

		private void Start()
		{
			_image = GetComponent<Image>();
		}

		public void OnSelect(BaseEventData eventData)
		{
			_image.color = _selectedColor;
			transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
			_tabArea.SetActive(true);

			foreach (var tabLabel in _otherTabLabels) tabLabel.GetComponent<Image>().color = _nomalColor;
			foreach (var tabArea in _otherTabAreas) tabArea.SetActive(false);
		}

		public void OnDeselect(BaseEventData eventData)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
		}
	}
}