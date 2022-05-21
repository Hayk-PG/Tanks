using UnityEngine;
using Photon.Pun;
using System;

public partial class MyPhoton : MonoBehaviour
{  
    public static Action OnNickNameSet { get; set; }

    public static void SetNickName(string nickName)
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.NickName = nickName;

            if (PhotonNetwork.NickName != null)
                OnNickNameSet?.Invoke();
        }
    }
}
