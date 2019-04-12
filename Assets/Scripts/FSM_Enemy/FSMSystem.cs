/*********************************************
 *
 *   Title: 有限状态机类。
 *
 *   Description: 这是一个有限状态机类，你的游戏中的每个NPC或GameObject都必须拥有这个类才能使用这个框架。
 *                它将NPC的状态存储在一个列表中，具有添加和删除状态的方法，以及基于传递给它的转换更改当前状态的方法(PerformTransition())。
 *                您可以在代码中的任何地方调用这个方法，比如在冲突测试中，或者在Update()或FixedUpdate()中。
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
/// <summary>
/// 状态机
/// </summary>
public class FSMSystem
{
    //所有状态
    private List<FSMStateBase> statesList;
    //当前状态
    private FSMStateBase currentState;
    //前一个状态
    public FSMStateBase BeforeState { get; set; }
    public FSMStateBase CurrentState
    {
        get { return currentState; }
    }
    public FSMSystem()
    {
        statesList = new List<FSMStateBase>();
    }
    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="_stateBase">State base.</param>
    public void AddState(FSMStateBase _stateBase)
    {
        if (_stateBase == null)
        {
            Debug.LogError("AddState error stateBase is null");
        }
        if (statesList.Count == 0)
        {
            statesList.Add(_stateBase);
            currentState = _stateBase;
            return;
        }
        for (int i = 0; i < statesList.Count; i++)
        {
            if (statesList[i] == _stateBase)
            {
                Debug.LogError("AddState error: Impossible to add state.");
                return;
            }
        }
        statesList.Add(_stateBase);
    }
    /// <summary>
    /// 移除状态
    /// </summary>
    /// <param name="_stateBase">State base.</param>
    public void RemoveState(FSMStateBase _stateBase)
    {
        for (int i = 0; i < statesList.Count; i++)
        {
            if (statesList[i] == _stateBase)
            {
                statesList.Remove(_stateBase);
                return;
            }
        }
        Debug.LogError("RemoveState error: It was not on the list of states");
    }
    /// <summary>
    /// 执行过渡条件
    /// </summary>
    /// <param name="_transition">Transition.</param>
    public void PerformTransition(Transition _transition)
    {
        if (_transition == Transition.None)
        {
            Debug.LogError("PerformTransition _transition error : transition is none");
            return;
        }
        FSMState stateBase = currentState.GetOutPutState(_transition);
        if (stateBase == FSMState.None)
        {
            Debug.LogError("PerformTransition _transition error : State does not have a target state  for transition");
            return;
        }
        for (int i = 0; i < statesList.Count; i++)
        {
            if (statesList[i].FSMState == stateBase)
            {
                currentState.DoBeforeLeaving();
                //只有这样切换才给上一个状态赋值,当前状态已经变成了上一个状态
                BeforeState = currentState;
                //当前状态已经被转换
                currentState = statesList[i];
                currentState.DoBeforeEntering();
            }
        }
    }
    /// <summary>
    /// 执行状态基类
    /// </summary>
    /// <param name="_stateBase">State base.</param>
    public void PerformTransition(FSMStateBase _stateBase)
    {
        Debug.Log(currentState.ToString() + " -> " + _stateBase.ToString());
        currentState.DoBeforeLeaving();
        currentState = _stateBase;
        currentState.DoBeforeEntering();
    }
}