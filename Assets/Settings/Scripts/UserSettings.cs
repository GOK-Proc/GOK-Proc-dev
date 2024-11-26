using UnityEngine;

namespace Settings
{
	[CreateAssetMenu]
	public class UserSettings : EncryptedScriptableObject
	{
		[Header("Audio Settings")]

		[Range(0, 10)]
		[SerializeField] private int _bgmVolume = 5;

		[Range(0, 10)]
		[SerializeField] private int _soundEffectVolume = 5;

		[Range(0, 10)]
		[SerializeField] private int _musicVolume = 5;

		[Range(0, 10)]
		[SerializeField] private int _rhythmEffectVolume = 5;

		[Header("Play Settings")]

		[SerializeField] private KeyConfigId _keyConfigId;

		[Range(0.5f, 3.0f)]
		[SerializeField] private float _highSpeed = 1f;

		[Header("General Settings")]

		[SerializeField] private ScreenMode _screenMode;

		[SerializeField] private FrameRate _frameRate;

		[SerializeField] private ScenarioDifficulty _scenarioDifficulty;
	}
}