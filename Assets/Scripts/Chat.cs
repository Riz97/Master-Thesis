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
  public static string result;

    private string input;
    private string input_aux;

    public static float elapsed_time;

   

    [SerializeField]
    public  TMP_Text Text;

    [SerializeField]
    public TMP_InputField InputField;

    [SerializeField]
    public List<string> Reminders_List = new List<string>();

    [SerializeField]
    public string First_Reminder;


    [SerializeField]
    TMP_Text Output_Text;

    
    

    //-------------------- OPEN AI CLIENT INFO ------------------------

    public static int  maxTokens = 700;
    public static double temperature = 0.5;
    public static double presencePenalty = 0.1;
    public static double frequencyPenalty = 0.1;
    public static Model model = Model.Davinci;

    //--------------------------------------------------------------------

    // Update is called once per frame
    async void Start()
    {

        
        //-----------------------INVISIBLE STRINGS HANDLER-----------------------
        
        input = string.Concat(First_Reminder, input);
        input_aux = string.Concat(First_Reminder, input_aux);

        for (int i = 0; i < Reminders_List.Count; i++)
        {

            input = input + "," + Reminders_List[i];

            input_aux = input_aux + "," + Reminders_List[i];
        }
        //--------------------------------------------------------------------------

       
    
       Debug.Log(input);
       

        if (input !=null  && input != input_aux)
        {
            
            float start_time = Time.time;
        
            var api = new OpenAIClient();
             result = await api.CompletionsEndpoint.CreateCompletionAsync(input, maxTokens: maxTokens, temperature: temperature, presencePenalty: presencePenalty, frequencyPenalty: frequencyPenalty, model: model);


            //Elapsed time for the generation of the script
             elapsed_time = Time.time - start_time;

            //It sets the text of the scroll view
            Text.SetText(result.ToString());
            

            
          
        }
       
    }

   

    //It handles the InputField string written by the user

    //Method called with On End Edit (writing ended)
    public void ReadStringInput (TMP_InputField InputField)

    {
        input = InputField.text.ToString();
        Start();
    }
}
