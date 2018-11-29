using NIMAudio;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class AudioOptionCommand : EventCommand
{
    [Inject]
    public IAudioService AudioService { get; set; }

    public override void Execute()
    {
		Log.I ("Execute");
        var arg = (ArgAudioOptionParam) evt.data;
        switch (arg.Option)
        {
            case EAudioOption.BeginRecord:
                BeginRecord();
                break;

            case EAudioOption.EndRecord:
                EndRecord();
                break;

            case EAudioOption.PlayAudio:
                PlayAudio(arg.AudioPath);
                break;

            case EAudioOption.StopPlayAudio:
                StopPlayAudio();
                break;

            case EAudioOption.CancelRecord:
                CancelRecord();
                break;
        }
    }

    private void BeginRecord()
    {
        #if UNITY_ANDROID
         WongJJ.Game.Core.Dispatcher.InvokeAsync(BeginRecordOnMainThread);
        #else
         AudioService.StartCaptureAudio(0, 180, 0, "0");
        #endif
    }

    private void BeginRecordOnMainThread()
    {
        AudioService.StartCaptureAudio(0, 180, 0, "0");
    }

    private void EndRecord()
    {
        AudioService.StopCaptureAudio();
    }

    private void CancelRecord()
    {
        AudioService.CancelCaptureAudio();
    }

    private void PlayAudio(string audioPath)
    {
        #if UNITY_ANDROID
         WongJJ.Game.Core.Dispatcher.InvokeAsync(PlayAudioOnMainThread, audioPath);
        #else
        AudioService.PlayAudio(audioPath, 0);
        #endif
    }

    private void PlayAudioOnMainThread(string audioPath)
    {
        AudioService.PlayAudio(audioPath, 0);
    }

    private void StopPlayAudio()
    {
        AudioService.StopPlayAudio();
    }
}

public struct ArgAudioOptionParam
{
    public EAudioOption Option;
    public string AudioPath;
}

public enum EAudioOption
{
    BeginRecord,
    EndRecord,
    CancelRecord,
    PlayAudio,
    StopPlayAudio,
}