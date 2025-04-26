using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelectorCtrl : MonoBehaviour
{
    private GameObject[] listaAvatars;

    void Start()
    {
        // crea una lista con una cantidad elementos de acuerdo a lo que contiene nuestro GameObject
        listaAvatars = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            listaAvatars[i] = transform.GetChild(i).gameObject;
        }

        // Inicio asegurar que los avatars no estén activos
        foreach (GameObject avatar in listaAvatars)
        {
            avatar.SetActive(false);
        }

        // Activamos por default el primer Avatar de la lista
        if (listaAvatars[0])
            listaAvatars[0].SetActive(true);
    }
}