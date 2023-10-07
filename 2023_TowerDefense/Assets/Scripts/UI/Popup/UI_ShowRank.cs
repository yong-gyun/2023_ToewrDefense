using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShowRank : UI_Popup
{
    enum GameObjects
    {
        ThreeTopRankContent,
        RankContent,
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<GameObject>(typeof(GameObjects));

        for (int i = 0; i < Get<GameObject>((int)GameObjects.ThreeTopRankContent).transform.childCount; i++)
        {
            if (i == Managers.Data.Ranks.Count)
                break;

            UI_Rank_Subitem subitem = Get<GameObject>((int)GameObjects.ThreeTopRankContent).transform.GetChild(i).GetComponent<UI_Rank_Subitem>();
            subitem.SetRank(i + 1, Managers.Data.Ranks[i].name, Managers.Data.Ranks[i].score);
        }

        for (int i = 3; i < Managers.Data.Ranks.Count; i++)
        {
            if (i == 10)
                break;

            UI_Rank_Subitem subitem = Managers.UI.MakeSubitemUI<UI_Rank_Subitem>(Get<GameObject>((int)GameObjects.RankContent).transform);
            subitem.SetRank(i + 1, Managers.Data.Ranks[i].name, Managers.Data.Ranks[i].score);
        }
        return true;
    }
}