using TMPro;
using Transition;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Settings
{
	public class AdjustOffset : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler
	{
		[SerializeField] private UserSettings _settings;
		[SerializeField] private TextMeshProUGUI _label;
		[SerializeField] private TextMeshProUGUI _valueText;
		[SerializeField] private Image _buttonImage;
		[SerializeField] private CustomButton _button;
		[SerializeField] private TabLabel _tabLabel;

		private static readonly Color32 _nomalColor = new Color32(r: 57, g: 57, b: 57, a: 255);
		private static readonly Color32 _selectedColor = new Color32(r: 214, g: 77, b: 42, a: 255);

		private static readonly Color _nomalButtonColor = Color.white;
		private static readonly Color32 _selectedButtonColor = new Color32(r: 255, g: 217, b: 0, a: 255);

		private void OnEnable()
		{
			UpdateView();
		}

		public void OnSelect(BaseEventData eventData)
		{
			_label.color = _selectedColor;
			_label.text = " " + _label.text;
			_buttonImage.color = _selectedButtonColor;
		}

		public void OnDeselect(BaseEventData eventData)
		{
			_label.color = _nomalColor;
			_label.text = _label.text.TrimStart(' ');
			_buttonImage.color = _nomalButtonColor;
		}

		public void OnSubmit(BaseEventData eventData)
		{
			_button.OnSubmit(eventData);
		}

		public void OnCancel(BaseEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(_tabLabel.gameObject);
		}

		private void UpdateView()
		{
			_valueText.text = _settings.JudgeOffset.ToString();
		}

		public void TransitionToAdjustment()
		{
			SceneTransitionManager.TransitionToAdjustment();
		}
	}
}