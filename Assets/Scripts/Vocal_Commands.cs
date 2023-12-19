using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Vocal_Commands : MonoBehaviour
{


    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button stopButton;

    [SerializeField]
    private TMP_Text text;




    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartRecording);
        stopButton.onClick.AddListener(StopRecording);
        

    }


    // Update is called once per frame
    void Update()
    {
        
    }



    private void StartRecording()
    {

    }

    private void StopRecording()
    {

    }


}
