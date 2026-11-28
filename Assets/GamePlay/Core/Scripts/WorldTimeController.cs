using UnityEngine;

public class WorldTimeController : MonoBehaviour
{
    [Header("Time Settings")]
    // converts our day into real-world seconds
    [SerializeField] private float dayDurationMinutes = 20f;

    [Header("Current Time")]
    // start at 6AM for now
    [SerializeField, Range(0f, 24f)] private float currentHour = 6f;

    public float CurrentHour => currentHour;

    // give other scripts access to world clock
    public static WorldTimeController Instance { get; private set; }
    // to identify how much time has passes
    public event System.Action<float> OnTimeElapsed;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            // prevent duplicate clocks from running if another scene contains this script
            // won't happen because we'll only have our world clock in first scene, but good fallback
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // preserve the clock and it's current time across scene transitions
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Time.deltaTime = elapsed time since previous frame
        // 24f / (dayDurationMinutes * 60f) = how many in-game hours pass per real world second
        float elapsedHours = Time.deltaTime * (24f / (dayDurationMinutes * 60f));
        
        currentHour += elapsedHours;

        // restart current hour at 0 when we reach midnight
        if(currentHour > 24f)
        {
            currentHour -= 24f;
        }

        OnTimeElapsed?.Invoke(elapsedHours);
    }
}
