using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Rhythm
{
    [CreateAssetMenu, System.Serializable]
    public class RecordList : ScriptableObject, IDataHandler<RecordData[]>
    {
#if UNITY_EDITOR
        private readonly string _path = Path.Combine(Application.dataPath, "Rhythm/Records/RecordList.json");
#else
		private readonly string _path = Path.Combine(Application.persistentDataPath, "RecordList.json");
#endif

        [System.Serializable]
        private struct RecordDataPair
        {
            public string Id;
            public RecordData[] Data;

            public RecordDataPair(string id, RecordData[] data)
            {
                Id = id;
                Data = data;
            }
        }

        [SerializeField] private List<RecordDataPair> _recordDataList;

        private void OnEnable()
        {
            if (_recordDataList == null) _recordDataList = new List<RecordDataPair>();

#if !UNITY_EDITOR
            Load();
#endif
        }

        private void OnDisable()
        {
#if !UNITY_EDITOR           
            Save();
#endif
        }

        private void Save()
        {
            var json = JsonUtility.ToJson(this);
            using var writer = new StreamWriter(_path);
            writer.Write(json);
        }

        private void Load()
        {
            if (!File.Exists(_path)) return;

            using var reader = new StreamReader(_path);
            var json = reader.ReadToEnd();
            JsonUtility.FromJsonOverwrite(json, this);
        }


        public RecordData[] this[string id]
        {
            get => _recordDataList.Find(x => x.Id == id).Data ?? new RecordData[System.Enum.GetValues(typeof(Difficulty)).Length];
            set
            {
                _recordDataList.RemoveAll(x => x.Id == id);
                _recordDataList.Add(new RecordDataPair(id, value));

#if !UNITY_EDITOR
                Save();
#endif
            }
        }
    }
}