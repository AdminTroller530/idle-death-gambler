using TMPro;
using UnityEngine;

public class FloorTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _floorTimerText;

    private int _minutes;
    private float _secondsReal;
    private string _secondsDisplay;
    private bool _isRunning = false;

    private const int MINUTES_DEFAULT = 2;
    private const int SECONDS_DEFAULT = 0;

    public void StartFloorTimer()
    {
        StartTimer(MINUTES_DEFAULT, SECONDS_DEFAULT);
    }

    public void StartTimer(int minutes, int seconds)
    {
        _floorTimerText.enabled = true;
        _minutes = minutes;
        _secondsReal = seconds;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _minutes = 0;
        _secondsReal = 0;
        _floorTimerText.enabled = false;
        _isRunning = false;
    }

    private void IncrementTimer()
    {
        _secondsReal -= Time.deltaTime;

        if (_secondsReal < 0) {
            _secondsReal += 60;
            if (_minutes > 0) _minutes--;
            else
            {
                // timer end
                _secondsReal = 0;
                _isRunning = false;
            }
        }

        _secondsDisplay = ((int)_secondsReal).ToString();
        if (_secondsDisplay.Length == 1) _secondsDisplay = "0" + _secondsDisplay;
        _floorTimerText.text = $"{_minutes}:{_secondsDisplay}";
    }

    private void Update()
    {
        if (_isRunning) IncrementTimer();
    }
}
