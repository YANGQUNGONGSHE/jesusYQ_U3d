using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class EditorUserDataOptionCommand : EventCommand {

    [Inject]
    public IAccountService AccountService { get; set; }

    [Inject]
    public UserModel UserModel { get; set; }

    private EditorUserData _mEditorUserData;

    public override void Execute()
    {
        
        Retain();
        _mEditorUserData = (EditorUserData)evt.data;

        switch (_mEditorUserData.Option)
        {
            case EditorOption.EditorHead:

                EditorHead(_mEditorUserData.Texture);
                break;
            case EditorOption.EditorDisplayName:
                EditDisplayName(_mEditorUserData.Content);
                break;
            case EditorOption.EditorSignature:
                EditorSignature(_mEditorUserData.Content);
                break;
            case EditorOption.EditorLocal:
                EditorLocal(_mEditorUserData.LocalStrings[0], _mEditorUserData.LocalStrings[1], _mEditorUserData.LocalStrings[2]);
                break;
            case EditorOption.EditorGender:
                EditorGender(_mEditorUserData.Content);
                break;
            case EditorOption.EditorBirthday:
                EditorBirthday(_mEditorUserData.Content);
                break;
        }
    }


    private void EditorHead(Texture2D  headTexture2D)
    {
        AccountService.ReqUpdateUserHead(headTexture2D, (b, s) =>
        {
            if (b)
            {
                UpdateAccountData();
            }
            else
            {
                if (_mEditorUserData.IsNewCreateed)
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.ReqFirstSetFail,s);
                }
                else
                {
                    UIUtil.Instance.ShowFailToast(s);
                }
            }

        });
    }

    private void EditDisplayName(string diaplayName)
    {
        AccountService.RequestUpdateDisplayName(diaplayName, (b, s) =>
        {
            if (b)
            {
                UpdateAccountData();
            }
            else
            {
                if (_mEditorUserData.IsNewCreateed)
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.ReqFirstSetFail,s);
                }
                else
                {
                    UIUtil.Instance.ShowFailToast(s);
                }
            }
        } );
        
    }

    private void EditorSignature(string signature)
    {
        
        AccountService.ReqUpdateSignature(signature, (b, s) =>
        {
            if (b)
            {
                UpdateAccountData();
            }
            else
            {
                UIUtil.Instance.ShowFailToast(s);
            }
        });
    }

    private void EditorLocal(string country,string state,string city )
    {
        AccountService.ReqUpdateLocal(country,state,city, (b, s) =>
        {
            if (b)
            {
                UpdateAccountData();
            }
            else
            {
                UIUtil.Instance.ShowFailToast(s);
            }
        } );
    }

    private void EditorBirthday(string birthdayDate)
    {
        AccountService.ReqUpdateBirthday(birthdayDate, (b, s) =>
        {
            if (b)
            {
                UpdateAccountData();
            }
            else
            {
                UIUtil.Instance.ShowFailToast(s);
            }
        } );
    }

    private void EditorGender(string gender)
    {
        AccountService.ReqUpdateGender(gender, (b, s) =>
        {
            if (b)
            {
               UpdateAccountData(); 
            }
            else
            {
                UIUtil.Instance.ShowFailToast(s);
            }
        });
    }

    private void UpdateAccountData()
    {
        AccountService.QueryAccountInfo((b, info) =>
        {
            Release();
            if (b)
            {
                UserModel.User = info.Account;
                LocalDataManager.Instance.SaveJsonObj(LocalDataObjKey.USER, info.Account);
                if (_mEditorUserData.Option == EditorOption.EditorHead)
                {
                   dispatcher.Dispatch(CmdEvent.Command.LoadAccountHeadT2D, UserModel.User.AvatarUrl);
                }
                if (_mEditorUserData.IsNewCreateed)
                {
                    dispatcher.Dispatch(_mEditorUserData.Option == EditorOption.EditorHead
                        ? CmdEvent.ViewEvent.ReqFirstSetHeadFinish
                        : CmdEvent.ViewEvent.ReqFirstSetDyNameFinish);
                }
                else
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.EditorAccountDataOptionFinish);
                }
            }
            else
            {
                if (_mEditorUserData.IsNewCreateed)
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.ReqFirstSetFail,info.ResponseStatus.Message);
                }
                else
                {
                    UIUtil.Instance.ShowFailToast(info.ResponseStatus.Message);
                }
            }
        });
    }
}

public class EditorUserData
{
    /// <summary>
    /// 编辑类型
    /// </summary>
    public EditorOption Option;
    /// <summary>
    /// 修改的内容
    /// </summary>
    public string Content;
    /// <summary>
    /// 修改头像Texture2D
    /// </summary>
    public Texture2D Texture;
    /// <summary>
    /// 修改所属地的内容
    /// </summary>
    public string[] LocalStrings;
    /// <summary>
    /// 是否是第一次登陆修改
    /// </summary>
    public bool IsNewCreateed;
}

public enum EditorOption
{
    EditorHead,
    EditorDisplayName,
    EditorSignature,
    EditorLocal,
    EditorBirthday,
    EditorGender,
}
