using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour

{
    private Animator _animator;
    private Movement _movement;

    private Vector3 _initialScale = Vector3.one;
    private Vector3 _flipScale = Vector3.one;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _movement = transform.parent.GetComponent<Movement>();

        _initialScale = transform.localScale;
        _flipScale = new Vector3(-_initialScale.x, _initialScale.y, _initialScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        HandleFlip();
        UpdateAnimator();
    }

    void HandleFlip()
    {
        if (_movement == null)
            return;
    }

    void UpdateAnimator()
    {
        if (_movement == null || _animator == null)
        {
            return;
        }

        _animator.SetBool("IsRunning", _movement.IsRunning);
        _animator.SetBool("IsJumping", _movement.IsJumping);
        //_animator.SetBool("isFalling", _movement.IsFalling);
    }
}
