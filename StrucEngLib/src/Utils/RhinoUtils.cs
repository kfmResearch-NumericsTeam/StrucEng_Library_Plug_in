using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using Eto.Drawing;
using Rhino;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using Font = Rhino.DocObjects.Font;

namespace StrucEngLib
{
    public class RhinoUtils
    {
        // true, if successful, selectType = true => select, false => unselect
        public static bool SelectLayerByName(RhinoDoc doc, string name, bool unSelectAll = true, bool selectType = true)
        {
            Rhino.DocObjects.RhinoObject[] rhobjs = doc.Objects.FindByLayer(name);
            if (rhobjs == null || rhobjs.Length < 1)
                return false;

            if (unSelectAll)
            {
                doc.Objects.UnselectAll();    
            }
            
            for (int i = 0; i < rhobjs.Length; i++) 
                rhobjs[i].Select(selectType);
            
            doc.Views.Redraw();
            return true;
        }
        
        public static bool SelectLayerByNames(RhinoDoc doc, List<string> names, bool unSelectAll = true)
        {
            doc.Objects.UnselectAll();
            foreach (var name in names)
            {
                SelectLayerByName(doc, name, false);

            }
            return true;
        }

        public static void UnSelectAll(RhinoDoc doc)
        {
            doc.Objects.UnselectAll();
            doc.Views.Redraw();
        }

        
        // Sets Text heights of all text entities in document to given size
        public static void NormalizeTextHeights(Rhino.RhinoDoc doc, double textHeight = 3.0)
        {
            try
            {
                var rhobjs = doc.Objects;
                foreach (var t in rhobjs)
                {
                    Rhino.Geometry.TextEntity tEntity = t.Geometry as Rhino.Geometry.TextEntity;
                    if (tEntity == null) continue;
                    tEntity.TextHeight = textHeight;
                    t.CommitChanges();
                }

                doc.Views.Redraw();
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        public static string SelectLayerByMouse(RhinoDoc doc)
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
    }
}