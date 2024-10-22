using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novel
{
    public class ScenarioData : ScriptableObject
    {
        public List<List<IOperationData>> ScenarioLines { get; set; } = new List<List<IOperationData>>();
    }
}