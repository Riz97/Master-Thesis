using RoslynCSharp;
using RoslynCSharp.Compiler;
using System.Collections;
using System.Collections.Generic;
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

        private TMP_Text Text= null;
        private ScriptDomain domain = null;
        private string sourceCode;

    

        /// <summary>
        /// Called by Unity.
        /// </summary>
        public void Start()
        {
        Text = GameObject.Find("Ai_Manager").GetComponent<Chat>().Text;
            
            

      
       
            // Compile and load code - Note that we use 'CompileAndLoadMainSource' which is the same as 'CompileAndLoadSource' but returns the main type in the compiled assembly
           

       
       // ScriptProxy proxy = type.CreateInstance();
    



   // proxy.SafeCall("ExampleMethod", "Safe World");

        //Waiter
        if(Text.text.ToString() != "static void Main()")
        {
            PrintAI_Thoughts();
        }
    }


    public void DoScript()

    {
        
     
        if (Text.text.ToString() != "static void Main()")
        {
        sourceCode = Output_Text.text.ToString();

        // Create domain
        domain = ScriptDomain.CreateDomain("Example Domain");

        // Compile and load code - Note that we use 'CompileAndLoadMainSource' which is the same as 'CompileAndLoadSource' but returns the main type in the compiled assembly
        ScriptType type = domain.CompileAndLoadMainSource(sourceCode, ScriptSecurityMode.UseSettings);

         // Create an instance of 'Example'
        ScriptProxy proxy = type.CreateInstance();
            
        // Call the method called 'ExampleMethod' and pass the string argument 'Safe World'
        // Note that any exceptions thrown by the target method will handled as indicated by the 'Safe' name
        proxy.SafeCall(sourceCode);
        }
         
      
        //
    }



    //---------------------------------------------- Waiter for the Text displayed whenever the AI write something-----------------------------

    public void PrintAI_Thoughts()
    {
        {StartCoroutine(WaitIA()); }
    }

     IEnumerator WaitIA()
    {
        yield return new WaitForSeconds(10);
        //Debug.Log(Text.text.ToString());
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
}

 










