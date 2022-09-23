using UnityEngine;
using UnityEngine.Events;

public class FeedBack : MonoBehaviour, IClickable
{
    public ParticleSystem feedBackParticle;

    [field: SerializeField]
    public UnityEvent OnClick { get; set; }
    public UnityEvent OnClose { get; set; }

    public void PlayFeedback()
    {
        feedBackParticle.transform.position = GetComponent<CircleCollider2D>().transform.position;
        feedBackParticle.Play();
    }
}
