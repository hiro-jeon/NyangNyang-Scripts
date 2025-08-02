using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public GameObject movementPrefab;

    [Header("Projectile Stats")]
    public float projectileSpeed;

    public override void Attack(Transform target)
    {
		// instantiate game objects
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        GameObject mover = Instantiate(movementPrefab, projectile.transform);
        mover.transform.SetParent(projectile.transform, false);

		// initiate projectile
        Projectile proj = projectile.GetComponent<Projectile>();

        proj.damage = damage;

		// initiate mover
        Mover mov = mover.GetComponent<Mover>();

		mov.speed = projectileSpeed;
		mov.lifetime = 3f;
		mov.Fire(target);
    }
}
    
/*
	// Legacy
    public override void Attack(Vector2 direction)   
    {
        Vector3 targetPosition = transform.position + (Vector3)direction.normalized * 5f;

        GameObject fakeTarget = new GameObject("FakeTarget");
        fakeTarget.transform.position = targetPosition;

        Attack(fakeTarget.transform);
        Destroy(fakeTarget, 1f);
    }

    // 디버그용 ------------------------------------------

    public bool isAttacking = false;
    Coroutine attackCoroutine = null;

    public IEnumerator AttackRoutine()
    {
        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    void Start()
    {
        isAttacking = true;
    }

    void Update()
    {
        if (isAttacking)
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            StopCoroutine(attackCoroutine);
        }
    }

    // --------------------------------------------------

*/
