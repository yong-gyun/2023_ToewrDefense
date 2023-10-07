using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        GameObject go = GameObject.Find("Env");

        BuildGrid[] buildGrids = go.GetComponentsInChildren<BuildGrid>();
        foreach (BuildGrid grid in buildGrids)
            grid.OnActive(false);

        Managers.UI.ShowSceneUI<UI_Menu>();
        Managers.Sound.Play("Menu", Define.Sound.Bgm);
        return true;
    }
}
