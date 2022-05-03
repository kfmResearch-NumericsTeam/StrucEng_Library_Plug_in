using Rhino;
using System;
using System.Security.Permissions;
using Rhino.PlugIns;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// StrucEngLibPlugin Plugin class
    /// </summary>
    public class StrucEngLibPlugin : Rhino.PlugIns.PlugIn
    {
        public static string Version = "0.0.1";
        public static string Website = "https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in";
        
        public StrucEngLibPlugin()
        {
            Instance = this;
        }

        public static StrucEngLibPlugin Instance { get; private set; }
        

        private Workbench _model;
        public Workbench GetModel()
        {
            return _model;
        }
    }
}