using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Novel
{
    // staticの必要性
    public static class ScenarioLoader
    {
        public static ScenarioData _ScenarioData { get; private set; }

        public static void MakeScenarioData(TextAsset textAsset)
        {
            _ScenarioData = ScriptableObject.CreateInstance<ScenarioData>();

            string[] lines =  textAsset.text.Split("\n");

            foreach (var line in lines.Take(lines.Length - 1))
            {
                List<OperationData> lineOperationList = new List<OperationData>();

                string[] items = line.Split(",");

                if (items[0] != "")
                {
                    lineOperationList.Add(new DialogueData(items[0], items[1]));
                }

                if (items[2] != "") 
                {
                    lineOperationList.Add(new CharacterLayoutData(items[2]));
                }

                if (items[3] != "")
                {
                    lineOperationList.Add(new BackgroundData(items[3]));
                }

                if (items[4] != "")
                {
                    lineOperationList.Add(new BgmData(items[3]));
                }

                if (items[5] != "")
                {
                    lineOperationList.Add(new OtherData());
                }

                _ScenarioData.ScenarioLines.Add(lineOperationList);
            }
        }
    }
}
