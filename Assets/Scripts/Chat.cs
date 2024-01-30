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
using System.Data.SqlTypes;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine.XR;
using UnityEngine.InputSystem.Android;



//TODO : Aggiungere un massimo di tentativi per la ricerca dello script da parte della IA

public class Chat : MonoBehaviour

    
{
  public static string result;

    private string input ;
    private string input_aux;
    

    private bool check = false;

    private int cc= 0;

    private const string Computing_Message = "Computing the script , just wait!!!!";

    public static float elapsed_time;

   


    List<string> Mandatory_Words = new List<string>() {"Find", "Find"+ "(" + "\"" + "Model_1"+ "\"" + ")",
                                                        "Find"+ "(" + "\"" + "Plane"+ "\"" + ")"};

    List<string> Material_Words = new List<string>() {
           "\"" + "Furniture/Material"+ "\"" , 
          "\"" + "Cars/Material"+ "\"" , 
           "\"" + "Nature/Material"+ "\"" };
       

    List<string> Furniture_Strings = new List<string>() {"Furniture", "Office" };

    List<string> Furniture_Models = new List<string>() {"Desk", "Chair" , "Bed" , "Table" , "Chest"};

    List<string> Car_Strings = new List<string>() {"Cars" , "Machines" ,"Asphalt" , "Sports Cars" , "Sport Car" , "Circuit" , "Starting Grid" };

    List<string> Car_Models = new List<string>() { "Red Car", "Blu Car", "Green Car" , "White Car", "Silver Car" };

    List<string> Nature_Strings = new List<string>() {"Tree" , "Nature" , "Rock" , "Rocks" , "Bush", "Garden" , "Grass" };

    List<string> Nature_Models = new List<string>() { "Tree", "Bush", "Mushroom", "Wood", "Stone" };

    List<string> Industrial_Strings = new List<string>() { };

    List<string> Industrial_Models = new List<string>() {};

    List<string> City_Strings = new List<string>() {  };

    List<string> City_Models = new List<string>() {  };

    [SerializeField]
    public TMP_Text Text;

    [SerializeField]
    public TMP_InputField InputField;

    [SerializeField]
    public List<string> Reminders_List = new List<string>();

    [SerializeField]
    public string First_Reminder;

    [SerializeField]
    public int Number_of_Objects;


    [SerializeField]
    TMP_Text Output_Text;

    [SerializeField]
    TMP_Text Info_Text; //User Mode Text

    
    string sceneName;

    public GameObject Models;



    //-------------------- OPEN AI CLIENT INFO ------------------------

    public static int  maxTokens = 1000;
    public static double temperature = 0.5;
    public static double presencePenalty = 0.1;
    public static double frequencyPenalty = 0.1;
    public static Model model = Model.GPT3_5_Turbo;

    //--------------------------------------------------------------------

    // Update is called once per frame
    async void Start()
        
    {

        

        //-----------------------------------Creation of the requested number of objects (executed only once) -------------------------------

        if(cc == 0)

        {
            for(int i = 0 ; i < Number_of_Objects; i++)
            {
            GameObject obj = Instantiate(Models,transform.position, Quaternion.identity);
            obj.name = "Model_" + i.ToString();
            }
        }
             
        //-----------------------------------------------------------------------------------------------------------------------------------


        cc++;

            // model = new Model("gpt-3.5-turbo");
         sceneName = SceneManager.GetActiveScene().name;

        //-----------------------INVISIBLE STRINGS HANDLER-----------------------

        if (check)
        {

     
        
        input = First_Reminder + input;
        input_aux = First_Reminder + input_aux;
            for (int i = 0; i < Reminders_List.Count; i++)
            {

                input = input + "," + Reminders_List[i];

                input_aux = input_aux + "," + Reminders_List[i];
                
            }
            check = false;
        }
        //--------------------------------------------------------------------------




      


        if (input != null && input != input_aux)
        {


            Debug.Log(input);


            float start_time = Time.time;

            var messages = new List<Message>
            {
                new Message(Role.User, input)
            };

            var api = new OpenAIClient();
            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo);
            result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            

        
            Debug.Log(result);


            if (ContainsAll(result, Mandatory_Words) && ContainsAny(result, Material_Words) && result[0] == 'u')
            {
             
                
                //Elapsed time for the generation of the script
                elapsed_time = Time.time - start_time;

                //It sets the text of the scroll view
                Text.color = new Color32(27, 255, 0, 255);
                Text.SetText(result.ToString());

          
                //--------------------------------------- User Mode Information ---------------------------------------

                if (sceneName == "VR_User_Scene" || sceneName == "User_Scene")
                {
                    Info_Text.text = ("Executing......");

                }

                //-----------------------------------------------------------------------------------------------------


            }
            else
            {

                Text.color = new Color32(27, 255, 0, 255);

                //--------------------------- User Mode Information -----------------------------------------

                if (sceneName == "VR_User_Scene" || sceneName == "User_Scene")
                {
                    Info_Text.text = ("Sorry, the IA was not able to generate a correct script. Wait! The IA is trying to generate another one :)");

                }

                //-------------------------------------------------------------------------------------

                Text.SetText("Sorry, the IA was not able to generate a correct script. Wait! The IA is trying to generate another one :)");
                Start();

            }

               



            
       }
       
    }



    //It handles the InputField string written by the user

    //Method called with On End Edit (writing ended)
    public void ReadStringInput(TMP_InputField InputField)

    {




        input = InputField.text.ToString();

        List<string> words_Furniture = isIn(input, Furniture_Models);
        List<string> words_Nature = isIn(input, Nature_Models);
        List<string> words_Cars = isIn(input, Car_Models);



        Text.SetText(Computing_Message);

        //---------------------User Mode Information --------------------------------------

        if (sceneName == "VR_User_Scene" || sceneName == "User_Scene")
        {
            Info_Text.text = (Computing_Message);

        }

        //--------------------------------------------------------------------------------



        check = true;

        //------------------------------------------------------------------------------------ BASE CASES ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        if (ContainsAny(input, Furniture_Strings))
        {
            input = " the first thing to do must be find the  gameobjects  called 'Model_0', 'Model_1' and 'Model_2' and  destroy them and YOU MUST  substitute them  with the gameobjects THAT YOU MUST   load  from the folder named 'Furniture' inside the folder  'Resources' called 'Table' ," +
                " 'Bed' and 'Chair' , You MUST RENAME THEM AS 'Model_1' 'Model_2' and 'Model_3' in the unity hierarchy MANDATORY" +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Bed at X 0 and Chair at X 3, and add just one collider per gameobject, find the gameobject named Plane and change its" +
                    " material with the material   called 'Material'THAT MUST BE LOADED inside the Furniture folder which is inside the folder Resources, using a method called Start , avoid any type of comments , you must write only code";

            Start();

        }




        else if (ContainsAny(input, Nature_Strings))
        {

            input = " the first thing to do must be find the  gameobjects  called 'Model_1', 'Model_2' and 'Model_3' and  destroy them and YOU MUST  substitute them  with the gameobjects THAT YOU MUST   load  from the folder named 'Nature' inside the folder  'Resources' called 'Tree' , " +
                "'Mushroom' and 'Stone' , You MUST RENAME THEM AS 'Model_1' 'Model_2' and 'Model_3' in the unity hierarchy MANDATORY" +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Tree at X 0 and Mushroom at X 3, and add just one collider per gameobject, find the gameobject named Plane and change its" +
                    " material with the material   called 'Material'THAT MUST BE LOADED inside the 'Nature' folder which is inside the folder Resources, using a method called Start , avoid any type of comments , you must write only code";
            Start();

        }
        else if (ContainsAny(input, Car_Strings))

        {

            input = "the first thing to do must be  gameobject called and  destroy them and  substitute them  with the gameobject called Model_1 , Model_2 and Model_3 respectively , remember that they are  positioned inside the folder named Nature inside Resources " +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Model_2 at X 0 and Model_3 at X 3, and add just one collider per gameobject, find the gameobject named Plane and change its" +
                    " material with the material called Material inside the Nature folder which is inside the folder Resources, using a method called Start, after every operation remember that the name of the new objects in the unity hierarchy must be Model_1 Model_2 Model_3";
            Start();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        else if (words_Furniture.Count() == Number_of_Objects)
        {
            input = " the first thing to do must be find the  gameobjects  called ";   

            input = Define_Models(Number_of_Objects, input) + " and  destroy them and YOU MUST  substitute them  with the gameobjects THAT YOU MUST   load  from the folder named 'Furniture' inside the folder  'Resources' called " ;

            input = Enum_Objects(words_Furniture, Number_of_Objects, input) + "You MUST RENAME THEM AS " ;

            input = Define_Models(Number_of_Objects, input) + " sin the unity hierarchy MANDATORY, at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for the second game object at X 0 and the third game object  at X 3, " +
              
              "and add just one collider per gameobject, find the gameobject named Plane and change its" +
              " material with the material   called 'Material'THAT MUST BE LOADED inside the Furniture folder which is inside the folder Resources," +
              " using a method called Start , avoid any type of comments , you must write only code"; 

            Start();
        }


       else if (words_Cars.Count() == 3)
        {
            Start();
        }

       else if (words_Nature.Count() == 3)
        {
            Start();
        }

        else
        {
            Text.color = new Color(255,0,0);
            Text.SetText("The model you asked is not implemented yet, sorry");

            //----------------------------------User Mode Information----------------------------------------------------------------------

           if (sceneName == "VR_User_Scene" || sceneName == "User_Scene")
           {
                Info_Text.text = ("The model you asked is not implemented yet, sorry");

            }
           //------------------------------------------------------------------------------------------------------------------
        }


            
    }

    //----------------------------AUXILIARIES FUNCTIONS-------------------------------------------------------

    


    public  string Enum_Objects(List<string> objects, int Number_of_Objects, string input){
        
        

        for(int i = 0; i < Number_of_Objects; i++)
        {
            input += objects[i] + " ";
        }

        return input;
        }

    public string Define_Models(int Number_of_Objects,string input)
    {



        for (int ii = 0; ii < Number_of_Objects; ii++)
        {
            input += " Model_" + ii.ToString();
            
        }

        return input;
    }

//Meta language

    public static bool ContainsAny(string s, List<string> substrings)
    {
        if (string.IsNullOrEmpty(s) || substrings == null)
            return false;

        return substrings.Any(substring => s.Contains(substring, StringComparison.CurrentCultureIgnoreCase));
    }

    public static bool ContainsAll(string s, List<string> substrings)
    {
        if (string.IsNullOrEmpty(s) || substrings == null)
            return false;

        return substrings.All(substring => s.Contains(substring, StringComparison.CurrentCultureIgnoreCase));
    }


    //It returns a subset of string of the input that are inside the vocabulary of accepted words
    public static List<string> isIn(string s,List<string> Furniture_Vocab)
    {
        List <string> subSet = new List<string>();

        //Subdivide the string in a List of substrings 
        List<string> aux = s.Split(' ').ToList();

        
        //For every string inside the list
        foreach (string str in aux)
        {

            //If it is accetable, add it to the final subset
        if(ContainsAny(str, Furniture_Vocab))
            {
                subSet.Add(str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower());
            }

        }
            
        
        return subSet; 
    }



   //---------------------------------------------------------------------------------------------------------
}
