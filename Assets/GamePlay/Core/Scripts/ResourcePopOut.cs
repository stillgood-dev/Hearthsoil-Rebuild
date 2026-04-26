using System.Collections;
using UnityEngine;

public class ResourcePopOut : MonoBehaviour
{
    [Header("Pop Out Parameters")]
    [SerializeField] private float popDistance = 0.35f;
    [SerializeField] private float popHeight = 0.2f;
    [SerializeField] private float popDuration = 0.15f;

    public void Pop(FacingDirection direction, int logHitNumber)
    {
        Vector3 start = transform.position;
        Vector3 end = start + PopDirectionOffset(direction, logHitNumber);

        StopAllCoroutines();
        StartCoroutine(PopOutRoutine(start, end));
    }

    private Vector3 PopDirectionOffset(FacingDirection direction, int logHitNumber)
    {
        float stackOffset = 0f;

        // stack them on top of each other
        switch (logHitNumber)
        {
            case 1:
                stackOffset = 0f;
                break;
            case 2:
                stackOffset = 0.1f;
                break;
            case 3:
                stackOffset = -0.1f;
                break;
            case 4:
                stackOffset = 0.2f;
                break;
        }

        switch (direction)
        {
            case FacingDirection.South:
                return new Vector3(popDistance, stackOffset, 0f);

            case FacingDirection.North:
                return new Vector3(-popDistance, stackOffset, 0f);

            default:
                return new Vector3(popDistance, stackOffset, 0f);
      
        }
    }

    private IEnumerator PopOutRoutine(Vector3 start, Vector3 end)
    {
        float elapsed = 0f;

        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / popDuration);

            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * popHeight;

            transform.position = pos;
            yield return null;
        }

        transform.position = end;
    }

}
