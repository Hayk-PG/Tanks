using System;

public interface ITab_Base 
{
    public event Action<ITab_Base> onSendTab;
}
