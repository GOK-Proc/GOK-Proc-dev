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
        private readonly IList<NoteData> _noteData;
        private readonly IList<LineData> _lineData;
        private readonly IList<bool> _isNoteCreated;
        private readonly IList<bool> _isLineCreated;
        private readonly NoteLayout _layout;
        private readonly (Vector2 UpperLeft, Vector2 LowerRight) _survivalRect;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<TapNote>> _notePools;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<HoldNote>> _holdPools;
        private readonly IDictionary<(NoteColor, bool), ObjectPool<HoldBand>> _bandPools;
        private readonly ObjectPool<RhythmGameObject> _linePool;
        private readonly IList<Transform> _holdMasks;

        private readonly ITimeProvider _timeProvider;
        private readonly IEffectDrawable _effectDrawable;

        private readonly List<Note> _notes;
        private readonly List<RhythmGameObject> _rhythmGameObjects;
        private int _noteCount;


        public NoteCreator(bool isVs, IList<NoteData> noteData, IList<LineData> lineData, in NoteLayout layout, JudgeRange judgeRange, double judgeOffset, IDictionary<(NoteColor, bool), TapNote> notePrefabs, IDictionary<(NoteColor, bool), HoldNote> holdPrefabs, IDictionary<(NoteColor, bool), HoldBand> bandPrefabs, RhythmGameObject linePrefab, Transform parent, IList<Transform> holdMasks, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider, ISoundPlayable soundPlayable, IEffectDrawable effectDrawable)
        {
            _isVs = isVs;
            _noteData = noteData;
            _lineData = lineData;
            _isNoteCreated = noteData.Select(x => false).ToArray();
            _isLineCreated = lineData.Select(x => false).ToArray();
            _layout = layout;
            _survivalRect = (new Vector2(float.NegativeInfinity, float.PositiveInfinity), new Vector2(float.PositiveInfinity, _layout.DestroyLineY));

            _notePools = notePrefabs.ToDictionary(x => x.Key, x => new ObjectPool<TapNote>(x.Value, parent, x => x.Initialize(judgeRange, judgeOffset, timeProvider, colorInputProvider, activeLaneProvider)));
            _holdPools = holdPrefabs.ToDictionary(x => x.Key, x => new ObjectPool<HoldNote>(x.Value, parent, x => x.Initialize(judgeRange, judgeOffset, timeProvider, colorInputProvider, activeLaneProvider)));
            _bandPools = bandPrefabs.ToDictionary(x => x.Key, x => new ObjectPool<HoldBand>(x.Value, parent, x => x.Initialize(timeProvider, colorInputProvider, soundPlayable)));
            _linePool = new ObjectPool<RhythmGameObject>(linePrefab, parent);
            _holdMasks = holdMasks;
            _timeProvider = timeProvider;
            _effectDrawable = effectDrawable;

            _notes = new List<Note>();
            _rhythmGameObjects = new List<RhythmGameObject>();
            _noteCount = 0;
        }

        public void Create()
        {
            (T obj, bool isNew) CreateNote<T>(NoteData note, ObjectPool<T> pool, Vector3 pos, double time, bool isLarge = false) where T : Note
            {
                IDisposable disposable = pool.Create(out var obj, out var isNew);
                var id = _noteCount++;
                obj.Create(pos, new Vector3(0f, -note.Speed), _survivalRect, note.Lane, time, id, disposable);
                if (_isVs && note.Color == NoteColor.Blue) _effectDrawable.DrawEnemyAttackEffect(isLarge, (float)(_effectDrawable.GetTimeToCreateEnemyAttackEffect(time) - _timeProvider.Time), id);
                return (obj, isNew);
            }

            (RhythmGameObject obj, bool isNew) CreateLine(LineData line, ObjectPool<RhythmGameObject> pool, Vector3 pos)
            {
                IDisposable disposable = pool.Create(out var obj, out var isNew);
                obj.Create(pos, new Vector3(0f, -line.Speed), _survivalRect, disposable);
                return (obj, isNew);
            }

            void Add((Note obj, bool isNew) note)
            {
                if (note.isNew)
                {
                    _notes.Add(note.obj);
                    _rhythmGameObjects.Add(note.obj);

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

            for (int i = 0; i < _noteData.Count; i++)
            {
                var note = _noteData[i];
                var firstPosition = new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * note.Lane, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - note.JustTime));

                if (!_isNoteCreated[i])
                {
                    if (firstPosition.y <= _layout.BeginLineY || _effectDrawable.GetTimeToCreateEnemyAttackEffect(note.JustTime) <= _timeProvider.Time)
                    {
                        if (_notePools.ContainsKey((note.Color, note.IsLarge)))
                        {
                            Add(CreateNote(note, _notePools[(note.Color, note.IsLarge)], firstPosition, note.JustTime, note.IsLarge));
                            _isNoteCreated[i] = true;
                        }

                        if (note.Length > 0)
                        {
                            if (_holdPools.ContainsKey((note.Color, note.IsLarge)) && note.Bpm > 0)
                            {
                                var deltaTime = 30 / note.Bpm;
                                var time = note.JustTime + deltaTime;
                                var endTime = note.JustTime + note.Length;

                                while (time < endTime && !Mathf.Approximately((float)time, (float)endTime))
                                {
                                    var position = new Vector3(firstPosition.x, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - time));
                                    Add(CreateNote(note, _holdPools[(note.Color, note.IsLarge)], position, time));
                                    time += deltaTime;
                                }

                                var lastPosition = new Vector3(firstPosition.x, _layout.JudgeLineY - note.Speed * (float)(_timeProvider.Time - endTime));
                                Add(CreateNote(note, _holdPools[(note.Color, note.IsLarge)], lastPosition, time));

                                IDisposable disposable = _bandPools[(note.Color, note.IsLarge)].Create(out var obj, out var isNew);

                                var bandLength = lastPosition.y - firstPosition.y;
                                var rect = (_survivalRect.UpperLeft, _survivalRect.LowerRight - new Vector2(0f, bandLength));
                                obj.Create(firstPosition, new Vector3(0f, -note.Speed), rect, note.Lane, bandLength, note.JustTime, endTime, _holdMasks[note.Lane], disposable);
                                if (isNew) _rhythmGameObjects.Add(obj);
                            }
                        }
                        
                    }
                }
            }

            for (int i = 0; i < _lineData.Count; i++)
            {
                var line = _lineData[i];
                var position = new Vector3(_layout.CenterX, _layout.JudgeLineY - line.Speed * (float)(_timeProvider.Time - line.JustTime));

                if (!_isLineCreated[i])
                {
                    if (position.y <= _layout.BeginLineY)
                    {
                        (var obj, var isNew) = CreateLine(line, _linePool, position);
                        if (isNew) _rhythmGameObjects.Add(obj);
                        _isLineCreated[i] = true;
                    }
                }
            }
        }
        
        public void MarkAsJudged()
        {
            foreach (var note in _notes)
            {
                note.MarkAsJudged();
            }
        }

        public void AdjustPosition(double difference)
        {
            foreach (var obj in _rhythmGameObjects)
            {
                if (obj.IsAlive)
                {
                    obj.AdjustPosition(difference);
                }
            }
        }
    }
}