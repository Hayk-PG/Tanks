using Photon.Pun;

public class WoodBoxContentSerializeView : BaseGameManagerSerializeView
{
    private int _contentIndex = 0;
    private int _weaponIndex = 0;


    protected override void Write(PhotonStream stream)
    {
        if (_woodenBoxSerializer.WoodBox != null)
        {
            _contentIndex = _woodenBoxSerializer.WoodBox.ContentIndex;
            _weaponIndex = _woodenBoxSerializer.WoodBox.WeaponIndex;
        }

        stream.SendNext(_contentIndex);
        stream.SendNext(_weaponIndex);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        _contentIndex = (int)stream.ReceiveNext();
        _weaponIndex = (int)stream?.ReceiveNext();

        if (_woodenBoxSerializer.WoodBox != null)
        {
            _woodenBoxSerializer.WoodBox.ContentIndex = _contentIndex;
            _woodenBoxSerializer.WoodBox.WeaponIndex = _weaponIndex;
        }
    }
}
