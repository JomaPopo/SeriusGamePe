using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conectrl : MonoBehaviourPunCallbacks
{
    public GameObject panelUsuario;
    public GameObject panelLobby;
    public GameObject panelRoom;

    void Start()
    {
        panelUsuario.SetActive(true);
        panelLobby.SetActive(false);
        panelRoom.SetActive(false);
    }

    void Update()
    {
        
    }

    public void ConectarAServidorPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            panelUsuario.SetActive(false);
            panelLobby.SetActive(true);
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
       
    }

   
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Bienvenido a Photon " + PhotonNetwork.NickName);
        panelLobby.SetActive(false);
        panelRoom.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }
    public void UnirseaSalaAIazar()
    {
        // Crea a una Sala o Room al azar para unirse.
        PhotonNetwork.JoinRandomRoom();
    }

    // Si no se puede unir a la sala al azar
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CrearSalayUnirse();
    }

    private void CrearSalayUnirse()
    {
        // Obtenemos el nombre del usuario
        string Usuario = PhotonNetwork.NickName;

        // Creamos una Sala con el nombre del usuario + un número al azar
        string nombreSala = "Sala de prueba" ;

        // Opciones de la Sala
        RoomOptions opcionesSala = new RoomOptions();
        opcionesSala.IsVisible = true;
        opcionesSala.IsOpen = true;
        opcionesSala.MaxPlayers = 20;
        

        PhotonNetwork.JoinOrCreateRoom(nombreSala, opcionesSala, TypedLobby.Default);
        // no callback

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Te uniste a la Sala: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("La sala cuenta con: " + PhotonNetwork.CurrentRoom.PlayerCount + " jugador(es).");

        Button botonSala = panelRoom.GetComponentInChildren<Button>();
        botonSala.enabled = false;

        Transform textSala = panelRoom.transform.Find("txtSala");
        TMP_Text mensajeSala = textSala.GetComponent<TMP_Text>();
        mensajeSala.text = "Te uniste a la sala: " + PhotonNetwork.CurrentRoom.Name + " con " +  PhotonNetwork.CurrentRoom.PlayerCount;
    }
}