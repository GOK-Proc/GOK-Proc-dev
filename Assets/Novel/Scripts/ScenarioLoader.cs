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

            // 先頭行(カラム名)と最後の行(空行)を除去
            foreach (var line in lines.Take(lines.Length - 1).Skip(1))
            {
                List<IOperationData> lineOperationList = new List<IOperationData>();

                string[] items = line.Split(",");

                if (items[0] != "")
                {
                    lineOperationList.Add(new DialogueData(items[0], items[1]));
                }

                if (items[2] != "") 
                {
                    lineOperationList.Add(new CharacterLayoutData(items[2], items[3]));
                }

                if (items[4] != "")
                {
                    lineOperationList.Add(new BackgroundData(items[4]));
                }

                if (items[5] != "")
                {
                    lineOperationList.Add(new SoundData(items[5]));
                }

                if (items[6] != "")
                {
                    lineOperationList.Add(new OtherData());
                }

                _ScenarioData.ScenarioLines.Add(lineOperationList);
            }
        }
    }
}
