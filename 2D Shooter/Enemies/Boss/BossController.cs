using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossController : MonoBehaviour, IAgentInput
{
    public static BossController instance;

    private void Awake()
    {
        instance = this;
    }

    [field: SerializeField]
    public UnityEvent OnFireButtonPressed { get; set; }
    public UnityEvent OnFireButtonReleased { get; set; } // Dont Use


    [field: SerializeField]
    public UnityEvent<Vector2> OnMovementKeyPressed { get; set; }

    [field: SerializeField]
    public UnityEvent<Vector2> OnPointerPositionChange { get; set; }


    public float knockBack, knockBackTime;

    public BossAction[] actions;
    private int currentAction;
    private float shotCounter, actionCounter;
    public int currentSequence;

    public BossSequence[] sequences;

    public bool canMove = true;

    public Slider healthBar;

    private Transform[] currentShotPoints;

    //All script references
    private Animator anim;
    private AgentMovement mover;
    private Enemy health;

    private void Start()
    {
        //Get all components
        health = GetComponent<Enemy>();
        anim = GetComponentInChildren<Animator>();
        mover = GetComponent<AgentMovement>();

        //Set the actions and actionCounter
        actions = sequences[currentSequence].actions;
        actionCounter = actions[currentAction].actionLenght;

        //Set healthbar
        healthBar.maxValue = health.Health;
        healthBar.value = health.Health;
    }

    private void Update()
    {
        healthBar.value = health.Health;

        if (health.Health >= 0)
        {
            ChangeSequence();

            if (actionCounter > 0)
            {
                actionCounter -= Time.deltaTime;

                if (actions[currentAction].shouldChaseTarget && canMove)
                {
                    MoveBoss();
                }
                else
                {
                    mover.StopImmediatelly();
                }
            }
            else
            {
                currentAction++;
                if (currentAction >= actions.Length)
                    currentAction = 0;

                actionCounter = actions[currentAction].actionLenght;
            }

            if (actions[currentAction].shouldShoot)
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    Attack();
                }
            }
        }
        else
        {
            mover.StopImmediatelly();
            StopAllCoroutines();
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnFireButtonPressed?.Invoke();
            coll.GetComponent<IKnockBack>().KnockBack((player.instance.transform.position - transform.position).normalized, knockBack, knockBackTime);
        }
    }

    public void Attack()
    {
        shotCounter = actions[currentAction].timeBetweenShot;
        anim.SetTrigger(actions[currentAction].animName);

        if (actions[currentAction].isParticle)
        {
            StartCoroutine(PerformParticleAttack());
        }
        else
        {
            StartCoroutine(PerformAttack());
        }
    }

    public IEnumerator PerformAttack()
    {
        if (!actions[currentAction].shouldChaseTarget)
            canMove = false;

        if (actions[currentAction].particlePos == null)
            actions[currentAction].particlePos = GetComponentInChildren<BulletParentTag>().transform;

        currentShotPoints = actions[currentAction].shotPoints;
        GameObject item = actions[currentAction].itemToShoot;

        yield return new WaitForSeconds(actions[currentAction].waitBeforeSpawn);

        foreach (Transform t in currentShotPoints)
        {
            Instantiate(item, t.position, t.rotation);
        }

        float timeTillMove = (actions[currentAction].waitBeforeSpawn - actions[currentAction].actionLenght) - 0.1f;
        yield return new WaitForSeconds(timeTillMove);

        canMove = true;
        currentShotPoints = null;
    }

    public IEnumerator PerformParticleAttack()
    {
        if (!actions[currentAction].shouldChaseTarget)
            canMove = false;

        actions[currentAction].itemToShoot.transform.localScale = actions[currentAction].particleScale.localScale;
        actions[currentAction].itemToShoot.transform.position = actions[currentAction].particlePos.position;
        GameObject attack = actions[currentAction].itemToShoot;
        yield return new WaitForSeconds(actions[currentAction].waitBeforeSpawn);

        attack.GetComponent<ParticleSystem>().Play();

        float timeTillMove = (actions[currentAction].waitBeforeSpawn - actions[currentAction].actionLenght) - 0.1f;
        yield return new WaitForSeconds(timeTillMove);

        canMove = true;
    }

    public void MoveBoss()
    {
        OnMovementKeyPressed?.Invoke((player.instance.transform.position - transform.position).normalized);
        OnPointerPositionChange?.Invoke(player.instance.transform.position);
    }

    public void ChangeSequence()
    {
        if (health.Health <= sequences[currentSequence].endSequenceHealth) //&& currentSequence < sequences.Length - 1)
        {
            currentSequence++;
            shotCounter = actions[currentAction].timeBetweenShot;
            actions = sequences[currentSequence].actions;
            currentAction = 0;
            actionCounter = actions[currentAction].actionLenght;

            //actions[currentAction].itemToShoot.transform.localScale = actions[currentAction].particleScale.localScale;
            //actions[currentAction].itemToShoot.transform.position = actions[currentAction].particlePos.position;
        }
    }
}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    [Range(1, 5)]
    public float actionLenght;

    [Header("Movement")]
    public bool shouldChaseTarget;

    [Header("Attack")]
    public string animName;
    public float waitBeforeSpawn;

    [Header("ParticleAttack")]
    public bool isParticle;
    public Transform particlePos;
    public Transform particleScale;

    [Header("Shooting")]
    public bool shouldShoot;
    public bool isMinion;
    public float timeBetweenShot;
    public Transform[] shotPoints;
    public GameObject itemToShoot;
}

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;

    public int endSequenceHealth;
}
