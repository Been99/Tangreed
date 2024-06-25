using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;
using static UnityEngine.ParticleSystem;

public class InputController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCam;
    [HideInInspector] public Animator animator;
    private ParticleSystem particle;
    private PlayerStatHandler playerStatHandler;
    /*Player*/
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;
    [SerializeField] private Transform AttackPivot;
    [SerializeField] private SpriteRenderer characterRenderer;


    /*Player Input*/
    private float moveInput;
    private Vector2 mouseDelta;
    private Vector2 mousePos;

    public float groundCheckRadius = 0.4f; // 바닥 체크 반경
    public LayerMask groundLayer; // 바닥 레이어
    public LayerMask platformLayer;
    private bool isGrounded;
    private bool isPlatform;
    private bool canDoubleJump;
    private bool isJumping;
    private bool isDown;
    private bool isBoost;
    public float moveSpeed = 5f;
    public float jumpForce = 50f;

    public Action PlayerAttack;

    float dashSpeed = 15f;
    float CheckTime = 0.25f;
    float TimeSpan = 0f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        mainCam = Camera.main; //mainCamera 태그 붙어있는 카메라를 가져옴
    }

    private void Start()
    {
        particle.Stop();
       //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //바닥 체크
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
        isPlatform = Physics2D.OverlapCircle(transform.position, groundCheckRadius, platformLayer);

        // 바닥에 닿았을 때 더블 점프 가능하도록 초기화
        if (isGrounded || isPlatform)
        {
            canDoubleJump = true;
            animator.SetBool("OnJump", false);
        }

        Jump();
        Boost();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed , rb.velocity.y);

    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
        if (context.phase == InputActionPhase.Performed)
        {
            animator.SetBool("OnMove", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("OnMove", false);
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
        Vector2 worldPos = mainCam.ScreenToWorldPoint(mouseDelta); // 카메라의 범위를 알아야함 월드의 좌표로 바꿔서
        mouseDelta = (worldPos - (Vector2)transform.position).normalized; //Transform의 위치에서 world 까지의 거리 구하기 =  world - transform)
        RotatePos(mouseDelta);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if(Input.GetKey(KeyCode.S))
            {
                isDown = true;
            }
            else
            {
                isJumping = true;
            }
        }
    }

    private void RotatePos(Vector2 direction)
    {
        float rotz = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //Y,X값을 넣어 각도를 구한 뒤 라디안을 각도로 변경

        characterRenderer.flipX = Mathf.Abs(rotz) > 90f; // 캐릭터 각도가 절대값이 90보다 크면 (Abs = Absolut value) 캐릭터를 뒤집음

        //armRenderer.flipY = characterRenderer.flipX;
        //armPivot.rotation = Quaternion.Euler(0, 0, MathF.Min(30f, rotz)); //쿼터니언 오일러를 통해각도를 지정해줌
        //AttackPivot.rotation = Quaternion.Euler(0, 0, rotz);

        if (Mathf.Abs(rotz) < 90f)
        {
            armPivot.rotation = Quaternion.Euler(0, 0, MathF.Min(30f, rotz)); //쿼터니언 오일러를 통해각도를 지정해줌
            //AttackPivot.rotation = Quaternion.Euler(0, 0, rotz);
        }
        else
        {
            armPivot.rotation = Quaternion.Euler(180, 0, MathF.Max(-210f, -rotz)); //쿼터니언 오일러를 통해각도를 지정해줌
            //AttackPivot.rotation = Quaternion.Euler(0, 0, -rotz);
        }
    }

    public void OnDashInPut(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isBoost = true;

            //추가적으로 횟수제한을 두고 게이지가 일정시간동안 채워지게 설정
        }
    }

    public void OnFireInput(InputAction.CallbackContext context) //공격 인풋 세팅
    {
        Attack();
        PlayerAttack?.Invoke();
    }


    private void Boost()
    {
        Vector2 CurPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 DashPos = CurPos + mouseDelta*2;
        if (isBoost)
        {
            TimeSpan += Time.deltaTime; //대쉬 시간 흐르기
            if (TimeSpan < CheckTime) //대쉬 시간이 딜레이 시간보다 작을 때는 
            {
                rb.gravityScale = 0; //플레이어 중력 값을 0으로
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
                if (rb.velocity.y < 0 && hit.collider != null)
                {
                    // Ground에 부딪히기 직전에 멈춤
                    transform.position = hit.point;
                    isBoost = false;
                    TimeSpan = 0f;
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 2;
                    if (particle.isPlaying)
                    {
                        particle.Stop();
                    }
                }
                else
                {
                    transform.position = Vector2.Lerp(transform.position, DashPos, Time.deltaTime * dashSpeed); // 대쉬
                    animator.SetBool("OnJump", true);
                    if (!particle.isPlaying)
                    {
                        particle.Play();
                    }
                }
            }
            else if (TimeSpan > CheckTime) //대쉬 시간이 딜레이 시간보다 클 때는
            {
                //IgnoreLayer = false;
                TimeSpan = 0f; //대쉬 시간 초기화(재사용하기 위해)
                rb.velocity = Vector2.zero; //플레이어의 가속도를 0으로
                rb.gravityScale = 2; //플레이어의 중력 값을 다시 원 상태로
                isBoost = false;
                if (particle.isPlaying)
                {
                    particle.Stop();
                }
            }
        }
    }
    private void Jump()
    {
        if(isDown)
        {
            StartCoroutine(DisableCollision());
        }
        else if (isJumping)
        {
            if (isGrounded || isPlatform)
            {
                // 바닥에 있을 때 점프
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("OnJump", true); ;
            }
            else if (canDoubleJump)
            {
                // 공중에 있을 때 더블 점프
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false; // 더블 점프 후 더 이상 점프 불가
                animator.SetTrigger("OnDoubleJump");
            }
            isJumping = false;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("OnAttack");
    }

    private IEnumerator DisableCollision()
    {
        Debug.Log("실행");
        // 발판의 콜라이더를 잠시 비활성화
        Collider2D platformCollider = null;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, platformLayer);

        if (hit.collider != null)
        {
            platformCollider = hit.collider;
            platformCollider.enabled = false;
            yield return new WaitForSeconds(1f);
            platformCollider.enabled = true;
        }

        isDown = false;
    }
}
