/*********************************************
 *
 *   Title: 被攻击
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

public class Hit : FSMStateBase
{
    public Hit(FSMSystem _fsm, Transform _enemyTransform, Transform _playerTransform, NavMeshAgent _agent, Animator _anim) : base(_fsm, _enemyTransform, _playerTransform, _agent, _anim)
    {
        this.fsmState = FSMState.Hit;
    }

    public override void Act(Transform _enemyTransform, Transform _playerTransform)
    {

    }

    public override void Reason(Transform _enemyTransform, Transform _playerTransform)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .5f)
        {
            fsm.PerformTransition(Transition.LostPlayer);
        }
    }
    public override void DoBeforeEntering()
    {
        //agent.Stop();
        agent.isStopped = true;
        anim.SetTrigger("Hit");
    }

    public override void DoBeforeLeaving()
    {
        //agent.Resume();
        agent.isStopped = false;
    }
}