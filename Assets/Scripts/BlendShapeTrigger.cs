using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeTrigger : Singleton<BlendShapeTrigger> {

    public string flickPrepAnimTrigger, flickAnimTrigger;
    SkinnedMeshRenderer rend;
    float waitTimeBeforeReset = 2;
    public CharacterAnimTrigger currentAnimTrigger;
    public Rigidbody currentRigidbody;
    private Animator _handAnimator;

    public float forceMultiplier;

    private void Start()
    {
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        _handAnimator = GetComponentInChildren<Animator>();
    }

    public void Flick()
    {
        StartCoroutine(FlickSequence());
    }

    IEnumerator FlickSequence()
    {
        currentAnimTrigger.CueCharacterFlail();
        currentRigidbody.AddExplosionForce(forceMultiplier, transform.position, 0.3f);
        //StartCoroutine(LerpBlendShape(0, 100, 0, 0.3f));
        //StartCoroutine(LerpBlendShape(1, 0, 100, 0.3f));
        _handAnimator.SetTrigger(flickAnimTrigger);
        yield return new WaitForSeconds(waitTimeBeforeReset);
        //StartCoroutine(LerpBlendShape(1, 100, 0, 0.5f));
    }

    public void SetFinger()
    {
        //StartCoroutine(LerpBlendShape(0, 0, 100, 0.5f));
        _handAnimator.SetTrigger(flickPrepAnimTrigger);
    }

    IEnumerator LerpBlendShape(int index, float start, float finish, float duration)
    {
        
        float startTime = Time.time;
        float t = 0;
        while (t < 1)
        {
            t = (Time.time - startTime) / duration;
            float newWeight = Mathf.SmoothStep(start, finish, t);
            rend.SetBlendShapeWeight(index, newWeight);
        }
        rend.SetBlendShapeWeight(index, finish);
        yield return null;
    }
        
}
