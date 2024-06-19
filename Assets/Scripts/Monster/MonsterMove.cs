using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour
{
    private Rigidbody2D _rb;
    private MonsterController monsterController;
    private MonsterMechanism monsterMechanism;
    private SpriteRenderer spriteRenderer;
    private float ultimatedMoveInterval = 1.5f; // 코루틴 움직임 주기,,,
    private float moveSpeed = 2f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        monsterController = GetComponent<MonsterController>();
        monsterMechanism = GetComponent<MonsterMechanism>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
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
                    Vector2 direction = monsterMechanism.RandomMovement();
                    ApplyMove(direction);
                    yield return new WaitForSeconds(ultimatedMoveInterval);
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
        direction = LinearMove(direction);

        if (monsterMechanism.IsGroundInDirection(direction))
        {
            Vector2 movement = direction * moveSpeed;
            _rb.velocity = movement;

            monsterController.OnMove(direction);

            if (direction.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            StopMove();
        }
    }

    private void StopMove()
    {
        _rb.velocity = Vector2.zero;
        monsterController.OnMove(Vector2.zero);
    }

    private Vector2 LinearMove(Vector2 direction) // 대각 이동 방지 메서드
    {
        if (direction.y > 0 && Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            direction = new Vector2(Mathf.Sign(direction.x), 0);
        }
        else
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                direction = new Vector2(Mathf.Sign(direction.x), 0);
            }
            else
            {
                direction = new Vector2(0, Mathf.Sign(direction.y));
            }
        }
        return direction;
    }
}
