/*********************************************
 *
 *   Title: 死亡
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

public class Dead : FSMStateBase
{
    public Dead(FSMSystem _fsm, Transform _enemyTransform, Transform _playerTransform, NavMeshAgent _agent, Animator _anim) : base(_fsm, _enemyTransform, _playerTransform, _agent, _anim)
    {
        this.fsmState = FSMState.Death;
    }
    public override void DoBeforeEntering()
    {
        anim.SetTrigger("Death");
        //agent.Stop();
        agent.isStopped = true;
        enemyTransform.GetComponent<CapsuleCollider>().enabled = false;
    }
    public override void DoBeforeLeaving()
    {

    }

    public override void Act(Transform _enemyTransform, Transform _playerTransform)
    {

    }

    public override void Reason(Transform _enemyTransform, Transform _playerTransform)
    {

    }
}