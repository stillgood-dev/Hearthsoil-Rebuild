using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSourceController : MonoBehaviour
{

    [Header("Player Refs")]
    [SerializeField] private PlayerLightSourceController playerLightController;
    [SerializeField] private bool playerInInteractZone;

    [Header("Light Refs")]
    [SerializeField] private Animator animator;
    [Tooltip("Light 2D object attached to light source")]
    [SerializeField] private GameObject light2D;
    [SerializeField] private bool lightLit = false;
    [Tooltip("Can you carry this light around?")]
    [SerializeField] private bool isStationary = true;

    public bool LightLit => lightLit;

    private void Awake()
    {
        if (light2D != null) light2D.SetActive(false);
    }

    public void SetPlayerInInteractZone(bool inRange, PlayerLightSourceController playerLightSourceController)
    {
        playerInInteractZone = inRange;
        playerLightController = playerLightSourceController;

        if (playerInInteractZone)
        {
            playerLightController?.SetLightTarget(this);
        } else
        {
            playerLightController?.ClearLightTarget(this);
        }
    }

    public void LightOn()
    {
        lightLit = true;
        if (isStationary)
        {
            animator.SetBool("Lit", lightLit);
            if (light2D != null) light2D.SetActive(true);
        }
    }

    public void LightOff()
    {
        if (!lightLit) return;

        lightLit = false;
        if (isStationary)
        {
            animator.SetBool("Lit", lightLit);
            if (light2D != null) light2D.SetActive(false);
        }
    }
}
