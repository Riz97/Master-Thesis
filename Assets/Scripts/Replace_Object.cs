using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replace_Object : MonoBehaviour
{
    // Start is called before the first frame update


    public Mesh cylinderMesh;

    private void Start()
    {
        if(this.gameObject.name == "Sphere")
        {
            this.gameObject.GetComponent<MeshFilter>().mesh = cylinderMesh;
        }
    }

}
