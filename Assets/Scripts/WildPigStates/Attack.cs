/*********************************************
 *
 *   Title: 攻击
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

public class Attack : FSMStateBase
{
    float timer = 0f;
    float timerInterval = 2f;
    public Attack(FSMSystem _fsm, Transform _enemyTransform, Transform _playerTransform, NavMeshAgent _agent, Animator _anim) : base(_fsm, _enemyTransform, _playerTransform, _agent, _anim)
    {
        fsmState = FSMState.Attack;
    }

    public override void DoBeforeEntering()
    {
        agent.isStopped = true;
        //agent.Stop();
        enemyTransform.rotation = Quaternion.LookRotation(playerTransform.position - enemyTransform.position);
        anim.SetTrigger("Attack");
    }

    public override void DoBeforeLeaving()
    {
        //  agent.Resume();
        agent.isStopped = false;

    }
    public override void Act(Transform _enemyTransform, Transform _playerTransform)
    {
        timer += Time.deltaTime;
        if (timer >= timerInterval)
        {
            timer = 0;
            enemyTransform.rotation = Quaternion.LookRotation(_playerTransform.position - _enemyTransform.position);
            anim.SetTrigger("Attack");
        }
    }

    public override void Reason(Transform _enemyTransform, Transform _playerTransform)
    {
        float dis = Vector3.Distance(_enemyTransform.position, _playerTransform.position);
        if (dis > 2f)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Skill"))
                fsm.PerformTransition(Transition.SawPlayer);
            if (dis > 15)
                fsm.PerformTransition(Transition.LostPlayer);
        }
    }
}