/*********************************************
 *
 *   Title: 状态基类
 *
 *   Description: 此类具有带有对的字典（Transition-StateID），指示当触发转换T并且当前状态为S1时FSM应该进入的新状态S2
 *                它具有添加和删除对的方法（Transition-StateID），这是一种在传递转换时检查要转到哪个状态的方法。
 *                在给出的示例中使用两种方法来检查应该触发哪个转换（Reason（））以及具有FSMState附加的GameObject应该执行哪些操作（Act（））。
 *                不必使用此架构，但必须在游戏中使用某种过渡动作代码。
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
/// 过渡条件
/// </summary>
public enum Transition
{
    None,
    //看到玩家
    SawPlayer,
    //追上玩家
    ReachPlayer,
    //丢失玩家
    LostPlayer,
    NoHp,
    //被攻击
    GetHit,
    //被攻击后
    Hitted,
    AfterAttack,
    Skill1,
    Skill2,
}
/// <summary>
/// 对应状态
/// </summary>
public enum FSMState
{
    None,
    //巡逻
    Patrol,
    //追击
    Chase,
    //攻击
    Attack,
    //受到攻击
    Hit,
    //技能
    Skill,
    //死亡
    Death,

    //Boss
    Idle,
    Skill1,
    Skill2,
}
/// <summary>
/// 状态基类
/// </summary>
public abstract class FSMStateBase
{
    /// <summary>
    /// 过渡条件与状态的映射
    /// </summary>
    protected Dictionary<Transition, FSMState> stateBaseDic = new Dictionary<Transition, FSMState>();
    //当前实体
    protected Transform enemyTransform;
    //玩家
    protected Transform playerTransform;
    //寻路组件
    protected NavMeshAgent agent;
    //动画状态机
    protected Animator anim;
    //状态机
    protected FSMSystem fsm;
    /// <summary>
    /// 当前状态类
    /// </summary>
    protected FSMState fsmState;
    public FSMState FSMState
    {
        get { return fsmState; }
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="_fsm">Fsm.</param>
    /// <param name="_enemyTransform">Enemy transform.</param>
    /// <param name="_playerTransform">Player transform.</param>
    /// <param name="_agent">Agent.</param>
    /// <param name="_anim">Animation.</param>
    public FSMStateBase(FSMSystem _fsm, Transform _enemyTransform, Transform _playerTransform, NavMeshAgent _agent, Animator _anim)
    {
        fsm = _fsm;
        enemyTransform = _enemyTransform;
        playerTransform = _playerTransform;
        agent = _agent;
        anim = _anim;
    }
    /// <summary>
    /// 根据过渡条件获取对应的映射状态
    /// </summary>
    /// <returns>The output state.</returns>
    /// <param name="trans">Trans.</param>
    public FSMState GetOutPutState(Transition _transition)
    {
        if (stateBaseDic.ContainsKey(_transition))
        {
            return stateBaseDic[_transition];
        }
        return FSMState.None;
    }
    /// <summary>
    /// 添加映射
    /// </summary>
    /// <param name="trans">过渡条件.</param>
    /// <param name="state">对应状态.</param>
    public void AddTransition(Transition transition, FSMState state)
    {
        if (transition == Transition.None || state == FSMState.None || stateBaseDic.ContainsKey(transition))
            return;
        stateBaseDic.Add(transition, state);
    }
    /// <summary>
    /// 移除映射
    /// </summary>
    /// <param name="trans">过渡条件.</param>
    public void RemoveTransition(Transition transition)
    {
        if (transition == Transition.None || !stateBaseDic.ContainsKey(transition))
        {
            return;
        }
        stateBaseDic.Remove(transition);
    }
    /// <summary>
    /// 进入状态之前
    /// </summary>
    public virtual void DoBeforeEntering()
    {

    }

    /// <summary>
    /// 离开状态之前
    /// </summary>
    public virtual void DoBeforeLeaving()
    {

    }

    /// <summary>
    /// 过渡条件的检测
    /// </summary>
    /// <param name="_enemyTransform">本身.</param>
    /// <param name="_playerTransform">玩家.</param>
    public abstract void Reason(Transform _enemyTransform, Transform _playerTransform);

    /// <summary>
    /// 执行状态
    /// </summary>
    /// <param name="_enemyTransform">本身.</param>
    /// <param name="_playerTransform">玩家.</param>
    public abstract void Act(Transform _enemyTransform, Transform _playerTransform);
}