using UnityEngine;

public class MonsterMechanism : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D boxCollider;

    private LayerMask playerLayer;
    private LayerMask groundLayer;
    private LayerMask platformLayer;

    private float checkDistance = 0.1f;
    public float detectionRadius ;
    private float gravityScaleNormal = 0f; // 기본 중력
    private float gravityScaleFalling = 10f; // 떨어질 때 중력
    private float maxFallSpeed = -10f; // 최대 낙하 속도

    public bool isGrounded = true;
    public bool isPlatformed = true;

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        groundLayer = LayerMask.GetMask("Ground");
        platformLayer = LayerMask.GetMask("Platform");
        _rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        CheckGroundAndPlatform();
        LimitFallSpeed();
    }

    private void CheckGroundAndPlatform()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - boxCollider.bounds.extents.y); // 콜라이더의 가장 하단부분을 레이캐스트의 시작점으로 잡기 위함
        RaycastHit2D groundHit = Physics2D.Raycast(origin, Vector2.down, checkDistance, groundLayer);
        RaycastHit2D platformHit = Physics2D.Raycast(origin, Vector2.down, checkDistance, platformLayer);

        if (groundHit.collider != null || platformHit.collider != null)
        {
            isGrounded = groundHit.collider != null;
            isPlatformed = platformHit.collider != null;

            _rb.gravityScale = gravityScaleNormal; // 중력을 즉시 0으로 설정
            if (_rb.velocity.y < 0) // 아래쪽으로 떨어지는 속도만 제거
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
            }

            if (groundHit.collider != null)
            {
                transform.position = new Vector2(transform.position.x, groundHit.point.y + boxCollider.bounds.extents.y);
            }
            else if (platformHit.collider != null)
            {
                transform.position = new Vector2(transform.position.x, platformHit.point.y + boxCollider.bounds.extents.y);
            }
        }
        else
        {
            isGrounded = false;
            isPlatformed = false;
            SmoothGravityScaleChange(gravityScaleFalling);
        }
    }

    public bool IsGroundInDirection(Vector2 direction)
    {
        Vector2 origin = new Vector2(transform.position.x + direction.x, transform.position.y - boxCollider.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, checkDistance, groundLayer);

        return hit.collider != null;
    }

    public bool IsPlatformDirection(Vector2 direction)
    {
        Vector2 origin = new Vector2(transform.position.x + direction.x, transform.position.y - boxCollider.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, checkDistance, platformLayer);

        return hit.collider != null;
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

    public Vector2 GetPlayerDirection()
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
    public Transform GetPlayerTransform()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(this.transform.position, detectionRadius, playerLayer);
        if (playerCollider != null)
        {
            return playerCollider.transform;
        }
        return null;
    }

    public bool IsPlayerDetected()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(this.transform.position, detectionRadius, playerLayer);
        return playerCollider != null;
    }
}