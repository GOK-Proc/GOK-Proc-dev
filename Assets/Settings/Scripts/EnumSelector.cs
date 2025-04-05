using KanKikuchi.AudioManager;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Settings
{
	[RequireComponent(typeof(Selectable))]
	public class EnumSelector : MonoBehaviour, ISelectHandler, IDeselectHandler, ICancelHandler
	{
		[SerializeField] private UserSettings _settings;
		[SerializeField] private TextMeshProUGUI _label;
		[SerializeField] private TextMeshProUGUI _valueText;
		[SerializeField] private Image _leftArrow;
		[SerializeField] private Image _rightArrow;
		[SerializeField] private Image _valueFrame;
		[SerializeField] private TabLabel _tabLabel;

		[Space(10)]
		[SerializeField] private EnumSettingItem _settingItem;

		private static readonly Color32 _nomalColor = new Color32(r: 57, g: 57, b: 57, a: 255);
		private static readonly Color32 _selectedColor = new Color32(r: 214, g: 77, b: 42, a: 255);

		private static readonly Dictionary<ScenarioDifficulty, Color> DifficultyColors = new Dictionary<ScenarioDifficulty, Color>()
		{
			{ ScenarioDifficulty.None, Color.white },
			{ ScenarioDifficulty.Easy, new Color(0.73f, 1.00f, 0.78f) },
			{ ScenarioDifficulty.Hard, new Color(1.00f, 0.73f, 0.73f) },
			{ ScenarioDifficulty.Expert, new Color(0.87f, 0.73f, 1.00f) }
		};

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

		public void OnCancel(BaseEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(_tabLabel.gameObject);
		}

		public void OnMove(BaseEventData eventData)
		{
			AxisEventData axisEventData = eventData as AxisEventData;

			if (axisEventData.moveDir == MoveDirection.Left)
			{
				Sub();
				SEManager.Instance.Play(SEPath.SYSTEM_SELECT, _settings.SoundEffectVolume / 10f);
			}
			else if (axisEventData.moveDir == MoveDirection.Right)
			{
				Add();
				SEManager.Instance.Play(SEPath.SYSTEM_SELECT, _settings.SoundEffectVolume / 10f);
			}

				UpdateView();
		}

		private void UpdateView()
		{
			switch (_settingItem)
			{
				case EnumSettingItem.KeyConfigId:
					_valueText.text = _settings.KeyConfigId.ToString();
					ToggleLeftArrowVisibility(!IsMinValue(_settings.KeyConfigId));
					ToggleRightArrowVisibility(!IsMaxValue(_settings.KeyConfigId));
					break;
				case EnumSettingItem.ScreenMode:
					_valueText.text = _settings.ScreenMode.ToString();
					ToggleLeftArrowVisibility(!IsMinValue(_settings.ScreenMode));
					ToggleRightArrowVisibility(!IsMaxValue(_settings.ScreenMode));
					break;
				case EnumSettingItem.FrameRate:
					_valueText.text = _settings.FrameRate.ToString();
					ToggleLeftArrowVisibility(!IsMinValue(_settings.FrameRate));
					ToggleRightArrowVisibility(!IsMaxValue(_settings.FrameRate));
					break;
				case EnumSettingItem.ScenarioDifficulty:
					_valueText.text = _settings.ScenarioDifficulty.ToString();
					_valueFrame.color = DifficultyColors[_settings.ScenarioDifficulty];
					ToggleLeftArrowVisibility(!IsMinValue(_settings.ScenarioDifficulty));
					ToggleRightArrowVisibility(!IsMaxValue(_settings.ScenarioDifficulty));
					break;
			}
		}

		private void Add()
		{
			switch (_settingItem)
			{
				case EnumSettingItem.KeyConfigId:
					_settings.KeyConfigId = IncrementEnum(_settings.KeyConfigId);
					break;
				case EnumSettingItem.ScreenMode:
					_settings.ScreenMode = IncrementEnum(_settings.ScreenMode);
					_settings.ApplySettings();
					break;
				case EnumSettingItem.FrameRate:
					_settings.FrameRate = IncrementEnum(_settings.FrameRate);
					_settings.ApplySettings();
					break;
				case EnumSettingItem.ScenarioDifficulty:
					_settings.ScenarioDifficulty = IncrementEnum(_settings.ScenarioDifficulty);
					break;
			}
			_settings.Save();
		}

		private void Sub()
		{
			switch (_settingItem)
			{
				case EnumSettingItem.KeyConfigId:
					_settings.KeyConfigId = DecrementEnum(_settings.KeyConfigId);
					break;
				case EnumSettingItem.ScreenMode:
					_settings.ScreenMode = DecrementEnum(_settings.ScreenMode);
					_settings.ApplySettings();
					break;
				case EnumSettingItem.FrameRate:
					_settings.FrameRate = DecrementEnum(_settings.FrameRate);
					_settings.ApplySettings();
					break;
				case EnumSettingItem.ScenarioDifficulty:
					_settings.ScenarioDifficulty = DecrementEnum(_settings.ScenarioDifficulty);
					break;
			}
			_settings.Save();
		}

		private void ToggleLeftArrowVisibility(bool toggle)
		{
			if (toggle)
			{
				Color color = _leftArrow.color;
				color.a = 1f;
				_leftArrow.color = color;
			}
			else
			{
				Color color = _leftArrow.color;
				color.a = 0f;
				_leftArrow.color = color;
			}
		}

		private void ToggleRightArrowVisibility(bool toggle)
		{
			if (toggle)
			{
				Color color = _rightArrow.color;
				color.a = 1f;
				_rightArrow.color = color;
			}
			else
			{
				Color color = _rightArrow.color;
				color.a = 0f;
				_rightArrow.color = color;
			}
		}

		private static bool IsMaxValue<T>(T enumValue) where T : Enum
		{
			int maxIndex = Enum.GetValues(typeof(T)).Length - 1;

			return Convert.ToInt32(enumValue) == maxIndex;
		}

		private static bool IsMinValue<T>(T enumValue) where T : Enum
		{
			if (typeof(T) == typeof(ScenarioDifficulty))
			{
				return Convert.ToInt32(enumValue) <= 1;
			}

			return Convert.ToInt32(enumValue) == 0;
		}

		private static T IncrementEnum<T>(T enumValue) where T : Enum
		{
			int currentIndex = Convert.ToInt32(enumValue);
			int maxIndex = Enum.GetValues(typeof(T)).Length - 1;

			if (currentIndex >= maxIndex)
			{
				return enumValue;
			}

			return (T)Enum.ToObject(typeof(T), currentIndex + 1);
		}

		private static T DecrementEnum<T>(T enumValue) where T : Enum
		{
			int currentIndex = Convert.ToInt32(enumValue);

			if (typeof(T) == typeof(ScenarioDifficulty) && currentIndex <= 1)
			{
				return enumValue;
			}

			if (currentIndex <= 0)
			{
				return enumValue;
			}

			return (T)Enum.ToObject(typeof(T), currentIndex - 1);
		}
	}
}