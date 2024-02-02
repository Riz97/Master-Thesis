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
using static System.Net.Mime.MediaTypeNames;



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
       
    List<string> Directions = new List<string>() { "Right", "Left" , "Middle","right", "left" , "center"};


    List<string> Furniture_Strings = new List<string>() {"Furniture", "Office" };

    List<string> Furniture_Models = new List<string>() {"Desk", "Chair" , "Bed" , "Table" , "Chest","Drawer","Shower", "Sink"};

    List<string> Car_Strings = new List<string>() {"Cars" , "Machines"  , "Starting Grid" };

    List<string> Car_Models = new List<string>() { "Red", "Blue", "Green" , "White", "Silver" , "Sport" , "Formula","Suv" };

    List<string> Nature_Strings = new List<string>() {"Nature"  , "Garden" , "Forest"};

    List<string> Nature_Models = new List<string>() { "Oak", "Bush", "Mushroom", "Wood", "Stone" , "Pine", "Flower"};

    List<string> Industrial_Strings = new List<string>() {"Industry","Industrial"};

    List<string> Industrial_Models = new List<string>() {"Cable","Garbage","Pallet", "PalletCar" , "Plank", "Tank" , "Tube"};

    List<string> City_Strings = new List<string>() {"City"};

    List<string> City_Models = new List<string>() {"Barrel" , "Bench" , "Bin" , "Dumpster" , "Hydrant", "Mailbox" , "Stoplight"};

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
        
        cc++;
        //-----------------------------------------------------------------------------------------------------------------------------------


       

         
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
        List<string> words_Industrial = isIn(input, Industrial_Models);
        List<string> words_City = isIn(input, City_Models);

        List<string> allWords = words_Cars.Concat(words_City).Concat(words_Industrial).Concat(words_Cars).Concat(words_Nature).Concat(words_Furniture).ToList();


        List<string> list_Directions_aux = isIn_Direction(input,Directions,allWords);
     

        for (int i =0; i < list_Directions_aux.Count; i++)
        {
            if (ContainsAny(list_Directions_aux[i], Directions))
            {
                list_Directions_aux.Remove(list_Directions_aux[i+1]);
            }


            else
            {
                list_Directions_aux[i] = " ";
            }

        }

        List<string> list_Directions = list_Directions_aux;


        Debug.Log(list_Directions_aux[0].ToString());


        Text.color = new Color32(27, 255, 0, 255);
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
                " 'Bed' and 'Chair' , You MUST RENAME THEM AS 'Model_0' 'Model_1' and 'Model_2' in the unity hierarchy MANDATORY" +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Bed at X 0 and Chair at X 3, and add just one collider per gameobject, find the gameobject named Plane and change its" +
                    " material with the material   called 'Material'THAT MUST BE LOADED inside the Furniture folder which is inside the folder Resources, using a method called Start , avoid any type of comments , you must write only code";

            Start();

        }




        else if (ContainsAny(input, Nature_Strings))
        {

            input = " the first thing to do must be find the  gameobjects  called 'Model_0', 'Model_1' and 'Model_2' and  destroy them and YOU MUST  substitute them  with the gameobjects THAT YOU MUST   load  from the folder named 'Nature' inside the folder  'Resources' called 'Tree' , " +
                "'Mushroom' and 'Stone' , You MUST RENAME THEM AS 'Model_0' 'Model_1' and 'Model_2' in the unity hierarchy MANDATORY" +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Tree at X 0 and Mushroom at X 3, and add just one collider per gameobject, find the gameobject named Plane and change its" +
                    " material with the material   called 'Material'THAT MUST BE LOADED inside the 'Nature' folder which is inside the folder Resources, using a method called Start , avoid any type of comments , you must write only code";
            Start();

        }
        else if (ContainsAny(input, Car_Strings))

        {

            input = "the first thing to do must be  gameobject called and  destroy them and  substitute them  with the gameobject called Model_0 , Model_1 and Model_2 respectively , remember that they are  positioned inside the folder named Nature inside Resources " +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Model_1 at X 0 and Model_2 at X 3, and add just one collider per gameobject, find the gameobject named Plane and change its" +
                    " material with the material called Material inside the Nature folder which is inside the folder Resources, using a method called Start, after every operation remember that the name of the new objects in the unity hierarchy must be Model_1 Model_2 Model_3";
            Start();
        }

        if (ContainsAny(input, City_Strings))
        {
            input = " the first thing to do must be find the  gameobjects  called 'Model_0', 'Model_1' and 'Model_2' and  destroy them and YOU MUST  substitute them  with the gameobjects THAT YOU MUST   load  from the folder named 'Furniture' inside the folder  'Resources' called 'Table' ," +
                " 'Bed' and 'Chair' , You MUST RENAME THEM AS 'Model_0' 'Model_1' and 'Model_2' in the unity hierarchy MANDATORY" +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Bed at X 0 and Chair at X 3, and add just one collider per gameobject, find the gameobject named Plane and change its" +
                    " material with the material   called 'Material'THAT MUST BE LOADED inside the Furniture folder which is inside the folder Resources, using a method called Start , avoid any type of comments , you must write only code";

            Start();

        }


        if (ContainsAny(input, Industrial_Strings))
        {
            input = " the first thing to do must be find the  gameobjects  called 'Model_0', 'Model_1' and 'Model_2' and  destroy them and YOU MUST  substitute them  with the gameobjects THAT YOU MUST   load  from the folder named 'Furniture' inside the folder  'Resources' called 'Table' ," +
                " 'Bed' and 'Chair' , You MUST RENAME THEM AS 'Model_0' 'Model_1' and 'Model_2' in the unity hierarchy MANDATORY" +
                    ",at Y position equals to -0.47, at X position -2.38 and Z position 29.46 and do the same for Bed at X 0 and Chair at X 3, and add just one collider per gameobject, find the gameobject named Plane and change its" +
                    " material with the material   called 'Material'THAT MUST BE LOADED inside the Furniture folder which is inside the folder Resources, using a method called Start , avoid any type of comments , you must write only code";

            Start();

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        else if (words_Furniture.Count() == Number_of_Objects)
        {

            input = Input_Request(input, Number_of_Objects, words_Furniture , "Furniture",list_Directions);

            Start();
        }


        else if (words_Cars.Count() == Number_of_Objects)
        {


            input = Input_Request(input, Number_of_Objects, words_Cars , "Cars", list_Directions);


            Start();
        }

        else if (words_Nature.Count() == Number_of_Objects)
        {

            input = Input_Request(input, Number_of_Objects, words_Nature, "Nature", list_Directions);

            Start();
        }

        else if (words_City.Count() == Number_of_Objects) 
        {
            input = Input_Request(input, Number_of_Objects, words_City, "City",list_Directions);

            Start();
        }

        else if (words_Industrial.Count() == Number_of_Objects) 
        {
            input = Input_Request(input, Number_of_Objects, words_Industrial,"Industrial", list_Directions);

            Start();

        }



        // Error Handler : if the user does not ask for the correct amount of models , which is set in the hierarchy

        else if (words_City.Count() != Number_of_Objects || words_Cars.Count() != Number_of_Objects || words_Industrial.Count() != Number_of_Objects  || words_Nature.Count() != Number_of_Objects  || words_Furniture.Count() != Number_of_Objects ) 
        {
            Text.color = new Color(255, 0, 0);
            Text.SetText("Error : you have to ask for the exactly amount of models requested  for this simulation");
        }

        else
        {
            Text.color = new Color(255, 0, 0);
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


    public float Random_PositionZ(List<string> list, int i)
    {
        float randomCoordinate = 0;
        Debug.Log(list[i]);

        if (list[i] == "Right" || list[i] == "right")
        {
            randomCoordinate = UnityEngine.Random.Range(9f, 23f);
        } 

        else if (list[i] == "Left" || list[i] == "left")
        {
            randomCoordinate = UnityEngine.Random.Range(10f, 20f);
        }

        else if (list[i] == "Center" || list[i] == "center")
        {
            randomCoordinate = UnityEngine.Random.Range(10f, 20f);
        }

        else
        {
            randomCoordinate = UnityEngine.Random.Range(2f, 30f);
        }
        return randomCoordinate;
    }

    public float Random_PositionX(List<string> list, int i)
    {
        float randomCoordinate = 0;
        Debug.Log(list[i]);

        if (list[i] == "Right" || list[i] == "right")
        {
            randomCoordinate = UnityEngine.Random.Range(14f, 17f);
        }

        else if (list[i] == "Left" || list[i] == "left")
        {
            randomCoordinate = UnityEngine.Random.Range(-9f, -11f);
        }

        else if (list[i] == "Center" || list[i] == "center")
        {
            randomCoordinate = UnityEngine.Random.Range(-4f, 5f);
        }

        else
        {
            randomCoordinate = UnityEngine.Random.Range(-2f, 30f);
        }

        return randomCoordinate;
    }


    public string Input_Request(string input, int Number_of_Objects, List<string> list, string Material, List<string> list_Directions)
    {
        

        input = " the first thing to do must be find the  gameobjects  called ";

        input = Define_Models(Number_of_Objects, input) + " and  destroy them and YOU MUST  substitute them  with the gameobjects THAT YOU MUST   load  from the folder named " + Material +  " inside the folder  'Resources' called ";

        input = Enum_Objects(list, Number_of_Objects, input) + "You MUST RENAME THEM AS ";

        input = Define_Models(Number_of_Objects, input) + " in the unity hierarchy MANDATORY";
        

        input = Define_Models_Coordinates(Number_of_Objects,input,list_Directions) + " and add just one collider per gameobject, find the gameobject named Plane and change its" +

        " material with the material   called 'Material'THAT MUST BE LOADED inside the " +  Material + " folder which is inside the folder Resources," +

        " using a method called Start , avoid any type of comments , you must write only code";


        return input;
    }

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
    public string Define_Models_Coordinates(int Number_of_Objects, string input, List<string> list)
    {



        for (int ii = 0; ii < Number_of_Objects; ii++)
        {
            input += " Model_" + ii.ToString() + " at  Y position equals to -0.47, at X Position equals to  " + Random_PositionX(list,ii).ToString() + " and Z position equals to " + Random_PositionZ(list,ii).ToString();

        }

        return input;
    }



    //Meta language

    //If the inside the string s there is at list one string of the substrings set return true
    //Otherwise , it returns false
    public static bool ContainsAny(string s, List<string> substrings)
    {
        if (string.IsNullOrEmpty(s) || substrings == null)
            return false;

        return substrings.Any(substring => s.Contains(substring, StringComparison.CurrentCultureIgnoreCase));
    }

    //If  inside the string s there are all the strings inside the list of substrings set return true
    //Otherwise , it returns false
    public static bool ContainsAll(string s, List<string> substrings)
    {
        if (string.IsNullOrEmpty(s) || substrings == null)
            return false;

        return substrings.All(substring => s.Contains(substring, StringComparison.CurrentCultureIgnoreCase));
    }


    //It returns a subset of string of the input that are inside the vocabulary of accepted words
    public static List<string> isIn(string s,List<string> Vocab)
    {
        List <string> subSet = new List<string>();

        //Subdivide the string in a List of substrings 
        List<string> aux = s.Split(' ').ToList();

        
        //For every string inside the list
        foreach (string str in aux)
        {

            //If it is accetable, add it to the final subset
        if(ContainsAny(str,Vocab))
            {
                
                subSet.Add(str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower());
            }

            

        }
            
        
        return subSet; 
    }

    //It returns a subset of string of the input that are inside the vocabulary of accepted words with the direction asked

    public static List<string> isIn_Direction(string s, List<string> Direction, List<string> allwords)
    {
        List<string> subSet = new List<string>();

        //Subdivide the string in a List of substrings 
        List<string> aux = s.Split(' ').ToList();


        //For every string inside the list
        foreach (string str in aux)
        {

            //If it is accetable, add it to the final subset
            if (ContainsAny(str, Direction) || ContainsAny(str,allwords))
            {

                subSet.Add(str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower());
            }

 


        }


        return subSet;
    }



    //---------------------------------------------------------------------------------------------------------
}
