using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class LaneObject : RhythmGameObject
    {
        public int Lane { get; private set; }
        public NoteColor Color { get; private set; }
        public bool IsLarge { get; private set; }


        public void Create(Vector3 position, Vector3 velocity, int lane, NoteColor color, bool isLarge)
        {
            Create(position, velocity);
            Lane = lane;
            Color = color;
            IsLarge = isLarge;
        }
    }
}