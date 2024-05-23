using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class LaneObjectManager : INoteProvider, ILaneObjectProvider
    {
        public bool ExistNotes { get; }
        public IEnumerable<Note> Notes { get; }
        public bool ExistLaneObjects { get; }
        public IEnumerable<LaneObject> LaneObjects { get; }

        public void Create()
        {

        }

        public void Destroy()
        {

        }
    }
}