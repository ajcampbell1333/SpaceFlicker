using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAsteroidScale : MonoBehaviour {

    IEnumerator scaleLerp;
    public float duration = 1;
    public float start = 0;
    public float finish = 1;
    public float rangeLowEnd = 0;
    public float rangeHighEnd = 1;

    private void Start()
    {
        
    }

    public void BeginLerp(int direction)
    {
        EndLerp();
        scaleLerp = ScaleLerp(direction);
    }

    public void EndLerp()
    {
        if (scaleLerp != null)
            StopCoroutine(scaleLerp);
    }

    IEnumerator ScaleLerp(int direction)
    {
        start = (direction > 0) ? rangeLowEnd : rangeHighEnd;
        finish = (direction > 0) ? rangeHighEnd : rangeLowEnd;
        float startTime = Time.time;
        float t = 0;
        while (t < 1)
        {
            t = (Time.time - startTime) / duration;
            float newScale = Mathf.SmoothStep(start, finish, t);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        transform.localScale = new Vector3(finish, finish, finish);
        yield return null;
    }
}
