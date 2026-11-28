using UnityEngine;
using UnityEngine.UI;

public class PlayerSatietyUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image satietyMeter;
    [SerializeField] private PlayerSatietyState satietyState;

    [Header("Sprites")]
    [SerializeField] private Sprite[] satietySprites;

    private void Update()
    {
        UpdateSatietyMeter();
    }

    private void UpdateSatietyMeter()
    {
        // round to nearest integer
        int index = Mathf.CeilToInt(satietyState.SatietyLevel / 10f);
        satietyMeter.sprite = satietySprites[index];
    }

}
