using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UsedAlertContent : UI_Base
{
    enum Texts
    {
        AlertText
    }

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;
        BindText(typeof(Texts));
        return true;
    }

    public void SetInfo(string description)
    {
        GetText((int)Texts.AlertText).text = description;
        Managers.Resource.Destory(gameObject, 1.5f);
    }
}
