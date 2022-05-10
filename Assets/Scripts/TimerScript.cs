using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public TMP_Text CurrentLap
    {
        get => currentLap;
    }

    public TMP_Text LastLap
    {
        get => lastLap;
    }

    [SerializeField]
    private TMP_Text currentLap;

    [SerializeField]
    private TMP_Text lastLap;

    private float time;
    private bool timerStarted;

    private void Update()
    {
        if (timerStarted)
        {
            time += Time.deltaTime;
            currentLap.text = $"Текущее время: {GetSeconds()}";
        }
    }

    //Resets timer to zero and returns the last lap time.
    public void Refresh()
    {
        lastLap.text = $"Прошлый круг: {GetSeconds()}";
        time = 0f;
    }

    private int GetSeconds()
    {
        return Mathf.FloorToInt(time % 60);
    }

    public void StopTimer()
    {
        timerStarted = false;
    }

    public void StartTimer()
    {
        timerStarted = true;
        time = 0f;
    }
}
