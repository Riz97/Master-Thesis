using RoslynCSharp;
using RoslynCSharp.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;


    /// 
    /// An example script that shows how to use the compiler service to compile and load a C# source code string and then call an instance method on a proxy object.
    /// 
    public class Domain : MonoBehaviour
    {

    [SerializeField]
    TMP_Text Output_Text;

    private const string Welcome_Message = "static void Main()";
        private ScriptDomain domain = null;
        private string sourceCode;
       
        public void Start()
        {
        
        //Waiter
        if(Output_Text.text.ToString() != Welcome_Message)
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

       

        //In this way we wait 3 seconds , in these 3 seconds the ai should be able to 
        //provide a correct script that Roslyin will compile at runtime
        yield return new WaitForSeconds(3);

        if (Output_Text.text.ToString() != Welcome_Message)
        {
        sourceCode = Output_Text.text.ToString();
        //Debug.Log(sourceCode);
        yield return new WaitForSeconds(5);
         
        // Create domain
        domain = ScriptDomain.CreateDomain("Example Domain");
        
        //Debug.Log("Domain creato");
        
        // Compile and load code - Note that we use 'CompileAndLoadMainSource' which is the same as 'CompileAndLoadSource' but returns the main type in the compiled assembly
        ScriptType type = domain.CompileAndLoadMainSource(sourceCode, ScriptSecurityMode.UseSettings);
        
        //Debug.Log("Type fattp");
        // Create an instance of 'Example'
        ScriptProxy proxy = type.CreateInstance(gameObject);

            //Debug.Log("Proxy creato");


            // Call the method called 'ExampleMethod' and pass the string argument 'Safe World'
            // Note that any exceptions thrown by the target method will handled as indicated by the 'Safe' name

            CreateLogFile(sourceCode);
        
        proxy.SafeCall(sourceCode);
        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------------------

    void CreateLogFile(string sourcecode)
    {

        //TODO mettere anche la domanda fatta a chatgpt prima della risposta

        string s_time = System.DateTime.Now.ToString("dd-MM-yy-hh-mm-ss");
        string path = Application.dataPath + "/Logs/" + s_time + ".txt";

        if(!File.Exists(path))
        {
            Debug.Log("File creato");
            File.WriteAllText(path,"Log \n\n");
        }
    
        File.AppendAllText(path,sourcecode+ "\n");
    }

}

 










