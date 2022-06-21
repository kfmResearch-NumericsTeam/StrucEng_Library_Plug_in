using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Layer;

namespace StrucEngLib
{
    /// <summary>
    /// </summary>
    public class SmMainView : Scrollable
    {
        private SmMainViewModel _vm;
        public static SmMainView Instance { get; private set; }

        public SmMainView(SmMainViewModel vm)
        {
            Instance = this;
            LoadUi(vm);
        }

        public void LoadUi(SmMainViewModel vm)
        {
            _vm = vm;
            BuildUi();
        }

        private void BuildUi()
        {
            BackgroundColor = new Label().BackgroundColor;
            Content = new DynamicLayout()
            {
                Padding = new Padding(10, 10),
                Spacing = new Size(0, 10),
                Rows =
                {
                    UiUtils.GenerateTitle("Step 1: Define Materials and Constraints"),
                    new SmLayerView(_vm.SmSettingVm),
                    UiUtils.GenerateTitle("Step 2: Define Model Settings"),
                    new SmSettingView(_vm.SmSettingVm),
                    UiUtils.GenerateTitle("Step 3: Run Analysis"),
                    new SmAnalysisView(_vm.AnalysisVm),
                    new Label()
                }
            };
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }
    }

    public class SmLayerView : DynamicLayout
    {
        private readonly SmSettingViewModel _vm;
        private ListBox _dropdownLayers;
        private DynamicLayout _propLayout;
        private SmAdditionalPropertyView _propLayoutHasData;
        private DynamicLayout _propLayoutNoData;

        public SmLayerView(SmSettingViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BuildGui()
        {
            AddRow(new GroupBox
            {
                Text = "Select Layer",
                Padding = new Padding(5)
                {
                },
                Content = new DynamicLayout
                {
                    Padding = new Padding(5),
                    Spacing = new Size(5, 1),
                    Rows =
                    {
                        new TableLayout
                        {
                            Spacing = new Size(5, 5),
                            Rows =
                            {
                                new TableRow
                                {
                                    ScaleHeight = false, Cells =
                                    {
                                        new TableCell((_dropdownLayers = new ListBox()
                                        {
                                        }), true)
                                    }
                                },
                            }
                        },
                    }
                }
            });
            AddRow(new GroupBox
            {
                Text = "Properties for Layer",
                Padding = new Padding(5),
                Content = _propLayout = new DynamicLayout
                {
                    Padding = new Padding(5),
                    Spacing = new Size(5, 5),
                }
            });
            _propLayoutNoData = new DynamicLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            _propLayoutNoData.Add(new Label() {Text = "No elements added to LinFe Model."});
            _propLayoutHasData = new SmAdditionalPropertyView(_vm);
            _propLayout.Add(_propLayoutHasData);
            _propLayout.Add(_propLayoutNoData);
        }

        class SmAdditionalPropertyView : DynamicLayout
        {
            private readonly SmSettingViewModel _vm;
            private Button _btShowImage;


            public SmAdditionalPropertyView(SmSettingViewModel _vm)
            {
                this._vm = _vm;
                Padding = new Padding(5);
                Spacing = new Size(5, 5);

                UiUtils.AddLabelTextRow(this, "d_strich_bot",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichBot), "40");
                UiUtils.AddLabelTextRow(this, "d_strich_top",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichTop), "40");
                UiUtils.AddLabelTextRow(this, "fc_k",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FcK), "20");
                UiUtils.AddLabelTextRow(this, "fc_theta_grad_kern",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FcThetaGradKern), "45");
                UiUtils.AddLabelTextRow(this, "fs_d",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FsD), "435");
                UiUtils.AddLabelTextRow(this, "alpha_bot",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.AlphaBot), "0");
                UiUtils.AddLabelTextRow(this, "beta_bot",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.BetaBot), "90");
                UiUtils.AddLabelTextRow(this, "alpha_top",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.AlphaTop), "0");
                UiUtils.AddLabelTextRow(this, "beta_top",
                    Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.BetaTop), "90");

                this.AddRow(null);
                this.AddRow(TableLayout.AutoSized((_btShowImage = new Button
                {
                    Size = new Size(110, -1),
                    Text = "Show Image...",
                })));

                _btShowImage.Click += (sender, args) =>
                {
                    var d = new ShowImageForm()
                    {
                        Owner = RhinoEtoApp.MainWindow
                    };
                    d.Show();
                };
            }
        }

        private void BindGui()
        {
            DataContext = _vm;
            _dropdownLayers.DataContext = _vm;
            _dropdownLayers.BindDataContext(c => c.DataStore, (SmSettingViewModel vm) => vm.Properties);
            _dropdownLayers.ItemTextBinding =
                Binding.Property<SmAdditionalPropertyViewModel, string>(vm => vm.Model.Layer.GetName());
            _dropdownLayers.SelectedValueBinding.BindDataContext(
                Binding.Property<SmSettingViewModel, object>((SmSettingViewModel vm) => vm.SelectedProperty));

            _propLayoutHasData.Bind<bool>(nameof(_propLayoutHasData.Visible), _vm, nameof(_vm.HasLayers));
            _propLayoutNoData.Bind<bool>(nameof(_propLayoutNoData.Visible), _vm, nameof(_vm.HasNoLayers));
            _propLayoutHasData.Bind<object>(nameof(_propLayoutHasData.DataContext), _vm, nameof(_vm.SelectedProperty));


            _vm.ViewModelInitialized += (sender, args) =>
            {
                try
                {
                    // preselect first entry if possible
                    _dropdownLayers.SelectedIndex = 0;
                }
                catch (Exception _)
                {
                    // XXX: Ignore
                }
            };
        }
    }

    public class SmAnalysisView : DynamicLayout
    {
        private Button _btnInspectPython;
        private Button _btnExecPython;
        private LinkButton _btnClearData;

        public SmAnalysisView(SmAnalysisViewModel vm)
        {
            BuildGui();
            BindGui();
        }

        private void BuildGui()
        {
            Padding = new Padding(5);
            Spacing = new Size(5, 1);

            AddRow(new GroupBox
            {
                Text = "Generate Code",
                Padding = new Padding(5),
                Content = new TableLayout
                {
                    Padding = new Padding(5),
                    Spacing = new Size(10, 10),
                    Rows =
                    {
                        new TableRow(
                            new TableCell(
                                (_btnInspectPython = new Button {Text = "Inspect Model..."}),
                                true
                            ),
                            new TableCell(
                                (_btnExecPython = new Button {Text = "Execute Model"}),
                                true
                            )
                        ),
                        new TableRow(
                            new TableCell(
                                (_btnClearData = new LinkButton() {Text = "Reset Data"}),
                                false
                            ))
                    }
                }
            });
        }

        private void BindGui()
        {
        }
    }

    public class SmSettingView : DynamicLayout
    {
        private readonly SmSettingViewModel _vm;
        private DropDown _dbSteps;
        private Label _lbNoStepsAdded;

        public SmSettingView(SmSettingViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BuildGui()
        {
            Padding = new Padding(5);
            Spacing = new Size(5, 1);
            DynamicLayout layout;
            AddRow(
                new GroupBox
                {
                    Text = "Settings",
                    Padding = new Padding(5),
                    Content = layout = new DynamicLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                        }
                    }
                });

            layout.Add(TableLayout.HorizontalScaled(_lbNoStepsAdded = new Label()
                {Text = "No steps added to LinFe Model.\n", Visible = false}));

            layout.AddRow(TableLayout.HorizontalScaled(new Label {Text = "Step"}, (_dbSteps = new DropDown())));
            UiUtils.AddLabelDropdownRowBoolean(layout, _vm, "Mindestbewehrung", nameof(_vm.Mindestbewehrung), true);
            UiUtils.AddLabelDropdownRowBoolean(layout, _vm, "Druckzoneniteration", nameof(_vm.Druckzoneniteration),
                true);
            UiUtils.AddLabelTextRow(layout, _vm, "Schubnachweis", nameof(_vm.Schubnachweis), "sia");
            UiUtils.AddLabelTextRow(layout, _vm, "axes_scale", nameof(_vm.AxesScale), "100");
        }

        private void BindGui()
        {
            DataContext = _vm;
            _dbSteps.DataContext = _vm;
            _dbSteps.DataStore = _vm.StepNames;
            _dbSteps.ItemTextBinding = Binding.Property((string order) => "Step " + order);
            _dbSteps.Bind<string>(nameof(_dbSteps.SelectedValue), _vm, nameof(_vm.SelectedStepName));
            _lbNoStepsAdded.Bind<bool>(nameof(_lbNoStepsAdded.Visible), _vm, nameof(_vm.NoStepsAdded));
        }
    }
}