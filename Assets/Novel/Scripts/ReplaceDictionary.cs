using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Novel
{
    public class ReplaceDictionary
    {
        public Dictionary<string, string> ReplaceDict { get; } = new Dictionary<string, string>();

        public ReplaceDictionary()
        {
            ReplaceDict["name"] = "オーザ";
            ReplaceDict["nothing"] = "";
        }
    }
}