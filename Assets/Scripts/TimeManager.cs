using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    private static TimeManager _instance;
    public static TimeManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
            _instance = this;
    }

    #endregion Singleton

    [SerializeField] private float StartingTime = 0f;
    [SerializeField] private float EndingTime = 0f;
    [SerializeField] private TMP_Text Clock;
    [SerializeField] private float Speed = 1f;
    private bool isTracking = false;
    public float _limittime { get; set; }

    private void Start()
    {
        StartTracking();
        Clock.text = CurrentTimeString;
    }

    private void FixedUpdate()
    {
        if (!IsOutOfTime())
        {
            Clock.text = CurrentTimeString;
        }
        else
            LevelManager.Instance.PlayerLose();
    }

    public void StartTracking()
    {
        StartingTime = Time.time;
        isTracking = true;
    }

    public void EndTracking()
    {
        EndingTime = Time.time;
        isTracking = false;
    }

    public float CurrentTime
    {
        get
        {
            if (isTracking == false)
                return -1;
            return (Time.time - StartingTime) * Speed;
        }
    }

    public float TotalTime
    {
        get
        {
            if (isTracking == false)
                return -1;
            return StartingTime - EndingTime;
        }
    }

    public string CurrentTimeString
    {
        get
        {
            float elapsedTime = (_limittime - (Time.time - StartingTime)) * Speed;
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            return $"{minutes:00}:{seconds:00}";
        }
    }

    public bool IsOutOfTime()
    {
        return ((_limittime - (Time.time - StartingTime)) <= 0);
    }
}
