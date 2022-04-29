using Rhino;
using System;
using CodeGenerator.Model;

namespace CodeGenerator
{
    /// <summary>
    /// StrucEngLibPlugin Plugin class
    /// </summary>
    public class StrucEngLibPlugin : Rhino.PlugIns.PlugIn
    {
        public StrucEngLibPlugin()
        {
            Instance = this;
        }

        public static StrucEngLibPlugin Instance { get; private set; }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.


        private Workbench _model;
        public Workbench GetModel()
        {
            return _model;
        }
    }
}