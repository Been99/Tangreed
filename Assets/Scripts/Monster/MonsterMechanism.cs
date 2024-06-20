using UnityEngine;

public class MonsterMechanism : MonoBehaviour
{
    private Rigidbody2D _rb;
    private LayerMask playerLayer;
    private LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private float downRayDistance = 0.01f;
    private float detectionRadius = 5f;

    public bool isGrounded = true;

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        groundLayer = LayerMask.GetMask("Ground");
        _rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        CheckGroundStatus();
    }

    private void CheckGroundStatus()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - boxCollider.bounds.extents.y); // 콜라이더의 가장 하단부분을 레이캐스트의 시작점으로 잡기 위함
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, downRayDistance, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
            _rb.gravityScale = 0;
            if (_rb.velocity.y < 0) // 아래쪽으로 떨어지는 속도만 제거
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
            }
        }
        else
        {
            isGrounded = false;
            _rb.gravityScale = 20;
        }
    }

    public Vector2 DirectionToTarget()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(this.transform.position, detectionRadius, playerLayer);

        if (playerCollider == null)
        {
            return Vector2.zero;
        }
        else
        {
            Vector2 direction = ((Vector2)playerCollider.transform.position - (Vector2)this.transform.position);
            return direction;
        }
    }

    public bool IsPlayerDetected()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(this.transform.position, detectionRadius, playerLayer);
        return playerCollider != null;
    }

    public bool IsGroundInDirection(Vector2 direction)
    {
        Vector2 origin = new Vector2(transform.position.x + direction.x, transform.position.y - boxCollider.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, downRayDistance, groundLayer);

        return hit.collider != null;
    }
}
