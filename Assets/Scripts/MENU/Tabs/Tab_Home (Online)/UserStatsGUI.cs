using UnityEngine;
using TMPro;

public class UserStatsGUI : MonoBehaviour, IReset, ITabOperation
{
    [SerializeField] [Space]
    private TMP_Text _txtLevel, _txtPoints, _txtWins, _txtLoses, _txtKills, _txtKdRatio, _txtTimePlayed, _txtQuits;

    public ITabOperation This { get; set; }
    public ITabOperation OperationHandler { get; set; }




    private void Awake() => This = this;

    private void PrintUserStats()
    {
        if(Data.Manager.Statistics == null)
        {
            TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.Authenticate);

            return;
        }

        _txtLevel.text = Data.Manager.Statistics[Keys.Level].ToString();
        _txtPoints.text = Data.Manager.Statistics[Keys.Points].ToString();
        _txtWins.text = Data.Manager.Statistics[Keys.Wins].ToString();
        _txtLoses.text = Data.Manager.Statistics[Keys.Losses].ToString();
        _txtKills.text = Data.Manager.Statistics[Keys.Kills].ToString();
        _txtKdRatio.text = Data.Manager.Statistics[Keys.KD].ToString();
        _txtTimePlayed.text = Data.Manager.Statistics[Keys.TimePlayed].ToString();
        _txtQuits.text = Data.Manager.Statistics[Keys.Quits].ToString();
    }

    public void SetDefault() => PrintUserStats();

    public void OnOperationSucceded()
    {
        
    }

    public void OnOperationFailed()
    {
        
    }
}
