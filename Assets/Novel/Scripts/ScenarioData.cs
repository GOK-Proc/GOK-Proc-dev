using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novel
{
    public class ScenarioData : ScriptableObject
    {
        public List<OperationData> ScenarioLines { get; private set; } = new List<OperationData>();

        
    }

    public class OperationData
    {
        public OperationType OperationType { get; set; }
        public string Name { get; set; }
        public string Dialogue { get; set; }
    }
}