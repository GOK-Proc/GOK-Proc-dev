using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        [SerializeField] private NotePrefab<Note>[] _notePrefabs;
        [SerializeField] private NotePrefab<HoldNote>[] _holdPrefabs;
        [SerializeField] private NotePrefab<HoldBand>[] _bandPrefabs;

        private RhythmGameObjectManager _rhythmGameObjectManager;
        private RhythmGameObjectMover _rhythmGameObjectMover;
        private TimeManager _timeManager;
        private InputManager _inputManager;
        private CursorController _cursorController;

        private void Awake()
        {
            var notePrefabs = _notePrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);
            var holdPrefabs = _holdPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);
            var bandPrefabs = _bandPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => x.Prefab);

            var notes = new List<NoteData>() { new NoteData(2f, 0, NoteColor.Red, false, 6, 0, 100), new NoteData(2f, 1, NoteColor.Red, false, 8, 0, 100), new NoteData(2f, 2, NoteColor.Red, false, 12, 0, 100) };

            _timeManager = new TimeManager();

            _rhythmGameObjectManager = new RhythmGameObjectManager(notes, _noteLayout, _judgeRange, notePrefabs, holdPrefabs, bandPrefabs, _noteParent, _timeManager, _inputManager, _cursorController);
            _rhythmGameObjectMover = new RhythmGameObjectMover(_rhythmGameObjectManager);
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            _rhythmGameObjectMover.Move();
            _rhythmGameObjectManager.Create();
            _rhythmGameObjectManager.Destroy();
        }
    }
}