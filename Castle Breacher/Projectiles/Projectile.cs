using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public ProjectileSO ProjectileSO;

    [HideInInspector] public GameObject Shooter;
    [HideInInspector] public GameObject Target;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        MoveToTarget();
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer(ProjectileSO.targetLayer))
        {
            if (coll.gameObject == Target)
                DealDamage();
            else if (!Target.gameObject.activeInHierarchy)
            {
                Target = coll.gameObject;
                DealDamage();
            }
        }
    }

    /// <summary>
    /// get the IHittable from the target and deal damage
    /// </summary>
    protected virtual void DealDamage()
    {
        var hittable = Target.GetComponent<IHittable>();
        hittable?.GetHit(ProjectileSO.damage, gameObject);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Move the projectile to the target and also looks at the target
    /// </summary>
    protected virtual void MoveToTarget()
    {
        if (transform.position == Target.transform.position)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// calculates the rotaiten with the given vector 2
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }
}
