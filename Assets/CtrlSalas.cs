using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    [SerializeField] private GameObject playerListingPrefab; // Prefab para cada usuario de la sala
    [SerializeField] private Transform contenedorJugadores; // Contenedor de la lista de jugadores
    [SerializeField] private Text nombreSala; // Texto del nombre de la sala
    [SerializeField] private GameObject roomPanel; // Panel de la sala
    [SerializeField] private GameObject lobbyPanel; // Panel del lobby
    [SerializeField] private GameObject btnIniciarJuego; // Botón para iniciar juego (solo visible para el Master)
    [SerializeField] private GameObject avatarSelector; // Selector de avatar

    [Header("Configuración")]
    [SerializeField] private int multiPlayerSceneIndex; // Índice de la escena multijugador

 

    void LlenarListaJugadores()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerListingPrefab, contenedorJugadores);
            Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;
        }
    }

    void BorrarJugadoresdeLista()
    {
        // Borra cada entrada de la lista de jugadores
        for (int i = contenedorJugadores.childCount - 1; i >= 0; i--)
        {
            Destroy(contenedorJugadores.GetChild(i).gameObject);
        }
    }

    public override void OnJoinedRoom()
    {
        roomPanel.SetActive(true); // Activa el panel de la sala activa
        mueveAvatar(296, 320); // Mueve la imagen del avatar
        lobbyPanel.SetActive(false);
        nombreSala.text = PhotonNetwork.CurrentRoom.Name;

        // Solo el Master Client puede iniciar el juego
        btnIniciarJuego.SetActive(PhotonNetwork.IsMasterClient);

        BorrarJugadoresdeLista();
        LlenarListaJugadores();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        BorrarJugadoresdeLista();
        LlenarListaJugadores();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        BorrarJugadoresdeLista();
        LlenarListaJugadores();

        // Si el que salió era el Master, asignar nuevo Master
        if (PhotonNetwork.IsMasterClient)
        {
            btnIniciarJuego.SetActive(true);
        }
    }

    public void StartGameOnClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; // Cierra la sala
            PhotonNetwork.LoadLevel(multiPlayerSceneIndex); // Carga la escena del juego
        }
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackOnClick() // Retorna al lobby
    {
        lobbyPanel.SetActive(true);
        mueveAvatar(276, 425);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }

    private void mueveAvatar(int posY, int posX)
    {
        Vector3 pos = avatarSelector.transform.position;
        pos.y = posY;
        pos.x = posX;
        avatarSelector.transform.position = pos;
    }
}