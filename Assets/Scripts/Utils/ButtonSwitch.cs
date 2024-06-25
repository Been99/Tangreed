using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonSwitch : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 활성화된 씬을 다시 로드
    }

    public void OnExitButton()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false; // 유니티 에디터 종료
        #else
        Application.Quit(); // 빌드된 게임 종료
        #endif
    }
}
