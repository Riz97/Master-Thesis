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

    private string input ;
    private string input_aux;

    public static float elapsed_time;

    List<string> Mandatory_Words = new List<string>() {"Find"};

    List<string> Furniture_Strings = new List<string>() {"Furniture", "Desk" , "Table" , "Chair" , "Office" };

    List<string> Car_Strings = new List<string>() {"Cars" , "Machines" ,"Asphalt" , "Sports Cars" , "Sport Car" , "Circuit" , "Starting Grid" };

    List<string> Nature_Strings = new List<string>() {"Tree" , "Nature" , "Rock" , "Rocks" , "Bush", "Garden" , "Grass" };

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

    public static int  maxTokens = 1000;
    public static double temperature = 0.5;
    public static double presencePenalty = 0.1;
    public static double frequencyPenalty = 0.1;
    public static Model model = Model.Davinci;

    //--------------------------------------------------------------------

    // Update is called once per frame
    async void Start()
    {


        //-----------------------INVISIBLE STRINGS HANDLER-----------------------

        input = First_Reminder + input;
        input_aux = First_Reminder + input_aux;
        for (int i = 0; i < Reminders_List.Count; i++)
        {

            input = input + "," + Reminders_List[i];

            input_aux = input_aux + "," + Reminders_List[i];
        }
        //--------------------------------------------------------------------------

       
    
       //Debug.Log(input);
        //Debug.Log(input_aux);
       

        if (input !=null  && input != input_aux)
        {
            


                Debug.Log(input);

                float start_time = Time.time;
                
                var api = new OpenAIClient();
                result = await api.CompletionsEndpoint.CreateCompletionAsync(input, maxTokens: maxTokens, temperature: temperature, presencePenalty: presencePenalty, frequencyPenalty: frequencyPenalty, model: model);


                //TODO WORK ON OUTPUT , ae non é presente find ecc , ignora e modifica il testo della scroll view , riavvia una nuova stampa del risultato, ricordarsi di modiifcare pure lo script Domain

                if(ContainsAny(result,Mandatory_Words)) 
                {  
                
                //Elapsed time for the generation of the script
                elapsed_time = Time.time - start_time;

                //It sets the text of the scroll view
                Text.color = new Color(27, 255, 0,255);
                Text.SetText(result.ToString());

            
                } else
            {

                Text.SetText("Sorry, the IA was not able to generate a correct script. Wait! The IA is trying to generate another one :)");
                Start();

            } 

               



            
        }
       
    }

   

    //It handles the InputField string written by the user

    //Method called with On End Edit (writing ended)
    public void ReadStringInput (TMP_InputField InputField)

    {
        input = InputField.text.ToString();


        if (ContainsAny(input, Furniture_Strings))
        {
            input = " the first thing to do must be  gameobject called and  destroy them and  substitute them  with the gameobject called Model_1 , Model_2 and Model_3 respectively , remember that they are  positioned inside the folder named Furniture inside Resources " +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Model_2 at X 0 and Model_3 at X 3, and add just one collider per gameobject, find the gameobject named Model_4 and change its" +
                    " material with the material called Material inside the Furniture folder, using a method called Start, after every operation remember that the name of the new objects in the unity hierarchy must be Model_1 Model_2 Model_3";

            Start();

        }

        else if (ContainsAny(input,Car_Strings))
        {

            input = "  called and  destroy them and  substitute them  with the gameobject called Model_1 , Model_2 and Model_3 respectively , remember that they are  positioned inside the folder named Cars inside Resources " +
                    ",at Y position equals to -0.4, at X position -2.38 and Z position 29.46 and do the same for Model_2 at X 0 Y 0.4 and Model_3 at X 3 and Y 0.4, and add just one collider per gameobject, find the gameobject named Model_4 and change its" +
                    " material with the material called Material inside the Cars folder, using a method called Start, after every operation remember that the name of the new objects in the unity hierarchy must be Model_1 Model_2 Model_3";
            Start();

        }
        else if (ContainsAny(input, Nature_Strings)) 
        
        { 
            
            input = "the first thing to do must be  gameobject called and  destroy them and  substitute them  with the gameobject called Model_1 , Model_2 and Model_3 respectively , remember that they are  positioned inside the folder named Nature inside Resources " +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Model_2 at X 0 and Model_3 at X 3, and add just one collider per gameobject, find the gameobject named Model_4 and change its" +
                    " material with the material called Material inside the Nature folder, using a method called Start, after every operation remember that the name of the new objects in the unity hierarchy must be Model_1 Model_2 Model_3";
            Start();
        }

        else
        {
            Text.color = new Color(255,0,0);
            Text.SetText("The model you asked is not implemented yet, sorry");
        }


            
    }


    public static bool ContainsAny(string s, List<string> substrings)
    {
        if (string.IsNullOrEmpty(s) || substrings == null)
            return false;

        return substrings.Any(substring => s.Contains(substring, StringComparison.CurrentCultureIgnoreCase));
    }
}
