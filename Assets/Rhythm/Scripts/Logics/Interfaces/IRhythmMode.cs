using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IRhythmMode
    {
        bool IsClear { get; }
        int Score { get; }
    }
}