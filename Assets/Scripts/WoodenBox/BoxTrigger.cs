using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    private Tile _triggerEnteredTile;
    private TileProps _triggerEnteredTilesProps;

    private Vector3 _triggerEnteredObjectsPosition;

    private bool _isTilePropsDetected;
    private bool _isTriggerEntered;

    public Action OnBoxTriggerEntered { get; set; }



   
    private void OnEnable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.WoodBoxSerializer.OnBoxTriggerEnteredSerializer += OnBoxTriggerEnteredSerializer;
        else
            PhotonNetwork.NetworkingClient.EventReceived += OnBoxTriggerEnteredSerializer;
    }

    private void OnDisable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.WoodBoxSerializer.OnBoxTriggerEnteredSerializer -= OnBoxTriggerEnteredSerializer;
        else
            PhotonNetwork.NetworkingClient.EventReceived -= OnBoxTriggerEnteredSerializer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Tags.AI) && !other.CompareTag(Tags.Player) && !other.CompareTag(Tags.Platform))
            GameSceneObjectsReferences.WoodBoxSerializer.BoxTriggerEntered(other);
    }

    private IEnumerator CompleteInteractions()
    {
        if (!_isTriggerEntered)
        {
            yield return new WaitForSeconds(0.1f);
            OnBoxTriggerEntered?.Invoke();
            _isTriggerEntered = true;
        }
    }

    public void InteractionWithTriggerEnteredObjects(object[] data)
    {
        _triggerEnteredObjectsPosition = (Vector3)data[0];

        if (GameSceneObjectsReferences.TilesData.TilesDict.ContainsKey(_triggerEnteredObjectsPosition))
        {
            _triggerEnteredTile = Get<Tile>.From(GameSceneObjectsReferences.TilesData.TilesDict[_triggerEnteredObjectsPosition]);
            _triggerEnteredTilesProps = Get<TileProps>.From(GameSceneObjectsReferences.TilesData.TilesDict[_triggerEnteredObjectsPosition]);
        }

        if (_triggerEnteredTile != null && _triggerEnteredTilesProps != null && !_triggerEnteredTile.IsProtected && !_isTilePropsDetected)
        {
            _triggerEnteredTilesProps.ActiveProps(TileProps.PropsType.MetalGround, true, null);
            _isTilePropsDetected = true;
        }

        StartCoroutine(CompleteInteractions());
    }

    private void OnBoxTriggerEnteredSerializer(object[] data)
    {
        InteractionWithTriggerEnteredObjects(data);
    }

    private void OnBoxTriggerEnteredSerializer(EventData eventData)
    {
        if(eventData.Code == EventInfo.Code_WoodBoxTriggerEntered)
        {
            object[] data = (object[])eventData.CustomData;
            InteractionWithTriggerEnteredObjects(data);
        }
    }
}

