using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class InstantiatePhotonPlayers : MonoBehaviour
{
    private const string _playerPrefabName = "PhotonPlayer";

    [SerializeField] private Text _playersText;


    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += Instantiate;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= Instantiate;
    }

    private void Instantiate(EventData data)
    {
        if (data.Code == EventInfo._code_InstantiatePlayers)
        {
            GameObject playerObj = PhotonNetwork.Instantiate(_playerPrefabName, Vector3.zero, Quaternion.identity);
            Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerObj.GetComponent<PhotonView>().CreatorActorNr);
            PhotonPlayer photonPlayer = playerObj.GetComponent<PhotonPlayer>();
            photonPlayer.InitializePlayer(player);
            _playersText.text += player.NickName + "/" + player.ActorNumber + "/" + photonPlayer.PhotonView.ViewID;
        }
    }
}
