using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBuildSpaceController : MonoBehaviour
{
    [SerializeField] List<Material> _previewerMats = new List<Material>();
    [SerializeField] List<BuildGrid> _touchtedList = new List<BuildGrid>();
    [SerializeField] GameObject _grid;
    Define.TowerType _type;
    int _size;

    public void OnBuild(Define.TowerType type)
    {
        int size = Managers.Data.TowerStatData[type].size;
        _type = type;
        _size = size;
        float pos = 0.07f;
        List<Renderer> renderers = new List<Renderer>();
        renderers.Add(Managers.Resource.Instantiate($"Subitem/BuildPreview_Subitem",
                new Vector3(pos, 0f, -pos), Quaternion.identity, transform).
                GetComponent<Renderer>());
        GameObject tower = Managers.Resource.Instantiate($"Tower/{type}", Vector3.up * 1.25f, Quaternion.identity, transform);
        BoxCollider box = gameObject.GetOrAddComponent<BoxCollider>();
        box.size = new Vector3(1.8f, 1f, 1.8f);

        if (size > 1)
        {
            float interval = 2f;
            float colSize = 1.8f;

            renderers.Add(Managers.Resource.Instantiate($"Subitem/BuildPreview_Subitem",
                new Vector3(interval + pos, transform.position.y, -pos), Quaternion.identity, transform).
                GetComponent<Renderer>());

            renderers.Add(Managers.Resource.Instantiate($"Subitem/BuildPreview_Subitem",
                new Vector3(0f, transform.position.y, -interval - pos), Quaternion.identity, transform).
                GetComponent<Renderer>());

            renderers.Add(Managers.Resource.Instantiate($"Subitem/BuildPreview_Subitem",
                new Vector3(interval + pos, transform.position.y, -interval - pos), Quaternion.identity, transform).
                GetComponent<Renderer>());

            tower.transform.position = new Vector3(interval / 2, 0.25f, -interval / 2);
            box.center = new Vector3(interval / 2f, 0f, -interval / 2f);
            box.size = new Vector3(colSize * 2f, 1f, colSize * 2f);
        }

        foreach (Renderer renderer in renderers)
            _previewerMats.Add(renderer.material);

        GameObject attackRangeViewer = Managers.Resource.Instantiate("Subitem/AttackRangeViewer", Vector3.zero, Quaternion.identity, tower.transform);
        attackRangeViewer.transform.localScale = new Vector3(Managers.Data.TowerStatData[_type].attackRange * Define.TILE_SIZE * 2f, 0.1f, Managers.Data.TowerStatData[_type].attackRange * Define.TILE_SIZE * 2f);
        OnEnter();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Managers.Game.CurrentGold < Managers.Data.TowerStatData[_type].price)
        {
            OnExit();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Buildable")))
        {
            _grid = hit.transform.gameObject;
            transform.position = new Vector3(hit.transform.position.x + 0.92f, 2.45f, hit.transform.position.z - 0.92f);
        }

        if(IsBuildable())
        {
            foreach (Material material in _previewerMats)
            {
                Color color = Color.green;
                color.a = 0.4f;
                material.color = color;
            }
            
            if(Input.GetMouseButtonDown(0))
                OnBuildTower();
        }
        else
        {
            foreach (Material material in _previewerMats)
            {
                Color color = Color.red;
                color.a = 0.4f;
                material.color = color;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Managers.Sound.Play("Interaction/Cancle");
                Managers.UI.MakeEffectUI<UI_MiddleAlert>().SetAlert("타워를 설치 할 수 없습니다.", 1f);
            }
        }
    }

    bool IsBuildable()
    {
        bool value = _touchtedList.Count == _size * _size;

        if (_grid != null)
            value &= _grid.GetComponent<BuildGrid>().IsUsing == false; 

        foreach (BuildGrid buildGrid in _touchtedList)
            value &= buildGrid.IsUsing == false && buildGrid.IsUnitOnTile == false;

        return value;
    }

    void OnBuildTower()
    {
        TowerController tc = Managers.Object.BuildTower(_type);
        Managers.Game.CurrentGold -= Managers.Data.TowerStatData[_type].price;

        if (_size > 1)
            tc.transform.position = new Vector3(transform.position.x + Define.TILE_SIZE / 2, 2.45f, transform.position.z - Define.TILE_SIZE / 2);
        else
            tc.transform.position = new Vector3(transform.position.x, 2.45f, transform.position.z); ;

        foreach (BuildGrid buildGrid in _touchtedList)
            buildGrid.SetUsing(tc.gameObject);

        UI_WaitingForBuildTower waitingBar = Managers.UI.MakeWorldSpcaeUI<UI_WaitingForBuildTower>();
        BoxCollider box = tc.GetComponent<BoxCollider>();
        waitingBar.transform.position = box.transform.position + Vector3.up * box.size.y * 1.5f; 
        waitingBar.ForWait(1f, () => { tc.OnStart(); });
        Managers.Sound.Play("Interaction/InstallTower");

        GameObject attackRangeViewer = Managers.Resource.Instantiate("Subitem/AttackRangeViewer", tc.transform);
        attackRangeViewer.transform.localScale = new Vector3(Managers.Data.TowerStatData[_type].attackRange * Define.TILE_SIZE * 2f, 0.1f, Managers.Data.TowerStatData[_type].attackRange * Define.TILE_SIZE * 2f);
        tc.SetAttackRangeViewer(attackRangeViewer);
    }

    void OnEnter()
    {
        foreach (BuildGrid grid in Managers.Object.Grids)
            grid.OnActive(true);

        Managers.Object.IsBuild = true;
    }

   public void OnExit()
   {
        foreach (BuildGrid grid in Managers.Object.Grids)
            grid.OnActive(false);

        Managers.Object.IsBuild = false;
        Managers.Resource.Destory(gameObject);
   }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("BuildGrid"))
        {
            BuildGrid buildGrid = other.GetComponent<BuildGrid>();

            if (_touchtedList.Contains(buildGrid) == false)
            {
                _touchtedList.Add(buildGrid);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BuildGrid"))
        {
            BuildGrid buildGrid = other.GetComponent<BuildGrid>();

            if (_touchtedList.Contains(buildGrid) == true)
                _touchtedList.Remove(buildGrid);
        }
    }
}
