using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using OpenAIClient.ChatEndpoint;
using OpenAI.Models;
using OpenAI;
using System.Threading.Tasks;
using OpenAI.Chat;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class Chat : MonoBehaviour

    
{
    public string result;

    private string input;

    [SerializeField]
    public static TMP_Text Text;

    [SerializeField]
    public TMP_InputField InputField;

 

    // Update is called once per frame
    async void Start()
    {

       
        if(input !=null)
        {
            var api = new OpenAIClient();
             result = await api.CompletionsEndpoint.CreateCompletionAsync(input, maxTokens: 200, temperature: 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1, model: Model.Davinci);
            Debug.Log(result);

            //It sets the text of the scroll view
            Text.SetText(result.ToString());
            Debug.Log(input);
            Debug.Log(result);
        }
       
    }

    //It handles the InputField string written by the user

    //Method called with On End Edit (writing ended)
    public void ReadStringInput (string s)

    {
        input = s;
        Start();
    }
}
