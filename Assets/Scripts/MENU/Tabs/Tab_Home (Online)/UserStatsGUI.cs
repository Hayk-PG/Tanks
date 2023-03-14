using UnityEngine;
using TMPro;

public class UserStatsGUI : MonoBehaviour, ITabOperation
{
    [SerializeField] [Space]
    private TMP_Text _txtLevel, _txtPoints, _txtWins, _txtLoses, _txtKills, _txtKdRatio, _txtTimePlayed, _txtQuits;

    public ITabOperation This { get; set; }

    public ITabOperation OperationHandler { get; set; }




    private void Awake() => This = this;

    private void OnEnable() => TabsOperation.Handler.onOperationSubmitted += DisplayUserStats;

    private void OnDisable() => TabsOperation.Handler.onOperationSubmitted -= DisplayUserStats;

    private void DisplayUserStats(ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {
        if(operation == TabsOperation.Operation.UserStats)
        {
            OperationHandler = handler;

            if(Data.Manager.Statistics == null)
            {
                OperationHandler.OnOperationFailed();

                return;
            }

            OperationHandler.OnOperationSucceded();

            _txtLevel.text = Data.Manager.Statistics[Keys.Level].ToString();

            _txtPoints.text = Data.Manager.Statistics[Keys.Points].ToString();

            _txtWins.text = Data.Manager.Statistics[Keys.Wins].ToString();

            _txtLoses.text = Data.Manager.Statistics[Keys.Losses].ToString();

            _txtKills.text = Data.Manager.Statistics[Keys.Kills].ToString();

            _txtKdRatio.text = Data.Manager.Statistics[Keys.KD].ToString();

            _txtTimePlayed.text = Data.Manager.Statistics[Keys.TimePlayed].ToString();

            _txtQuits.text = Data.Manager.Statistics[Keys.Quits].ToString();
        }
    }

    public void OnOperationSucceded()
    {
        
    }

    public void OnOperationFailed()
    {
        
    }
}
