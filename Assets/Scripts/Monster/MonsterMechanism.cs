using UnityEngine;

public class MonsterMechanism : MonoBehaviour
{
    private Rigidbody2D _rb;
    private LayerMask playerLayer;
    private LayerMask groundLayer;
    private float detectionRadius = 5f;
    private float rayDistance = 1f;
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
            return ((Vector2)playerCollider.transform.position - (Vector2)this.transform.position);
        }
    }

    public bool IsPlayerDetected()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(this.transform.position, detectionRadius, playerLayer);
        return playerCollider != null;
    }

    public bool IsGroundInDirection(Vector2 direction)
    {
        // 몬스터 아래로 레이 방향 설정
        Vector2 origin = new Vector2(transform.position.x + direction.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayDistance, groundLayer);

        return hit.collider != null;
    }

    public Vector2 RandomMovement()
    {
        float randomDirection = Random.Range(-3f, 3f);
        return new Vector2(randomDirection, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision) // 혹시라도 몬스터가 떨어지는 상황 생길까봐 넣어둠
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            _rb.gravityScale = normalGravityScale; // 원상 복귀
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