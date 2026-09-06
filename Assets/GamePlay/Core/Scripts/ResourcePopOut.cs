using System.Collections;
using UnityEngine;

public class ResourcePopOut : MonoBehaviour
{
    public void Pop(
        FacingDirection direction,
        int hitNumber,
        bool stack = false,
        bool cluster = false,
        float clusterSpreadX = 0f,
        float clusterSpreadY = 0f,
        float popDistance = 0.35f,
        float popDuration = 0.15f,
        float popHeight = 0.2f
        )
    {
        Vector3 start = transform.position;
        Vector3 end = start + PopDirectionOffset(
            direction, 
            hitNumber, 
            stack, 
            cluster,
            clusterSpreadX,
            clusterSpreadY,
            popDistance);

        StopAllCoroutines();
        StartCoroutine(PopOutRoutine(start, end, popDuration, popHeight));
    }

    private Vector3 PopDirectionOffset(
        FacingDirection direction, 
        int hitNumber, 
        bool stack, 
        bool cluster,
        float clusterSpreadX,
        float clusterSpreadY,
        float popDistance
        )
    {
        float stackOffset = 0f;
        float randomX = 0f;
        float randomY = 0f;

        if (stack)
        {
            // stack them on top of each other
            switch (hitNumber)
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
        }

        if (cluster)
        {
            randomX = Random.Range(-clusterSpreadX, clusterSpreadX);
            randomY = Random.Range(-clusterSpreadY, clusterSpreadY);

            switch (direction)
            {
                case FacingDirection.North:
                    return new Vector3(
                        randomX,
                        popDistance + randomY,
                        0f
                    );

                case FacingDirection.South:
                    return new Vector3(
                        randomX,
                        -popDistance + randomY,
                        0f
                    );

                case FacingDirection.East:
                    return new Vector3(
                        popDistance + randomX,
                        randomY,
                        0f
                    );

                case FacingDirection.West:
                    return new Vector3(
                        -popDistance + randomX,
                        randomY,
                        0f
                    );
            }
        }

        // for stacking logs
        switch (direction)
        {
            case FacingDirection.South:
                return new Vector3(
                    popDistance + randomX,
                    stackOffset + randomY,
                    0f
                );

            case FacingDirection.North:
                return new Vector3(
                    -popDistance + randomX,
                    stackOffset + randomY,
                    0f
                );

            default:
                return new Vector3(
                    popDistance + randomX,
                    stackOffset + randomY,
                    0f
                );

        }
    }

    private IEnumerator PopOutRoutine(Vector3 start, Vector3 end, float popDuration, float popHeight)
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
