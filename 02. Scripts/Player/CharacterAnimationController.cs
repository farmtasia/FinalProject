using System;
using System.Collections;
using UnityEngine;

public class CharacterAnimationController : AnimationController
{
    private static readonly int isWalking = Animator.StringToHash("isWalking");    // 걷기

    private static readonly int isCasting = Animator.StringToHash("isCasting");    // 낚싯대 던지기
    private static readonly int isReeling = Animator.StringToHash("isReeling");    // 낚싯대 흔들림
    private static readonly int isCaughting = Animator.StringToHash("isCaughting");    // 잡아올리기
    private static readonly int isDig = Animator.StringToHash("isDig");    // 땅 파기
    private static readonly int isWatering = Animator.StringToHash("isWatering");    // 물 주기
    private static readonly int isBasic = Animator.StringToHash("isBasic");    // 기본상태로 설정

    public Action OnCastingEvent;
    public Action OnReelingEvent;
    public Action OnCaughtingEvent;
    public Action OnWateringEvent; // 밭 뿐만 아니라 나무에도 사용되어야 함
    public Action OnStopMoveEvent; // 걷는 애니메이션 바로 종료
    public Action OnDigEvent;

    private readonly float magnityteThreshold = 0.1f;
    [SerializeField] private float wateringTime;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        controller.OnMoveEvent += Move;
        Field.OnDigEvent += Dig;    //  보류
        OnDigEvent += Dig;
        OnCastingEvent += Casting;
        OnReelingEvent += Reeling;
        OnCaughtingEvent += Caughting;
        OnWateringEvent += Watering;
        OnStopMoveEvent += StopMove;
    }

    private void Move(Vector2 vector)
    {
        bool isMoving = vector.magnitude > magnityteThreshold;
        animator.SetBool(isWalking, isMoving);
    }

    // TODO: 다른 행동들도 추가
    private void Casting()
    {
        animator.SetBool(isBasic, false);
        animator.SetBool(isCaughting, false);
        animator.SetBool(isCasting, true);
    }

    private void Reeling()
    {
        animator.SetBool(isCasting, false);
        animator.SetBool(isReeling, true);
    }

    private void Caughting()
    {
        animator.SetBool(isReeling, false);
        animator.SetBool(isCaughting, true);
    }

    private void Dig()
    {
        GameManager.Instance.Player.topDownMovement.HoldOnMoveSpeed(0f);
        StartCoroutine(DigGround());
    }

    private void Watering()
    {
        StartCoroutine(GiveWater(wateringTime));
    }

    private IEnumerator GiveWater(float waitTime)
    {
        StopMove();
        animator.SetBool(isWatering, true);
        yield return new WaitForSeconds(waitTime); // 루프가 켜져있어서 이렇게 설정함
        animator.SetBool(isWatering, false);
    }

    private IEnumerator DigGround()
    {
        StopMove();
        animator.SetBool(isDig, true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool(isDig, false);
        // 다빈 추가 : 좋은 방법은 아니지만 임시설정
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.Player.topDownMovement.ResetMoveSpeed();
    }

    private void StopMove()
    {
        animator.SetBool(isWalking, false);
    }
}