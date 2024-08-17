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

        [SerializeField] private AudioClip _sound;
        public readonly AudioClip Sound => _sound;

        [SerializeField] private string _composer;
        public readonly string Composer => _composer;

        [SerializeField] private string _description;
        public readonly string Description => _description;
    }
}