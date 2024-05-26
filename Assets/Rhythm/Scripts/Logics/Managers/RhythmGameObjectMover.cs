using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class RhythmGameObjectMover
    {
        private readonly IRhythmGameObjectProvider _rhythmGameObjectProvider;

        public RhythmGameObjectMover(IRhythmGameObjectProvider rhythmGameObjectProvider)
        {
            _rhythmGameObjectProvider = rhythmGameObjectProvider;
        }

        public void Move()
        {
            foreach (var obj in _rhythmGameObjectProvider.RhythmGameObjects)
            {
                obj.Move();
            }
        }
    }
}