//===================================================
//作    者：王家俊  http://www.unity3d.com  QQ：394916173
//创建时间：2016-08-25 20:11:45
//备    注：该类可以直接继承，也可以仿照重写 例：(runstate,idlestate,attackstate)：PlayerBaseState  (openstate,closestate)：SceneBoxBaseState
//===================================================
using System;

namespace WongJJ.Game.Core
{
    public class FSMBaseState : IFSMState
    {
        //Class controllerClass = null;

        //public JJFSMBaseState(Class controllerClass) //引用传进来赋值，子类各个状态拿到引用操作
        //{
        //    controllerClass = controllerClass;
        //}

        public virtual int GetStateId()
        {
            return 0;
        }

        public virtual void OnStateEnter(FSMStateMachine machine, IFSMState lastState, params object[] param)
        {
        }

        public virtual void OnStateExecute()
        {
        }

        public virtual void OnStateFixedUpdate()
        {
        }

        public virtual void OnStateLateUpdate()
        {
        }

        public virtual void OnStateLeave(IFSMState nextState, params object[] param)
        {
        }
    }
}