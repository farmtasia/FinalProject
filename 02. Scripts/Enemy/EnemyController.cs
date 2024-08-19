using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : TopDownController
{
    private GameManager gameManager;
    protected Transform target {  get; private set; }

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;
        target = gameManager.Player.GetComponent<Transform>();
    }

    protected virtual void FixedUpdate()
    {

    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }
}
