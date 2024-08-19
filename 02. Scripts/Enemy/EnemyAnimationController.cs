using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimationController : AnimationController
{
    private static readonly int isIdle = Animator.StringToHash("isIdle");
    private static readonly int isWalking = Animator.StringToHash("isWalking");
    private static readonly int isHit = Animator.StringToHash("isHit");
    private static readonly int isAttack = Animator.StringToHash("isAttack");
    private static readonly int isDeath = Animator.StringToHash("isDeath");

    private readonly float magnityteThreshold = 0.1f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 vector)
    {
        bool isMoving = vector.magnitude > magnityteThreshold;
        animator.SetBool(isWalking, isMoving);
    }

    private void SetIdle()
    {
        animator.SetBool(isWalking, false);
    }

    private void SetHit()
    {
        animator.SetBool(isHit, true);
    }

    private void SetAttack()
    {
        animator.SetBool(isAttack, true);
    }

    private void SetDeath()
    {
        animator.SetBool(isDeath, true);
    }

}
