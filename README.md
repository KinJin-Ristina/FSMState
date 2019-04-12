# FSMState
Unity FSM有限状态机
这是一个有限状态机框架，你的游戏中的每个NPC或GameObject都必须拥有这个类才能使用这个框架。
它将NPC的状态存储在一个列表中，具有添加和删除状态的方法，以及基于传递给它的转换更改当前状态的方法(PerformTransition())。
您可以在代码中的任何地方调用这个方法，比如在冲突测试中，或者在Update()或FixedUpdate()中。
