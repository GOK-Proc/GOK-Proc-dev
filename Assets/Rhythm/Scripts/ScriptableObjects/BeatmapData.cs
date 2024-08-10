using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Rhythm
{
    [CreateAssetMenu]
    public class BeatmapData : ScriptableObject
    {
        public Dictionary<string, BeatmapInformation> BeatmapDictionary => _beatmaps.ToDictionary(x => x.Id, x => x);

        [SerializeField] private BeatmapInformation[] _beatmaps;
 
#if UNITY_EDITOR
        private void OnValidate()
        {
            var set = new HashSet<string>();

            foreach (var item in _beatmaps)
            {
                if (item.Id == string.Empty) continue;

                if (set.Contains(item.Id))
                {
                    Debug.LogWarning($"There is data with duplicate IDs: { item.Id }");
                }
                else
                {
                    set.Add(item.Id);
                }
            }
        }
#endif
    }
}
