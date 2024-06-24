using TMPro;
using UnityEngine;

public class BlinkManager : MonoBehaviour
{
    private float time;
    public float blinkTime = 1f;
    private bool isBlinking = false;
    private TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartBlinking();
    }

    private void Update()
    {
        if (isBlinking)
        {
            if (time < blinkTime)
            {
                tmpText.color = new Color(1, 1, 1, 1f - time / blinkTime);
            }
            else
            {
                time = 0;
                tmpText.color = Color.white;
            }
            time += Time.deltaTime;
        }
    }

    public void StartBlinking()
    {
        isBlinking = true;
        gameObject.SetActive(true);
    }
}