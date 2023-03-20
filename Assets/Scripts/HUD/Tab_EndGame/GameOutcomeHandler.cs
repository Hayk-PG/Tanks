using System;
using UnityEngine;

public class GameOutcomeHandler : MonoBehaviour
{
    private static GameOutcomeHandler Instance { get; set; }

    public enum Operation { Start, GameResultTab, ScoresTab, StatsTab, ItemsTab, ButtonsTab, UIShiny, CleanUp, MenuScene }

    [SerializeField]
    private Animator _animator;

    public static event Action<IGameOutcomeHandler, Operation, Animator, object[]> onSubmit;



    
    private void Awake() => Instance = this;

    public static void SubmitOperation(IGameOutcomeHandler handler, Operation operation, object[] data = null) => onSubmit?.Invoke(handler, operation, Instance._animator, data);
}
