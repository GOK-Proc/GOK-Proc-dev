using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public class BeatmapInformation
    {
        [SerializeField] private string _id;
        public string Id { get => _id; }

        [SerializeField] private string _title;
        public string Title { get => _title; }

        [SerializeField] private AudioClip _sound;
        public AudioClip Sound { get => _sound; }

        [SerializeField] private double _offset;
        public double Offset { get => _offset; }

        [SerializeField] private string _composer;
        public string Composer { get => _composer; }

        [SerializeField] private string _notesDesigner;
        public string NotesDesigner { get => _notesDesigner; }

        [SerializeField] private TextAsset _file;
        public TextAsset File { get => _file; }

    }
}