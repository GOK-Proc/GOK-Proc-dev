using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct RhythmSetting
    {
        [SerializeField] private float _scrollSpeed;
        public float ScrollSpeed { readonly get => _scrollSpeed; set => _scrollSpeed = value; }

        [SerializeField] private double _offset;
        public double Offset { readonly get => _offset; set => _offset = value; }

        [SerializeField] private double _judgeOffset;
        public double JudgeOffset { readonly get => _judgeOffset; set => _judgeOffset = value; }

        [SerializeField] private KeyConfig _keyConfig;
        public KeyConfig KeyConfig { readonly get => _keyConfig; set => _keyConfig = value; }

        [SerializeField] private RhythmVolumeSetting _volumeSetting;
        public RhythmVolumeSetting VolumeSetting { readonly get => _volumeSetting; set => _volumeSetting = value; }
    }

    [System.Serializable]
    public struct RhythmVolumeSetting
    {
        [SerializeField, Range(0f, 1f)] private float _track;
        public float Track { readonly get => _track; set => _track = Mathf.Clamp01(value); }

        [SerializeField, Range(0f, 1f)] private float _se;
        public float Se { readonly get => _se; set => _se = Mathf.Clamp01(value); }

        [SerializeField, Range(0f, 1f)] private float _noteSe;
        public float NoteSe { readonly get => _noteSe; set => _noteSe = Mathf.Clamp01(value); }
    }
}