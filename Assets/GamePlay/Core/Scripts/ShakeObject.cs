using UnityEngine;
using System.Collections;

public class ShakeObject : MonoBehaviour
{
    [Header("Must be dragged into Damageable Object Controller!")]
    [SerializeField] private Transform sprites;

    [Header("Impact Shake")]
    [SerializeField] private float shakeDistance = 0.01f;
    [SerializeField] private float shakeDuration = 0.12f;
    [SerializeField] private int shakeCount = 3;

    private Vector3 spriteStartPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (sprites != null)
            spriteStartPosition = sprites.localPosition;
    }

    public void Shake()
    {
        if (sprites == null) return;

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float stepDuration = shakeDuration / (shakeCount * 2);

        for (int i = 0; i < shakeCount; i++)
        {
            sprites.localPosition =
                spriteStartPosition + Vector3.left * shakeDistance;

            yield return new WaitForSeconds(stepDuration);

            sprites.localPosition =
                spriteStartPosition + Vector3.right * shakeDistance;

            yield return new WaitForSeconds(stepDuration);
        }

        sprites.localPosition = spriteStartPosition;
        shakeCoroutine = null;
    }
}
