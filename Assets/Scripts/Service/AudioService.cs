using NIMAudio;
using System;
using System.Collections.Generic;
using WongJJ.Game.Core;

public class AudioService : IAudioService
{
    public AudioService()
    {
        RegisterCallback();
    }

    public void RegisterCallback()
    {
        //注册->语音开始播放
        AudioAPI.RegStartPlayCb((int resCode, string filePath) =>
        {
            Log.I("开始播放语音，状态:" + resCode + " ,路径：" + filePath);
        });

        //注册->语音停止播放（人为）
        AudioAPI.RegStopPlayCb((int resCode, string filePath) =>
        {
            Log.I("停止（取消）语音，状态:" + resCode + " ,路径：" + filePath);
        });

        //注册->语音播放（结束）
        AudioAPI.RegPlayEndCb((int resCode, string filePath) =>
        {
            Log.I("播放语音结束，状态:" + resCode + " ,路径：" + filePath);
        });

        //注册-> 开始采集语音
        AudioAPI.RegStartCaptureCb((int resCode) =>
        {
            Log.I("开始采集语音，状态:" + resCode);
        });

        //注册-> 停止采集语音
        AudioAPI.RegStopCaptureCb((int resCode, string file_path, string file_ext, int file_size, int audio_duration) =>
        {
            Log.I("结束采集语音，状态:" + resCode + " ,路径：" + file_path + " ,后缀名：" + file_ext + " ,大小:" + file_size + " ,时长:" + audio_duration);
            NotificationCenter.DefaultCenter().PostNotification(NotifiyName.OnStopCaptureCb, this,
                new StopCaptureCbParam() { ResCode = resCode, FilePath = file_path, FileExt = file_ext, FileSize = file_size, AudioDuration = audio_duration });
        });

        //注册-> 取消采集语音
        AudioAPI.RegCancelCaptureCb((int resCode) =>
        {
            Log.I("取消录制，你不想录了啊？" + "状态: " + resCode );
        });

        //注册-> 获取当前语音录制时间
        AudioAPI.RegGetCaptureTimeCb((int resCode) =>
        {
            Log.I("正在录制, 时间： " + resCode);
            if (resCode > 59)
            {
                StopCaptureAudio();
            }
        });

        //注册-> 获取当前语音播放时间
        AudioAPI.RegGetPlayCurrentPositionCb((int resCode) =>
        {
            Log.I("正在播放, resCode是什么？看看---->>>>>>： " + resCode);
        });

        //注册-> 获取录制设备
        AudioAPI.RegGetCaptureDevices((int resCode, List<string> devices) =>
        {
            for (int i = 0; i < devices.Count; i++)
            {
                Log.I("-----:" + devices[i]);
            }
        });

    }

    public bool PlayAudio(string filePath, NIMAudioType audioFormat)
    {
        return AudioAPI.PlayAudio(filePath, audioFormat);
    }

    public bool StopPlayAudio()
    {
        return AudioAPI.StopPlayAudio();
    }

    public bool StartCaptureAudio(int audio_format, int volume, int loudness, string capture_device)
    {
        return AudioAPI.StartCapture(audio_format, volume, loudness, capture_device);
    }

    public bool StopCaptureAudio()
    {
        return AudioAPI.StopCapture();
    }

    public bool CancelCaptureAudio()
    {
        return AudioAPI.CancelCapture();
    }

    public bool GetAudioCaptureTime()
    {
        return AudioAPI.GetCaptureTime();
    }

    public bool GetPlayCurrentPosition()
    {
        return AudioAPI.GetPlayCurrentPosition();
    }

    public int GetPlayTime()
    {
        return AudioAPI.GetPlayTime();
    }

    public bool GetCaptureDevices()
    {
        return AudioAPI.GetCaptureDevices();
    }

}