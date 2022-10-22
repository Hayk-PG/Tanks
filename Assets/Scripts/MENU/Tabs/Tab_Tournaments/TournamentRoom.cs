using UnityEngine;
using TMPro;

public class TournamentRoom : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _masterOpponentName, _secondOpponentName;

    [SerializeField]
    private string _nameRoom;

    public string MasterOpponentName { get => _masterOpponentName.text; private set => _masterOpponentName.text = value; }
    public string SecondOpponentName { get => _secondOpponentName.text; private set => _secondOpponentName.text = value; }
    public string RoomName { get => _nameRoom; private set => _nameRoom = value; }
    

    public void SetData(TournamentRoomData tournamentRoomData)
    {
        MasterOpponentName = tournamentRoomData.MasterOpponentName;
        SecondOpponentName = tournamentRoomData.SecondOpponentName;
        RoomName = tournamentRoomData.RoomName;
    }
}
