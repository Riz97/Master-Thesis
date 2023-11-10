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
        private ScriptDomain domain = null;
       // private TMP_Text Text = Chat.Text;
        
        private const string sourceCode = @"
        using UnityEngine;
        class Example
        {
            void ExampleMethod(string input)
            {
                Debug.Log(""Hello "" + input);
            }
        }";

        /// <summary>
        /// Called by Unity.
        /// </summary>
        public void Start()
        {
            // Create domain
            domain = ScriptDomain.CreateDomain("Example Domain");


            // Compile and load code - Note that we use 'CompileAndLoadMainSource' which is the same as 'CompileAndLoadSource' but returns the main type in the compiled assembly
            ScriptType type = domain.CompileAndLoadMainSource(sourceCode, ScriptSecurityMode.UseSettings);

            // Create an instance of 'Example'
            ScriptProxy proxy = type.CreateInstance();
   

            // Call the method called 'ExampleMethod' and pass the string argument 'Safe World'
            // Note that any exceptions thrown by the target method will handled as indicated by the 'Safe' name
            proxy.SafeCall("ExampleMethod", "Safe World");
        }
    }

 










