using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueSO : ScriptableObject
{
    public Sprite Portrait;
    public Dialogue[] sentences;
}

[System.Serializable]
public class Dialogue
{
    [TextArea(5, 5)]
    public string sentence;
}
