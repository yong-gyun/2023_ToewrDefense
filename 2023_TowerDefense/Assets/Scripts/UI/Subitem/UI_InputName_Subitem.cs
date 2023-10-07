using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InputName_Subitem : UI_Base
{
    enum Texts
    {
        Text
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        return true;
    }

    public void SetInfo(char c)
    {
        GetText((int)Texts.Text).text = $"{c}";
    }
}
