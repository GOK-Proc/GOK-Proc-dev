﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ReplaceDictionary
{
    public Dictionary<string, string> ReplaceDict { get; } = new Dictionary<string, string>();

    public ReplaceDictionary()
    {
        ReplaceDict["name"] = "白黒";
    }
}