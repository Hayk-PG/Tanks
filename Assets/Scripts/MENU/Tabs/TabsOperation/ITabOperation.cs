public interface ITabOperation 
{
    ITabOperation This { get; set; }
    ITabOperation OperationHandler { get; set; }


    void OnOperationSucceded();

    void OnOperationFailed();
}
