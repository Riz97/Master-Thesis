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
using System.Linq;

public class Chat : MonoBehaviour

    
{
  private string result;

    private string input;

    [SerializeField]
    public  TMP_Text Text;

    [SerializeField]
    public TMP_InputField InputField;

    [SerializeField]
    public List<string> Reminders = new List<string>();

 

    // Update is called once per frame
    async void Start()
    {
        for(int i = 0; i < Reminders.Count; i++)
        InputField.text = string.Join(',', Reminders[i]);



        if (input !=null)
        {
            var api = new OpenAIClient();
             result = await api.CompletionsEndpoint.CreateCompletionAsync(input, maxTokens: 200, temperature: 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1, model: Model.Davinci);
            

            //It sets the text of the scroll view
            Text.SetText(result.ToString());


            
          
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
