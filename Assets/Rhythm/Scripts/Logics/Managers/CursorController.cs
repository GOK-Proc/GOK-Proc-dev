using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Rhythm
{
    public class CursorController : IActiveLaneProvider
    {
        public IEnumerable<int> ActiveLanes
        {
            get
            {
                for (int i = 0; i < _laneCount; i++)
                {
                    if (_timeSinceLaneDeactivated[i] <= _extension)
                    {
                        yield return i;
                    }
                }
            }
        }

        private readonly int _laneCount;
        private readonly float _extension;
        private readonly NoteLayout _layout;
        private readonly float _duration;
        private readonly double[] _timeSinceLaneDeactivated;
        private readonly ObjectPool<EffectObject> _cursorPool;
        private readonly IMoveInputProvider _vectorInputProvider;

        private int _currentLane;
        private EffectObject _cursor;

        public CursorController(int laneCount, float extension, NoteLayout layout, float duration, EffectObject cursorPrefab, Transform parent, IMoveInputProvider vectorInputProvider)
        {
            _laneCount = laneCount;
            _extension = extension;
            _layout = layout;
            _duration = duration;
            _cursorPool = new ObjectPool<EffectObject>(cursorPrefab, parent);
            _vectorInputProvider = vectorInputProvider;

            _timeSinceLaneDeactivated = new double[laneCount];
            _currentLane = 0;

            for (int i = 0; i < _laneCount; i++)
            {
                if (i == _currentLane)
                {
                    _timeSinceLaneDeactivated[i] = 0;
                }
                else
                {
                    _timeSinceLaneDeactivated[i] = extension;
                }
            }

            Create();
        }

        public void Move()
        {
            var move = _vectorInputProvider.Move;
            var isMoved = false;

            if (move > 0)
            {
                if (_currentLane < _laneCount - 1)
                {
                    _currentLane++;
                    _timeSinceLaneDeactivated[_currentLane] = 0;
                    isMoved = true;
                } 
            }
            else if (move < 0)
            {
                if (_currentLane > 0)
                {
                    _currentLane--;
                    _timeSinceLaneDeactivated[_currentLane] = 0;
                    isMoved = true;
                }
            }

            if (isMoved)
            {
                if (_cursor != null)
                {
                    _cursor.Stop((t, s, d) =>
                    {
                        t.DOLocalMoveX(_layout.FirstLaneX + _currentLane * _layout.LaneDistanceX, _duration).OnComplete(() =>
                        {
                            d?.Invoke();
                        });
                    });
                }
                Create();
            }
        }

        public void Update()
        {
            for (int i = 0; i < _laneCount; i++)
            {
                if (i == _currentLane)
                {
                    _timeSinceLaneDeactivated[i] = 0;
                }
                else
                {
                    _timeSinceLaneDeactivated[i] += Time.deltaTime;
                }
            }
        }

        private void Create()
        {
            IDisposable disposable = _cursorPool.Create(out _cursor, out _);
            var pos = new Vector3(_layout.FirstLaneX + _currentLane * _layout.LaneDistanceX, _layout.JudgeLineY, 0f);

            _cursor.Create(disposable);
            _cursor.Play(pos);
        }
    }
}