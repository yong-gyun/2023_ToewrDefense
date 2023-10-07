using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TowerController SelectedTower { get; private set; }
    UI_TowerHud _hud;
    List<Vector3> _savePositions = new List<Vector3>();
    UI_Pause _puaseUI = null;
    int _moveProtectedBaseIdx = 0;
    int _patrolUnitIdx = 0;
    int _savePosIdx = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Managers.Object.IsBuild == false)
        {
            if (_puaseUI == null)
            {
                _puaseUI = Managers.UI.ShowPopupUI<UI_Pause>();
                Debug.Log("Check");
            }
        }

        if(Input.GetMouseButtonDown(0) && Managers.Object.IsBuild == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                TowerController tc = hit.transform.GetComponent<TowerController>();

                if (tc != null)
                {
                    if(SelectedTower != null)
                    {
                        if (SelectedTower.Type != Define.TowerType.ProtectedTower && SelectedTower.Type != Define.TowerType.LastProtectedTower)
                            SelectedTower.AttackRangeViewer.SetActive(false);
                        
                        if (_hud != null)
                            _hud.ClosePopupUI();
                    }

                    SelectedTower = tc;
                    
                    if(_hud == null)
                    {
                        _hud = Managers.UI.ShowPopupUI<UI_TowerHud>();
                        _hud.SetInfo(SelectedTower);
                    }
                    else
                    {
                        _hud.SetInfo(SelectedTower);
                    }

                    if(SelectedTower != null)
                    {
                        if (tc.Type != Define.TowerType.ProtectedTower && SelectedTower.Type != Define.TowerType.LastProtectedTower)
                        {
                            if(SelectedTower.IsStart)
                                SelectedTower.AttackRangeViewer.SetActive(true);
                        }
                    }    
                }
                else
                {
                    if(SelectedTower != null)
                    {
                        if (SelectedTower.Type != Define.TowerType.ProtectedTower && SelectedTower.Type != Define.TowerType.LastProtectedTower)
                            SelectedTower.AttackRangeViewer.SetActive(false);
                        SelectedTower = null;

                        if (_hud != null)
                        {
                            _hud.ClosePopupUI();
                        }
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnMoveLastProtectedBase();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnMoveProtectedBase();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnMovePatrolUnitPos();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnMoveSavePos();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            _savePositions.Add(Camera.main.transform.position);
        }
    }

    void OnMoveLastProtectedBase()
    {
        if (Managers.Object.LastProtectedTower != null)
        {
            Vector3 lastProtectedBasePos = Managers.Object.LastProtectedTower.transform.position;
            Vector3 pos = new Vector3(lastProtectedBasePos.x, Camera.main.transform.position.y, Managers.Object.LastProtectedTower.transform.position.z);
            Camera.main.transform.position = pos;
        }
    }

    void OnMoveProtectedBase()
    {
        Vector3 protectedBasePos = Managers.Object.ProtectedTowers[_moveProtectedBaseIdx++ % Managers.Object.ProtectedTowers.Count].transform.position;
        Vector3 pos = new Vector3(protectedBasePos.x, Camera.main.transform.position.y, protectedBasePos.z);
        Camera.main.transform.position = pos;
    }

    void OnMovePatrolUnitPos()
    {
        if(Managers.Object.PatrolUnits.Count > 0)
        {
            Vector3 patrolUnitPos = Managers.Object.PatrolUnits[_patrolUnitIdx++ % Managers.Object.PatrolUnits.Count].transform.position;
            Vector3 pos = new Vector3(patrolUnitPos.x, Camera.main.transform.position.y, patrolUnitPos.z);
            Camera.main.transform.position = pos;
        }
    }

    void OnMoveSavePos()
    {
        if (_savePositions.Count > 0)
        {
            Vector3 savePos = _savePositions[_savePosIdx++ % _savePositions.Count];
            Vector3 pos = new Vector3(savePos.x, Camera.main.transform.position.y, savePos.z);
            Camera.main.transform.position = pos;
        }
        else
        {
            OnMoveLastProtectedBase();
        }
    }
}