using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    private Animator animator;
    private MonsterController monsterController;

    private static readonly float magnitudeThreshold = 0.1f;
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        monsterController = GetComponent<MonsterController>();
    }

    private void Start()
    {
        monsterController.OnMoveEvent += Moving;
    }
    private void Moving(Vector2 direction)
    {
        animator.SetBool(isMovingHash, direction.magnitude > magnitudeThreshold);
    }
}
