using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;

public class InputController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCam;
    [HideInInspector] public Animator animator;

    /*Player*/
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;
    [SerializeField] private SpriteRenderer characterRenderer;


    /*Player Input*/
    private float moveInput;
    private Vector2 mouseDelta;
    private Vector2 mousePos;

    public float groundCheckRadius = 0.4f; // 바닥 체크 반경
    public LayerMask groundLayer; // 바닥 레이어
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isJumping;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float boostForce = 0.005f;

    public Action PlayerHit;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        mainCam = Camera.main; //mainCamera 태그 붙어있는 카메라를 가져옴
    }

    private void Start()
    {
       //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //바닥 체크
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

        // 바닥에 닿았을 때 더블 점프 가능하도록 초기화
        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed , rb.velocity.y);

        if (isJumping)
        {
            Debug.Log("점프!");
            if (isGrounded)
            {
                // 바닥에 있을 때 점프
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("OnJump");
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
            isJumping = true;
        }
    }

    private void RotatePos(Vector2 direction)
    {
        float rotz = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //Y,X값을 넣어 각도를 구한 뒤 라디안을 각도로 변경

        characterRenderer.flipX = Mathf.Abs(rotz) > 90f; // 캐릭터 각도가 절대값이 90보다 크면 (Abs = Absolut value) 캐릭터를 뒤집음
        armRenderer.flipY = characterRenderer.flipX;
        armPivot.rotation = Quaternion.Euler(0, 0, rotz); //쿼터니언 오일러를 통해각도를 지정해줌
    }

    public void OnDashInPut(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 mouseWorldPosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
        Vector2 characterWorldPosition2D = new Vector2(transform.position.x, transform.position.y);
        mousePos = (mouseWorldPosition2D - characterWorldPosition2D).normalized;
        Debug.Log(mousePos);

        if (context.phase == InputActionPhase.Started)
        {
            rb.AddForce(mousePos * boostForce, ForceMode2D.Impulse);

            //추가적으로 횟수제한을 두고 게이지가 일정시간동안 채워지게 설정
        }
    }

    public void OnFireInput(InputAction.CallbackContext context) //공격 인풋 세팅
    {
        //무기 Sprite쪽에 붙일 예정
    }

    public void OnHit()
    {
        PlayerHit?.Invoke(); //Player Manager로 이동 예정
    }
}
