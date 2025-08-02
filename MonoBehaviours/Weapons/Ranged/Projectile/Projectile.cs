using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
    public int damage;
    public Vector2 direction;

    public virtual void Initialize(int damage)
    {
        this.damage = damage;
    }

    protected abstract void OnHit(Collider2D other);
}
