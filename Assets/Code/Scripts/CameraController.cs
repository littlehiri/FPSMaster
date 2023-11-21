using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _target;

    // Start is called before the first frame update
    void Start()
    {
        //Busca al objeto y rellena la referencia
        _target = GameObject.Find("Player").transform.GetChild(1).transform;
    }

    // Para prevenir el lag de la cámara, con LateUpdate nos aseguramos de que primero se hace el Update del jugador
    void LateUpdate()
    {
        transform.position = _target.position;
        transform.rotation = _target.rotation;
    }
}
