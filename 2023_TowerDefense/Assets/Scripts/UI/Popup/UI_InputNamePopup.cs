using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InputNamePopup : UI_Popup
{
    enum Texts
    {
        NameText
    }

    enum GameObjects
    {
        Content
    }

    string _nameText = "";
    bool _isCompleted;
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<GameObject>(typeof(GameObjects));
        BindText(typeof(Texts));

        int start = 'A';
        int end = 'Z';
        for (int i = start; i <= end; i++)
        {
            char c = (char)i;
            UI_InputName_Subitem subitem = Managers.UI.MakeSubitemUI<UI_InputName_Subitem>(Get<GameObject>((int)GameObjects.Content).transform);
            subitem.gameObject.BindEvent((evt) =>
            {
                Managers.Sound.Play("Interaction/ButtonClick");
                _nameText += c; 
            }, Define.UIEvent.Click);
            subitem.SetInfo(c);
        }

        return true;
    }

    private void Update()
    {
        if (_isCompleted)
            return;

        GetText((int)Texts.NameText).text = _nameText;

        if (_nameText.Length == 3)
            OnCompletedInputName();
    }

    void OnCompletedInputName()
    {
        Managers.Data.Ranks.Add(new Data.RankData(_nameText, Managers.Game.CurrentScore));
        Managers.Scene.LoadAsync(Define.SceneType.Menu);
        _isCompleted = true;
    }
}
