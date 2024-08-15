using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IComboCountable
    {
        int Combo { get; }
        int MaxCombo { get; }
    }
}