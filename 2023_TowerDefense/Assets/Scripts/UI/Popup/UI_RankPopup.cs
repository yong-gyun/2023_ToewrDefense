using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RankPopup : UI_ShowRank
{
    enum Buttons
    {
        CloseButton
    }

    protected override bool Init()
    {   
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent((evt) => 
        {
            Managers.Sound.Play("Interaction/ButtonClick"); 
            ClosePopupUI(); 
        }, Define.UIEvent.Click);
        return true;
    }
}