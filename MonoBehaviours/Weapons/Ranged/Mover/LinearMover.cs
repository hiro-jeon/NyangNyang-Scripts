using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMover : Mover
{
   private Projectile projectile;

   public override void Fire(Transform target)
   {
      // 추후 제거 예정
      projectile = transform.parent.GetComponent<Projectile>();

      projectile.direction = (target.position - transform.parent.position).normalized;

      Quaternion rotation = Quaternion.FromToRotation(Vector2.right, projectile.direction.normalized);
      projectile.transform.rotation = rotation;     


      Destroy(projectile.gameObject, lifetime);
   }

   void Update()
   {
      if (projectile != null)
      {
         projectile.transform.position += (Vector3)(projectile.transform.right * speed * Time.deltaTime);
      }
      else
      {
         Destroy(projectile.gameObject, lifetime);
      }
   }
}