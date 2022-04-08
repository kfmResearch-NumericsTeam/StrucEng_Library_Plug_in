﻿using Rhino;
using System;

namespace CodeGenerator
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class CodeGeneratorPlugin : Rhino.PlugIns.PlugIn
    {

        // public CodeGenPanelModel CodeGenPanelModel;
        // public CodeGenPanelCtrl CodeGenPanelCtrl;
        
        public CodeGeneratorPlugin()
        {
            Instance = this;
            // InitializeMvc();
        }

        ///<summary>Gets the only instance of the CodeGeneratorPlugin plug-in.</summary>
        public static CodeGeneratorPlugin Instance { get; private set; }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.


        public void InitializeMvc()
        {
            // CodeGenPanelModel = new CodeGenPanelModel();
            // CodeGenPanelCtrl = new CodeGenPanelCtrl(CodeGenPanelModel);
        }
    }
}