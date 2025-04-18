﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Transition;
using Settings;

namespace Rhythm
{
    public class RhythmGameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private EventManager _eventManager;

        [Space(20)]
        [Header("Rhythm Settings")]
        [SerializeField] private int _laneCount;
        [SerializeField] private NoteLayout _noteLayout;
        [SerializeField] private JudgeRange _judgeRange;
        [SerializeField] private float _cursorExtension;
        [SerializeField] private float _cursorDuration;
        [SerializeField] private double _startDelay;
        [SerializeField] private double _adjustThreshold;

        [Space(20)]
        [Header("Battle Settings")]
        [SerializeField] private float _playerHitPoint;
        [SerializeField] private JudgeRate[] _judgeRates;
        [SerializeField] private LostRate[] _lostRates;
        [SerializeField] private int _largeRate;
        [SerializeField] private ComboBonus[] _comboBonus;

        [Space(20)]
        [Header("Score Settings")]
        [SerializeField] private float[] _scoreRates;
        [SerializeField] private int[] _scoreRankBorders;
        [SerializeField] private GaugeRate[] _gaugeRates;

        [Space(20)]
        [Header("Objects")]
        [SerializeField] private Transform _noteParent;
        [SerializeField] private Transform _cursorParent;
     
        [System.Serializable]
        private struct NotePrefab<T> where T : RhythmGameObject
        {
            public NoteColor Color;
            public bool IsLarge;
            public T Prefab;
        }

        [SerializeField] private NotePrefab<TapNote>[] _notePrefabs;
        [SerializeField] private NotePrefab<HoldNote>[] _holdPrefabs;
        [SerializeField] private NotePrefab<HoldBand>[] _bandPrefabs;
        [SerializeField] private RhythmGameObject _linePrefab;
        [SerializeField] private EffectObject _cursorPrefab;

        [Space(20)]
        [Header("Inputs")]
        [SerializeField] private PlayerInput _playerInput;

        [System.Serializable]
        private struct Sound
        {
            public string Id;
            public AudioClip Clip;
            public bool IsNoteSe;
            public int SourceCount;
            public bool IsLoop;
        }

        [System.Serializable]
        private struct IntroSound
        {
            public string Id;
            public AudioClip IntroClip;
            public AudioClip MainClip;
            public bool IsLoop;
        }

        [Space(20)]
        [Header("Sounds")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _seSource;
        [SerializeField] private IntroSoundPlayer _introSoundPlayer;
        [SerializeField] private Sound[] _sounds;
        [SerializeField] private IntroSound[] _introSounds;
        [SerializeField] private AudioSource _subAudioSource;

        [Space(20)]
        [Header("Beatmap")]
        [SerializeField] private BeatmapData _beatmapData;
        [SerializeField] private string _defaultId;
        [SerializeField] private Difficulty _defaultDifficulty;
        [SerializeField] private bool _defaultIsVs;
        [SerializeField] private TutorialId _tutorialId;

        [Space(20)]
        [Header("Rhythm Game Settings")]
        [SerializeField] private RhythmSetting _setting;
        [SerializeField] private UserSettings _userSettings;

        [Space(20)]
        [Header("Record")]
        [SerializeField] private RecordList _recordList;

        [Space(20)]
        [Header("Tutorial")]
        [SerializeField] private TutorialData _tutorialData;
        [SerializeField] private float _skipDelay;


        private NoteCreator _noteCreator;
        private NoteJudge _noteJudge;
        private TimeManager _timeManager;
        private ScoreManager _scoreManager;
        private InputManager _inputManager;
        private CursorController _cursorController;
        private SoundPlayer _soundPlayer;
        private LaneEffectManager _laneEffectManager;
        private TutorialManager _tutorialManager;
 
        private HeaderInformation _headerInformation;
        private double _endTime;
        private bool _canSkipTutorial;

        private void Awake()
        {
            var id = SceneTransitionManager.CurrentRhythmId.ToString();
            var difficulty = SceneTransitionManager.CurrentDifficulty;
            var isVs = SceneTransitionManager.CurrentIsVs;
            var tutorialId = SceneTransitionManager.CurrentTutorialId;

            _setting.ScrollSpeed *= _userSettings.HighSpeed;
            _setting.JudgeOffset = _userSettings.JudgeOffset / 5;
            _setting.KeyConfig = _userSettings.KeyConfigId;
            _setting.VolumeSetting.Track = _userSettings.MusicVolume / 10f;
            _setting.VolumeSetting.Se = _userSettings.BattleEffectVolume / 10f;
            _setting.VolumeSetting.NoteSe = _userSettings.RhythmEffectVolume / 10f;

            var dictionary = _beatmapData.BeatmapDictionary;

            if (id == "None" && tutorialId == TutorialId.None)
            {
                id = _defaultId;
                difficulty = _defaultDifficulty;
                isVs = _defaultIsVs;
                tutorialId = _tutorialId;

                Debug.Log("The settings were overridden.");
            }
            else
            {
                if (!dictionary.ContainsKey(id))
                {
                    throw new System.Exception("The specified ID does not exist.");
                }
            }

            if (!isVs) tutorialId = TutorialId.None;

            if (tutorialId != TutorialId.None) difficulty = Difficulty.Easy;

            _canSkipTutorial = tutorialId == TutorialId.Battle;

            var beatmapInfo = tutorialId != TutorialId.None ? _tutorialData.Beatmap : dictionary[id];
            var notesData = beatmapInfo.Notes[(int)difficulty];

            (var notes, var lines, var endTime) = BeatmapLoader.Parse(notesData.File, beatmapInfo.Offset + _setting.Offset, _setting.ScrollSpeed);
            _endTime = endTime;

            _headerInformation = new HeaderInformation(beatmapInfo, difficulty);
            _uiManager.DrawHeader(_headerInformation, tutorialId != TutorialId.None);

            var notePrefabs = _notePrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);
            var holdPrefabs = _holdPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);
            var bandPrefabs = _bandPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);

            _timeManager = new TimeManager();

            _playerInput.SwitchCurrentActionMap(_setting.KeyConfig.ToString());
            var attackActions = _playerInput.currentActionMap.actions.Where(x => x.name.StartsWith("Attack")).ToArray();
            var defenseActions = _playerInput.currentActionMap.actions.Where(x => x.name.StartsWith("Defense")).ToArray();
            var moveActions = _playerInput.currentActionMap.actions.Where(x => x.name.StartsWith("Move")).ToArray(); ;

            _inputManager = new InputManager(attackActions, defenseActions, moveActions);

            var sounds = _sounds.ToDictionary(x => x.Id, x => new SoundPlayer.AudioClipData(x.Clip, x.IsNoteSe, x.SourceCount, x.IsLoop));
            var introSounds = _introSounds.ToDictionary(x => x.Id, x => new SoundPlayer.IntroAudioData(x.IntroClip, x.MainClip, x.IsLoop));

            _soundPlayer = new SoundPlayer(_audioSource, _seSource, _subAudioSource, _introSoundPlayer, beatmapInfo.Sound, sounds, introSounds, _setting.VolumeSetting);

            _cursorController = new CursorController(_laneCount, _cursorExtension, _noteLayout, _cursorDuration, _cursorPrefab, _cursorParent, _inputManager, _soundPlayer);

            _scoreManager = new ScoreManager(isVs, id, difficulty, tutorialId != TutorialId.None, _judgeRates, _lostRates, _comboBonus, _scoreRates, _scoreRankBorders, _gaugeRates, BeatmapLoader.GetNoteCount(notes), BeatmapLoader.GetNotePointCount(notes, _largeRate), _playerHitPoint, _largeRate, _soundPlayer, _uiManager, _uiManager, _uiManager, _recordList);

            _noteCreator = new NoteCreator(isVs, notes, lines, _noteLayout, _judgeRange, _setting.JudgeOffset, notePrefabs, holdPrefabs, bandPrefabs, _linePrefab, _noteParent, _timeManager, _inputManager, _cursorController, _soundPlayer, _uiManager);
            _noteJudge = new NoteJudge(isVs, _noteLayout, _noteCreator, _scoreManager, _scoreManager, _scoreManager, _scoreManager, _soundPlayer, _uiManager);

            _laneEffectManager = new LaneEffectManager(_noteLayout, _inputManager, _cursorController, _soundPlayer, _uiManager);

            _tutorialManager = new TutorialManager(tutorialId != TutorialId.None, _setting.KeyConfig, _tutorialData, _playerInput, _subAudioSource, _soundPlayer, _timeManager, _uiManager);

            _eventManager.Initialize(isVs, tutorialId, _setting.KeyConfig, _scoreManager, _soundPlayer, _soundPlayer, _inputManager, _inputManager, _uiManager, _uiManager, _tutorialManager);
            _eventManager.SetSoundVolumeSlider(_setting.VolumeSetting);

            _uiManager.SetClearGaugeBorder(_gaugeRates[(int)difficulty].Border);
            _uiManager.SetBackgroundSprite(beatmapInfo.BackgroundSprite);
            _uiManager.SetEnemySprite(beatmapInfo.EnemySprite);
            _uiManager.SetPauseKeyConfig(_setting.KeyConfig);
            _uiManager.SwitchUI(isVs);
        }

        // Start is called before the first frame update
        private void Start()
        {
            var complete = true;

            IEnumerator RhythmGameUpdate()
            {
                void Update()
                {
                    _tutorialManager.Update();
                    _noteCreator.Create();
                    _inputManager.Update();
                    _cursorController.Move();
                    _cursorController.Update();
                    _laneEffectManager.Flash();
                    _noteJudge.Judge();
                }

                _timeManager.StartTimer(-_startDelay);

                while (_timeManager.Time < -_skipDelay)
                {
                    Update();
                    yield return null;
                }

                if (_canSkipTutorial)
                {
                    Time.timeScale = 0;
                    _playerInput.SwitchCurrentActionMap("None");
                    _uiManager.DrawSkipScreen();
                    _eventManager.SelectSkipNoButton();
                }

                while (_timeManager.Time < Time.deltaTime / 2)
                {
                    Update();
                    yield return null;
                }

                _soundPlayer.PlayMusic();
                _noteCreator.AdjustPosition(_soundPlayer.Time - _timeManager.Time);
                _timeManager.Time = _soundPlayer.Time;

                while (_timeManager.Time < _endTime)
                {
                    if (_soundPlayer.IsPlayingMusic)
                    {
                        var difference = _soundPlayer.Time - _timeManager.Time;

                        if (Mathf.Abs((float)difference) >= _adjustThreshold)
                        {
                            _noteCreator.AdjustPosition(difference);
                            _timeManager.Time = _soundPlayer.Time;
                        }
                    }

                    Update();

                    if (_scoreManager.IsGameOver)
                    {
                        _noteCreator.MarkAsJudged();
                        complete = false;
                        break;
                    }

                    yield return null;
                }

                _playerInput.SwitchCurrentActionMap("None");

                if (_scoreManager.IsWin || _scoreManager.IsClear)
                {
                    _soundPlayer.PlayIntroSE("Victory", 0.7f);
                    _uiManager.StopWarningLayer();
                }
                else
                {
                    _soundPlayer.PlaySE("Lose", 0.7f);
                }

                if (!complete)
                {
                    _uiManager.DrawKnockout();
                    _soundPlayer.FadeOutMusic(0.5f);
                    yield return new WaitForSeconds(2f);
                }

                _eventManager.SelectNextButton();
                _scoreManager.DisplayResult(_headerInformation);
                _scoreManager.SaveRecordData();
            }

            StartCoroutine(RhythmGameUpdate());
        }

        private void OnDestroy()
        {
            _userSettings.MusicVolume = (int)(_setting.VolumeSetting.Track * 10);
            _userSettings.BattleEffectVolume = (int)(_setting.VolumeSetting.Se * 10);
            _userSettings.RhythmEffectVolume = (int)(_setting.VolumeSetting.NoteSe * 10);
            _userSettings.Save();
        }
    }
}