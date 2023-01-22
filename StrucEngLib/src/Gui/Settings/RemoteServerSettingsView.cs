using System;
using System.Reflection;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Runtime;
using Bitmap = System.Drawing.Bitmap;

namespace StrucEngLib.Gui.Settings
{
    /// <summary>Remote Server Settings</summary>
    public class RemoteServerSettingsView : DynamicLayout
    {
        private TextBox _tbServerEndpoint;

        public RemoteServerSettingsView(SettingsMainViewModel vm)
        {
            Padding = new Padding(5, 5, 5, 10);
            Spacing = new Size(5, 5);
            AddRow(new Label()
            {
                Text =
                    "Server Endpoint"
            }, _tbServerEndpoint = new TextBox());
            
            
            _tbServerEndpoint.Bind<string>(nameof(TextBox.Text), vm,
                nameof(vm.RemoteServer));
        }
    }
}