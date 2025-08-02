using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{
    CircleCollider2D circleCollider;    // 디버그용

    public float pursuitSpeed;
    public float wanderSpeed;
    float currentSpeed;

    public float directionChangeInterval;
    public bool followPlayer;

    Coroutine moveCoroutine;

    Rigidbody2D rb2d;
    Animator animator;

    Transform targetTransform = null;

    Vector3 endPosition;
    float currentAngle = 0;
    
    // A*
    MapGenerator generator;

    private AStar aStar;
    private List<Vector2Int> currentPath = null;
    private int pathIndex = 0;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>(); // 디버그용
        currentSpeed = wanderSpeed;

        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        aStar = new AStar();
        MapGenerator generator = FindObjectOfType<MapGenerator>();

        if (generator != null)
        {
            aStar.SetBlockedTiles(generator.blockedTiles);
        }
        else
        {
            aStar.SetBlockedTiles(new HashSet<Vector2Int>());
        }

        StartCoroutine(WanderRoutine());
    }

    void Update()
    {
        // Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;
            targetTransform = collision.gameObject.transform;

            Vector2Int startTile = Vector2Int.FloorToInt(rb2d.position);
            Vector2Int goalTile = Vector2Int.FloorToInt(targetTransform.position);
            currentPath = aStar.FindPath(startTile, goalTile);
            pathIndex = 0;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            // moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            moveCoroutine = StartCoroutine(FollowPathCoroutine());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isWalking", false);
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            targetTransform = null;
        }

        currentSpeed = wanderSpeed;
        moveCoroutine = StartCoroutine(WanderRoutine());
    }

    public IEnumerator WanderRoutine()
    {
        while (targetTransform == null)
        {
            ChooseNewEndpoint();
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon && targetTransform == null)
        {
            if (rigidBodyToMove != null)
            {
                animator.SetBool("isWalking", true);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
                rb2d.MovePosition(newPosition);
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("isWalking", false);
    }

    private IEnumerator FollowPathCoroutine()
    {
        animator.SetBool("isWalking", true);

        while (currentPath != null && pathIndex < currentPath.Count)
        {
            Vector3 targetPos = new Vector3(currentPath[pathIndex].x + 0.5f, currentPath[pathIndex].y + 0.5f, 0);

            while ((rb2d.position - (Vector2)targetPos).sqrMagnitude > 0.01f)
            {
                Vector3 newPosition = Vector3.MoveTowards(rb2d.position, targetPos, pursuitSpeed * Time.deltaTime);
                rb2d.MovePosition(newPosition);
                yield return new WaitForFixedUpdate();
            }

            pathIndex++;
        }
        animator.SetBool("isWalking", false);
        currentPath = null;
    }

    void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle);
    }

    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }

    void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    private HashSet<Vector2Int> GetBlockedTiles()
    {
        return new HashSet<Vector2Int>();
    }
}
