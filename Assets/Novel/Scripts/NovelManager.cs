using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace Novel
{
    public class NovelManager : MonoBehaviour
    {
        [SerializeField] private NovelId _novelId;
        [SerializeField] private ScenarioData _scenarioData;
        [SerializeField] private NovelOperation _novelOperation;

        [SerializeField] private int _currentLine = 0;

        private bool _doFirstLine = false;

        private List<AsyncOperationHandle> _handles = new List<AsyncOperationHandle>();

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
            foreach (var handle in _handles)
            {
                Addressables.Release(handle);
            }
        }

        private IEnumerator Initialize()
        {
            // シナリオのロード
            yield return LoadAsset<TextAsset>(_novelId.ToString(), scenario =>
            {
                ScenarioLoader.MakeScenarioData(scenario);
            });
            _scenarioData = ScenarioLoader._ScenarioData;

            // キャラクターアセットのロード
            yield return LoadAssets<GameObject>("NovelCharacter", characterAssets =>
            {
                foreach (GameObject characterAsset in characterAssets)
                {
                    _novelOperation.CharacterPrefabs[characterAsset.name] = characterAsset;
                }
            });


            _doFirstLine = true;
        }


        private IEnumerator LoadAsset<T>(string address, Action<T> callback)
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            yield return handle;

            _handles.Add(handle);

            callback(handle.Result);
        }

        private IEnumerator LoadAssets<T>(string addresses, Action<IList<T>> callback)
        {
            var handle = Addressables.LoadAssetsAsync<T>(addresses, null);
            yield return handle;

            _handles.Add(handle);

            callback(handle.Result);
        }
    }
}
