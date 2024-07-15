using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novel
{
    public static class ScenarioLoader
    {
        public static void MakeScenarioData(TextAsset textAsset)
        {
            string[] lines =  textAsset.text.Split("\n");

            foreach (var line in lines)
            {
                string[] items = line.Split(",");

                if (items[0] != "")
                {
                    OperationData operationData = new DialogueData(items[0], items[1]);
                }
            }
        }
    }
}
