using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct RhythmOption
    {
        [SerializeField] private float _scrollSpeed;
        public float ScrollSpeed { get => _scrollSpeed; set => _scrollSpeed = value; }

        [SerializeField] private double _offset;
        public double Offset { get => _offset; set => _offset = value; }

        [SerializeField] private double _judgeOffset;
        public double JudgeOffset { get => _judgeOffset; set => _judgeOffset = value; }

        [SerializeField] private RhythmVolumeOption _volumeOption;
        public RhythmVolumeOption VolumeOption { get => _volumeOption; set => _volumeOption = value; }
    }

    [System.Serializable]
    public struct RhythmVolumeOption
    {
        [SerializeField, Range(0f, 1f)] private float _track;
        public float Track { get => _track; set => _track = Mathf.Clamp01(value); }

        [SerializeField, Range(0f, 1f)] private float _se;
        public float Se { get => _se; set => _se = Mathf.Clamp01(value); }

        [SerializeField, Range(0f, 1f)] private float _noteSe;
        public float NoteSe { get => _noteSe; set => _noteSe = Mathf.Clamp01(value); }
    }
}