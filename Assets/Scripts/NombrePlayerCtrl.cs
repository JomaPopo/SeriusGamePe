using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NombrePlayerCtrl : MonoBehaviour
{
    public void SetNombrePlayer(string strNombrePlayer)
    {
        if (!string.IsNullOrEmpty(strNombrePlayer))
        {
            PhotonNetwork.NickName = strNombrePlayer;
        }
    }
}