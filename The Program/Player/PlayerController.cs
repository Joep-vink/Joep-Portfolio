using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public MovementDataSO MovementData;

    public CinemachineVirtualCamera cinemachine;

    public UnityEvent OnWalk;

    [Header("Debug")]
    public float currentVelocity = 0;
    public float currentNoise;

    [Header("Private")]
    private CharacterController controller;
    private Camera cam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float maxAmplitudeGain;

    private void OnDisable()
    {
        currentVelocity = 0;
        currentNoise = 0;
    }

    private void Start()
    {
        cam = Camera.main;

        noise = cinemachine.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        maxAmplitudeGain = noise.m_AmplitudeGain;

        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(Quaternion.identity.x, cam.transform.eulerAngles.y, Quaternion.identity.z);

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical")); //save the input 

        MoveAgent(input);
    }
    private void MoveAgent(Vector2 input)
    {
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        currentVelocity = CalculateSpeed(input);

        controller.Move(move.normalized * currentVelocity / 20);
    }

    /// <summary>
    /// Calculate the velocity and clamp it
    /// </summary>
    /// <param name="movementInput"></param>
    /// <returns></returns>
    private float CalculateSpeed(Vector3 movementInput)
    {
        if (movementInput.magnitude > 0)
        {
            OnWalk?.Invoke();
            AudioManager.instance.Play("PlayerStep");
            currentVelocity += MovementData.acceleration * Time.deltaTime;
            currentNoise = Mathf.Clamp(currentNoise += MovementData.acceleration * 4 * Time.deltaTime, 0, maxAmplitudeGain);
        }
        else
        {
            currentVelocity -= MovementData.deacceleration * Time.deltaTime;
            currentNoise = Mathf.Clamp(currentNoise -= MovementData.acceleration * 4 * Time.deltaTime, 0, maxAmplitudeGain);
        }

        noise.m_AmplitudeGain = Mathf.Clamp(currentNoise, 0, maxAmplitudeGain);

        return Mathf.Clamp(currentVelocity, 0, MovementData.maxSpeed);
    }

    public void PlayAudio(AudioSource clip)
    {
        if (!clip.isPlaying)
            clip.Play(0);
    }
}
