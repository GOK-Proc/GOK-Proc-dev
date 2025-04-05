using UnityEngine;
using System;
using KanKikuchi.AudioManager;

namespace Settings
{
	[CreateAssetMenu]
	public class UserSettings : EncryptedScriptableObject
	{
		[Header("Audio Settings")]

		[Range(0, 10)]
		[SerializeField] private int _bgmVolume = 5;
		public int BgmVolume { get => _bgmVolume; set => _bgmVolume = Math.Clamp(value, 0, 10); }

		[Range(0, 10)]
		[SerializeField] private int _soundEffectVolume = 5;
		public int SoundEffectVolume { get => _soundEffectVolume; set => _soundEffectVolume = Math.Clamp(value, 0, 10); }

		[Range(0, 10)]
		[SerializeField] private int _novelEffectVolume = 5;
		public int NovelEffectVolume { get => _novelEffectVolume; set => _novelEffectVolume = Math.Clamp(value, 0, 10); }

		[Range(0, 10)]
		[SerializeField] private int _musicVolume = 5;
		public int MusicVolume { get => _musicVolume; set => _musicVolume = Math.Clamp(value, 0, 10); }

		[Range(0, 10)]
		[SerializeField] private int _battleEffectVolume = 5;
		public int BattleEffectVolume { get => _battleEffectVolume; set => _battleEffectVolume = Math.Clamp(value, 0, 10); }

		[Range(0, 10)]
		[SerializeField] private int _rhythmEffectVolume = 5;
		public int RhythmEffectVolume { get => _rhythmEffectVolume; set => _rhythmEffectVolume = Math.Clamp(value, 0, 10); }

		[Header("Play Settings")]

		[SerializeField] private KeyConfigId _keyConfigId;
		public KeyConfigId KeyConfigId { get => _keyConfigId; set => _keyConfigId = value; }

		[Range(-1f, 1f)]
		[SerializeField] private float _judgeOffset = 0f;
		public float JudgeOffset
		{
			get => _judgeOffset;
			// 小数第2位までの値に丸める
			// NOTE: _judgeOffsetの範囲は-1.0f～1.0fで、0.05f刻みの値
			set => _judgeOffset = Math.Clamp((float)Math.Round(value, 2, MidpointRounding.AwayFromZero), -1f, 1f);
		}

		[Range(0.5f, 3.0f)]
		[SerializeField] private float _highSpeed = 1f;
		public float HighSpeed
		{
			get => _highSpeed;
			set => _highSpeed = Math.Clamp((float)Math.Round(value * 10) / 10, 0.5f, 3.0f);
		}

		[Header("General Settings")]

		[SerializeField] private ScreenMode _screenMode;
		public ScreenMode ScreenMode { get => _screenMode; set => _screenMode = value; }

		[SerializeField] private FrameRate _frameRate;
		public FrameRate FrameRate { get => _frameRate; set => _frameRate = value; }

		[SerializeField] private ScenarioDifficulty _scenarioDifficulty;
		public ScenarioDifficulty ScenarioDifficulty { get => _scenarioDifficulty; set => _scenarioDifficulty = value; }

		public void ApplySettings()
		{
			BGMManager.Instance.ChangeBaseVolume(BgmVolume / 10f);

			SEManager.Instance.ChangeBaseVolume(SoundEffectVolume / 10f);

			switch (ScreenMode)
			{
				case ScreenMode.Windowed:
					Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, FullScreenMode.Windowed);
					break;
				case ScreenMode.FullScreen:
					Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
					break;
			}

			switch (FrameRate)
			{
				case FrameRate.VSync:
					QualitySettings.vSyncCount = 1;
					Application.targetFrameRate = -1;
					break;
				case FrameRate.Fps30:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 30;
					break;
				case FrameRate.Fps60:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 60;
					break;
				case FrameRate.Fps120:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 120;
					break;
				case FrameRate.Fps144:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 144;
					break;
				case FrameRate.Fps240:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 240;
					break;
				case FrameRate.Unlimited:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = -1;
					break;
			}
		}
	}
}