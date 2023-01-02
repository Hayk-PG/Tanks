﻿using UnityEngine;
using Photon.Pun;
using System;

public class GlobalTileController : MonoBehaviourPun
{
    [SerializeField] private GameObject _prefab;
    private TilesData _tilesData;
    private ChangeTiles _changeTiles;

    public Action<Vector3> OnCreateNewTile { get; set; }


    private void Awake()
    {
        _tilesData = FindObjectOfType<TilesData>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
    }

    public void Modify(Vector3 newTilePosition)
    {  
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode,
            delegate 
            {
                CreateNewTile(newTilePosition);
            }, 
            delegate 
            {
                photonView.RPC("CreateNewTile", RpcTarget.AllViaServer, newTilePosition);
            });
    }

    [PunRPC]
    private void CreateNewTile(Vector3 newTilePosition)
    {
        OnCreateNewTile?.Invoke(newTilePosition);
        _changeTiles.UpdateTiles(newTilePosition);
    }

    public void Modify(Vector3 currentTilePosition, TileModifyManager.TileModifyType tileModifyType)
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, 
            delegate 
            {
                ActivateTileProps(currentTilePosition, tileModifyType);
            }, 
            delegate 
            {
                photonView.RPC("ActivateTileProps", RpcTarget.AllViaServer, currentTilePosition, tileModifyType);
            });
    }

    [PunRPC]
    private void ActivateTileProps(Vector3 currentTilePosition, TileModifyManager.TileModifyType tileModifyType)
    {
        TileProps tileProps = _tilesData.TilesDict[currentTilePosition].GetComponent<TileProps>();

        TileProps.PropsType propsType = tileModifyType == TileModifyManager.TileModifyType.BuildConcreteTiles ?
        TileProps.PropsType.MetalCube : tileModifyType == TileModifyManager.TileModifyType.UpgradeToConcreteTiles ?
        TileProps.PropsType.MetalGround : TileProps.PropsType.Sandbags;

        tileProps?.ActiveProps(propsType, true, null);
    }
}
