using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Scene : BaseScene
{
    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        Cheat.IsEnemyMovealbe = true;
        Cheat.IsTowerAttackable = true;
        Managers.Game.CurrentStage = 1;
        SceneType = Define.SceneType.Stage1;
        Managers.Object.IninMap();
        Managers.UI.ShowSceneUI<UI_Main>();
        Managers.UI.ShowPopupUI<UI_NextWavePopup>();

        GameObject go = GameObject.Find("@MinimapCamera");

        if(go == null) 
            Managers.Resource.Instantiate("Camera/@MinimapCamera");

        Managers.Game.CurrentStage = 1;
        Camera.main.gameObject.GetOrAddComponent<CameraController>();
        gameObject.GetOrAddComponent<Cheat>();
        return true;
    }

    private void Update()
    {
        if(Managers.Object.SpawnPool.IsStarted)
            Managers.Game.CurrentTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
            Time.timeScale = 0.25f;
    }
}