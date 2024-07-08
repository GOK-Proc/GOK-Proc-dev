using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Novel.ScenarioData;

namespace Novel
{
    public class NovelManager : MonoBehaviour
    {
        [SerializeField] private NovelId _novelId;
        // private Dictionary<NovelId, ScenarioData> _scenarioList;
        [SerializeField] private ScenarioData _scenario;
        [SerializeField] private NovelOperation _novelOperation;

        [SerializeField] private int _currentLine = 0;

        private void Start()
        {
            //_scenario = _scenarioList[_novelId];
            /* ここから実験用 */
            LineData cls = new LineData("白井", "おはようございます。");
            cls.LineOperation = _novelOperation.UpdateText;
            _scenario.ScenarioLines.Add(cls);
            /* ここから実験用 */
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _scenario.ScenarioLines[_currentLine].CallLineOperation();
                _currentLine++;
            }
        }
    }
}
