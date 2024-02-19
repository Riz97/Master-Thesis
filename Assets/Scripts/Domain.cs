using Microsoft.CodeAnalysis;
using OpenAI.Chat;
using RoslynCSharp;
using RoslynCSharp.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// 
/// An example script that shows how to use the compiler service to compile and load a C# source code string and then call an instance method on a proxy object.
/// 
public class Domain : MonoBehaviour
    {

    [SerializeField]
    TMP_Text Output_Text;

    [SerializeField]
    TMP_Text Input_Text;

    //-------------------- SYSTEM MESSAGES----------------------------------------------------------

    private const string Welcome_Message = "static void Main()";
    private const string Error_Message = "The model you asked is not implemented yet, sorry";
    private const string Wait_Message = "Sorry, the IA was not able to generate a correct script. Wait! The IA is trying to generate another one :)";
    private const string Computing_Message = "Computing the script , just wait!!!!";

    //------------------------------------------------------------------------------------------------

    private ScriptDomain domain = null;
    private string sourceCode;
 
    
   

    static string s_time = System.DateTime.Now.ToString("dd-MM-hh-mm-ss");
    string path = Application.dataPath + "/Logs/" + s_time + ".txt";

    public void Start()
    {
        

        

       

        //Waiter
        if (Output_Text.text.ToString() != Welcome_Message && Output_Text.text.ToString() != Error_Message && Output_Text.text.ToString() != Wait_Message && Output_Text.text != "Executing......")
        {
            PrintAI_Thoughts();
        }

       

    
    }

    //Method attached to the Button "ok" and "enter" , whenever they are clicked the script created is executed
    public void DoScript()

    {
        
     PrintAI_Thoughts();
          
    }

    //---------------------------------------------- SCRIPT EXECUTOR -----------------------------

    public void PrintAI_Thoughts()
    {
        {StartCoroutine(WaitIA()); }
    }

  

    //Wait for 5 seconds for the creation of the CHATGPT script
     IEnumerator WaitIA()
    {



        //In this way we wait 10 seconds only the first time the app is launched
        //, in these  seconds the ai should be able to 
        //provide a correct script that Roslyin will compile at runtime


        yield return new WaitForSeconds(30);

        
        if (Output_Text.text.ToString() == Wait_Message)
        {
            Debug.Log("sono qua");
            
            yield return new WaitForSeconds(30);
        }
         

        if (Output_Text.text.ToString() != Welcome_Message && Output_Text.text.ToString() != Error_Message && Output_Text.text.ToString() != Wait_Message && Output_Text.text != "Executing......")
        {
            sourceCode = Output_Text.text.ToString();
            


            yield return new WaitForSeconds(30);

            



                // Create domain
                domain = ScriptDomain.CreateDomain("Example Domain");



                // Compile and load code - Note that we use 'CompileAndLoadMainSource' which is the same as 'CompileAndLoadSource' but returns the main type in the compiled assembly
                ScriptType type = domain.CompileAndLoadMainSource(sourceCode, ScriptSecurityMode.UseSettings);

                // Create an instance of 'Example'
                ScriptProxy proxy = type.CreateInstance(gameObject);

                CreateLogFile(sourceCode, Input_Text);




                // Call the method called 'ExampleMethod' and pass the string argument 'Safe World'
                // Note that any exceptions thrown by the target method will handled as indicated by the 'Safe' name
                proxy.SafeCall(sourceCode);
            


            if(Chat.input_auxx == "Office" || Chat.input_auxx == "office" ||
                Chat.input_auxx == "Apartment" || Chat.input_auxx == "apartment" ||
                    Chat.input_auxx == "Nature" || Chat.input_auxx == "nature" ||
                Chat.input_auxx == "Forest" || Chat.input_auxx == "forest" ||
                Chat.input_auxx == "Grid" || Chat.input_auxx == "grid" ||
                Chat.input_auxx == "City" || Chat.input_auxx == "city" ||
                Chat.input_auxx == "Industry" || Chat.input_auxx == "industry")

            {
                
                Chat.Bases = true;

            }

            else
            {
                Chat.Custom = true;
            }
           
            
        } 

        

    }

    //-------------------------------------------------------------------------------------------------------------------------------------------




    //------------------------------------------------- LOG FILES FUNCTION ------------------------------------------

    void CreateLogFile(string sourcecode, TMP_Text Input_Text)
    {



        if (!File.Exists(path))
        {

            File.WriteAllText(path, "LOG GENERATED FOR THE SESSION" + "\n" + "Model : " + Chat.model.ToString());

        }

        File.AppendAllText(path, "\nYou wrote the following  sentence : " +
            Input_Text.text + "\n" + "\n" + "The script generated by the AI is the following: \no " + sourcecode + "\n" +
            "Elapsed time for the generation of the script : " + Chat.elapsed_time + " seconds"
            + "\n" + "The IA required " + Chat.tries + " tries , for obtaining an accetable script");
    }


    //--------------------------------------------------------------------------------------------------------------

}

 










