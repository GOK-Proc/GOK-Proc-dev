using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gallery
{
    [CreateAssetMenu]
    public class TrackData : ScriptableObject
    {
        public Dictionary<string, TrackInformation> AlbumDictionary =>
            _tracks.ToDictionary(x => x.Id, x => x);

        [SerializeField] private TrackInformation[] _tracks;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var set = new HashSet<string>();

            foreach (var item in _tracks)
            {
                if (item.Id == string.Empty)
                {
                    continue;
                }

                if (!set.Add(item.Id))
                {
                    Debug.LogWarning($"There are data with duplicate IDs: {item.Id}");
                }
            }
        }
#endif
    }
}