using UnityEngine;

public class EndingMusicTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.PlayEndingMusic();
    }
}