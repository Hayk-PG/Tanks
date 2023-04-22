public interface IGameOutcomeHandler 
{
    public IGameOutcomeHandler This { get; set; }
    public IGameOutcomeHandler OperationHandler { get; set; }


    public void OnSucceed();

    public void OnFailed();
}
