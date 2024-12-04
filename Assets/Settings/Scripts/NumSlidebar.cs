using KanKikuchi.AudioManager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Settings
{
	[RequireComponent(typeof(Selectable))]
	public class NumSlidebar : MonoBehaviour, ISelectHandler, IDeselectHandler
	{
		[SerializeField] private UserSettings _settings;
		[SerializeField] private TextMeshProUGUI _label;
		[SerializeField] private TextMeshProUGUI _valueText;
		[SerializeField] private Slider _slider;

		[Space(10)]
		[SerializeField] private NumericSettingItem _settingItem;
		[SerializeField] private float _step;

		private static readonly Color32 _nomalColor = new Color32(r: 57, g: 57, b: 57, a: 255);
		private static readonly Color32 _selectedColor = new Color32(r: 214, g: 77, b: 42, a: 255);

		private void OnEnable()
		{
			UpdateView();
		}

		public void OnSelect(BaseEventData eventData)
		{
			_label.color = _selectedColor;
			_label.text = " " + _label.text;
		}

		public void OnDeselect(BaseEventData eventData)
		{
			_label.color = _nomalColor;
			_label.text = _label.text.TrimStart(' ');
		}

		public void OnMove(BaseEventData eventData)
		{
			AxisEventData axisEventData = eventData as AxisEventData;

			if (axisEventData.moveDir == MoveDirection.Left) Sub();
			else if (axisEventData.moveDir == MoveDirection.Right) Add();

			UpdateView();
		}

		private void UpdateView()
		{
			switch (_settingItem)
			{
				case NumericSettingItem.BgmVolume:
					_valueText.text = _settings.BgmVolume.ToString();
					_slider.value = _settings.BgmVolume;
					break;
				case NumericSettingItem.SoundEffectVolume:
					_valueText.text = _settings.SoundEffectVolume.ToString();
					_slider.value = _settings.SoundEffectVolume;
					break;
				case NumericSettingItem.NovelEffectVolume:
					_valueText.text = _settings.NovelEffectVolume.ToString();
					_slider.value = _settings.NovelEffectVolume;
					break;
				case NumericSettingItem.MusicVolume:
					_valueText.text = _settings.MusicVolume.ToString();
					_slider.value = _settings.MusicVolume;
					break;
				case NumericSettingItem.BattleEffectVolume:
					_valueText.text = _settings.BattleEffectVolume.ToString();
					_slider.value = _settings.BattleEffectVolume;
					break;
				case NumericSettingItem.RhythmEffectVolume:
					_valueText.text = _settings.RhythmEffectVolume.ToString();
					_slider.value = _settings.RhythmEffectVolume;
					break;
				case NumericSettingItem.HighSpeed:
					_valueText.text = _settings.HighSpeed.ToString();
					_slider.value = _settings.HighSpeed;
					break;
			}
		}

		private void Add()
		{
			switch (_settingItem)
			{
				case NumericSettingItem.None:
					return;
				case NumericSettingItem.BgmVolume:
					_settings.BgmVolume += (int)_step;
					BGMManager.Instance.ChangeBaseVolume(_settings.BgmVolume / 10f);
					break;
				case NumericSettingItem.SoundEffectVolume:
					_settings.SoundEffectVolume += (int)_step;
					SEManager.Instance.ChangeBaseVolume(_settings.SoundEffectVolume / 10f);
					break;
				case NumericSettingItem.NovelEffectVolume:
					_settings.NovelEffectVolume += (int)_step;
					break;
				case NumericSettingItem.MusicVolume:
					_settings.MusicVolume += (int)_step;
					break;
				case NumericSettingItem.BattleEffectVolume:
					_settings.BattleEffectVolume += (int)_step;
					break;
				case NumericSettingItem.RhythmEffectVolume:
					_settings.RhythmEffectVolume += (int)_step;
					break;
				case NumericSettingItem.HighSpeed:
					_settings.HighSpeed += _step;
					break;
			}
			_settings.Save();
		}

		private void Sub()
		{
			switch (_settingItem)
			{
				case NumericSettingItem.None:
					return;
				case NumericSettingItem.BgmVolume:
					_settings.BgmVolume -= (int)_step;
					BGMManager.Instance.ChangeBaseVolume(_settings.BgmVolume / 10f);
					break;
				case NumericSettingItem.SoundEffectVolume:
					_settings.SoundEffectVolume -= (int)_step;
					SEManager.Instance.ChangeBaseVolume(_settings.SoundEffectVolume / 10f);
					break;
				case NumericSettingItem.NovelEffectVolume:
					_settings.NovelEffectVolume -= (int)_step;
					break;
				case NumericSettingItem.MusicVolume:
					_settings.MusicVolume -= (int)_step;
					break;
				case NumericSettingItem.BattleEffectVolume:
					_settings.BattleEffectVolume -= (int)_step;
					break;
				case NumericSettingItem.RhythmEffectVolume:
					_settings.RhythmEffectVolume -= (int)_step;
					break;
				case NumericSettingItem.HighSpeed:
					_settings.HighSpeed -= _step;
					break;
			}
			_settings.Save();
		}
	}
}