using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorWeapon : Weapon
{
    [Header("Sector")]
    public float innerRadius = 0f;
    public float outerRadius = 2.0f;
    public int degree = 60;
    private int segments = 30;

    [Header("Target")]
    public LayerMask targetLayer;

    List<Vector2> points;
    PolygonCollider2D sectorCollider;
    ContactFilter2D filter;

    void Awake()
    {
    	// 점들 목록 생성하기
		points = PointsBuilder.GetSectorPoints(
				transform.right,
				innerRadius,
				outerRadius,
				degree,
				segments
		);
    }

    void Start()
    {
		SetCollider();
    }

    public override void Attack(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;

        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, direction);
        transform.rotation = rotation;
        Damage(target);
    }

    void Damage(Transform target)
    {
        Collider2D[] hits = new Collider2D[20];
        int count = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hits);

        for (int i = 0; i < count; i++)
		{
			Enemy enemy = hits[i].transform.gameObject.GetComponent<Enemy>();
			if (enemy != null)
			{
				enemy.TakeDamage(damage);
			}
		}
    }

    void SetCollider()
    {
		// 콜라이더 생성
        sectorCollider = gameObject.AddComponent<PolygonCollider2D>();

        // 콜라이더 설정
        sectorCollider.pathCount = 1;
        sectorCollider.SetPath(0, points.ToArray());
        sectorCollider.isTrigger = true;

		// 레이어 필터링 설정
        filter = new ContactFilter2D();
        filter.SetLayerMask(targetLayer);
        filter.useLayerMask = true;
    }

	void OnDrawGizmos()
	{
		if (points == null || points.Count == 0)
		{
			points = PointsBuilder.GetSectorPoints(
					transform.right,
					innerRadius,
					outerRadius,
					degree,
					segments
			);
		}

		Vector3 origin = transform.position;
		Quaternion rotation = transform.rotation;
		Gizmos.color = Color.green;

		for (int i = 0; i < points.Count - 1; i++)
		{
			Vector3 p1 = origin + rotation * (Vector3)points[i];
			Vector3 p2 = origin + rotation * (Vector3)points[i + 1];
			Gizmos.DrawLine(p1, p2);
		}

		Vector3 last = origin + rotation * (Vector3)points[points.Count - 1];
		Vector3 first = origin + rotation * (Vector3)points[0];
		Gizmos.DrawLine(last, first);

		float length = outerRadius + 1;

		Gizmos.color = Color.white;
		Vector3 direction = transform.right;
		Gizmos.DrawLine(origin, origin + direction * length);
	}
}
