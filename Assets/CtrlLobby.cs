using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CtrlLobby : MonoBehaviourPunCallbacks
{
    public Text nombreSala;
    public Text maxSala;
    public Text nombreJugador;
    public Text nombreJugadorLobby;
    public Text conectando;
    public GameObject avatarSelector;

    [SerializeField]
    private GameObject btnConectarLobby; //Botón para unirse a un Lobby

    [SerializeField]
    private GameObject panelLobby; //Panel para mostrar el Lobby

    [SerializeField]
    private GameObject panelInicio; //Panel para el menú principal

    [SerializeField]
    private InputField txtNombreJugador; //Ingreso del texto con el nombre del usuario

    private string nombreSalastr;
    private string jugador;
    private int capacidadSala;

    private List<RoomInfo> salasenLista; //lista de salas

    [SerializeField]
    private Transform contenedordesalas; // contenedor para las salas disponibles

    [SerializeField]
    private GameObject prefabListaSala; //Prefab para mostrar cada sala en el lobby

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        // Activar botón para conectar al lobby
        btnConectarLobby.SetActive(true);
        conectando.text = "";
        // Se inicializar array para salas
        salasenLista = new List<RoomInfo>();

        /*
        // Se busca al jugador guardado en las preferencias
        if(PlayerPrefs.HasKey("Alias"))
        {
            // Si el alias del jugador no está guardado
            if (PlayerPrefs.GetString("Alias") == "")
            {
                // Se asigna uno al azar que puede ser cambiado
                PhotonNetwork.Nickname = "Jugador " + Random.Range(0, 1000);
            }
            else
            {
                PhotonNetwork.Nickname = PlayerPrefs.GetString("Alias");
            }
        }
        else
        {
            PhotonNetwork.Nickname = "Jugador " + Random.Range(0, 1000);
        }
        */
        txtNombreJugador.text = PhotonNetwork.NickName;
    }


    public void JoinLobbyOnClick()
    {
        panelInicio.SetActive(false);
        panelLobby.SetActive(true);
        mueveAvatar(275);

        setNombreJugador();

        PhotonNetwork.JoinLobby(); //Primero trata de unirse a un Lobby
    }

    private void setNombreJugador()
    {
        string jugador = nombreJugador.text;
        PlayerPrefs.SetString("Alias", jugador);
        PhotonNetwork.NickName = jugador;
        Debug.Log("Nom: " + PhotonNetwork.NickName);
        nombreJugadorLobby.text = jugador;
    }

    private void mueveAvatar(int posY)
    {
        Vector3 pos = avatarSelector.transform.position;
        pos.y = posY;
        avatarSelector.transform.position = pos;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        BorrarSalasdelista();

        foreach (RoomInfo room in roomList)
        {
            if (salasenLista != null)
            {
                int tempIndex = salasenLista.FindIndex(ByName(room.Name));

                if (tempIndex != -1)
                {
                    salasenLista.RemoveAt(tempIndex);
                    Destroy(contenedordesalas.GetChild(tempIndex).gameObject);
                }
            }

            if (room.PlayerCount > 0)
            {
                salasenLista.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void BorrarSalasdelista()
    {
        for (int i = contenedordesalas.childCount - 1; i >= 0; i--)
        {
            Destroy(contenedordesalas.GetChild(i).gameObject);
        }
    }

    void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject templisting = Instantiate(prefabListaSala, contenedordesalas);
            Salaselect tempButton = templisting.GetComponent<Salaselect>();
            int capacidad = int.Parse(maxSala.text);
            tempButton.SetRoom(room.Name, capacidad, room.PlayerCount);
        }
    }

    public void setRoomName()
    {
        nombreSalastr = nombreSala.text;
    }

    public void setCapacidadSala()
    {
        capacidadSala = int.Parse(maxSala.text);
    }

    public void CreateRoomOnClick()
    {
        setRoomName();
        Debug.Log("Creando nueva sala: " + nombreSalastr);
        RoomOptions opcionesSala = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)capacidadSala
        };
        PhotonNetwork.CreateRoom(nombreSalastr, opcionesSala);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Fallo en crear una nueva sala, seguramente ya existe una sala con ese nombre.");
    }

    public void Salirdelobby()
    {
        panelInicio.SetActive(true);
        panelLobby.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}