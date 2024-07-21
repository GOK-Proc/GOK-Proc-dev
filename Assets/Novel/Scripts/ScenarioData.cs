using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novel
{
    public class ScenarioData : ScriptableObject
    {
        [field: SerializeField] public List<OperationData> ScenarioLines { get; private set; } = new List<OperationData>();
    }
}