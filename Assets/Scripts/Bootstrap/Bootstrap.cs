using NIM;
using NIMAudio;
using strange.extensions.context.impl;

public class Bootstrap : ContextView
{
    [Inject]
    public IImService ImService { get; set; }

    void Start () 
    {
        context = new MainMVCS(this, true);
        context.Start();
    }

    private void OnApplicationQuit()
    {
        AudioAPI.UninitModule();
        ImService.LogoutIm(NIMLogoutType.kNIMLogoutAppExit, (result) =>
        {
            ClientAPI.Cleanup();
        });
    }
}
