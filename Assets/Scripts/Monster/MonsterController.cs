using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Action<Vector2> OnMoveEvent;

    public void OnMove(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }
}
