using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IResultProvider
    {
        void DisplayResult(in HeaderInformation header);
        void SaveRecordData();
    }
}