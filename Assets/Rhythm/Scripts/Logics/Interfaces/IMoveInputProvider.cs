using UnityEngine;

namespace Rhythm
{
    public interface IMoveInputProvider
    {
        float Move { get; }
        bool IsMoveInputValid { get; set; }
    }
}