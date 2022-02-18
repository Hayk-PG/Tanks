
public interface IScore
{
    int Score { get; set; }
    IDamage IDamage { get; set; }


    void GetScore(int score, IDamage iDamage);
}
