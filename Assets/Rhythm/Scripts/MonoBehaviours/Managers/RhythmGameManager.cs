using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Transition;

namespace Rhythm
{
    public class RhythmGameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private UIManager _uiManager;

        [Space(20)]
        [Header("Rhythm Settings")]
        [SerializeField] private int _laneCount;
        [SerializeField] private NoteLayout _noteLayout;
        [SerializeField] private JudgeRange _judgeRange;
        [SerializeField] private float _cursorExtension;
        [SerializeField] private float _cursorDuration;
        [SerializeField] private double _startDelay;

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
        [SerializeField] private Transform _holdMaskParent;
     
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
        [SerializeField] private EffectObject _cursorPrefab;
        [SerializeField] private Transform _holdMaskPrefab;

        [Space(20)]
        [Header("Inputs")]
        [SerializeField] private PlayerInput _playerInput;

        [System.Serializable]
        private struct Sound
        {
            public string Id;
            public AudioClip Clip;
        }

        [Space(20)]
        [Header("Sounds")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Sound[] _sounds;

        [Space(20)]
        [Header("Beatmap")]
        [SerializeField] private BeatmapData _beatmapData;
        [SerializeField] private bool _overrideSettings;
        [SerializeField] private string _defaultId;
        [SerializeField] private Difficulty _defaultDifficulty;
        [SerializeField] private bool _defaultIsVs;

        [Space(20)]
        [Header("Options")]
        [SerializeField] private float _baseScroll;


        private NoteCreator _noteCreator;
        private NoteJudge _noteJudge;
        private TimeManager _timeManager;
        private ScoreManger _scoreManager;
        private InputManager _inputManager;
        private CursorController _cursorController;
        private SoundPlayer _soundPlayer;
        private LaneEffectManager _laneEffectManager;

        private HeaderInformation _headerInformation;
        private double _endTime;

        private void Awake()
        {
            var id = SceneTransitionManager.CurrentRhythmId.ToString();
            var difficulty = SceneTransitionManager.CurrentDifficulty;
            var isVs = SceneTransitionManager.CurrentIsVs;

            var dictionary = _beatmapData.BeatmapDictionary;

            if (_overrideSettings || !dictionary.ContainsKey(id))
            {
                id = _defaultId;
                difficulty = _defaultDifficulty;
                isVs = _defaultIsVs;

                if (!_overrideSettings) Debug.LogWarning("The specified ID does not exist. Using the default ID.");
            }

            var beatmapInfo = dictionary[id];
            var notesData = beatmapInfo.Notes[(int)difficulty];

            (var notes, var endTime) = BeatmapLoader.Parse(notesData.File, beatmapInfo.Offset, _baseScroll);
            _endTime = endTime;

            _headerInformation = new HeaderInformation(beatmapInfo, difficulty);
            _uiManager.DrawHeader(_headerInformation);

            var notePrefabs = _notePrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);
            var holdPrefabs = _holdPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);
            var bandPrefabs = _bandPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);

            var holdMasks = new List<Transform>();

            for (int i = 0; i < _laneCount; i++)
            {
                var pos = _holdMaskPrefab.position;
                pos.x = _noteLayout.FirstLaneX + _noteLayout.LaneDistanceX * i;
                holdMasks.Add(Instantiate(_holdMaskPrefab, pos, _holdMaskPrefab.rotation, _holdMaskParent));
            }

            _timeManager = new TimeManager();

            var attackActions = new InputAction[] { _playerInput.actions["Attack1"], _playerInput.actions["Attack2"], _playerInput.actions["Attack3"] };
            var defenseActions = new InputAction[] { _playerInput.actions["Defense1"], _playerInput.actions["Defense2"], _playerInput.actions["Defense3"] };
            var moveActions = new InputAction[] { _playerInput.actions["Move1"], _playerInput.actions["Move2"], _playerInput.actions["Move3"] };

            _inputManager = new InputManager(attackActions, defenseActions, moveActions);

            var sounds = _sounds.ToDictionary(x => x.Id, x => x.Clip);

            _soundPlayer = new SoundPlayer(_audioSource, beatmapInfo.Sound, sounds);

            _cursorController = new CursorController(_laneCount, _cursorExtension, _noteLayout, _cursorDuration, _cursorPrefab, _cursorParent, _inputManager);

            _scoreManager = new ScoreManger(isVs, difficulty, _judgeRates, _lostRates, _comboBonus, _scoreRates, _scoreRankBorders, _gaugeRates, BeatmapLoader.GetNoteCount(notes), BeatmapLoader.GetNotePointCount(notes, _largeRate), _playerHitPoint, _uiManager, _uiManager);

            _noteCreator = new NoteCreator(isVs, notes, _noteLayout, _judgeRange, notePrefabs, holdPrefabs, bandPrefabs, _noteParent, holdMasks, _timeManager, _inputManager, _cursorController, _uiManager);
            _noteJudge = new NoteJudge(isVs, _noteLayout, _noteCreator, _scoreManager, _scoreManager, _scoreManager, _uiManager);

            _laneEffectManager = new LaneEffectManager(_noteLayout, _inputManager, _cursorController, _uiManager);

            _uiManager.SetClearGaugeBorder(_gaugeRates[(int)difficulty].Border);
            _uiManager.SwitchUI(isVs);
        }

        // Start is called before the first frame update
        private void Start()
        {
            IEnumerator RhythmGameUpdate()
            {
                void Update()
                {
                    _noteCreator.Create();
                    _inputManager.Update();
                    _cursorController.Move();
                    _cursorController.Update();
                    _laneEffectManager.Flash();
                    _noteJudge.Judge();
                }

                _timeManager.StartTimer(-_startDelay);

                while (_timeManager.Time < Time.deltaTime / 2)
                {
                    Update();
                    yield return null;
                }

                _soundPlayer.PlayMusic();

                while (_timeManager.Time < _endTime)
                {
                    Update();
                    yield return null;
                }

                _scoreManager.DisplayResult(_headerInformation);
            }

            StartCoroutine(RhythmGameUpdate());
        }
    }
}