using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class HoldNote : Note
    {
        public override Judgement Judge(double time)
        {
            return default;
        }
    }
}