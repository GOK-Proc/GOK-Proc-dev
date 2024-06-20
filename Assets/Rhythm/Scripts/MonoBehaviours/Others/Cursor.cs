using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class Cursor : RhythmGameObject
    {
        public void ChangeVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }
    }
}