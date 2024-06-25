using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip startSceneMusic;
    public AudioClip[] mainSceneMusicClips;
    public AudioClip bossMusic;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            PlayStartSceneMusic();
        }
        else if (scene.name == "MainScene")
        {
            StopStartSceneMusic();
            PlayMainSceneMusic();
        }
    }

    public void PlayStartSceneMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = startSceneMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopStartSceneMusic()
    {
        if (audioSource.isPlaying && audioSource.clip == startSceneMusic)
        {
            audioSource.Stop();
        }
    }

    public void PlayMainSceneMusic()
    {
        StartCoroutine(PlayMusicClipsSequentially());
    }

    private IEnumerator PlayMusicClipsSequentially()
    {
        foreach (var clip in mainSceneMusicClips)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length);
        }
    }

    private void StopAllMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayBossMusic()
    {
        StopAllMusic();

        audioSource.clip = bossMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
