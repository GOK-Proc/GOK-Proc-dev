using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gallery
{
    [CreateAssetMenu]
    public class Album : ScriptableObject
    {
        public Dictionary<string, MusicInformation> AlbumDictionary =>
            _musics.ToDictionary(x => x.Id, x => x);

        [SerializeField] private MusicInformation[] _musics;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var set = new HashSet<string>();

            foreach (var item in _musics)
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