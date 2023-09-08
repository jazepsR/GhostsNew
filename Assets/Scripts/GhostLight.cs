using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostLight : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float moveTime = 5;
    public float startIdleTime = 2;
    public AnimationCurve moveCurve;
    bool moveCompleted = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        StopAllCoroutines();
        if(!moveCompleted)
            StartCoroutine("MoveCoroutine");
    }
    private IEnumerator MoveCoroutine()
    {
        transform.position = startPoint.position;
        yield return new WaitForSecondsRealtime(startIdleTime);
        moveCompleted = true;
        float t = 0;
        while(t<=1)
        {
            transform.position = Vector3.LerpUnclamped(startPoint.position, endPoint.position, moveCurve.Evaluate(t));
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime/moveTime;
        }
        transform.position = endPoint.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
