using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
   public float speed;
   public float lifetime;

   public abstract void Fire(Transform target);
}
