using System;
using UnityEngine;

[Serializable]
public class SyncCoinsMsg
{
    public long newValue;
    public Transform source;
    public Transform target;
}