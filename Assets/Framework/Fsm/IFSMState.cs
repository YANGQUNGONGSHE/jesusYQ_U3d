//===================================================
//作    者：王家俊  http://www.unity3d.com  QQ：394916173
//创建时间：2016-08-24 23:48:32
//备    注：状态接口,不建议直接实现 建议编写基类实现该接口 子类继承 参照：JJFSMBaseState
//===================================================

namespace WongJJ.Game.Core
{
    public interface IFSMState
    {
        int GetStateId();

        void OnStateEnter(FSMStateMachine machine, IFSMState lastState, params object[] param);

        void OnStateExecute();

        void OnStateFixedUpdate();

        void OnStateLateUpdate();

        void OnStateLeave(IFSMState nextState, params object[] param);

    }
}