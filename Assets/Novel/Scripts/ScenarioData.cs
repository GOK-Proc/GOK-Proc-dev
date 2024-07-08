using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novel
{
    [CreateAssetMenu]
    public class ScenarioData : ScriptableObject
    {
        public List<LineData> ScenarioLines { get; private set; } = new List<LineData>();

        public class LineData
        {
            public delegate void LineOperationDelegate(params object[] args);
            public LineOperationDelegate LineOperation { get; set; }
            public object[] Paramaters { get; set; }

            public LineData(params object[] args)
            {
                Paramaters = args;
            }

            public void CallLineOperation()
            {
                LineOperation(Paramaters);
            }
        }
    }
}