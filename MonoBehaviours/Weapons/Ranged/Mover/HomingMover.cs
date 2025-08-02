using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMover : Mover
{
   private Projectile projectile;
   private Transform target;
   public float rotateSpeed = 200f;


   public override void Fire(Transform target)
   {
      projectile = transform.parent.GetComponent<Projectile>();
      this.target = target;

      Destroy(transform.parent.gameObject, lifetime);
   }

   void Update()
   {
      if (projectile == null)
      {
         return ;
      }

      if (target == null)
      {
         projectile.transform.position += (Vector3)(projectile.direction * speed * Time.deltaTime);
      }
      else
      {
         projectile.direction = (target.position - transform.position).normalized;
         float rotateAmount = Vector3.Cross(projectile.transform.right, projectile.direction).z;

         projectile.transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);
           
         projectile.transform.position += (Vector3)(projectile.direction * speed * Time.deltaTime);
      }
      
   }
}
