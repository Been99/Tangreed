using UnityEngine;

public class MonsterMechanism : MonoBehaviour
{
    private Rigidbody2D _rb;
    public LayerMask playerLayer;
    private LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private float groundCheckDistance = 0.1f;
    private float detectionRadius = 5f;
    private float gravityScaleNormal = 0f; // 기본 중력
    private float gravityScaleFalling = 10f; // 떨어질 때 중력
    private float maxFallSpeed = -10f; // 최대 낙하 속도

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
        LimitFallSpeed();
    }

    private void CheckGroundStatus()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - boxCollider.bounds.extents.y); // 콜라이더의 가장 하단부분을 레이캐스트의 시작점으로 잡기 위함
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
            _rb.gravityScale = gravityScaleNormal; // 중력을 즉시 0으로 설정
            if (_rb.velocity.y < 0) // 아래쪽으로 떨어지는 속도만 제거
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
            }
            transform.position = new Vector2(transform.position.x, hit.point.y + boxCollider.bounds.extents.y);
        }
        else
        {
            isGrounded = false;
            SmoothGravityScaleChange(gravityScaleFalling);
        }
    }

    private void SmoothGravityScaleChange(float targetGravityScale)
    {
        float gravityScaleChangeSpeed = 10f; // 변경 속도
        _rb.gravityScale = Mathf.Lerp(_rb.gravityScale, targetGravityScale, Time.fixedDeltaTime * gravityScaleChangeSpeed);
    }

    private void LimitFallSpeed()
    {
        if (_rb.velocity.y < maxFallSpeed)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, maxFallSpeed);
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
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        return hit.collider != null;
    }
}