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
        


        //Waiter
        if(Output_Text.text.ToString() != "static void Main()")
        {
            PrintAI_Thoughts();
        }
    }


    public void DoScript()

    {
        
     PrintAI_Thoughts();
          
      
        //
    }



    //---------------------------------------------- Waiter for the Text displayed whenever the AI write something-----------------------------

    public void PrintAI_Thoughts()
    {
        {StartCoroutine(WaitIA()); }
    }

     IEnumerator WaitIA()
    {
        
        sourceCode = Output_Text.text.ToString();
        Debug.Log(sourceCode);
        yield return new WaitForSeconds(5);
         
        // Create domain
        domain = ScriptDomain.CreateDomain("Example Domain");
Debug.Log("Domain creato");
        // Compile and load code - Note that we use 'CompileAndLoadMainSource' which is the same as 'CompileAndLoadSource' but returns the main type in the compiled assembly
        ScriptType type = domain.CompileAndLoadMainSource(sourceCode, ScriptSecurityMode.UseSettings);
        Debug.Log("Type fattp");
        // Create an instance of 'Example'
        ScriptProxy proxy = type.CreateInstance(gameObject);
        Debug.Log("Proxy creato");


        // Call the method called 'ExampleMethod' and pass the string argument 'Safe World'
        // Note that any exceptions thrown by the target method will handled as indicated by the 'Safe' name

        
        proxy.SafeCall(sourceCode);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
}

 










