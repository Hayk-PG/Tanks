using UnityEngine;
using TMPro;

public class TournamentRoom : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _masterPlayerName, _secondPlayerName;

    [SerializeField]
    private string roomName;

    public string MasterPlayerID { get; private set; }
    public string SecondPlayerID { get; private set; }
    public string MasterPlayerName { get => _masterPlayerName.text; private set => _masterPlayerName.text = value; }
    public string SecondPlayerName { get => _secondPlayerName.text; private set => _secondPlayerName.text = value; }
    public string RoomName { get => roomName; private set => roomName = value; }
    

    public void SetPlayerName(TournamentRoomData tournamentRoomData)
    {
        if(tournamentRoomData.MasterPlayerID != null)
        {
            MasterPlayerID = tournamentRoomData.MasterPlayerID;
            ExternalData.Profile.Get(MasterPlayerID, result => { MasterPlayerName = result.PlayerProfile.DisplayName; });
        }
        if(tournamentRoomData.SecondPlayerID != null)
        {
            SecondPlayerID = tournamentRoomData.SecondPlayerID;
            ExternalData.Profile.Get(SecondPlayerID, result => { SecondPlayerName = result.PlayerProfile.DisplayName; });
        }
    }

    public void DeleteMasterPlayerName()
    {
        MasterPlayerID = "";
        MasterPlayerName = "";
    }
    public void DeleteSecondPlayerName()
    {
        SecondPlayerID = "";
        SecondPlayerName = "";
    }

    public void DeleteAllCredentials()
    {
        DeleteMasterPlayerName();
        DeleteSecondPlayerName();
    }
}
