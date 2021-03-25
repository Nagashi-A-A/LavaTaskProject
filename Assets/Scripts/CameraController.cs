using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 20.0f;
    public float BorderThickness = 15.0f;
    
    [SerializeField] Camera cam;
    
    //Поворот камеры в режиме стрельбы
    void Update()
    {
        Vector3 pos = transform.position;
        

        if (Input.mousePosition.x >= Screen.width - BorderThickness)
        {
            
            cam.transform.Rotate(new Vector3(0, 1, 0), 0.2f, Space.World);
        }

        if (Input.mousePosition.x <= BorderThickness)
        {
            cam.transform.Rotate(new Vector3(0, 1, 0), -0.2f, Space.World);
        }


    }
}
