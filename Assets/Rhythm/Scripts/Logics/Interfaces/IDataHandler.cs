using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataHandler<T>
{
    T this[string id] { get; set; }
}