using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    [CreateAssetMenu]
    public class BeatmapData : ScriptableObject
    {
        public Dictionary<string, BeatmapInformation> BeatmapDictionary { get; private set; } = new Dictionary<string, BeatmapInformation>();

        [SerializeField] private BeatmapInformation[] _beatmaps;
 
#if UNITY_EDITOR
        private void OnValidate()
        {
            BeatmapDictionary.Clear();

            foreach (var item in _beatmaps)
            {
                if (item.Id == string.Empty) continue;

                if (BeatmapDictionary.ContainsKey(item.Id))
                {
                    Debug.LogWarning("There is data with duplicate IDs.");
                }
                else
                {
                    BeatmapDictionary.Add(item.Id, item);
                }
            }
        }
#endif
    }
}
