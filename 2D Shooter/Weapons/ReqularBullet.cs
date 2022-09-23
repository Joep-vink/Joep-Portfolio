using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqularBullet : bullet
{
    protected Rigidbody2D rb;
    private bool isDead = false;

    public bool isBoss;

    public override BulletDataSo BulletData
    {
        get => base.BulletData;
        set
        {
            if (!isBoss)
            {
                base.BulletData = value;

                rb = GetComponent<Rigidbody2D>();
                rb.drag = BulletData.Friction;
            }
        }
    }

    private void Start()
    {
        if (isBoss)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.drag = BulletData.Friction;
        }
    }

    private void FixedUpdate()
    {
        if (rb != null && BulletData != null)
        {
            rb.MovePosition(transform.position + BulletData.BulletSpeed * transform.right * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (isDead)
        {
            return;
        }
        isDead = true;

        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            HitObstacle(coll);
        }
        if (coll.gameObject.layer == LayerMask.NameToLayer(BulletData.EnemyLayer))
        {
            var hittable = coll.GetComponent<IHittable>();
            hittable?.GetHit(BulletData.Damage, gameObject);
            HitEnemy(coll);
        }
        Destroy(gameObject);
    }

    private void HitEnemy(Collider2D coll)
    {
        var knockBack = coll.GetComponent<IKnockBack>();
        knockBack?.KnockBack(transform.right, BulletData.KnockBackPower, BulletData.KnockBackDelay);
        Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
        Instantiate(BulletData.ImpactEnemyPrefab, coll.transform.position + (Vector3)randomOffset, Quaternion.identity);
    }

    private void HitObstacle(Collider2D coll)
    { 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 5, LayerMask.GetMask("Obstacle"));
        if (hit.collider != null)
        {
            Instantiate(BulletData.ImpactObstaclePrefab, hit.point, Quaternion.identity);
        }
    }
}
