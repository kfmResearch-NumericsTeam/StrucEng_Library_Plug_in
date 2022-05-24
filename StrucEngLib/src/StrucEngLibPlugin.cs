using Rhino;
using System;
using System.Security.Permissions;
using Eto.Forms;
using Rhino.FileIO;
using Rhino.PlugIns;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// StrucEngLibPlugin Plugin class
    /// </summary>
    public class StrucEngLibPlugin : Rhino.PlugIns.PlugIn
    {
        public static string Version = "0.0.1-alpha4";
        public static string Website = "https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in";

        private static string _modelKey = "model";
        
        public MainViewModel MainViewModel { get; private set; }

        public StrucEngLibPlugin()
        {
            Instance = this;
            MainViewModel = new MainViewModel(new Workbench());
        }

        public static StrucEngLibPlugin Instance { get; private set; }

        protected override bool ShouldCallWriteDocument(FileWriteOptions options) => true;

        protected override void WriteDocument(
            RhinoDoc doc,
            BinaryArchiveWriter archive,
            FileWriteOptions options)
        {
            MainViewModel.UpdateModel();
            var data = WorkbenchFactory.Instance.SerializeToString(MainViewModel.Workbench);
            var dict = new Rhino.Collections.ArchivableDictionary(1, "StrucEngLib");
            dict.Set(_modelKey, data);
            archive.WriteDictionary(dict);
        }

        protected override void ReadDocument(
            RhinoDoc doc,
            BinaryArchiveReader archive,
            FileReadOptions options)
        {
            Rhino.Collections.ArchivableDictionary dict = archive.ReadDictionary();
            var data = dict.GetString(_modelKey, null);
            MainViewModel.Dispose();
            var bench = WorkbenchFactory.Instance.DeserializeFromString(data);
            MainViewModel = new MainViewModel(bench);
        }
    }
}