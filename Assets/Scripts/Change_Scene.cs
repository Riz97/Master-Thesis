using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change_Scene : MonoBehaviour
{
    // Start is called before the first frame update
   public void  Developer_Scene()
    {
        SceneManager.LoadScene("Developer_Scene");
    }

    public void User_Scene()
    {
        SceneManager.LoadScene("User_Scene");
    }

    public void Useful_Info_Scene()
    {

    }
}