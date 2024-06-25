using UnityEngine;

public class BossMusicTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.PlayBossMusic();
    }
}