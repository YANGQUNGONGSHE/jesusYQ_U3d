using NIMAudio;
using System;

public interface IAudioService
{
    /// <summary>
    /// NIM SDK 播放,通过回调获取开始播放状态。android平台需在主线程调用
    /// </summary>
    /// <param name="filePath">播放文件绝对路径</param>
    /// <param name="audioFormat">播放音频格式，AAC : 0， AMR : 1</param>
    /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
    bool PlayAudio(string filePath, NIMAudioType audioFormat);

    /// <summary>
    /// NIM SDK 停止播放,通过回调获取停止播放状态
    /// </summary>
    /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
    bool StopPlayAudio();

    /// <summary>
    /// 录制语音 android平台需在主线程调用
    /// </summary>
    /// <param name="audio_format">音频格式，AAC : 0， AMR : 1</param>
    /// <param name="volume">音量(0 - 255, 默认180) pc有效</param>
    /// <param name="loudness">默认0   pc有效</param>
    /// <param name="capture_device">capture_device 录音设备 pc有效</param>
    /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
    bool StartCaptureAudio(int audio_format, int volume, int loudness, string capture_device);
    
    /// <summary>
    /// 停止录制语音
    /// </summary>
    /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
    bool StopCaptureAudio();

    /// <summary>
    /// 取消录制并删除临时文件
    /// </summary>
    /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>时文件
    /// </summary>
    bool CancelCaptureAudio();

    /// <summary>
    /// 获取采集时间，采集时间由所注册的回调返回
    /// </summary>
    /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
    bool GetAudioCaptureTime();

    /// <summary>
    /// 获取播放文件的时长
    /// </summary>
    /// <returns>播放文件的时长，异常为-1</returns>
    int GetPlayTime();

    /// <summary>
    /// 获取播放时间，播放时间由所注册的回调返回
    /// </summary>
    /// <returns><c>true</c> 调用成功, <c>false</c> 调用失败</returns>
    bool GetPlayCurrentPosition();

    /// <summary>
    ///  设置扬声器 ios,android有效
    /// </summary>
    /// <param name="speaker">true:扬声器开启.false:扬声器关闭</param>
    /// <param name="context">当前上下文，android 必须.ios无效</param>
    //void SetPlaySpeaker(bool speaker, IntPtr context);

    /// <summary>
    /// 获取扬声器状态 ios，android有效
    /// </summary>
    /// <param name="context">当前上下文，android 必须.ios无效</param>
    /// <returns><c>true</c> 扬声器开启 <c>false</c> 扬声器关闭</returns>
    //GetPlaySpeaker(IntPtr context);
}