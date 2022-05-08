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

    private float _time;
    private bool _isPressed;

    private void Update()
    {
        if (_isPressed)
        {
            _time += Time.deltaTime;
            currentLap.text = $"Текущее время: {GetSeconds()}";
        }
    }

    //Resets timer to zero and returns the last lap time
    public void Refresh()
    {
        lastLap.text = $"Прошлый круг: {GetSeconds()}";
        _time = 0f;
    }

    private int GetSeconds()
    {
        return Mathf.FloorToInt(_time % 60);
    }

    public void StopTimer()
    {
        _isPressed = false;
    }

    public void StartTimer()
    {
        _isPressed = true;
    }
}
