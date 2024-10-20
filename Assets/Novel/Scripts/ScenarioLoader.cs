﻿using System.Collections;
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
                    lineOperationList.Add(new DialogueData(items[0], items[2]));
                }

                lineOperationList.Add(new HighlightData(items[1], items[3] != "", items[0] == ""));

                if (items[3] != "") 
                {
                    lineOperationList.Add(new CharacterLayoutData(items[3], items[4]));
                }

                if (items[5] != "")
                {
                    lineOperationList.Add(new BackgroundData(items[5]));
                }

                if (items[6] != "")
                {
                    lineOperationList.Add(new SoundData(items[6]));
                }

                if (items[7] != "")
                {
                    lineOperationList.Add(new OtherData());
                }

                _ScenarioData.ScenarioLines.Add(lineOperationList);
            }
        }
    }
}
