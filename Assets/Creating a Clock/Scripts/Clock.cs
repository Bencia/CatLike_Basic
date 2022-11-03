using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    Transform hoursPivot, minutesPivot, secondsPivot;
    const float hoursToDegree = -30f, minutesToDegree = -6f, secondsToDegree = -6f;
    private void Update()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursPivot.localRotation = Quaternion.Euler(0, 0, hoursToDegree * (float)time.TotalHours);
        minutesPivot.localRotation = Quaternion.Euler(0, 0, minutesToDegree * (float)time.TotalMinutes);
        secondsPivot.localRotation = Quaternion.Euler(0, 0, secondsToDegree * (float)time.TotalSeconds);
    }
}
