using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Novel
{
    public static class ScenarioLoader
    {
        public static ScenarioData ScenarioData { get; private set; }

        public static void MakeScenarioData(TextAsset textAsset)
        {
            ScenarioData = ScriptableObject.CreateInstance<ScenarioData>();

            string[] lines =  textAsset.text.Split("\n");

            foreach (var line in lines.Take(lines.Length - 1))
            {
                string[] items = line.Split(",");

                if (items[0] != "")
                {
                    ScenarioData.ScenarioLines.Add(new DialogueData(items[0], items[1]));
                }

                if (items[2] != "") 
                {
                    ScenarioData.ScenarioLines.Add(new CharacterLayoutData(items[2]));
                }

                if (items[3] != "")
                {
                    ScenarioData.ScenarioLines.Add(new BackgroundData(items[3]));
                }
            }
        }
    }
}
