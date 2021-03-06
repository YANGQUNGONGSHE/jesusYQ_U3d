using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WongJJ.Game.Core
{
    public class TimerManager : MonoBehaviour
    {
        /// <summary>
        /// 全局实例
        /// </summary>
        private static TimerManager _instance = null;
	
        /// <summary>
        /// 定时器字典
        /// </summary>
        private readonly Dictionary<string, Timer> _mTimerList = new Dictionary<string, Timer>();
	
        /// <summary>
        ///   增加队列
        /// </summary>
        private readonly Dictionary<string, Timer> _mAddTimerList = new Dictionary<string, Timer>();
	
        /// <summary>
        ///   销毁队列
        /// </summary>
        private readonly List<string> _mDestroyTimerList = new List<string>();

        public delegate void TimerManagerHandler();

        public delegate void TimerManagerHandlerArgs(params object[] args);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 全局实例
        /// </summary>
        /// -----------------------------------------------------------------------------
        public static TimerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType(typeof(TimerManager)) as TimerManager;
                    }
                }
			
                return _instance;
            }
        }
	
        // Use this for initialization
        void Awake()
        {
            if (TimerManager.Instance != null && TimerManager.Instance != this)
            {
                UnityEngine.Object.Destroy(this);
                return;
            }
		
            _instance = this;
        }
	
        // Update is called once per frame
        void Update()
        {
            if (_mDestroyTimerList.Count > 0)
            {
                //>从销毁队列中销毁指定内容
                foreach (string i in _mDestroyTimerList)
                {
                    _mTimerList.Remove(i);
                }
			
                //清空
                _mDestroyTimerList.Clear();
            }
		
            if (_mAddTimerList.Count > 0)
            {
                //>从增加队列中增加定时器
                foreach (KeyValuePair<string, Timer> i in _mAddTimerList)
                {
                    if (i.Value == null)
                    {
                        continue;
                    }
				
                    if (_mTimerList.ContainsKey(i.Key))
                    {
                        _mTimerList[i.Key] = i.Value;
                    }
                    else
                    {
                        _mTimerList.Add(i.Key, i.Value);
                    }
                }
			
                //清空
                _mAddTimerList.Clear();
            }
		
            if (_mTimerList.Count > 0)
            {
                //响应定时器
                foreach (Timer timer in _mTimerList.Values)
                {
                    if (timer == null)
                    {
                        return;
                    }
				
                    timer.Run();
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 增加定时器
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public bool AddTimer(string key, float duration, TimerManagerHandler handler)
        {
            return Internal_AddTimer(key, TIMER_MODE.Normal, duration, handler);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 增加持续定时器
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public bool AddTimerRepeat(string key, float duration, TimerManagerHandler handler)
        {
            return Internal_AddTimer(key, TIMER_MODE.Repeat, duration, handler);
        }

        public bool AddTimer(string key, float duration, TimerManagerHandlerArgs handler, params object[] args)
        {
            return Internal_AddTimer(key, TIMER_MODE.Normal, duration, handler, args);
        }

        public bool AddTimerRepeat(string key, float duration, TimerManagerHandlerArgs handler, params object[] args)
        {
            return Internal_AddTimer(key, TIMER_MODE.Repeat, duration, handler, args);
        }

        /// <summary>
        /// 暂停带有前缀的所有定时器
        /// </summary>
        /// <param name="prefix"></param>
        public void BreakTimerWithPrefix(string prefix)
        {
            if (_mTimerList != null && _mTimerList.Count > 0)
            {
                string[] arr = new string[_mTimerList.Count];
                _mTimerList.Keys.CopyTo(arr, 0);
			
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].StartsWith(prefix))
                    {
                        BreakTimer(arr[i]);
                    } 
                }
            }
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public void BreakTimer(string key)
        {
            if (!_mTimerList.ContainsKey(key))
            {
                return;
            }
		
            Timer timer = _mTimerList[key];
            timer.Break();
        }

        /// <summary>
        /// 重启带有前缀的所有定时器
        /// </summary>
        /// <param name="prefix"></param>
        public void ResumeTimerWithPrefix(string prefix)
        {
            if (_mTimerList != null && _mTimerList.Count > 0)
            {
                string[] arr = new string[_mTimerList.Count];
                _mTimerList.Keys.CopyTo(arr, 0);
			
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].StartsWith(prefix))
                    {
                        ResumeTimer(arr[i]);
                    } 
                }
            }
        }

	
        /// <summary>
        /// 重启计时器
        /// </summary>
        public void ResumeTimer(string key)
        {
            if (!_mTimerList.ContainsKey(key))
            {
                return;
            }
		
            Timer timer = _mTimerList[key];
            timer.Resume();
        }

        /// <summary>
        /// 销毁带有前缀的所有定时器
        /// </summary>
        /// <param name="prefix"></param>
        public void ClearTimerWithPrefix(string prefix)
        {
            if (_mTimerList != null && _mTimerList.Count > 0)
            {
                foreach (string timerKey in _mTimerList.Keys)
                {
                    if (timerKey.StartsWith(prefix))
                    {
                        Destroy(timerKey);
                    } 
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 销毁指定定时器
        /// </summary>
        /// <param name="key">标识符</param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public bool Destroy(string key)
        {
            if (!_mTimerList.ContainsKey(key))
            {
                return false;
            }
		
            if (!_mDestroyTimerList.Contains(key))
            {
                _mDestroyTimerList.Add(key);
            }
		
            return true;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 增加定时器
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        private bool Internal_AddTimer(string key, TIMER_MODE mode, float duration, TimerManagerHandler handler)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
		
            if (duration < 0.0f)
            {
                return false;
            }
		
            Timer timer = new Timer(key, mode, Time.time, duration, handler, this);
		
            if (_mAddTimerList.ContainsKey(key))
            {
                _mAddTimerList[key] = timer;
            }
            else
            {
                _mAddTimerList.Add(key, timer);
            }
		
            return true;
        }

        private bool Internal_AddTimer(string key, TIMER_MODE mode, float duration, TimerManagerHandlerArgs handler, params object[] args)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
		
            if (duration < 0.0f)
            {
                return false;
            }
		
            Timer timer = new Timer(key, mode, Time.time, duration, handler, this, args);
		
            if (_mAddTimerList.ContainsKey(key))
            {
                _mAddTimerList[key] = timer;
            }
            else
            {
                _mAddTimerList.Add(key, timer);
            }
		
            return true;
        }

        public bool IsRunning(string key)
        {
            return _mTimerList.ContainsKey(key);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///  定时器模式
        /// </summary>
        /// -----------------------------------------------------------------------------
        private enum TIMER_MODE
        {
            Normal,
            Repeat,
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 获取指定定时器剩余时间
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public float GetTimerLeft(string key)
        {
            if (!_mTimerList.ContainsKey(key))
            {
                return 0.0f;
            }
		
            Timer timer = _mTimerList[key];
            return timer.TimeLeft;
        }

        /// <summary>
        /// 获取带有前缀的定时器剩余时间
        /// </summary>
        /// <param name="prefix"></param>
        public float GetTimerLeftWithPrefix(string prefix)
        {
            if (_mTimerList != null && _mTimerList.Count > 0)
            {
                string[] arr = new string[_mTimerList.Count];
                _mTimerList.Keys.CopyTo(arr, 0);
			
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].StartsWith(prefix))
                    {
                        return GetTimerLeft(arr[i]);
                    } 
                }
            }

            return 0.0f;
        }

        private class Timer
        {
            /// <summary>
            ///   名称
            /// </summary>
            private readonly string _mName;
		
            /// <summary>
            ///   模式
            /// </summary>
            private readonly TIMER_MODE _mMode;
		
            /// <summary>
            ///   开始时间
            /// </summary>
            private float _mStartTime;
		
            /// <summary>
            ///   时长
            /// </summary>
            private readonly float _mDuration;

            /// <summary>
            ///  中断
            /// </summary>
            private bool _mBreak = false;

            /// <summary>
            ///  中断开始时间
            /// </summary>
            private float _mBreakStart;

            /// <summary>
            ///  中断开始时间
            /// </summary>
            private float _mBreakDuration = 0;

            /// <summary>
            ///   定时器委托
            /// </summary>
            private readonly TimerManagerHandler _mTimerEvent;
		
            private readonly TimerManagerHandlerArgs _mTimerArgsEvent;
		
            private readonly TimerManager _mManger;
		
            private readonly object[] _mArgs = null;

            /// -----------------------------------------------------------------------------
            /// <summary>
            /// 开始时间
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            /// -----------------------------------------------------------------------------
            public float StartTime
            {
                get
                {
                    return _mStartTime;
                }
                set
                {
                    _mStartTime = value;
                }
            }

            /// -----------------------------------------------------------------------------
            /// <summary>
            /// 剩余时间
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            /// -----------------------------------------------------------------------------
            public float TimeLeft
            {
                get
                {
                    return Mathf.Max(0.0f, _mDuration - (Time.time - _mStartTime) + _mBreakDuration);
                }
            }

            /// -----------------------------------------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            /// -----------------------------------------------------------------------------
            public Timer(string name, TIMER_MODE mode, float startTime, float duration, TimerManagerHandler handler, TimerManager manager)
            {
                _mName = name;
                _mMode = mode;
                _mStartTime = startTime;
                _mDuration = duration;
                _mTimerEvent = handler;
                _mManger = manager;
            }

            public Timer(string name, TIMER_MODE mode, float startTime, float duration, TimerManagerHandlerArgs handler, TimerManager manager, params object[] args)
            {
                _mName = name;
                _mMode = mode;
                _mStartTime = startTime;
                _mDuration = duration;
                _mTimerArgsEvent = handler;
                _mManger = manager;
                _mArgs = args;
            }

            /// -----------------------------------------------------------------------------
            /// <summary>
            /// 运行事件
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            /// -----------------------------------------------------------------------------
            public void Run()
            {
                if (_mBreak)
                {
                    return;
                }

                if (this.TimeLeft > 0.0f)
                {
                    return;
                }
			
                if (this._mTimerEvent != null)
                {
                    this._mTimerEvent();
                }
			
                if (this._mTimerArgsEvent != null)
                {
                    this._mTimerArgsEvent(_mArgs);
                }
			
                if (_mMode == TIMER_MODE.Normal)
                {
                    _mManger.Destroy(this._mName);
                }
                else
                {
                    _mStartTime = Time.time;
                    _mBreakDuration = 0;
                }
                return;
            }

            public void Break()
            {
                if (_mBreak)
                {
                    return;
                }

                _mBreak = true;
                _mBreakStart = Time.time;
            }

            public void Resume()
            {
                if (!_mBreak)
                {
                    return;
                }

                _mBreakDuration += (Time.time - _mBreakStart);
                _mBreak = false;
            }
        }
    }
}