using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class InstantiateOnlinePlayers : MonoBehaviour
{
    private readonly string _onlinePlayer = "OnlinePlayer";

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
            GameObject onlinePlayer = PhotonNetwork.Instantiate(_onlinePlayer, Vector3.zero, Quaternion.identity);
            Player player = PhotonNetwork.CurrentRoom.GetPlayer(onlinePlayer.GetComponent<PhotonView>().CreatorActorNr);
            PhotonPlayerController photonPlayer = onlinePlayer.GetComponent<PhotonPlayerController>();
            photonPlayer.InitializePlayer(player);
            _playersText.text += player.NickName + "/" + player.ActorNumber + "/" + photonPlayer.PhotonView.ViewID;
        }
    }
}
