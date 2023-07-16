using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public partial class ShowPopUpMsg
{
    public int popUpType;
    public PopUpLayer popUpLayer;
    public bool ignoreQueue;
    public object popUpData;
    public Action<int> closeCallback;

    public bool isClosed;
}

