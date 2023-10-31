using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replace_Object : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private MeshFilter modelToChange;

    [SerializeField]
    private Mesh modelToUse;

   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            modelToChange.mesh = modelToUse;
        }
    }
      }
