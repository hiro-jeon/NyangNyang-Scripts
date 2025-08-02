using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombard : Skill
{

    [Header("Bombard")]
    public float radius = 3f;
	public float range = 10f;
	public float speed = 3f;
	
	[Header("Target")]
    public LayerMask targetLayer;

	[Header("Debug")]
	public Color color = Color.red;

    void Start()
    {
        StartCoroutine(Repeat());
    }

    public override void Damage(Vector2 vector)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(vector, radius, targetLayer);

        foreach (var target in targets)
        {
            target.GetComponent<Enemy>()?.TakeDamage((int)damage);
        }
    }

    IEnumerator Repeat()
    {
        while (true)
        {
        	Vector2 position = (Vector2)transform.position + range * Random.insideUnitCircle;

            Damage(position);
            yield return new WaitForSeconds(1 / speed);
        }
    }
}
