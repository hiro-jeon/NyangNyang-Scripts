using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D && collision.CompareTag("Enemy"))
        {
            OnHit(collision);
            Destroy(gameObject);
        }
    }

    protected override void OnHit(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}
