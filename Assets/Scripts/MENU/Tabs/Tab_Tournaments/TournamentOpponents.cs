using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentOpponents : MonoBehaviour
{
    [SerializeField] private TournamentRoom _prefab;
    [SerializeField] private List<TournamentRoom> _cachedTournamentRooms = new List<TournamentRoom>();

    private TournamentMembers _tournamentMembers;


    private void Awake()
    {
        _tournamentMembers = Get<TournamentMembers>.From(gameObject);
    }

    private void OnEnable()
    {
        _tournamentMembers.onShareProperties += ReceiveMemberProperties;
    }

    private void OnDisable()
    {
        _tournamentMembers.onShareProperties -= ReceiveMemberProperties;
    }

    private void ReceiveMemberProperties(TitleGroupProperties titleGroupProperties, int memberIndex)
    {
        if(memberIndex == _cachedTournamentRooms.Count)
        {
            TournamentRoom tournamentRoom = Instantiate(_prefab, transform);
            _cachedTournamentRooms.Add(tournamentRoom);
        }
        else
        {
            foreach (var item in _cachedTournamentRooms)
            {
                Destroy(item.gameObject);
            }

            _cachedTournamentRooms.Clear();
        }
    }
}
