using UnityEngine;

namespace Gallery
{
    [System.Serializable]
    public struct TrackInformation
    {
        [SerializeField] private string _id;
        public readonly string Id => _id;

        [SerializeField] private string _title;
        public readonly string Title => _title;

        [SerializeField] private bool _hasBeatmap;
        public readonly bool HasBeatmap => _hasBeatmap;

        [SerializeField] private AudioClip _sound;
        public readonly AudioClip Sound => _sound;

        [SerializeField] private string _composer;
        public readonly string Composer => _composer;

        [SerializeField] private string _description;
        public readonly string Description => _description;

        [SerializeField] private Sprite _thumbnail;
        public readonly Sprite Thumbnail => _thumbnail;
    }
}