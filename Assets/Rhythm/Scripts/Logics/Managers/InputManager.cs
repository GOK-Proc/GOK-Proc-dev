using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class InputManager : IColorInputProvider, IVectorInputProvider
    {
        public Vector2 Vector { get; }

        public bool IsColorPressed(NoteColor color)
        {
            return default;
        }

        public bool IsColorPressedThisFrame(NoteColor color)
        {
            return default;
        }

        public bool IsColorJudged(NoteColor color)
        {
            return default;
        }

        public void CompleteColorJudge(NoteColor color)
        {

        }
    }
}