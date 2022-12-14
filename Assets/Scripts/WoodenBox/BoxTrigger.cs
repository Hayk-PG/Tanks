using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    private Tile _triggerEnteredTile;
    private TileProps _triggerEnteredTilesProps;
    private WoodBoxSerializer _woodBoxSerializer;
    private TilesData _tilesData;

    private Vector3 _triggerEnteredObjectsPosition;

    private bool _isTilePropsDetected;
    private bool _isTriggerEntered;

    public Action OnBoxTriggerEntered { get; set; }



    private void Awake()
    {
        _woodBoxSerializer = FindObjectOfType<WoodBoxSerializer>();
        _tilesData = FindObjectOfType<TilesData>();
    }

    private void OnEnable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _woodBoxSerializer.OnBoxTriggerEnteredSerializer += OnBoxTriggerEnteredSerializer;
        else
            PhotonNetwork.NetworkingClient.EventReceived += OnBoxTriggerEnteredSerializer;
    }

    private void OnDisable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _woodBoxSerializer.OnBoxTriggerEnteredSerializer -= OnBoxTriggerEnteredSerializer;
        else
            PhotonNetwork.NetworkingClient.EventReceived -= OnBoxTriggerEnteredSerializer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Tags.AI) && !other.CompareTag(Tags.Player) && !other.CompareTag(Tags.Platform))
            _woodBoxSerializer.BoxTriggerEntered(other);
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

        if (_tilesData.TilesDict.ContainsKey(_triggerEnteredObjectsPosition))
        {
            _triggerEnteredTile = Get<Tile>.From(_tilesData.TilesDict[_triggerEnteredObjectsPosition]);
            _triggerEnteredTilesProps = Get<TileProps>.From(_tilesData.TilesDict[_triggerEnteredObjectsPosition]);
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

