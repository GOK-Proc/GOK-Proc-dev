using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Novel
{
    public class NovelManager : MonoBehaviour
    {
        [SerializeField] private NovelId _novelId;
        [SerializeField] private ScenarioData _scenario;
        [SerializeField] private NovelOperation _novelOperation;

        [SerializeField] private int _currentLine = 0;

        private void Start()
        {
            StartCoroutine(LoadAsset());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _currentLine++;
            }
        }

        public void OnDestroy()
        {
            StartCoroutine(UnloadAsset());
        }

        private IEnumerator LoadAsset()
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(_novelId.ToString());
            yield return handle;

            ScenarioLoader.MakeScenarioData(handle.Result);
        }

        private IEnumerator UnloadAsset()
        {
            // 後で書く
            yield return null;
        }
    }
}
