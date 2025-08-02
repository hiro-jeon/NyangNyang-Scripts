using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header("Stats")]
    public int damage;
	public int time;

    public abstract void Damage(Vector2 vector);
}
