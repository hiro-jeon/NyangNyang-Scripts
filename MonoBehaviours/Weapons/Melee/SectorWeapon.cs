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
        Damage(target);
    }

    void Damage(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;

        transform.position = this.transform.position;

        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, direction);
        transform.rotation = rotation;

        Collider2D[] hits = new Collider2D[20];
        int count = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hits);

        transform.rotation = Quaternion.identity;

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

    /*
    // Debug --------------------------------------------------

    public GameObject rendererPrefab;
    
    void ShowClawMarker(Vector3 position, float radius)
    {
        GameObject bombMarker = Instantiate(rendererPrefab, position, Quaternion.identity);
        CircleRenderer renderer = bombMarker.GetComponent<CircleRenderer>();

        renderer.Initialize(radius);
    }

    // Debug --------------------------------------------------
    */
}
