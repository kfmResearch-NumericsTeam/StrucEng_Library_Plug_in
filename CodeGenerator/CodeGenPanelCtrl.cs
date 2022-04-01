using Rhino;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;

namespace CodeGenerator
{
    public class CodeGenPanelCtrl
    {
        private CodeGenPanelModel _model;
        private CodeGenPanelView _view;

        public CodeGenPanelCtrl(CodeGenPanelModel model)
        {
            this._model = model;
        }

        public void setView(CodeGenPanelView view)
        {
            this._view = view;
        }

        public void OnGenerateModel()
        {
        }

        public void OnInspectPython()
        {
        }

        public void OnAddLayer(string layerName)
        {
            // TODO: Validate name?
            _model.AddNewLayer(layerName);
            _model.LayerToAdd = "";
            _view.UpdateView();
        }

        public void OnSelectLayerInDropdown(int index)
        {
            // TODO: Validate index
            var l = _model.Layers[index];
            _model.CurrentLayer = l;
            _view.UpdateView();
        }

        public void OnMouseSelectLayer()
        {
            Rhino.RhinoApp.WriteLine("OnMouse select");
            var doc = Rhino.RhinoDoc.ActiveDoc;
            var str = SelectLayer(doc);
            Rhino.RhinoApp.WriteLine("val {0}", str);
            if (str != null)
            {
                _model.LayerToAdd = str;
            }
            _view.UpdateView();
        }

        public string SelectLayer(RhinoDoc doc)
        {
            const ObjectType selectionType = ObjectType.AnyObject;
            GetObject go = new GetObject();
            go.SetCommandPrompt("Select an object");
            go.GeometryFilter = selectionType;
            go.GroupSelect = false;
            go.SubObjectSelect = false;
            go.EnableClearObjectsOnEntry(true);
            go.EnableUnselectObjectsOnExit(false);
            go.DeselectAllBeforePostSelect = true;
            RhinoObject selected = null;

            // select a single element
            for (;;)
            {
                GetResult res = go.GetMultiple(1, 1);
                if (res != GetResult.Object)
                {
                    Rhino.RhinoApp.WriteLine("A");
                    return null;
                }
                if (go.ObjectsWerePreselected)
                {
                    Rhino.RhinoApp.WriteLine("B");
                    go.EnablePreSelect(false, true);
                    continue;
                }
                break;
            }

            // keep selection
            RhinoObject rhinoObject = go.Object(0).Object();
            if (null == rhinoObject)
            {
                Rhino.RhinoApp.WriteLine("C");
                return null;
            }

            selected = rhinoObject;
            rhinoObject.Select(true);
            doc.Views.Redraw();

            // get name
            int index = selected.Attributes.LayerIndex;
            string name = doc.Layers[index].Name;
            return name;
        }
    }
}