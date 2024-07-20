using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public struct BeatmapInformation
    {
        [SerializeField] private string _id;
        public readonly string Id => _id;

        [SerializeField] private string _title;
        public readonly string Title => _title;

        [SerializeField] private AudioClip _sound;
        public readonly AudioClip Sound => _sound;

        [SerializeField] private double _offset;
        public readonly double Offset => _offset;

        [SerializeField] private string _composer;
        public readonly string Composer => _composer;

        [SerializeField] private string _notesDesigner;
        public readonly string NotesDesigner => _notesDesigner;

        [SerializeField] private TextAsset _file;
        public readonly TextAsset File => _file;

    }
}