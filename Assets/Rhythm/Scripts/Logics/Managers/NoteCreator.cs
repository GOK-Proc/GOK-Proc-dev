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

        private readonly bool _isVs;
        private readonly IList<NoteData> _data;
        private readonly IList<bool> _isCreated;
        private readonly NoteLayout _layout;
        private readonly (Vector2 UpperLeft, Vector2 LowerRight) _survivalRect;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<TapNote>> _notePools;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<HoldNote>> _holdPools;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<HoldBand>> _bandPools;
        private readonly IList<Transform> _holdMasks;

        private readonly ITimeProvider _timeProvider;
        private readonly IEffectDrawable _effectDrawable;

        private readonly List<Note> _notes;
        private int _noteCount;


        public NoteCreator(bool isVs, IList<NoteData> data, in NoteLayout layout, JudgeRange judgeRange, double judgeOffset, IDictionary<(NoteColor, bool), TapNote> notePrefabs, IDictionary<(NoteColor, bool), HoldNote> holdPrefabs, IDictionary<(NoteColor, bool), HoldBand> bandPrefabs, Transform parent, IList<Transform> holdMasks, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider, IEffectDrawable effectDrawable)
        {
            _isVs = isVs;
            _data = data;
            _isCreated = data.Select(x => false).ToList();
            _layout = layout;
            _survivalRect = (new Vector2(float.NegativeInfinity, float.PositiveInfinity), new Vector2(float.PositiveInfinity, _layout.DestroyLineY));

            _notePools = notePrefabs.ToDictionary(x => x.Key, x => new ObjectPool<TapNote>(x.Value, parent, x => x.Initialize(judgeRange, judgeOffset, timeProvider, colorInputProvider, activeLaneProvider)));
            _holdPools = holdPrefabs.ToDictionary(x => x.Key, x => new ObjectPool<HoldNote>(x.Value, parent, x => x.Initialize(judgeRange, judgeOffset, timeProvider, colorInputProvider, activeLaneProvider)));
            _bandPools = bandPrefabs.ToDictionary(x => x.Key, x => new ObjectPool<HoldBand>(x.Value, parent, x => x.Initialize(timeProvider, colorInputProvider)));
            _holdMasks = holdMasks;
            _timeProvider = timeProvider;
            _effectDrawable = effectDrawable;

            _notes = new List<Note>();
            _noteCount = 0;
        }

        public void Create()
        {
            (T obj, bool isNew) Create<T>(NoteData note, ObjectPool<T> pool, Vector3 pos, double time) where T : Note
            {
                IDisposable disposable = pool.Create(out var obj, out var isNew);
                var id = _noteCount++;
                obj.Create(pos, new Vector3(0f, -note.Speed), _survivalRect, note.Lane, time, id, disposable);
                if (_isVs && note.Color == NoteColor.Blue) _effectDrawable.DrawEnemyAttackEffect((float)(_effectDrawable.GetTimeToCreateEnemyAttackEffect(time) - _timeProvider.Time), id);
                return (obj, isNew);
            }

            void Add((Note obj, bool isNew) note)
            {
                if (note.isNew)
                {
                    _notes.Add(note.obj);

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
                var note = _data[i];
                var firstPosition = new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - note.JustTime));

                if (!_isCreated[i])
                {
                    if (firstPosition.y <= _layout.BeginLineY || _effectDrawable.GetTimeToCreateEnemyAttackEffect(note.JustTime) >= _timeProvider.Time)
                    {
                        if (_notePools.ContainsKey((note.Color, note.IsLarge)))
                        {
                            Add(Create(note, _notePools[(note.Color, note.IsLarge)], firstPosition, note.JustTime));
                            _isCreated[i] = true;
                        }

                        if (note.Length > 0)
                        {
                            if (_holdPools.ContainsKey((note.Color, note.IsLarge)) && note.Bpm > 0)
                            {
                                var deltaTime = 30 / note.Bpm;
                                var time = note.JustTime + deltaTime;
                                var endTime = note.JustTime + note.Length;

                                while (time < endTime)
                                {
                                    var position = new Vector3(firstPosition.x, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - time));
                                    Add(Create(note, _holdPools[(note.Color, note.IsLarge)], position, time));
                                    time += deltaTime;
                                }

                                var lastPosition = new Vector3(firstPosition.x, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - endTime));
                                Add(Create(note, _holdPools[(note.Color, note.IsLarge)], lastPosition, time));

                                IDisposable disposable = _bandPools[(note.Color, note.IsLarge)].Create(out var obj, out _);

                                var bandPosition = (firstPosition + lastPosition) / 2;
                                var rect = (_survivalRect.UpperLeft, _survivalRect.LowerRight - new Vector2(0f, bandPosition.y));
                                obj.Create(bandPosition, new Vector3(0f, -note.Speed), rect, note.Lane, (lastPosition - firstPosition).y, note.JustTime, endTime, _holdMasks[note.Lane], disposable);
                            }
                        }
                        
                    }
                }
            }
        }
    }
}