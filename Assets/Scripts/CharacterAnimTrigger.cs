using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimTrigger : MonoBehaviour {

    public string triggerName;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        BlendShapeTrigger fingerTrigger = FindObjectOfType(typeof(BlendShapeTrigger)) as BlendShapeTrigger;
        fingerTrigger.currentAnimTrigger = this;
        Rigidbody rb = transform.parent.GetComponentInChildren<Rigidbody>();
        fingerTrigger.currentRigidbody = rb;
    }
    public void CueCharacterFlail()
    {
        _animator.SetTrigger(triggerName);
    }
}
