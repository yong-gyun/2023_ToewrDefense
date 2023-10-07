using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Rank_Subitem : UI_Base
{
    enum Texts
    {
        ScoreText,
        NameText,
        RankText
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindText(typeof(Texts));
        return true;
    }

    public void SetRank(int rank, string name, int score)
    {
        if (rank > 3)
            GetText((int)Texts.RankText).text = $"{rank}";

        GetText((int)Texts.NameText).text = $"{name}";
        GetText((int)Texts.ScoreText).text = $"{score}";
    }
}
