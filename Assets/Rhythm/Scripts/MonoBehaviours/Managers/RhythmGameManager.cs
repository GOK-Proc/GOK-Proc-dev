using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rhythm
{
    public class RhythmGameManager : MonoBehaviour
    {
        [SerializeField] private NoteLayout _noteLayout;
        [SerializeField] private JudgeRange _judgeRange;
        [SerializeField] private Transform _noteParent;

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
        [SerializeField] private PlayerInput _playerInput;

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

            var notes = new List<NoteData>() { new NoteData(2f, 0, NoteColor.Red, false, 6, 0, 100), new NoteData(2f, 1, NoteColor.Red, false, 8, 0, 100), new NoteData(2f, 2, NoteColor.Red, false, 12, 0, 100) };

            _timeManager = new TimeManager();

            var attackActions = new InputAction[] { _playerInput.actions["Attack1"], _playerInput.actions["Attack2"], _playerInput.actions["Attack3"] };
            var defenseActions = new InputAction[] { _playerInput.actions["Defense1"], _playerInput.actions["Defense2"], _playerInput.actions["Defense3"] };

            _inputManager = new InputManager(attackActions, defenseActions, null);
            _cursorController = new CursorController(3, 0.05f, _inputManager);
            _scoreManger = new ScoreManger();

            _noteCreator = new NoteCreator(notes, _noteLayout, _judgeRange, notePrefabs, holdPrefabs, bandPrefabs, _noteParent, _timeManager, _inputManager, _cursorController);
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