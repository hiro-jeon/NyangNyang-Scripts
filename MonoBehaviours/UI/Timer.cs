using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float startTime = 120f;

    private float currentTime;
    private bool isFinished = false;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (isFinished)
        {
            return ;
        }
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0f, currentTime);

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    
        if (currentTime <= 0f)
        {
            isFinished = true;
            OnTimerEnd();
        }
    }

    void OnTimerEnd()
    {
        Debug.Log("⏰ Timer finished!");
    }
}