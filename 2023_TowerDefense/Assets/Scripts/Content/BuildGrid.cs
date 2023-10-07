using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGrid : MonoBehaviour
{
    public bool IsUnitOnTile;
    public bool IsUsing;
    GameObject _grid;
    [SerializeField] GameObject _collisionUnit;
    [SerializeField] GameObject _collisionTower;

    private void Awake()
    {
        tag = "BuildGrid";
        _grid = gameObject.FindChild("BuildGrid");
    }

    private void Update()
    {
        if(IsUnitOnTile)
        {
            if(_collisionUnit == null)
                IsUnitOnTile = false;
        }

        if (IsUsing)
        {
            if (_collisionTower == null)
                IsUsing = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Unit"))
        {
            _collisionUnit = other.gameObject;
            IsUnitOnTile = true;
        }
    }

    public void SetUsing(GameObject tower)
    {
        if(tower == null)
        {
            IsUsing = false;
            return;
        }

        _collisionTower = tower;
        IsUsing = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            _collisionUnit = null;
            IsUnitOnTile = false;
        }
    }

    public void OnActive(bool active)
    {
        if(_grid == null)
            _grid = gameObject.FindChild("BuildGrid");

        _grid.SetActive(active);
    }
}
