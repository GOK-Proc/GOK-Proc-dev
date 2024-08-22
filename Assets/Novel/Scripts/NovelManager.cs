using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Novel
{
    public class NovelManager : MonoBehaviour
    {
        [SerializeField] private NovelId _novelId;
        [SerializeField] private ScenarioData _scenarioData;
        [SerializeField] private NovelOperation _novelOperation;

        [SerializeField] private int _currentLine = 0;

        private bool _doFirstLine = false;


        private void Start()
        {
            StartCoroutine(Initialize());
        }

        private void Update()
        {
            if (_doFirstLine)
            {
                CallLineOperation();

                _currentLine++;

                _doFirstLine = false;
            }

            if (Input.GetKeyDown(KeyCode.Return) && !_doFirstLine)
            {
                CallLineOperation();

                _currentLine++;
            }
        }

        private void CallLineOperation()
        {
            List<OperationData> lineOperationList = _scenarioData.ScenarioLines[_currentLine];

            // 各データに対して、foreachでExecuteOperationを実行できた方が綺麗かも
            foreach (OperationData lineOperation in lineOperationList)
            {
                lineOperation.ExecuteOperation(_novelOperation);
            }
        }

        public void OnDestroy()
        {
            StartCoroutine(UnloadAsset());
        }

        private IEnumerator Initialize()
        {
            // シナリオのロード
            yield return LoadAsset(_novelId.ToString(), scenario =>
            {
                ScenarioLoader.MakeScenarioData((TextAsset)scenario);
            });
            _scenarioData = ScenarioLoader._ScenarioData;

            // キャラクタアセットのロード
            yield return LoadAsset(AssetLabels.CharacterLables, characterImages =>
            {
                for (int i = 0; i < AssetLabels.CharacterLables.Count; i++)
                {
                    _novelOperation.CharacterPrefabs[AssetLabels.CharacterLables[i]] = (GameObject)characterImages[i];
                }
            });


            _doFirstLine = true;
        }

        private IEnumerator LoadAsset(string address, Action<UnityEngine.Object> callback)
        {
            var handle = Addressables.LoadAssetAsync<UnityEngine.Object>(address);
            yield return handle;

            callback(handle.Result);
        }

        private IEnumerator LoadAsset(List<string> addresses, Action<List<UnityEngine.Object>> callback)
        {
            var handle = Addressables.LoadAssetAsync<List<UnityEngine.Object>>(addresses);
            yield return handle;

            callback(handle.Result);
        }


        private IEnumerator UnloadAsset()
        {
            // 後で書く
            yield return null;
        }
    }
}
