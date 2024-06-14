using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rhythm
{
    public class NoteCreator : INoteProvider
    {
        public IEnumerable<Note> Notes
        {
            get
            {
                foreach (var note in _notes)
                {
                    if (note.IsAlive) yield return note;
                }
            }
        }

        private readonly IList<(NoteData, bool)> _data;
        private readonly NoteLayout _layout;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<TapNote>> _notePools;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<HoldNote>> _holdPools;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<HoldBand>> _bandPools;
        private readonly ITimeProvider _timeProvider;

        private readonly List<Note> _notes;


        public NoteCreator(IList<NoteData> data, NoteLayout layout, JudgeRange judgeRange, IDictionary<(NoteColor, bool), TapNote> notePrefabs, IDictionary<(NoteColor, bool), HoldNote> holdPrefabs, IDictionary<(NoteColor, bool), HoldBand> bandPrefabs, Transform parent, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider)
        {
            _data = data.Select(x => (x, false)).ToList();
            _layout = layout;
            _notePools = notePrefabs.ToDictionary(x => x.Key, x => new ObjectPool<TapNote>(x.Value, parent, x => (x as Note).Initialize(judgeRange, timeProvider, colorInputProvider, activeLaneProvider)));
            _holdPools = holdPrefabs.ToDictionary(x => x.Key, x => new ObjectPool<HoldNote>(x.Value, parent, x => (x as Note).Initialize(judgeRange, timeProvider, colorInputProvider, activeLaneProvider)));
            _bandPools = bandPrefabs.ToDictionary(x => x.Key, x => new ObjectPool<HoldBand>(x.Value, parent));
            _timeProvider = timeProvider;

            _notes = new List<Note>();
        }

        public void Create()
        {
            void Add(Note obj, bool isNew)
            {
                if (isNew)
                {
                    _notes.Add(obj);

                    for (int i = _notes.Count - 1; i >= 1; i--)
                    {
                        if (_notes[i].CompareTo(_notes[i - 1]) < 0)
                        {
                            (_notes[i], _notes[i - 1]) = (_notes[i - 1], _notes[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _notes.Count - 1; i++)
                    {
                        if (_notes[i].CompareTo(_notes[i + 1]) > 0)
                        {
                            (_notes[i], _notes[i + 1]) = (_notes[i + 1], _notes[i]);
                        }
                    }
                }
            }

            for (int i = 0; i < _data.Count; i++)
            {
                var (note, isCreated) = _data[i];
                var pos = new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - note.JustTime));

                if (!isCreated)
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
                                IDisposable disposable = _notePools[(note.Color, note.IsLarge)].Create(out var obj, out var isNew);
                                obj.Create(pos, new Vector3(0f, -note.Speed), new Vector3(pos.x, _layout.DestroyNoteY), note.Lane, note.JustTime, disposable);
                                _data[i] = (note, true);
                                Add(obj, isNew);
                            }
                        }
                    }
                }
            }
        }
    }
}