using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // 순회 기능
    public Transform player; // [ ] 왜 Transform?
    public float radius = 1f;
    public float orbitSpeed = 90f; // degree per second

    private float angle = 0f;

    // 
    // 공격 기능
    public float detectionRadius = 1f;
    public LayerMask enemyLayerMask;
    
    private bool isAttacking;
    private GameObject target = null;
    public float attackSpeed;

    // 코루틴 기능
    Coroutine attackCoroutine;

    void Update()
    {
        if (target)
        {
            AttackEnemy();   
        }
        else
        {
            Circle();
            DetectEnemy();
        }
    }

    void Circle()
    {
        angle += orbitSpeed * Time.deltaTime;
        if (angle > 360f)
        {
            angle -= 360f;
        }

        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        transform.localPosition = offset;
    }

    void DetectEnemy()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, detectionRadius, enemyLayerMask);
        if (enemy)
        {
            target = enemy.gameObject;
            isAttacking = true;
        }
        else
        {
            target = null;
        }
    }

    void AttackEnemy()
    {
        if (attackCoroutine != null)
        {
            return ;
        }
        else
        {
            attackCoroutine = StartCoroutine(Attack());
        }
        target = null;
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            if (isAttacking)
            {
                // target.TakeDamage();
                yield return new WaitForSeconds(1 / attackSpeed);
            }
            else
            {
                StopCoroutine(Attack());
            }
        }
    }
}
