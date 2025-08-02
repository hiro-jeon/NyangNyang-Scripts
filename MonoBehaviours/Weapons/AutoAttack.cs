using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public LayerMask enemyLayer;

    private Coroutine attackRoutine;
    private Transform currentTarget;
    private Weapon weapon;

    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
		// 타겟이 없거나, 유효하지 않음
        if (!IsTargetValid(currentTarget))
        {
            Transform newTarget = FindNearest();

			// 타겟이 갱신됨
            if (currentTarget != newTarget)
            {
                currentTarget = newTarget;

				// 루틴이 켜져있음
                if (attackRoutine == null && currentTarget != null)
                {
                    attackRoutine = StartCoroutine(Attack());
                }
            }
        }
    }

    bool IsTargetValid(Transform target)
    {
		// 오브젝트가 Destroyed 되었는 지 여부
        if (target == null) return false;

		// 오브젝트가 멀어졌는지 여부
        float dist = Vector2.Distance(transform.position, target.position);
        if (dist > weapon.range) return false;

        return (true);
    }
    
    Transform FindNearest()
    {
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, weapon.range, enemyLayer);

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closest = hit.transform;
                closestDist = dist;
            }
        }
        return (closest);
    }    

    IEnumerator Attack()
    {
        while (IsTargetValid(currentTarget))
        {
            weapon.Attack(currentTarget);
            yield return new WaitForSeconds(1 / weapon.speed);
        }
        currentTarget = null;
        attackRoutine = null;
    }
}
