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

        [SerializeField] private Sprite _backgroundSprite;
        public readonly Sprite BackgroundSprite => _backgroundSprite;

        [SerializeField] private Sprite _enemySprite;
        public readonly Sprite EnemySprite => _enemySprite;

        [SerializeField] private NotesInformation[] _notes;
        public readonly NotesInformation[] Notes => _notes;

    }

    [System.Serializable]
    public struct NotesInformation
    {
        [SerializeField] private int _level;
        public readonly int Level => _level;

        [SerializeField] private string _notesDesigner;
        public readonly string NotesDesigner => _notesDesigner;

        [SerializeField] private TextAsset _file;
        public readonly TextAsset File => _file;
    }

    public struct HeaderInformation
    {
        public string Title { get; private set; }
        public string Composer { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public int Level { get; private set; }

        public HeaderInformation(in BeatmapInformation beatmap, Difficulty difficulty)
        {
            Title = beatmap.Title;
            Composer = beatmap.Composer;
            Difficulty = difficulty;
            Level = beatmap.Notes[(int)difficulty].Level;
        }
    }
}