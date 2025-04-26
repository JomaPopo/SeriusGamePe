using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Cada una es un bot�n en la lista
public class Salaselect : MonoBehaviour
{
    [SerializeField]
    private Text nameText; // texto con nombre de sala
    [SerializeField]
    private Text sizeText; // muestra tama�o de la sala

    private string roomName;
    private int roomSize;
    private int playerCount;

    // M�todo enlazado con el bot�n que une al jugador a una sala
    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    // Se llama por el controlador de lobbys para cada nueva sala que se a�ade a la lista
    public void SetRoom(string nombreIngresado, int capacidad, int cantidad)
    {
        roomName = nombreIngresado;
        roomSize = capacidad;
        playerCount = cantidad;
        nameText.text = nombreIngresado;
        sizeText.text = cantidad + "/" + capacidad;
    }
}