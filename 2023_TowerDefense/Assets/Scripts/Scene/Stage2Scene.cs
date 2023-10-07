using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Scene : BaseScene
{
    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        Cheat.IsEnemyMovealbe = true;
        Cheat.IsTowerAttackable = true;
        SceneType = Define.SceneType.Stage2;
        Managers.Game.CurrentStage = 2;
        Managers.UI.ShowSceneUI<UI_Main>();
        Managers.UI.ShowPopupUI<UI_NextWavePopup>();
        Managers.Object.IninMap();

        GameObject go = GameObject.Find("@MinimapCamera");

        if(go == null) 
            Managers.Resource.Instantiate("Camera/@MinimapCamera");
        
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetSize(-26.5f, 30.25f, -19.5f, 13.5f);
        gameObject.GetOrAddComponent<Cheat>();
        return true;
    }

    private void Update()
    {
        if(Managers.Object.SpawnPool.IsStarted)
            Managers.Game.CurrentTime += Time.deltaTime;
    }
}