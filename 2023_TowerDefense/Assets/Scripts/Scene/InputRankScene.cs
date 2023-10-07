using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRankScene : BaseScene
{
    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        GameObject go = GameObject.Find("Env");
        SceneType = Define.SceneType.InputRank;
        BuildGrid[] buildGrids = go.GetComponentsInChildren<BuildGrid>();
        foreach (BuildGrid grid in buildGrids)
            grid.OnActive(false);

        Managers.UI.ShowPopupUI<UI_ShowRank>();
        Managers.UI.ShowPopupUI<UI_InputNamePopup>();

        int count = Managers.Data.Ranks.Count;

        if (count < 10)
        {
            if (count - 1 < 0 || Managers.Data.Ranks[count - 1].score < Managers.Game.CurrentScore)
                Managers.UI.ShowPopupUI<UI_InputNamePopup>();
        }
        else
        {
            if (Managers.Data.Ranks[9].score < Managers.Game.CurrentScore)
                Managers.UI.ShowPopupUI<UI_InputNamePopup>();
        }
        return true;
    }
}
