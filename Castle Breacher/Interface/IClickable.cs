using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IClickable
{
    UnityEvent OnClick { get; set; }

    UnityEvent OnClose { get; set; }
}
