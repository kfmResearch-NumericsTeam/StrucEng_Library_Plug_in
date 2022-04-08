using System;
using System.Collections.Generic;
using System.IO;
using CodeGenerator.ui_model;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;

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
            PythonCodeGenerator codeGen = new PythonCodeGenerator(_model);
            var generated = codeGen.Generate();
            ExecutePython(generated);
        }

        public void OnInspectPython()
        {
            PythonCodeGenerator codeGen = new PythonCodeGenerator(_model);
            var sourceCode = codeGen.Generate();

            var dialog = new InspectPythonDialog(sourceCode);
            var dialogRc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (dialogRc == Eto.Forms.DialogResult.Ok)
            {
                sourceCode = dialog.Source;
            }
        }

        protected void ExecutePython(string s)
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            File.WriteAllText(fileName, s);
            Rhino.RhinoApp.WriteLine(fileName);
            Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
        }

        public void OnAddLayer(string layerName)
        {
            if (layerName == "")
            {
                Rhino.RhinoApp.WriteLine("Layer name is empty");
                return;
            }

            // TODO: Validate name?
            _model.AddNewLayer(layerName, LayerType.ELEMENT);
            _model.LayerToAdd = "";
            _view.UpdateView();
        }

        public void OnSelectLayerInDropdown(int index)
        {
            if (index < 0 || index >= _model.Layers.Count)
            {
                return;
            }

            var l = _model.Layers[index];
            _model.CurrentLayer2 = l;
            _view.UpdateView();
        }

        public void OnMouseSelectLayer()
        {
            var doc = Rhino.RhinoDoc.ActiveDoc;
            var str = SelectLayer(doc);
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
            go.SetCommandPrompt("Select an object with your mouse");
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
                    return null;
                }

                if (go.ObjectsWerePreselected)
                {
                    go.EnablePreSelect(false, true);
                    continue;
                }

                break;
            }

            // keep selection
            RhinoObject rhinoObject = go.Object(0).Object();
            if (null == rhinoObject)
            {
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

        public void OnLayerDelete()
        {
            if (_model.CurrentLayer2 == null)
            {
                Rhino.RhinoApp.WriteLine("No Layer selected to delete");
                return;
            }

            _model.DeleteLayer(_model.CurrentLayer2);
            _view.UpdateView();
        }
    }
}