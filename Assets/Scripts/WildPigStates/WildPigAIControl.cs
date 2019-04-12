/*********************************************
 *
 *   Title: 敌人控制器
 *
 *   Description: 敌人的状态切换,巡逻,攻击,死亡,追击等
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

public class WildPigAIControl : MonoBehaviour
{
    //玩家
    private Transform player;
    //巡逻点
    private GameObject[] wayPoints;
    //寻路组件
    private NavMeshAgent agent;
    //动画状态机
    private Animator anim;
    //状态机
    private FSMSystem fsm;
    #region 状态机
    Patrol patrol;
    Chase chase;
    Attack attack;
    Dead dead;
    Hit hit;
    /// <summary>
    /// 当前血量
    /// </summary>
    private int currentHp = 100;
    #endregion
    void Awake()
    {
        Init();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        fsm = new FSMSystem();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        wayPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");

        patrol = new Patrol(fsm, transform, player, agent, anim, wayPoints);
        chase = new Chase(fsm, transform, player, agent, anim);
        attack = new Attack(fsm, transform, player, agent, anim);
        dead = new Dead(fsm, transform, player, agent, anim);
        hit = new Hit(fsm, transform, player, agent, anim);

        #region 改变状态
        //看家玩家之后改变自己的状态为追击  看见玩家 -> 追击玩家
        patrol.AddTransition(Transition.SawPlayer, FSMState.Chase);
        //巡逻过程中,可能会被玩家攻击
        patrol.AddTransition(Transition.GetHit, FSMState.Hit);
        //巡逻中玩家进入它的范围 -> 开始攻击
        patrol.AddTransition(Transition.ReachPlayer, FSMState.Attack);

        //追击的过程中,玩家移速过快,距离范围过大  丢失玩家 -> 开始巡逻
        chase.AddTransition(Transition.LostPlayer, FSMState.Patrol);
        //追击过程中,追到了玩家 -> 开始攻击
        chase.AddTransition(Transition.ReachPlayer, FSMState.Attack);
        //收到攻击
        chase.AddTransition(Transition.GetHit, FSMState.Hit);

        //攻击过程中 玩家移动,只要还在距离范围内,就会触发追击,直至玩家超出指定距离
        attack.AddTransition(Transition.SawPlayer, FSMState.Chase);
        //攻击过程中,玩家突然死亡,或者距离超出  就去巡逻
        attack.AddTransition(Transition.LostPlayer, FSMState.Patrol);
        //收到攻击
        attack.AddTransition(Transition.GetHit, FSMState.Hit);

        //被击打的时候,也会存在丢失玩家的情况
        hit.AddTransition(Transition.LostPlayer, FSMState.Patrol);
        #endregion

        fsm.AddState(patrol);
        fsm.AddState(chase);
        fsm.AddState(attack);
        fsm.AddState(dead);
        fsm.AddState(hit);
    }
    void Update()
    {
        fsm.CurrentState.Reason(transform, player);
        fsm.CurrentState.Act(transform, player);
        if (currentHp <= 0)
        {
            fsm.PerformTransition(dead);
        }
    }
    /// <summary>
    /// 死亡 通过动画时间添加的
    /// </summary>
    public void OnDead()
    {
        //或者利用对象池进行回收
        Destroy(this.gameObject, 1.5f);
    }
}