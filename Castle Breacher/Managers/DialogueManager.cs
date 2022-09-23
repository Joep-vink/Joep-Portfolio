using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    void Awake() => instance = this;

    [field: SerializeField]
    private UnityEvent OnStartDialogue;

    [field: SerializeField]
    private UnityEvent OnStopDialogue;

    [SerializeField] private List<DialogueSO> allDialogue = new List<DialogueSO>();

    [Header("Dialogue")]
    public TextMeshProUGUI TalkerText;
    public Image talkerPortrait;

    [Header("Private")]
    private bool isInConversation = false;
    private int sentenceCount;

    public void SetDialogue()
    {
        talkerPortrait.sprite = allDialogue[0].Portrait;
        TalkerText.text = allDialogue[0].sentences[sentenceCount].sentence.ToString();

        OnStartDialogue?.Invoke();

        isInConversation = true;
    }

    public void Conversation()
    {
        if (sentenceCount == allDialogue[0].sentences.Length)
        {
            isInConversation = false;
            allDialogue.RemoveAt(0);
            sentenceCount = 0;
            OnStopDialogue?.Invoke();
        }
        else
        {
            TalkerText.text = allDialogue[0].sentences[sentenceCount].sentence.ToString();

            sentenceCount++;
        }
    }
}
