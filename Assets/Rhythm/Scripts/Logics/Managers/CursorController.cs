using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class CursorController : IActiveLaneProvider
    {
        public IEnumerable<int> ActiveLanes { get; }

        public void Move()
        {

        }
    }
}