using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float range;
    public int damage;

    public abstract void Attack(Transform target);
}
