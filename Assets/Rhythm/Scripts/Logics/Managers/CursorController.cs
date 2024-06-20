using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
                    if (_timeSinceLaneDeactivated[i] < _extension)
                    {
                        yield return i;
                    }
                }
            }
        }

        private readonly int _laneCount;
        private readonly float _extension;
        private readonly NoteLayout _layout;
        private readonly Vector3 _velocity;
        private readonly double[] _timeSinceLaneDeactivated;
        private readonly ObjectPool<Cursor> _cursorPool;
        private readonly IMoveInputProvider _vectorInputProvider;

        private int _currentLane;
        private Cursor _cursor;

        public CursorController(int laneCount, float extension, NoteLayout layout, Vector3 velocity, Cursor cursorPrefab, Transform parent, IMoveInputProvider vectorInputProvider)
        {
            _laneCount = laneCount;
            _extension = extension;
            _layout = layout;
            _velocity = velocity;
            _cursorPool = new ObjectPool<Cursor>(cursorPrefab, parent);
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

            if (move > 0)
            {
                if (_currentLane < _laneCount - 1)
                {
                    _currentLane++;
                    _timeSinceLaneDeactivated[_currentLane] = 0;

                    if (_cursor != null)
                    {
                        _cursor.ChangeVelocity(Mathf.Sign(move) * _velocity);
                    }
                    Create();
                } 
            }
            else if (move < 0)
            {
                if (_currentLane > 0)
                {
                    _currentLane--;
                    _timeSinceLaneDeactivated[_currentLane] = 0;

                    if (_cursor != null)
                    {
                        _cursor.ChangeVelocity(Mathf.Sign(move) * _velocity);
                    }
                    Create();
                }
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
            var lanePosition = new Vector2(_layout.FirstLaneX + _currentLane * _layout.LaneDistanceX, _layout.JudgeLineY);
            var delta = new Vector2(_layout.LaneDistanceX, -1f);
            _cursor.Create(lanePosition, Vector3.zero, (lanePosition - delta, lanePosition + delta), disposable);
        }
    }
}