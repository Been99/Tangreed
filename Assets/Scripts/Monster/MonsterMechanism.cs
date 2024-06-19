using UnityEngine;

public class MonsterMechanism : MonoBehaviour
{
    private Rigidbody2D _rb;
    private LayerMask playerLayer;
    private LayerMask groundLayer;
    private float downRayDistance = 1f;
    private float detectionRadius = 5f;
    private float normalGravityScale;
    private float fallingGravityScale = 20f;

    public bool isGrounded = true;

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        groundLayer = LayerMask.GetMask("Ground");
        _rb = GetComponent<Rigidbody2D>();
        normalGravityScale = _rb.gravityScale;
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
            Vector2 direction = ((Vector2)playerCollider.transform.position - (Vector2)this.transform.position).normalized;
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
        Vector2 origin = new Vector2(transform.position.x + direction.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, downRayDistance, groundLayer);

        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision) // 혹시라도 몬스터가 떨어지는 상황 생길까봐 넣어둠
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            _rb.gravityScale = normalGravityScale;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // 혹시라도 몬스터가 떨어지는 상황 생길까봐 넣어둠
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            _rb.gravityScale = fallingGravityScale;
        }
    }
}