/*********************************************
 *
 *   Title: 巡逻
 *
 *   Description: 
 *
 *   Author: 自律
 *
 *   Date: 2019.4.12
 *
 *   Modify: 
 * 
 *********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : FSMStateBase
{
    protected GameObject[] patorlPoint;
    //巡逻点索引
    int index;
    float timer;
    float timerInterval = 3f;
    AnimatorStateInfo info;
    public Patrol(FSMSystem _fsm, Transform _enemyTransform, Transform _playerTransform, NavMeshAgent _agent, Animator _anim, GameObject[] _patorlPoint) : base(_fsm, _enemyTransform, _playerTransform, _agent, _anim)
    {
        patorlPoint = _patorlPoint;
        fsmState = FSMState.Patrol;
    }

    public override void Act(Transform _enemyTransform, Transform _playerTransform)
    {
        if (agent.remainingDistance < .1f)
        {
            info = anim.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Walk"))
            {
                anim.SetBool("Walk", false);
            }
            timer += Time.deltaTime;
            if (timer >= timerInterval)
            {
                timer = 0;
                if (!info.IsName("Walk"))
                {
                    anim.SetBool("Walk", true);
                }
                index = Random.Range(0, patorlPoint.Length);
                agent.SetDestination(patorlPoint[index].transform.position);
            }
        }
    }

    public override void Reason(Transform _enemyTransform, Transform _playerTransform)
    {
        float distance = Vector3.Distance(_enemyTransform.position, _playerTransform.position);
        //追上玩家
        if (distance <= 1.5f)
        {
            fsm.PerformTransition(Transition.ReachPlayer);
        }
        else if (distance <= 10f)
        {
            fsm.PerformTransition(Transition.SawPlayer);
        }
    }

    public override void DoBeforeEntering()
    {
        //agent = enemyTransform.GetComponent<NavMeshAgent>();
        agent.SetDestination(patorlPoint[0].transform.position);
        anim.SetBool("Walk", true);
    }
    public override void DoBeforeLeaving()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Walk") || info.IsName("Idle"))
        {
            anim.SetBool("Walk", false);
        }
    }
}