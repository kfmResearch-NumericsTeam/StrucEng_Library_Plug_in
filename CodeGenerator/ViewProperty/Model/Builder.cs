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
                if (set.Displacement == null)
                {
                    set.Displacement = new Displacement();
                }

                set.Displacement.Uz = selection.GetByKey("uz").Value;
                set.Displacement.Ux = selection.GetByKey("ux").Value;
                set.Displacement.Uz = selection.GetByKey("uz").Value;
                set.Displacement.Rotx = selection.GetByKey("rotx").Value;
                set.Displacement.Rotz = selection.GetByKey("rotz").Value;
                set.Displacement.Roty = selection.GetByKey("roty").Value;
            }
        }

        public static void MapGroupToMaterial(PropertyGroup selection, Element el)
        {
            if (selection == null) return;
            if (selection.Key == "elast")
            {
                if (el.MaterialElastic == null)
                {
                    el.MaterialElastic = new MaterialElastic();
                }

                el.MaterialElastic.E = selection.GetByKey("e").Value;
                el.MaterialElastic.V = selection.GetByKey("v").Value;
                el.MaterialElastic.P = selection.GetByKey("p").Value;
            }
        }

        public static void MapGroupToSection(PropertyGroup selection, Element el)
        {
            if (selection == null) return;
            if (selection.Key == "shell_section")
            {
                if (el.ShellSection == null)
                {
                    el.ShellSection = new ShellSection();
                }

                el.ShellSection.Thickness = selection.GetByKey("thick").Value;
            }
        }

        public static SectionViewModel BuildMaterials(Element el)
        {
            var s = BuildMaterials();
            var vm = new SectionViewModel(s);
            if (el.MaterialElastic == null) return vm;

            PropertyGroup g = s.GetByKey("elast");
            g.GetByKey("e").Value = el.MaterialElastic.E;
            g.GetByKey("v").Value = el.MaterialElastic.V;
            g.GetByKey("p").Value = el.MaterialElastic.P;
            vm.SelectedPropertyGroup = g;
            return vm;
        }

        public static SectionViewModel BuildSections(Element el)
        {
            var s = BuildSections();
            var vm = new SectionViewModel(s);
            if (el.ShellSection == null) return vm;
            PropertyGroup g = s.GetByKey("shell_section");
            g.GetByKey("thick").Value = el.ShellSection.Thickness;
            vm.SelectedPropertyGroup = g;
            return vm;
        }

        public static SectionViewModel BuildDisplacement(Set set)
        {
            var s = BuildDisplacement();
            var vm = new SectionViewModel(s);
            if (set.Displacement == null) return vm;
            PropertyGroup g = s.GetByKey("general");
            g.GetByKey("uz").Value = set.Displacement.Uz;
            g.GetByKey("ux").Value = set.Displacement.Ux;
            g.GetByKey("uz").Value = set.Displacement.Uz;
            g.GetByKey("rotx").Value = set.Displacement.Rotx;
            g.GetByKey("rotz").Value = set.Displacement.Rotz;
            g.GetByKey("roty").Value = set.Displacement.Roty;
            vm.SelectedPropertyGroup = g;
            return vm;
        }
    }
}