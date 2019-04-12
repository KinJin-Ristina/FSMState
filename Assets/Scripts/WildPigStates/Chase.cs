/*********************************************
 *
 *   Title: 追击
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
/// <summary>
/// 追击
/// </summary>
public class Chase : FSMStateBase
{
    public Chase(FSMSystem _fsm, Transform _enemyTransform, Transform _playerTransform, NavMeshAgent _agent, Animator _anim) : base(_fsm, _enemyTransform, _playerTransform, _agent, _anim)
    {
        fsmState = FSMState.Chase;
    }
    public override void DoBeforeEntering()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            anim.SetBool("Run", true);
        agent.speed = 2f;
    }

    public override void DoBeforeLeaving()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            anim.SetBool("Run", false);
        agent.speed = 1f;

    }
    public override void Act(Transform _enemyTransform, Transform _playerTransform)
    {
        agent.SetDestination(_playerTransform.position);
    }

    public override void Reason(Transform _enemyTransform, Transform _playerTransform)
    {
        //追到玩家
        if (Vector3.Distance(_enemyTransform.position, _playerTransform.position) <= 1.5f)
        {
            fsm.PerformTransition(Transition.ReachPlayer);
        }
        //丢失玩家
        if (Vector3.Distance(_enemyTransform.position, playerTransform.position) >= 15f)
        {

            fsm.PerformTransition(Transition.LostPlayer);
        }
    }
}