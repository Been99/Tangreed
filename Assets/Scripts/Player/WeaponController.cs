using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private PlayerAttackSO swordSO;
    private InputController input;
    private BoxCollider2D box2D;
    private Animator animator;

    private bool isActivated = false;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        input = GetComponentInParent<InputController>();
        box2D = GetComponent<BoxCollider2D>();
        input.PlayerAttack += OnAttack;

        if(box2D != null)
        {
            box2D.enabled = false;
        }
    }


    void OnAttack()
    {
        if(!isActivated)
        {
            StartCoroutine(ActivateCollider());
        }
    }

    private IEnumerator ActivateCollider()
    {
        if (box2D != null)
        {
            isActivated = true;
            box2D.enabled = true;
            animator.SetTrigger("OnAttack");

            // 지정된 시간 동안 대기
            yield return new WaitForSeconds(1f);

            // 대기 후 BoxCollider2D 비활성화
            box2D.enabled = false;
            isActivated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsLayerMatched(swordSO.target.value, collision.gameObject.layer))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                // 충돌한 오브젝트의 체력을 감소시킵니다.
                bool isAttackApplied = healthSystem.ChangeHealth(-swordSO.power);

                Debug.Log($"{swordSO.power}만큼의 데미지");
            }
        }
    }

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }

}
