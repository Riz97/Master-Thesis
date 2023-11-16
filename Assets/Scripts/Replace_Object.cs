using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Replace_Object : MonoBehaviour
{
    // Start is called before the first frame update


    

    private void Start()
    {
 GameObject Sphere = GameObject.Find("Sphere");

      if(Sphere != null)
        {
            Debug.Log("beccato");
            Mesh cubeMesh = (Mesh)Resources.Load("Capsule", typeof(Mesh));
            Sphere.GetComponent<MeshFilter>().mesh = cubeMesh;
        }
    }

}
