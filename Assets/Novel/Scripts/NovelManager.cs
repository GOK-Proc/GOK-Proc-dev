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
            StartCoroutine(LoadAsset());
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

        private IEnumerator LoadAsset()
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(_novelId.ToString());
            yield return handle;

            ScenarioLoader.MakeScenarioData(handle.Result);
            _scenarioData = ScenarioLoader._ScenarioData;

            _doFirstLine = true;
        }

        private IEnumerator UnloadAsset()
        {
            // 後で書く
            yield return null;
        }
    }
}
