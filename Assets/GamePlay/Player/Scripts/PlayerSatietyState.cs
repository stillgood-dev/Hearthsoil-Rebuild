using UnityEngine;

public class PlayerSatietyState : MonoBehaviour
{

    [Header("Satiety")]
    [SerializeField, Range(0, 100)] private float satietyLevel = 100f;
    [SerializeField] private float depletionPerHour = 5f;

    public float SatietyLevel => satietyLevel;

    private void Start()
    {
        // subscribe a method to an event class
        // Whenever WorldTimeController invokes OnTimeElapsed,
        // automatically call DepleteSatiety() and pass its elapsed hours
        WorldTimeController.Instance.OnTimeElapsed += DepleteSatiety;
    }

    public void DepleteSatiety(float elapsedHours)
    {
        // substract satiety proportionally to amount of in-game time that has passed
        satietyLevel -= elapsedHours * depletionPerHour;
        // prevent from dropping below 0
        satietyLevel = Mathf.Max(0f, satietyLevel);
    }

    public void RestoreSatiety(float amount)
    {
        satietyLevel = Mathf.Min(100f, satietyLevel + amount);
    }

    private void OnDestroy()
    {
        // if another WorldTimeController exists, don't call DepleteSatiety
        if(WorldTimeController.Instance != null)
            // unsubscribe method from an event class
            WorldTimeController.Instance.OnTimeElapsed -= DepleteSatiety;
    }
}
