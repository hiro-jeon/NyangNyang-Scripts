using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CoinMagnet : MonoBehaviour
{
    public float detectRadius = 4f;
    public float moveSpeed = 5f;
    public float inertiaDuration = 0.5f;

    private Rigidbody2D rb;
    private Transform player;

    private enum State { Idle, Tracking, Inertia }
    private State currentState = State.Idle;

    private float inertiaTimer = 0f;
    private Vector2 lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distance < detectRadius)
                    currentState = State.Tracking;
                break;

            case State.Tracking:
                if (distance > detectRadius)
                {
                    currentState = State.Inertia;
                    lastDirection = (player.position - transform.position).normalized;
                    inertiaTimer = inertiaDuration;
                }
                else
                {
                    Vector2 newPos = Vector2.MoveTowards(rb.position, player.position, moveSpeed * Time.deltaTime);
                    rb.MovePosition(newPos);
                }
                break;

            case State.Inertia:
                if (distance < detectRadius)
                {
                    currentState = State.Tracking;
                }
                else
                {
                    if (inertiaTimer > 0)
                    {
                        rb.MovePosition(rb.position + lastDirection * moveSpeed * Time.deltaTime);
                        inertiaTimer -= Time.deltaTime;
                    }
                    else
                    {
                        currentState = State.Idle;
                    }
                }
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
