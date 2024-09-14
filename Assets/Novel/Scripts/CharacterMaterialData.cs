using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Novel
{
    [CreateAssetMenu]
    public class NovelMaterialData : ScriptableObject
    {
        public Dictionary<string, CharacterMatarial> CharacterMaterialDict => _characterMaterials.ToDictionary(x => x.Name, x => x);
        public Dictionary<string, Sprite> BackgroundMaterialDict => _backgroundMaterials.ToDictionary(x => x.name, x => x);

        [SerializeField] private CharacterMatarial[] _characterMaterials;
        [SerializeField] private Sprite[] _backgroundMaterials;
    }

    [System.Serializable]
    public class CharacterMatarial
    {
        [SerializeField] private string _name;
        public string Name => _name;

        public GameObject Default => _default;

        public Dictionary<string, GameObject> CharacterDifferenceDict
        { 
            get
            {
                var characterDifferenceDict = _characterDifferences.ToDictionary(x => x.name, x => x);
                characterDifferenceDict.Add(_default.name, _default);
                return characterDifferenceDict;
            }
        }

        [SerializeField] private GameObject _default;
        [SerializeField] private GameObject[] _characterDifferences;
    }
}
