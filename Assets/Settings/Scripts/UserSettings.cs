﻿using UnityEngine;
using System;

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
		public float JudgeOffset { get => _judgeOffset; set => _judgeOffset = Math.Clamp(value, -1f, 1f); }

		[Range(0.5f, 3.0f)]
		[SerializeField] private float _highSpeed = 1f;
		public float HighSpeed { get => _highSpeed; set => _highSpeed = Math.Clamp(value, 0.5f, 3.0f); }

		[Header("General Settings")]

		[SerializeField] private ScreenMode _screenMode;
		public ScreenMode ScreenMode { get => _screenMode; set => _screenMode = value; }

		[SerializeField] private FrameRate _frameRate;
		public FrameRate FrameRate { get => _frameRate; set => _frameRate = value; }

		[SerializeField] private ScenarioDifficulty _scenarioDifficulty;
		public ScenarioDifficulty ScenarioDifficulty { get => _scenarioDifficulty; set => _scenarioDifficulty = value; }
	}
}