using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class BottomBarCommand : EventCommand
{
    public override void Execute()
    {
        var uid = (UiId)evt.data;
        iocViewManager.CloseCurrentOpenNew((int)uid);
    }
}
