using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour
{
    private Rigidbody2D _rb;
    private MonsterController monsterController;
    private MonsterMechanism monsterMechanism;
    private SpriteRenderer spriteRenderer;
    private float coroutineMoveInterval = 1.5f; // 코루틴 움직임 주기
    private float moveSpeed = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        monsterController = GetComponent<MonsterController>();
        monsterMechanism = GetComponent<MonsterMechanism>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(UltimatedMove());
    }

    private IEnumerator UltimatedMove()
    {
        while (true)
        {
            if (monsterMechanism.isGrounded)
            {
                if (monsterMechanism.IsPlayerDetected())
                {
                    Vector2 direction = monsterMechanism.DirectionToTarget();
                    ApplyMove(direction);
                }
                else
                {
                    StopMove();
                    yield return new WaitForSeconds(coroutineMoveInterval); // 1. 멈추고 일정 시간 대기

                    Vector2 direction = RandomMovement();
                    ApplyMove(direction);
                    yield return new WaitForSeconds(coroutineMoveInterval); // 2. 이동 후 일정 시간 대기
                }
            }
            else
            {
                StopMove();
            }
            yield return null;
        }
    }

    private void ApplyMove(Vector2 direction)
    {
        direction = direction.normalized;

        direction = LinearMove(direction);

        if (monsterMechanism.IsGroundInDirection(direction))
        {
            Vector2 movement = direction * moveSpeed;
            _rb.velocity = movement;

            monsterController.OnMove(direction);
            spriteRenderer.flipX = direction.x > 0;
        }
    }

    private Vector2 LinearMove(Vector2 direction) // 대각 이동 방지 메서드
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return new Vector2(direction.x, 0);
        }
        else
        {
            return new Vector2(0, direction.y);
        }
    }

    private void StopMove()
    {
        _rb.velocity = Vector2.zero;
        monsterController.OnMove(Vector2.zero);
    }

    public Vector2 RandomMovement()
    {
        float randomDirection = Random.Range(-3f, 3f);
        return new Vector2(randomDirection, 0);
    }
}

