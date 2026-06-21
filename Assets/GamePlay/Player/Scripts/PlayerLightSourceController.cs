using UnityEngine;

public class PlayerLightSourceController : MonoBehaviour
{
    [Header("Light Source Refs")]
    [SerializeField] private LightSourceController currentLightSource;
    [SerializeField] private LightSourceController lockedLightSource;


    public void SetLightTarget(LightSourceController light)
    {
        if (!light) return;
        currentLightSource = light;
    }

    public void ClearLightTarget(LightSourceController light)
    {
        if (!light) return;
        if (currentLightSource == light)
            currentLightSource = null;
    }

    public bool ToggleLight()
    {
        if (currentLightSource == null) return false;

        lockedLightSource = currentLightSource;

        if (lockedLightSource.LightLit)
            lockedLightSource.LightOff();
        else
            lockedLightSource.LightOn();

        return true;
    }
}
