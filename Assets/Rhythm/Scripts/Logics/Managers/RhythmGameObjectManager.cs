using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rhythm
{
    public class RhythmGameObjectManager : INoteProvider, IRhythmGameObjectProvider
    {
        public IEnumerable<Note> Notes
        {
            get
            {
                for (int i = 0; i < _rhythmGameObjects.Count; i++)
                {
                    if (_isObjectAlives[i])
                    {
                        if (_rhythmGameObjects[i] is Note note)
                        {
                            yield return note;
                        }
                    }
                }
            }
        }

        public IEnumerable<RhythmGameObject> RhythmGameObjects
        {
            get
            {
                for (int i = 0; i < _rhythmGameObjects.Count; i++)
                {
                    if (_isObjectAlives[i])
                    {
                        yield return _rhythmGameObjects[i];
                    }
                }
            }
        }


        private readonly IList<NoteData> _data;
        private readonly NoteLayout _layout;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<Note>> _notePools;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<HoldNote>> _holdPools;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<HoldBand>> _bandPools;
        private readonly ITimeProvider _timeProvider;

        private readonly int?[] _objectIndexes;
        private readonly List<RhythmGameObject> _rhythmGameObjects;
        private readonly List<bool> _isObjectAlives;

        private const int DestroyedIndex = -1;

        public RhythmGameObjectManager(IList<NoteData> data, NoteLayout layout, JudgeRange judgeRange, IDictionary<(NoteColor, bool), Note> notePrefabs, IDictionary<(NoteColor, bool), HoldNote> holdPrefabs, IDictionary<(NoteColor, bool), HoldBand> bandPrefabs, Transform parent, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider)
        {
            _data = data;
            _layout = layout;
            _notePools = notePrefabs.ToDictionary(x => x.Key, x => new ObjectPool<Note>(x.Value, parent, x => (x as Note).Initialize(judgeRange, timeProvider, colorInputProvider, activeLaneProvider)));
            _holdPools = holdPrefabs.ToDictionary(x => x.Key, x => new ObjectPool<HoldNote>(x.Value, parent, x => (x as Note).Initialize(judgeRange, timeProvider, colorInputProvider, activeLaneProvider)));
            _bandPools = bandPrefabs.ToDictionary(x => x.Key, x => new ObjectPool<HoldBand>(x.Value, parent));
            _timeProvider = timeProvider;

            _objectIndexes = new int?[data.Count];
            _rhythmGameObjects = new List<RhythmGameObject>();
            _isObjectAlives = new List<bool>();
        }

        public void Create()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                var note = _data[i];
                var pos = new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - note.JustTime));

                if (!_objectIndexes[i].HasValue)
                {
                    if (pos.y <= _layout.CreateNoteY)
                    {
                        if (note.Length > 0)
                        {

                        }
                        else
                        {
                            if (_notePools.ContainsKey((note.Color, note.IsLarge)))
                            {
                                var newObject = _notePools[(note.Color, note.IsLarge)].Create();
                                newObject.Create(pos, new Vector3(0f, -note.Speed), note.Lane, note.JustTime);
                                _objectIndexes[i] = _rhythmGameObjects.Count;
                                _rhythmGameObjects.Add(newObject);
                                _isObjectAlives.Add(true);
                            }
                        }
                    }
                }
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                var note = _data[i];
                var pos = new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - note.JustTime));

                if (_objectIndexes[i].HasValue && _objectIndexes[i].Value != DestroyedIndex)
                {
                    if (pos.y <= _layout.DestroyNoteY)
                    {
                        if (note.Length > 0)
                        {

                        }
                        else
                        {
                            if (_notePools.ContainsKey((note.Color, note.IsLarge)))
                            {
                                var oldObject = _rhythmGameObjects[_objectIndexes[i].Value] as Note;
                                oldObject.Destroy();
                                _notePools[(note.Color, note.IsLarge)].Destroy(oldObject);
                                _isObjectAlives[_objectIndexes[i].Value] = false;
                                _objectIndexes[i] = DestroyedIndex;
                            }
                        }
                    }
                }
            }
        }
    }
}