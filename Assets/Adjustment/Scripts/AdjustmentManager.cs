using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using Settings;
using Transition;

public class AdjustmentManager : MonoBehaviour
{
    [SerializeField] private UserSettings _userSettings;
    [SerializeField] private CustomButton _startButton;
    [SerializeField] private CustomButton _skipButton;
    [SerializeField] private CustomButton _oKButton;
    [SerializeField] private CustomButton _retryButton;
    [SerializeField] private Slider _slider;
    [SerializeField] private Transform _auto;
    [SerializeField] private Transform _manual;

    [SerializeField] private float _bpm;
    [SerializeField] private float _offset;
    [SerializeField] private float _delay;

    private AudioSource _audioSource;
    private InputAction _pressAnyKeyAction = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");

    private void OnEnable() => _pressAnyKeyAction.Enable();
    private void OnDisable() => _pressAnyKeyAction.Disable();

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _auto.gameObject.SetActive(true);
        _manual.gameObject.SetActive(false);

        SelectAuto();
    }

    private void SelectAuto()
    {
        _startButton.Select();
    }

    private void SelectManual()
    {
        _slider.Select();
    }

    public void OnStartButtonClick()
    {
        var interval = 60 / _bpm;
        var differences = new List<double>();

        IEnumerator Adjust()
        {
            _startButton.interactable = false;
            _skipButton.interactable = false;

            yield return new WaitForSeconds(_delay);

            var startTime = Time.timeAsDouble;
            _audioSource.Play();

            yield return new WaitUntil(() => _audioSource.isPlaying);

            _audioSource.time = (float)(Time.timeAsDouble - startTime);

            while (_audioSource.isPlaying)
            {
                if (_pressAnyKeyAction.triggered)
                {
                    var time = Time.timeAsDouble - startTime - _offset;
                    var difference = (time - interval / 2) % interval - interval / 2;

                    if (difference >= -interval / 2) differences.Add(difference);
                }
                yield return null;
            }

            yield return new WaitForSeconds(_delay);

            _auto.gameObject.SetActive(false);
            _manual.gameObject.SetActive(true);

            _startButton.interactable = true;
            _skipButton.interactable = true;

            var judgeOffset = differences.Any() ? differences.Average() : 0;

            // 0.01秒のズレにつき，スライダーの値1とする
            _slider.value = Mathf.RoundToInt((float)judgeOffset * 100);

            SelectManual();
        }

        StartCoroutine(Adjust());
    }

    public void OnSkipButtonClick()
    {
        _auto.gameObject.SetActive(false);
        _manual.gameObject.SetActive(true);

        // JudgeOffsetの値0.05につき，スライダーの値1とする
        _slider.value = Mathf.RoundToInt(_userSettings.JudgeOffset * 20);

        SelectManual();
    }

    public void OnOKButtonClick()
    {
        _oKButton.interactable = false;
        _retryButton.interactable = false;

        // スライダーの値1につき，JudgeOffsetの値0.05とする
        _userSettings.JudgeOffset = _slider.value / 20f;
        _userSettings.Save();

        SceneTransitionManager.TransitionToSettings();
    }

    public void OnRetryButtonClick()
    {
        _auto.gameObject.SetActive(true);
        _manual.gameObject.SetActive(false);

        SelectAuto();
    }
}
