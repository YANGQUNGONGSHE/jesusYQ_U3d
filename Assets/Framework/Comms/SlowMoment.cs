using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WongJJ.Game.Core
{
    public class SlowMoment : KeepSingletion<SlowMoment>
    {
        private bool m_IsSacleing = false;
        private float m_EndScaleTime = 0f;
        private Action m_FinishCallbacl;

        /// <summary>
        /// 时间缩放
        /// </summary>
        /// <param name="timeScale">缩放比例 大于1 快 小于1 慢</param>
        /// <param name="continueTime">持续时间</param>
        public void TimeScale(float timeScale, float continueTime,Action callback = null)
        {
            m_IsSacleing = true;
            Time.timeScale = timeScale;
            m_FinishCallbacl = callback;
            m_EndScaleTime = Time.realtimeSinceStartup + continueTime;
        }

        void Update()
        {
            if (m_IsSacleing)
            {
                if (Time.realtimeSinceStartup > m_EndScaleTime)
                {
                    Time.timeScale = 1;
                    m_IsSacleing = false;
                    if (m_FinishCallbacl != null)
                        m_FinishCallbacl();
                }
            }
        }
    }
}