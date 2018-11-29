using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using WongJJ.Game.Core;

public class CommUtil : KeepSingletion<CommUtil>
{
    public const int EveryMsgShowTimeInterval = 1;

    private string mDianxin = @"^1[3578][01379]\d{8}$";

    private string mYidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";

    private string mLiantong = @"^1[34578][01256]\d{8}$";

    public string BuildImLocalExtension(ImLocalExtension ext)
    {
        if (ext == null) return "";

        StringBuilder sb = new StringBuilder();
        sb.Append(ext.MsgType.ToString());
        sb.Append('|');
        sb.Append(ext.UniqueId);
        sb.Append('|');
        return sb.ToString();
    }
    
    public ImLocalExtension ParseImLocalExtension(string ext)
    {
        if (string.IsNullOrEmpty(ext)) return null;
        string[] arr = ext.Split('|');
        var localExt = new ImLocalExtension
        {
            MsgType = (ImLocalExtension.EMsgType) Enum.Parse(typeof(ImLocalExtension.EMsgType), arr[0]),
            UniqueId = arr[1]
        };
        return localExt;
    }

    public string FormatTime2CostomData(DateTime orgTime)
    {
        DateTime now = DateTime.Now;
        int dayDiff , hoursDiff, minutesDiff;
        StringBuilder sb = new StringBuilder();
        TimeSpan ts1 = new TimeSpan(orgTime.Ticks);
        TimeSpan ts2 = new TimeSpan(now.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        dayDiff = ts.Days;
        hoursDiff = ts.Hours;
        minutesDiff = ts.Minutes;
        
        if (dayDiff < 1)
        {
            sb.Append(orgTime.ToShortTimeString());
        }
        else if (dayDiff >= 1 && dayDiff < 2)
        {
            sb.Append("昨天 ");
            sb.Append(orgTime.ToShortTimeString());
        }
        else if (dayDiff >= 2 && dayDiff < 3)
        {
            sb.Append("前天 ");
            sb.Append(orgTime.ToShortTimeString());
        }
        else
        {
            if (orgTime.Year != now.Year)
            {
                sb.Append(orgTime.ToShortDateString());
            }
            sb.Append(orgTime.Month.ToString());
            sb.Append("-");
            sb.Append(orgTime.Day.ToString());
            sb.Append(" ");
            sb.Append(orgTime.ToShortTimeString());
        }
        return sb.ToString();
    }
    
    public bool NeedToShowSendTime(long closerTime, long farAwayTime)
    {
        DateTime d1 = NimUtility.DateTimeConvert.FromTimetag(closerTime);
        DateTime d2 = NimUtility.DateTimeConvert.FromTimetag(farAwayTime);
        TimeSpan ts1 = new TimeSpan(d1.Ticks);
        TimeSpan ts2 = new TimeSpan(d2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        return ts.Minutes >= EveryMsgShowTimeInterval;
    }
    
    /// <summary>
    /// 电信手机号码正则
    /// </summary>
    /// <returns></returns>
    public Regex DxRegex()
    {
        return new Regex(mDianxin);
    }
    
    /// <summary>
    /// 移动手机号码正则
    /// </summary>
    /// <returns></returns>
    public Regex YdRegex()
    {
        return  new Regex(mYidong);
    }
    
    /// <summary>
    /// 联通手机号码正则
    /// </summary>
    /// <returns></returns>
    public Regex LtRegex()
    {
        return new Regex(mLiantong);
    }
}

public class ImLocalExtension
{
    public enum EMsgType
    {
        Text,
        Image,
        Audio
    }

    public EMsgType MsgType;
    public string UniqueId;
}

public class StopCaptureCbParam
{
    public int ResCode;
    public string FilePath;
    public string FileExt;
    public int FileSize;
    public int AudioDuration;
}

public struct ArgAutoDownloadSucc
{
    /// <summary>
    /// 返回会话ID
    /// </summary>
    public string RetSessionId;
    /// <summary>
    /// 返回消息ID
    /// </summary>
    public string RetMsgId;
}

public struct ArgSelectedMember
{
    public bool IsSelected;
    public string MemberUid;
}

public struct ArgSelectedTeamMember
{
    public SysTemModel SysTemModel;
    public bool IsSelected;
    public string Tid;
    public string Uid;
}