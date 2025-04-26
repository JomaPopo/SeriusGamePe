using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlConexion : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int versionJuego;

    void Start()
    {
        PhotonNetwork.GameVersion = versionJuego.ToString();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor " + PhotonNetwork.CloudRegion);
    }
}