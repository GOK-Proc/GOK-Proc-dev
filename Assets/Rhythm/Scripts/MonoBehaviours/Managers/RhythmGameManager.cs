using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rhythm
{
    public class RhythmGameManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _laneCount;
        [SerializeField] private NoteLayout _noteLayout;
        [SerializeField] private JudgeRange _judgeRange;
        [SerializeField] private float _cursorExtension;
        [SerializeField] private float _cursorSpeed;

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
        [SerializeField] private Cursor _cursorPrefab;
        [SerializeField] private Transform _holdMaskPrefab;
        [SerializeField] private PlayerInput _playerInput;

        [Space(20)]
        [Header("Beatmap")]
        [SerializeField] private TextAsset _beatmapFile;
        [SerializeField] private double _offset;

        [Space(20)]
        [Header("Options")]
        [SerializeField] private float _baseScroll;


        private NoteCreator _noteCreator;
        private NoteJudge _noteJudge;
        private TimeManager _timeManager;
        private ScoreManger _scoreManger;
        private InputManager _inputManager;
        private CursorController _cursorController;

        private void Awake()
        {
            var notePrefabs = _notePrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);
            var holdPrefabs = _holdPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);
            var bandPrefabs = _bandPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);

            var notes = BeatmapLoader.Parse(_beatmapFile, _offset, _baseScroll);

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
            _cursorController = new CursorController(_laneCount, _cursorExtension, _noteLayout, new Vector3(_cursorSpeed, 0f), _cursorPrefab, _cursorParent, _inputManager);
            _scoreManger = new ScoreManger();

            _noteCreator = new NoteCreator(notes, _noteLayout, _judgeRange, notePrefabs, holdPrefabs, bandPrefabs, _noteParent, holdMasks, _timeManager, _inputManager, _cursorController);
            _noteJudge = new NoteJudge(_noteCreator, _scoreManger);
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            _noteCreator.Create();
            _inputManager.Update();
            _cursorController.Move();
            _cursorController.Update();
            _noteJudge.Judge();
        }
    }
}