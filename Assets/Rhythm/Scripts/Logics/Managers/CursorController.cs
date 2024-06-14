using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private readonly IVectorInputProvider _vectorInputProvider;
        private readonly double[] _timeSinceLaneDeactivated;

        private int _currentLane;

        public CursorController(int laneCount, float extension, IVectorInputProvider vectorInputProvider)
        {
            _laneCount = laneCount;
            _extension = extension;
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
        }

        public void Move()
        {
            var vector = _vectorInputProvider.Vector;

            if (vector.x > 0)
            {
                if (_currentLane < _laneCount - 1)
                {
                    _currentLane++;
                    _timeSinceLaneDeactivated[_currentLane] = 0;
                } 
            }
            else if (vector.x < 0)
            {
                if (_currentLane > 0)
                {
                    _currentLane--;
                    _timeSinceLaneDeactivated[_currentLane] = 0;
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
    }
}