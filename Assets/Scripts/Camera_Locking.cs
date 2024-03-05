using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Locking : MonoBehaviour
{
    [SerializeField]
    GameObject cameraOffset;

    float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset.transform.position = new Vector3(transform.position.x, y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        cameraOffset.transform.position = new Vector3(transform.position.x,y,transform.position.z);
    }
}
