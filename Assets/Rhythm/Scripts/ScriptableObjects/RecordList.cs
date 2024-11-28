using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Rhythm
{
    [CreateAssetMenu, System.Serializable]
    public class RecordList : EncryptedScriptableObject, IDataHandler<RecordData[]>
    {
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

        [SerializeField] private List<RecordDataPair> _recordDataList = new List<RecordDataPair>();

        public RecordData[] this[string id]
        {
            get => _recordDataList.Find(x => x.Id == id).Data ?? new RecordData[System.Enum.GetValues(typeof(Difficulty)).Length];
            set
            {
                _recordDataList.RemoveAll(x => x.Id == id);
                _recordDataList.Add(new RecordDataPair(id, value));

                Save();
            }
        }
    }
}