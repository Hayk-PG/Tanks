using System;
using UnityEngine;

public class TournamentRoomButton : MonoBehaviour
{
    [SerializeField] private TournamentRoom _tournamentRoom;

    public event Action<TournamentRoom> onClickTournamentRoomButton;

    public void OnClick()
    {
        onClickTournamentRoomButton?.Invoke(_tournamentRoom);
    }
}
