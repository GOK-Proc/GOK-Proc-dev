using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rhythm
{
    public class LaneEffectManager
    {
        private readonly NoteLayout _layout;
        private readonly IColorInputProvider _colorInputProvider;
        private readonly IActiveLaneProvider _activeLaneProvider;
        private readonly IEffectDrawable _effectDrawable;

        public LaneEffectManager(in NoteLayout layout, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider, IEffectDrawable effectDrawable)
        {
            _layout = layout;
            _colorInputProvider = colorInputProvider;
            _activeLaneProvider = activeLaneProvider;
            _effectDrawable = effectDrawable;
        }

        public void Flash()
        {
            void FlashColor(NoteColor color)
            {
                if (_colorInputProvider.IsColorPressedThisFrame(color))
                {
                    _effectDrawable.DrawLaneFlash(new Vector3(_layout.FirstLaneX + _layout.LaneDistanceX * _activeLaneProvider.ActiveLanes.First(), 0f, 0f), color);
                }
            }

            FlashColor(NoteColor.Red);
            FlashColor(NoteColor.Blue);
        }
    }
}