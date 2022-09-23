using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Agent : MonoBehaviour, IHittable, IAgent
{
    [Header("Agent")]
    [SerializeField] private Transform _hitParticle;

    public int Health { get => _health; set { _health = Mathf.Clamp(value, 0, _agentSO.maxHealth); } }

    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }

    [field: SerializeField]
    public UnityEvent OnDie { get; set; }

    [field: SerializeField]
    public UnityEvent Onspawn { get; set; }

    public Slider HealthBar;

    [HideInInspector] public bool IsDead = false;

    [Header("Protected/Private")]
    protected float _timer;
    protected int _health;

    private IAttack _attack;

    [Header("Spawner")]
    private float _timeSummoner;

    #region Usefull properties
    private SpriteRenderer _sprite;
    protected ObjectPool _pool;
    [SerializeField] protected ObjectPool _summonerPool;
    protected AgentAnimations _anim;
    protected AgentSO _agentSO;
    [HideInInspector] public AIBrain _brain;

    float IAgent.Health => throw new System.NotImplementedException();
    #endregion

    private void Start()
    {
        _attack = GetComponent<IAttack>();
        _sprite = GetComponentInChildren<SpriteRenderer>();

        _pool = GetComponentInChildren<ObjectPool>();
        _anim = GetComponentInChildren<AgentAnimations>();
        _agentSO = GetComponent<AIBrain>().agentSO;
        _brain = GetComponent<AIBrain>();

        //Set all values
        _health = _agentSO.maxHealth;
        _timer = _agentSO.timeBetweenAttack;

        if (_agentSO.AgentType == AgentTypeEnum.Summoner)
            _timeSummoner = _agentSO.SummonerTime;

        HealthBar.maxValue = _health;
        HealthBar.value = _health;

        Onspawn?.Invoke();
    }

    public virtual void Update()
    {
        if (_agentSO.AgentType == AgentTypeEnum.Summoner)
        {
            _timeSummoner -= Time.deltaTime;

            if (_timeSummoner <= 0)
            {
                _attack.AttackOnStart(_brain, _agentSO, _anim, _summonerPool);

                _timeSummoner = _agentSO.SummonerTime;
            }
        }
    }

    private void OnEnable()
    {
        if (!IsDead) return;

        //Set all values
        _health = _agentSO.maxHealth;
        _timer = _agentSO.timeBetweenAttack;

        HealthBar.maxValue = _health;
        HealthBar.value = _health;

        IsDead = false;
        Onspawn?.Invoke();
    }

    /// <summary>
    /// Calls the right function for the type of enemy
    /// </summary>
    public void Attack()
    {
        if (IsDead) return;

        if (_attack == null)
            Debug.LogError("Could not find IAttack on " + gameObject.name);

        _timer -= Time.deltaTime;

        if (_timer <= 0 && _brain.Target.gameObject.activeInHierarchy)
        {
            _attack.Attack(_brain, _agentSO, _anim, _pool);

            _timer = _agentSO.timeBetweenAttack;
        }
    }

    /// <summary>
    /// Health - damage then invokes the OnGetHit and checks if the hp is lower than 0
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageDealer"></param>
    public virtual void GetHit(int damage, GameObject damageDealer)
    {
        if (IsDead) return;

        Health -= damage;
        OnGetHit?.Invoke();
        HealthBar.value = Health;

        if (Health <= 0)
        {
            IsDead = true;

            _brain.Target = null;

            OnDie?.Invoke();
            UiManager.instanace.UpdateCurrency(_agentSO.DropOnDeath);
        }
    }

    /// <summary>
    /// look at the object your moving to
    /// </summary>
    public virtual void LookAt()
    {
        if (IsDead) return;

        if (_brain.Target != null && _brain.Target.gameObject.activeInHierarchy)
        {
            if (_brain.Target.position.x <= transform.position.x)
            {
                _hitParticle.localScale = new Vector3(.5f, .5f, 1);
                _sprite.flipX = true;
            }
            else
            {
                _hitParticle.localScale = new Vector3(-.5f, .5f, 1);
                _sprite.flipX = false;
            }
        }
        else
        {
            if (_brain.point.x <= transform.position.x)
            {
                _sprite.flipX = true;
                _hitParticle.localScale = new Vector3(.5f, .5f, 1);
            }
            else if (_brain.point.x >= transform.position.x)
            {
                _sprite.flipX = false;
                _hitParticle.localScale = new Vector3(-.5f, .5f, 1);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (IsDead) return;

        if (coll.gameObject.layer == LayerMask.NameToLayer(_agentSO.targetLayer))
        {
            if (_brain.Target == null || !_brain.Target.gameObject.activeInHierarchy)
            {
                _brain.Target = coll.gameObject.transform;
            }
        }
    }
}
