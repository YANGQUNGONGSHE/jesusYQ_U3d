using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class FamousPersonCell : BaseCell<FamousPersonModel>
{


    private Text _mName;
    private CircleRawImage _mHeadImage;
    protected override void OnAwake()
    {
        _mName = transform.Find("Name").GetComponent<Text>();
        _mHeadImage = transform.Find("HeadImage").GetComponent<CircleRawImage>();
    }

    public override void InitUi(int index, FamousPersonModel t, Action<int, FamousPersonModel> onCellClickCallback = null, Action<int, FamousPersonModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        SetDefaultUi();
    }
    private void SetDefaultUi()
    {
        _mName.text = t.Name;
        if (t.HTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.HeadImageUrl))
            {
                HttpManager.RequestImage(t.HeadImageUrl+LoadPicStyle.ThumbnailHead, headTexture2D =>
                {
                    if (headTexture2D)
                    {
                        t.HTexture2D = headTexture2D;
                        _mHeadImage.texture = headTexture2D;
                    }
                });
            }
            else
            {
                _mHeadImage.texture = DefaultImage.Head;
            }
        }
        else
        {
            _mHeadImage.texture = t.HTexture2D;
        }
        
    }
}
