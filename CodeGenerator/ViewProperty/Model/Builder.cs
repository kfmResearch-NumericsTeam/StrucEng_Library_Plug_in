using CodeGenerator.Model;

namespace CodeGenerator
{
    public class Builder
    {
        public static PropertySection BuildSections()
        {
            return new PropertySection("sec", "Sections")
            {
                Groups =
                {
                    new PropertyGroup("shell_section", "ShellSection")
                    {
                        Properties =
                        {
                            new Property {Key = "thick", Label = "Thickness", Default = "0"}
                        }
                    }
                }
            };
        }

        public static PropertySection BuildMaterials()
        {
            return new PropertySection("mat", "Materials")
            {
                Groups =
                {
                    new PropertyGroup("elast", "Elastic")
                    {
                        Properties =
                        {
                            new Property {Key = "e", Label = "E", Default = "33700"},
                            new Property {Key = "v", Label = "v", Default = "0.0"},
                            new Property {Key = "p", Label = "p", Default = "2500/10**9"}
                        }
                    }
                }
            };
        }

        public static PropertySection BuildDisplacement()
        {
            return new PropertySection("displ", "Displacements")
            {
                Groups =
                {
                    new PropertyGroup("general", "General Displacement")
                    {
                        Properties =
                        {
                            new Property {Key = "ux", Label = "ux", Default = ""},
                            new Property {Key = "uy", Label = "uy", Default = ""},
                            new Property {Key = "uz", Label = "uz", Default = ""},
                            new Property {Key = "rotx", Label = "rotx", Default = ""},
                            new Property {Key = "roty", Label = "roty", Default = ""},
                            new Property {Key = "rotz", Label = "rotz", Default = ""}
                        }
                    }
                }
            };
        }

        public static void MapGroupToDisplacement(PropertyGroup selection, Set set)
        {
            if (selection == null) return;
            if (selection.Key == "general")
            {
                if (set.SetDisplacement == null)
                {
                    set.SetDisplacement = new SetDisplacement();
                }

                set.SetDisplacement.Uz = selection.GetByKey("uz").Value;
                set.SetDisplacement.Ux = selection.GetByKey("ux").Value;
                set.SetDisplacement.Uz = selection.GetByKey("uz").Value;
                set.SetDisplacement.Rotx = selection.GetByKey("rotx").Value;
                set.SetDisplacement.Rotz = selection.GetByKey("rotz").Value;
                set.SetDisplacement.Roty = selection.GetByKey("roty").Value;
            }
        }

        public static void MapGroupToMaterial(PropertyGroup selection, Element el)
        {
            if (selection == null) return;
            if (selection.Key == "elast")
            {
                if (el.ElementMaterialElastic == null)
                {
                    el.ElementMaterialElastic = new ElementMaterialElastic();
                }

                el.ElementMaterialElastic.E = selection.GetByKey("e").Value;
                el.ElementMaterialElastic.V = selection.GetByKey("v").Value;
                el.ElementMaterialElastic.P = selection.GetByKey("p").Value;
            }
        }

        public static void MapGroupToSection(PropertyGroup selection, Element el)
        {
            if (selection == null) return;
            if (selection.Key == "shell_section")
            {
                if (el.ElementShellSection == null)
                {
                    el.ElementShellSection = new ElementShellSection();
                }

                el.ElementShellSection.Thickness = selection.GetByKey("thick").Value;
            }
        }

        public static SectionViewModel BuildMaterials(Element el)
        {
            var s = BuildMaterials();
            var vm = new SectionViewModel(s);
            if (el.ElementMaterialElastic == null) return vm;

            PropertyGroup g = s.GetByKey("elast");
            g.GetByKey("e").Value = el.ElementMaterialElastic.E;
            g.GetByKey("v").Value = el.ElementMaterialElastic.V;
            g.GetByKey("p").Value = el.ElementMaterialElastic.P;
            vm.SelectedPropertyGroup = g;
            return vm;
        }

        public static SectionViewModel BuildSections(Element el)
        {
            var s = BuildSections();
            var vm = new SectionViewModel(s);
            if (el.ElementShellSection == null) return vm;
            PropertyGroup g = s.GetByKey("shell_section");
            g.GetByKey("thick").Value = el.ElementShellSection.Thickness;
            vm.SelectedPropertyGroup = g;
            return vm;
        }

        public static SectionViewModel BuildDisplacement(Set set)
        {
            var s = BuildDisplacement();
            var vm = new SectionViewModel(s);
            if (set.SetDisplacement == null) return vm;
            PropertyGroup g = s.GetByKey("general");
            g.GetByKey("uz").Value = set.SetDisplacement.Uz;
            g.GetByKey("ux").Value = set.SetDisplacement.Ux;
            g.GetByKey("uz").Value = set.SetDisplacement.Uz;
            g.GetByKey("rotx").Value = set.SetDisplacement.Rotx;
            g.GetByKey("rotz").Value = set.SetDisplacement.Rotz;
            g.GetByKey("roty").Value = set.SetDisplacement.Roty;
            vm.SelectedPropertyGroup = g;
            return vm;
        }
    }
}