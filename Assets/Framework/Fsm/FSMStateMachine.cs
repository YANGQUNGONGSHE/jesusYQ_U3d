//===================================================
//作    者：王家俊  http://www.unity3d.com  QQ：394916173
//创建时间：2016-08-24 23:48:20
//备    注：有限状态机（FSM）
//===================================================
using UnityEngine;
using System.Collections.Generic;

namespace WongJJ.Game.Core
{
    public class FSMStateMachine
    {
        /// <summary>
        /// 所有状态<Key状态ID Value状态实例>
        /// </summary>
        private Dictionary<int, IFSMState> m_AllStates;

        private IFSMState m_CurrentState;

        public FSMStateMachine()
        {
            m_AllStates = new Dictionary<int, IFSMState>();
            m_CurrentState = null;
        }

        #region 核心逻辑

        /// <summary>
        /// 注册一个状态
        /// </summary>
        /// <param name="stateId">状态ID</param>
        /// <param name="state">状态实例</param>
        /// <returns>重复注册将返回false</returns>
        public bool RegisterState(IFSMState state)
        {
            if (state == null)
            {
                Debug.LogError("state is Null !");
                return false;
            }

            if (m_AllStates.ContainsKey(state.GetStateId()))
            {
                Debug.LogError(string.Format("Machine already have state with id:{0}", state.GetStateId()));
                return false;
            }

            m_AllStates.Add(state.GetStateId(), state);
            return true;
        }

        /// <summary>
        /// 卸载一个状态
        /// </summary>
        /// <param name="stateId">状态ID</param>
        /// <returns>如果该状态正在执行中，返回false</returns>
        public bool UnRegisterState(int stateId)
        {
            IFSMState state = null;
            m_AllStates.TryGetValue(stateId, out state);

            if (state == null)
            {
                Debug.LogError(string.Format("Machine can not found state with id:{0} in all states", stateId));
                return false;
            }

            if (state == m_CurrentState)
            {
                Debug.LogError(string.Format("Machine state with id:{0} is Executing, can not be UnRegister, you must be stopState first", stateId));
                return false;
            }

            m_AllStates.Remove(stateId);
            return true;
        }

        /// <summary>
        /// 根据状态ID获取一个状态
        /// </summary>
        /// <param name="stateId">状态ID</param>
        /// <returns></returns>
        public IFSMState GetState(int stateId)
        {
            IFSMState state = null;
            m_AllStates.TryGetValue(stateId, out state);
            return state;
        }

        /// <summary>
        /// 如果需要切换的状态就是当前状态，强制再次切换
        /// </summary>
        /// <param name="stateId">State identifier.</param>
        /// <param name="param1">Param1.</param>
        /// <param name="param2">Param2.</param>
        public void ChangeStateSkipConstraint(int stateId)
        {
            ChangeState(stateId, true, null);
        }

        /// <summary>
        /// 如果需要切换的状态就是当前状态，强制再次切换
        /// </summary>
        /// <param name="stateId">State identifier.</param>
        /// <param name="param">Parameter.</param>
        public void ChangeStateSkipConstraint(int stateId, params object[] param)
        {
            ChangeState(stateId, true, param);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateId">State identifier.</param>
        public void ChangeState(int stateId)
        {
            ChangeState(stateId, false, null);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateId">State identifier.</param>
        /// <param name="param">Parameter.</param>
        public void ChangeState(int stateId, params object[] param)
        {
            ChangeState(stateId, false, param);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateId">状态ID</param>
        /// <param name="param1">预留参数1</param>
        /// <param name="param2">预留参数2</param>
        /// <returns>状态null或者状态正在执行返回false</returns>
        public bool ChangeState(int stateId, bool isSkipConstraint, params object[] param)
        {
            IFSMState newState = null;
            m_AllStates.TryGetValue(stateId, out newState);

            if (newState == null)
            {
                Debug.LogError(string.Format("Machine can not found state with id:{0} in all states, Are you Register ?", stateId));
                return false;
            }

            if (!isSkipConstraint)
            {
                if (m_CurrentState != null && stateId == m_CurrentState.GetStateId())
                {
                    return false;
                }
            }

            if (OnBeforeStateChange != null)
            {
                if (m_CurrentState != null)
                    OnBeforeStateChange(m_CurrentState, newState, param);
            }

            if (m_CurrentState != null)
            {
                m_CurrentState.OnStateLeave(newState, param);
            }

            if (OnStateChanging != null)
            {
                if (m_CurrentState != null)
                    OnStateChanging(m_CurrentState, newState, param);
            }

            IFSMState tmpState = m_CurrentState;

            newState.OnStateEnter(this, m_CurrentState, param);
            m_CurrentState = newState;

            if (OnAfterStateChange != null)
            {
                if (tmpState != null)
                    OnAfterStateChange(tmpState, newState, param);
            }

            return true;
        }

        /// <summary>
        /// 停止当前状态执行
        /// </summary>
        /// <param name="param1">预留参数1</param>
        /// <param name="param2">预留参数2</param>
        /// <returns></returns>
        public bool StopCurrentState(object param1 = null, object param2 = null)
        {
            if (m_CurrentState == null)
            {
                Debug.LogError("Machine Currentstate is not found or null ");
                return false;
            }

            m_CurrentState.OnStateLeave(null, param1, param2);
            m_CurrentState = null;
            return true;
        }

        /// <summary>
        /// 检查状态是否在执行
        /// </summary>
        /// <param name="stateId">状态ID</param>
        /// <returns></returns>
        public bool IsStateExecuting(int stateId)
        {
            if (m_CurrentState.GetStateId() != stateId)
            {
                return false;
            }

            if (m_CurrentState == null)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region 事件委托

        /// <summary>
        /// 状态转换中的事件 <A状态 B状态 参数1 参数2>
        /// </summary>
        public System.Action<IFSMState, IFSMState, object[]> OnStateChanging;

        /// <summary>
        /// 状态转换之前事件 <A状态 B状态 参数1 参数2>
        /// </summary>
        public System.Action<IFSMState, IFSMState, object[]> OnBeforeStateChange;

        /// <summary>
        /// 状态转换之后事件 <A状态 B状态 参数1 参数2>
        /// </summary>
        public System.Action<IFSMState, IFSMState, object[]> OnAfterStateChange;

        #endregion

        #region 执行方法

        public void ExecuteStateOnUpdate()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnStateExecute();
            }
        }

        public void ExecuteStateOnFixedUpdate()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnStateFixedUpdate();
            }
        }

        public void ExecuteStateOnLateUpdate()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnStateLateUpdate();
            }
        }

        #endregion

        #region 属性 成员变量

        /// <summary>
        /// 返回当前状态实例
        /// </summary>
        public IFSMState CurrentState
        {
            get
            {
                return m_CurrentState;
            }
        }

        /// <summary>
        /// 返回当前状态ID
        /// </summary>
        public int CurrentStateId
        {
            get
            {
                return m_CurrentState.GetStateId();
            }
        }

        #endregion
    }
}