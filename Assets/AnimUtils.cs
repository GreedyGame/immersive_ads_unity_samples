using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public static class AnimUtils
{
    #region PUBLIC_METHODS
    public static void DoPunchScale(GameObject target, float punchStrength, float duration)
    {
        if (target != null)
        {
            target.GetComponent<MonoBehaviour>().StartCoroutine(AnimateScale(target.transform, punchStrength, duration));
        }
        else
        {
            Debug.LogError("Target GameObject is null.");
        }
    }
    public static void DoScale(Transform target, Vector3 targetScale, float duration)
    {
        if (target != null)
        {
            target.GetComponent<MonoBehaviour>().StartCoroutine(ScaleOverTime(target, targetScale, duration));
        }
        else
        {
            Debug.LogError("Target GameObject is null.");
        }
    }
    public static void DoString(TextMeshProUGUI text, string targetString, float duration)
    {
        if (text != null)
        {
            text.GetComponent<MonoBehaviour>().StartCoroutine(AnimateString(text, targetString, duration));
        }
        else
        {
            Debug.LogError("Target GameObject is null.");
        }
    }
    public static void DoAnchorMoveX(RectTransform rectTransform, float targetX, float duration, MonoBehaviour caller)
    {
        if (rectTransform != null)
        {
            caller.StartCoroutine(AnchorMoveXOverTime(rectTransform, targetX, duration));
        }
        else
        {
            Debug.LogError("RectTransform is null.");
        }
    }
    public static void DoMove(Transform target, Vector3 destination, float duration)
    {
        if (target != null)
        {
            target.GetComponent<MonoBehaviour>().StartCoroutine(MoveOverTime(target, destination, duration));
        }
        else
        {
            Debug.LogError("Target GameObject is null.");
        }
    }
    public static void DoMoveLocal(Transform target, Vector3 destination, float duration, MonoBehaviour caller)
    {
        if (target != null)
        {
            if(caller != null)
            {
                caller.StartCoroutine(MoveLocalOverTime(target, destination, duration));
            }
            else
            {
                Debug.LogError("no monobehavior");
            }
        }
        else
        {
            Debug.LogError("Target GameObject is null.");
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private static IEnumerator MoveOverTime(Transform target, Vector3 destination, float duration)
    {
        Vector3 start = target.position;
        float time = 0;

        while (time < duration)
        {
            target.position = Vector3.Lerp(start, destination, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.position = destination;
    }

    private static IEnumerator MoveLocalOverTime(Transform target, Vector3 destination, float duration)
    {
        Vector3 start = target.localPosition;
        float time = 0;

        while (time < duration)
        {
            target.localPosition = Vector3.Lerp(start, destination, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.localPosition = destination;
    }
    static IEnumerator AnchorMoveXOverTime(RectTransform rectTransform, float targetX, float duration)
    {
        float elapsedTime = 0f;
        float startX = rectTransform.anchoredPosition.x;
        
        while (elapsedTime < duration)
        {
            float newX = Mathf.Lerp(startX, targetX, elapsedTime / duration);
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        rectTransform.anchoredPosition = new Vector2(targetX, rectTransform.anchoredPosition.y);
    }
    
    public static IEnumerator AnimateString(TextMeshProUGUI textComponent, string targetString, float duration)
    {
        textComponent.text = "";
        foreach (char c in targetString)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(duration);
        }
        // string startString = textComponent.text;
        // float elapsedTime = 0f;

        // while (elapsedTime < duration)
        // {
        //     textComponent.text = LerpString(startString, targetString, elapsedTime / duration);
        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }

        // Ensure the final text is exactly the target string
        // textComponent.text = targetString;
    }

    private static string LerpString(string start, string end, float t)
    {
        int length = Mathf.Min(start.Length, end.Length);
        int lerpLength = Mathf.RoundToInt(length * t);

        return start.Substring(0, lerpLength) + end.Substring(lerpLength, length - lerpLength);
    }
    
    private static IEnumerator AnimateScale(Transform targetTransform, float punchStrength, float duration)
    {
        Vector3 originalScale = targetTransform.localScale;
        Vector3 targetScale = originalScale + Vector3.one * punchStrength;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            targetTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset to original scale
        targetTransform.localScale = originalScale;
    }

    private static IEnumerator ScaleOverTime(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 start = target.localScale;
        float time = 0;

        while (time < duration)
        {
            target.localScale = Vector3.Lerp(start, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.localScale = targetScale;
    }

    private static IEnumerator RotateOverTime(Transform target, Vector3 targetAngles, float duration)
    {
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = Quaternion.Euler(targetAngles);
        float timePassed = 0f;

        while (timePassed < duration)
        {
            float progress = timePassed / duration;
            target.rotation = Quaternion.Lerp(startRotation, endRotation, progress);
            timePassed += Time.deltaTime;
            yield return null;
        }

        target.rotation = endRotation;
    }
    #endregion
}
