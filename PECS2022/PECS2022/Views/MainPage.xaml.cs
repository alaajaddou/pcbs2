using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Plugin.Geolocator;
using PECS2022.Interfaces;
using PECS2022.Models;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using PECS2022.Util;

namespace PECS2022.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static MainPage instance { get; set; }
        private const string BuildingCodeField = "رقم_تعداد_المبن_1";
        private const string BuildingGLOBALID = "GLOBALID";
        private const string BuildingNameField = "اسم_المبنى";
        private const string BuildingOwnerField = "اسم_مالك_المبنى";
        private const string BuildingNumberOfFloorsField = "عدد_الطوابق";
        private const string BuildingPolyName = "NAMEAR";
        private const string ENUMID = "EA_SERID";
        private const string BuildingEnumCode = "رقم_منطقة_العد";
        private static bool ShowAllLayers = true;
        private static bool ShowCompletedLayer = true;
        private static bool ShowNotVisitedLayer = true;
        private static bool ShowNotCompletedLayer = true;
        private static bool ShowDeletedSampleLayer = true;
        private static bool ShowOtherSampleLayer = true;
        private const string BuildingOwner = "اسم_مالك_المبنى";
        private const string BuildingStatus = "حالة_المبنى";
        public const string BuildingNumberOfFloors = "عدد_الطوابق";
        public const string BuildingNumberOfUnits = "مجموع__الوحدات_ا";

        private const string BuildingEA = "رقم_منطقة_العد";
        public const string BuildingKey = "رقم_تعداد_المبنى";

        public static string CurrentBuildingNo { get; set; }

        private static Dictionary<string, string> BuilingInfoFields;
        private static Dictionary<string, string> BuilingPolyFields;

        public MainPage()
        {
            InitializeComponent();

            instance = this;

            //Set width,height of vertical,horizontal tool bar
            StackLayout tempStackLayout;
            tempStackLayout = pnlHorizontalToolBar.Children[0] as StackLayout;
            pnlHorizontalToolBar.WidthRequest = (tempStackLayout.Children.Count + 1) * 50;

            tempStackLayout = pnlVerticalToolBar.Children[0] as StackLayout;
            pnlVerticalToolBar.HeightRequest = tempStackLayout.Children.Count * 50;

            Application.Current.MainPage.Title = "test";
            BuilingInfoFields = new Dictionary<string, string>
            {
                { BuildingOwner, "اسم مالك المبنى" },
                { BuildingStatus, "حالة المبنى" },
                { BuildingNumberOfFloors, "عدد الطوابق" },

                { BuildingKey, "رقم المبنى في منطقة العد" },
                { BuildingEA, "رقم منطقة العد" },
                { BuildingCodeField, "معرف المبنى" }
            };
            //عدد_الطوابق


            BuilingPolyFields = new Dictionary<string, string>
            {
                { "NAMEAR", "اسم المبنى" },
                { "BUILDTYPE_DOMAIN_DESC", "استخدام المبنى" }
            };

            LocalIsraeliSettlementInfo = new LayerInfo() { DisplayName = "المستوطنات", TableName = "sett2019", InitialOpicity = 1.0f, Order = 1, FieldName = "NAME_AR", BorderLineColor = Color.Black, FontColor = Color.GreenYellow, FontSize = 20, BorderLineSize = 0, EnableLabeling = true };
            LocalIsraeliSetlmentOutpostEInfo = new LayerInfo() { DisplayName = "البؤر الاستيطانية", TableName = "post2019", InitialOpicity = 1.0f, Order = 2, FieldName = "NAMEMANE", BorderLineColor = Color.Black, FontColor = Color.GreenYellow, FontSize = 20, BorderLineSize = 1.0f, EnableLabeling = true };
            LocalLayerWallInfo = new LayerInfo() { DisplayName = "جدار الفصل العنصري", TableName = "wall2019", InitialOpicity = 1.0f, Order = 3, EnableLabeling = false };
            LocalBarriersInfo = new LayerInfo() { DisplayName = "عوائق", TableName = "barriers2019", InitialOpicity = 1.0f, Order = 4, FieldName = "BARRTYPE_DESC", BorderLineColor = Color.Yellow, FontColor = Color.Red, FontSize = 18, BorderLineSize = 1.0f, EnableLabeling = true };
            LocalLocalityLayerInfo = new LayerInfo() { DisplayName = "حدود التجمع السكاني", TableName = "localities2019", InitialOpicity = 0.8f, Order = 5, FieldName = "NAMEAR", BorderLineColor = Color.DarkRed, FontColor = Color.DarkRed, FontSize = 24, BorderLineSize = 0.0f, EnableLabeling = true };
            LocalBlockLayerInfo = new LayerInfo() { DisplayName = "حدود منطقة العد", TableName = "ea2019", InitialOpicity = 0.3f, Order = 6, FieldName = "EA_SERID", BorderLineColor = Color.DarkOrange, FontColor = Color.Black, FontSize = 26, BorderLineSize = 3.0f, EnableLabeling = true };

            LocalStreetsLayerInfo = new LayerInfo() { DisplayName = "الطرق والشوارع", TableName = "roads2019", InitialOpicity = 1.0f, Order = 7, FieldName = "ST_NAME", BorderLineColor = Color.Red, FontColor = Color.Yellow, FontSize = 22, BorderLineSize = 1.0f, EnableLabeling = true };

            OtherFeaturesLayerInfo = new LayerInfo() { DisplayName = "موجودات أخرى", TableName = "otherfeatures2019", InitialOpicity = 1.0f, Order = 8, FieldName = "DESCRIPTION", BorderLineColor = Color.Yellow, FontColor = Color.Red, FontSize = 18, BorderLineSize = 1.0f, EnableLabeling = true };
            LocalLandmarksLayerInfo = new LayerInfo() { DisplayName = "المعالم", TableName = "landmarks2019", InitialOpicity = 1.0f, Order = 9, FieldName = "NAME", BorderLineColor = Color.White, FontColor = Color.BlueViolet, FontSize = 18, BorderLineSize = 1.0f, EnableLabeling = true };
            //LocalOrganalBlockLayerInfo = new LayerInfo() { DisplayName = "منطقة العد الاصلية", TableName = "ea2019", InitialOpicity = 0.3f, Order = 10, EnableLabeling = false };
            LocalBuildingPolygonLayerInfo = new LayerInfo() { DisplayName = "الأبنية", TableName = "buildpolygon2019", InitialOpicity = 0.4f, Order = 10, FieldName = "NAMEAR", BorderLineColor = Color.Wheat, FontColor = Color.DarkBlue, FontSize = 16, BorderLineSize = 1, EnableLabeling = true };
            LocalBuildingLayerInfo = new LayerInfo() { DisplayName = "مواقع الأبنية", TableName = "buildingsp2019", InitialOpicity = 0.7f, Order = 11, FieldName = BuildingKey, BorderLineColor = Color.DarkBlue, FontColor = Color.DarkBlue, FontSize = 16, BorderLineSize = 1, EnableLabeling = true };

            LocalStartPointLayerInfo = new LayerInfo() { DisplayName = "نقطة البداية", TableName = "startpoint2019", InitialOpicity = 1.0f, Order = 12, EnableLabeling = false };




         }


        #region Properties and Utils Functions
        public MapOperation CurrentMapOperation { get; set; }
        public FeatureLayer LocalBuildingLayer { get; private set; }
        public FeatureLayer LocalBuildingPolygonLayer { get; private set; }
        public FeatureLayer LocalLocalityLayer { get; private set; }
        public FeatureLayer LocalBlockLayer { get; private set; }
        public FeatureLayer LocalOtherLayer { get; private set; }
        public FeatureLayer OtherFeaturesLayer { get; private set; }
        public FeatureLayer LocalLandmarksLayer { get; private set; }
        public FeatureLayer LocalStreetsLayer { get; private set; }
        public FeatureLayer LocalStartPointLayer { get; private set; }
        public FeatureLayer LocalBarriers { get; private set; }
        public FeatureLayer LocalLayerWall { get; private set; }
        public FeatureLayer LocalIsraeliSettlement { get; private set; }
        public FeatureLayer LocalIsraeliSetlmentOutpostE { get; private set; }
        public FeatureLayer LocalOrganalBlockLayer { get; private set; }


        public LayerInfo LocalBuildingLayerInfo { get; set; }
        public LayerInfo LocalBuildingPolygonLayerInfo { get; set; }
        public LayerInfo LocalLocalityLayerInfo { get; set; }
        public LayerInfo LocalBlockLayerInfo { get; set; }
        public LayerInfo LocalOtherLayerInfo { get; set; }
        public LayerInfo OtherFeaturesLayerInfo { get; set; }
        public LayerInfo LocalLandmarksLayerInfo { get; set; }
        public LayerInfo LocalStreetsLayerInfo { get; set; }
        public LayerInfo LocalStartPointLayerInfo { get; set; }
        public LayerInfo LocalBarriersInfo { get; set; }
        public LayerInfo LocalLayerWallInfo { get; set; }
        public LayerInfo LocalIsraeliSettlementInfo { get; set; }
        public LayerInfo LocalIsraeliSetlmentOutpostEInfo { get; set; }
        public LayerInfo LocalOrganalBlockLayerInfo { get; set; }

        public List<LayerInfo> LayerInfos { get; set; }

        private string GetJsonLabelDef(string fieldName, Color borderLineColor, Color color, float fontSize, float borderLineSize = 0.0f)
        {
            StringBuilder otherLabelsBuilder = new StringBuilder();
            otherLabelsBuilder.AppendLine("{");
            otherLabelsBuilder.AppendLine($"\"labelExpression\": \"[{fieldName}]\",");
            //     Align labels above the center of each point
            otherLabelsBuilder.AppendLine("\"labelPlacement\": \"esriServerPointLabelPlacementOnCenter\",");
            //     Use a smaller beige text symbol
            otherLabelsBuilder.AppendLine("\"symbol\": {");
            otherLabelsBuilder.AppendLine($"\"borderLineColor\": [{(int)(borderLineColor.R * 255)},{(int)(borderLineColor.G * 255)},{(int)(borderLineColor.B * 255)},{(int)(borderLineColor.A * 255)}],");
            //borderLineSize
            otherLabelsBuilder.AppendLine($"\"borderLineSize\": {borderLineSize},");

            otherLabelsBuilder.AppendLine($"\"color\": [{(int)(color.R * 255)},{(int)(color.G * 255)},{(int)(color.B * 255)},{(int)(color.A * 255)}],");
            otherLabelsBuilder.AppendLine($"\"font\": {{\"size\": {fontSize} ,\"weight\":\"bold\"}},");
            otherLabelsBuilder.AppendLine("\"type\": \"esriTS\"}");
            //     This definition is for cities with a value of 'N' for the capital attribute

            otherLabelsBuilder.AppendLine("}");

            var json = otherLabelsBuilder.ToString();

            return json;
        }

        private class LayerOrderInfo
        {
            internal FeatureLayer Layer { get; set; }
            internal LayerInfo LayerInfo { get; set; }


        }

        #endregion


        protected override void OnAppearing()
        {
            base.OnAppearing();

            OnAppearingFunc();

            if (Device.RuntimePlatform == Device.UWP)
            {
                //StartLocationDisplay();
            }
        }


        private async void OnAppearingFunc()
        {
            if (!GeneralApplicationSettings.IsFirstTimeSyncDone)
            {
                await Task.Delay(400);

                //force user to update settings first
                await DisplayAlert("", "يجب تحديث الاعدادت اولا", GeneralMessages.Ok);
                UpdateSettingsPage page = new UpdateSettingsPage();
                await Navigation.PushAsync(page, true);
            }

            else
            {
                CrossGeolocator.Current.PositionError += Current_PositionError;
                CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
                if (GeneralApplicationSettings.NeedUpdateLocs)
                {
                    ShowLoading();
                    ChangeLocation();
                    imgLoading.IsVisible = false;
                    GeneralApplicationSettings.NeedUpdateLocs = false;
                }
                else if (GeneralApplicationSettings.NeedUpdateMap)
                {
                    GeneralApplicationSettings.NeedUpdateMap = false;
                    await LoadSamples();
                }

            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (Device.RuntimePlatform == Device.UWP)
            {
                //StopLocationDisplay();
            }

            CrossGeolocator.Current.PositionError -= Current_PositionError;
            CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {

            Device.BeginInvokeOnMainThread(() => { });

        }

        private void Current_PositionError(object sender, Plugin.Geolocator.Abstractions.PositionErrorEventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert(GeneralMessages.Error, "برجاء ضبط اعدادات ال GPS أولا", GeneralMessages.Cancel);


            });

        }

        private async Task<bool> SelectBuildingAsync(string buildingCode, FeatureCollectionLayer featureCollectionLayer ,bool showMsg = true)
        {
            if (string.IsNullOrEmpty(buildingCode) || featureCollectionLayer == null) return false;

            bool found = false;

            QueryParameters compqueryParams = new QueryParameters() { WhereClause = $" {BuildingCodeField}   in ({buildingCode})" };
            foreach (var layer in featureCollectionLayer.Layers)
            {
                //LocalSampleLayer.Layers[0].SelectFeaturesAsync()
                layer.ClearSelection();
                var table = layer.FeatureTable;
                // var  table= layer.t
                FeatureQueryResult compqueryResult = await table.QueryFeaturesAsync(compqueryParams);

                List<Feature> features = compqueryResult.ToList();

                if (features.Any())
                {
                    // Create an envelope.
                    EnvelopeBuilder envBuilder = new EnvelopeBuilder(SpatialReferences.WebMercator);

                    layer.SelectFeatures(features);

                    features.ForEach((feature) => envBuilder.UnionOf(feature.Geometry.Extent));

                    // Zoom to the extent of the selected feature(s).

                    if (features.Count == 1)
                    {
                        found = true;

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //layer.SelectFeatures(features);
                            await MyMapView.SetViewpointGeometryAsync(envBuilder.ToGeometry());
                            await MyMapView.SetViewpointScaleAsync(150);
                        });
                    }

                    break;

                    // ToastManager.LongAlert($"عدد نتائج البحث هو {features.Count()}");
                }

            }

            if (showMsg && !found)
            {
                await DisplayAlert("", "المبنى غير معرف في التجمع الحالي", GeneralMessages.Ok);
            }

            return found;
        }

        private async void LoadMap()
        {
            if (GeneralApplicationSettings.LocationForm == null) return;

            string localityCode = GeneralApplicationSettings.LocationForm.GetLocalityFullCode();

            if (string.IsNullOrEmpty(localityCode) || localityCode.Length != 8)
            {
                await DisplayAlert("", "لا يوجد مناطق متاحة", GeneralMessages.Ok);
                return;
            }

            ResetActiveButtons();

            ShowLoading();
            txtSearch.Text = string.Empty;
            if (featCollectionTable != null)
            {
                MyMapView.Map.OperationalLayers.Remove(featCollectionTable);
                featCollectionTable = null;
            }
            CurrentMapOperation = MapOperation.None;
            var govCode = localityCode.Substring(0, 4);


            //Create new Map
            IFileStorage fileStorage = DependencyService.Get<IFileStorage>();


            string tpkName = govCode;

            if (tpkName.StartsWith("24")) tpkName = "24";

            string tpkFileName = $"{tpkName}.tpk";
            string geoDBFileName = $"{localityCode}.geodatabase";
            bool valid = true;
            string tpkRoot = await fileStorage.GetTPKLocalPath();
            string tpkPath = Path.Combine(tpkRoot, tpkFileName);
            string geoDBRoot = await fileStorage.GetGEOLocalPath();
            string geodatabaseFilePath = string.Empty;

            geodatabaseFilePath = Path.Combine(geoDBRoot, geoDBFileName);
            //}
            //else
            //{

            //     geodatabaseFilePath = Path.Combine(geoDBRoot, govCode, geoDBFileName);
            //}



            FileInfo tpkfileInfo = new FileInfo(tpkPath);
            FileInfo geoDBfileInfo = new FileInfo(geodatabaseFilePath);

            if (!tpkfileInfo.Exists)
            {
                bool success = await fileStorage.MoveTPKToLocal(tpkFileName);

                if (!success)
                {
                    await DisplayAlert("", "حدث خطاء اثناء نقل ملف ت ب ك الى ملف العمل برجاء التاكد من وجود الملف على الجهاز ووجود مساحة تخزين كافية", GeneralMessages.Cancel);

                    valid = false;
                }

            }

            if (!geoDBfileInfo.Exists)
            {
                bool success = await fileStorage.MoveGEODBToLocal(geoDBFileName);
                if (!success)
                {
                    await DisplayAlert("", "حدث خطاء اثناء نقل ملف جيو داتا بيس الى ملف العمل برجاء التاكد من وجود الملف على الجهاز ووجود مساحة تخزين كافية", GeneralMessages.Cancel);

                    valid = false;

                }
            }

            if (valid)
            {
                BuildMap(tpkPath, geodatabaseFilePath);

            }

        }


        private async void BuildMap(string tpkPath, string geodatabaseFilePath)
        {
            Map myMap = new Map();

            var serviceUri = new Uri($"file://{tpkPath}");

            //Create new image layer from the url
            var imageLayer = new ArcGISTiledLayer(serviceUri)
            {
                Name = "TileMapLayer",
                Id = "TileMapLayer"
            };
            imageLayer.LoadStatusChanged += ImageLayer_LoadStatusChanged;
            //imageLayer.Opacity = 0.8f;
            imageLayer.Brightness = 35;
            //Add created layer to the basemaps collection
            myMap.Basemap.BaseLayers.Add(imageLayer);


            //this.MyMapView.LocationDisplay.AutoPanMode = Esri.ArcGISRuntime.UI.LocationDisplayAutoPanMode.Recenter;
            // Assign the map to the MapView
            MyMapView.Map = myMap;

            // Open the mobile geodatabase.
            Geodatabase mobileGeodatabase = await Geodatabase.OpenAsync(geodatabaseFilePath);

            await CreateFeatureLayers(mobileGeodatabase);

            MyMapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Recenter;
            this.MyMapView.LocationDisplay.ShowAccuracy = true;
            this.MyMapView.LocationDisplay.ShowLocation = true;

            this.MyMapView.InteractionOptions = new MapViewInteractionOptions();
            this.MyMapView.InteractionOptions.IsRotateEnabled = false;

            //this.MyMapView.LocationDisplay.UseCourseSymbolOnMovement = true;
            this.MyMapView.LocationDisplay.StatusChanged -= LocationDisplay_StatusChanged;
            this.MyMapView.LocationDisplay.LocationChanged -= LocationDisplay_LocationChanged;
            this.MyMapView.LocationDisplay.StatusChanged += LocationDisplay_StatusChanged;
            this.MyMapView.LocationDisplay.LocationChanged += LocationDisplay_LocationChanged;
            this.MyMapView.IsVisible = true;

            StartLocationDisplay();
        }


        private int LoadedLayers = 0;
        private int LayerCount = 0;

        private async Task CreateFeatureLayers(Geodatabase gdb)
        {

            try
            {
                LoadedLayers = 0;
                LayerCount = 0;
                if (gdb.GeodatabaseFeatureTables.Count<GeodatabaseFeatureTable>() == 0)
                    throw new ApplicationException("Downloaded geodatabase has no feature tables.");
                //if (this.LocalBuildingLayer != null)
                //    this.MyMapView.Map.Layers.Remove((Layer)this.LocalBuildingLayer);
                List<LayerOrderInfo> layerOrderList = new List<LayerOrderInfo>();
                foreach (GeodatabaseFeatureTable geodatabaseFeatureTable in gdb.GeodatabaseFeatureTables.ToList<GeodatabaseFeatureTable>())
                {
                    GeodatabaseFeatureTable table = geodatabaseFeatureTable;

                    if (table.DisplayName.Trim() == "EA_E2018_O") continue;
                    FeatureLayer featureLayer = new FeatureLayer
                    {
                        Id = table.DisplayName,
                        Name = table.DisplayName,
                        FeatureTable = (FeatureTable)table,

                        Opacity = 0.45
                    };
                    FeatureLayer lyr = featureLayer;
                    var tableName = table.DisplayName.Trim();
                    if (LocalBuildingLayerInfo != null && LocalBuildingLayerInfo.TableName == tableName)
                    {
                        lyr.Name = LocalBuildingLayerInfo.DisplayName;
                        this.LocalBuildingLayer = lyr;
                        lyr.Opacity = LocalBuildingLayerInfo.InitialOpicity;
                        lyr.MinScale = 2000;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalBuildingLayerInfo });
                    }
                    else if (LocalBuildingPolygonLayerInfo != null && LocalBuildingPolygonLayerInfo.TableName == tableName)
                    {
                        lyr.Name = LocalBuildingPolygonLayerInfo.DisplayName;
                        this.LocalBuildingPolygonLayer = lyr;
                        lyr.Opacity = LocalBuildingPolygonLayerInfo.InitialOpicity;
                        lyr.MinScale = 2000;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalBuildingPolygonLayerInfo });

                    }
                    else if (LocalBlockLayerInfo != null && LocalBlockLayerInfo.TableName == tableName)
                    {
                        lyr.Name = LocalBlockLayerInfo.DisplayName;
                        this.LocalBlockLayer = lyr;
                        lyr.Opacity = LocalBlockLayerInfo.InitialOpicity;
                        lyr.MinScale = 20000;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalBlockLayerInfo });

                    }
                    else if (LocalOrganalBlockLayerInfo != null && LocalOrganalBlockLayerInfo.TableName == tableName)
                    {
                        lyr.Name = LocalOrganalBlockLayerInfo.DisplayName;
                        this.LocalOrganalBlockLayer = lyr;
                        lyr.Opacity = LocalOrganalBlockLayerInfo.InitialOpicity;
                        lyr.IsVisible = false;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalOrganalBlockLayerInfo });

                    }
                    else if (LocalLocalityLayerInfo != null && LocalLocalityLayerInfo.TableName == tableName)
                    {
                        lyr.Name = LocalLocalityLayerInfo.DisplayName;
                        lyr.Opacity = LocalLocalityLayerInfo.InitialOpicity;
                        this.LocalLocalityLayer = lyr;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalLocalityLayerInfo });

                    }
                    else if (table.DisplayName.Trim() == "الأحياء")
                    {
                        lyr.Name = "الأحياء";
                        lyr.IsVisible = false;
                    }
                    else if (OtherFeaturesLayerInfo != null && OtherFeaturesLayerInfo.TableName == tableName)
                    {
                        lyr.Name = OtherFeaturesLayerInfo.DisplayName;
                        this.LocalOtherLayer = lyr;
                        this.OtherFeaturesLayer = lyr;
                        lyr.MinScale = 1800;
                        lyr.Opacity = OtherFeaturesLayerInfo.InitialOpicity;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = OtherFeaturesLayerInfo });

                    }
                    else if (LocalLandmarksLayerInfo != null && LocalLandmarksLayerInfo.TableName == tableName)
                    {
                        lyr.Name = LocalLandmarksLayerInfo.DisplayName;
                        lyr.Opacity = LocalLandmarksLayerInfo.InitialOpicity;
                        this.LocalLandmarksLayer = lyr;
                        lyr.MinScale = 1800;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalLandmarksLayerInfo });

                    }
                    else if (LocalStreetsLayerInfo != null && LocalStreetsLayerInfo.TableName == tableName)
                    {
                        lyr.Name = LocalStreetsLayerInfo.DisplayName;
                        lyr.Opacity = LocalStreetsLayerInfo.InitialOpicity;
                        this.LocalStreetsLayer = lyr;
                        lyr.MinScale = 3000;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalStreetsLayerInfo });

                    }
                    else if (LocalBarriersInfo != null && LocalBarriersInfo.TableName == tableName)
                    {
                        lyr.Name = LocalBarriersInfo.DisplayName;
                        lyr.Opacity = LocalBarriersInfo.InitialOpicity;
                        this.LocalBarriers = lyr;
                        lyr.MinScale = 3000;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalBarriersInfo });

                    }
                    else if (LocalStartPointLayerInfo != null && LocalStartPointLayerInfo.TableName == tableName)
                    {
                        lyr.Name = LocalStartPointLayerInfo.DisplayName;
                        lyr.Opacity = LocalStartPointLayerInfo.InitialOpicity;
                        this.LocalStartPointLayer = lyr;
                        lyr.MinScale = 2000;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalStartPointLayerInfo });

                    }

                    else if (LocalLayerWallInfo != null && LocalLayerWallInfo.TableName == tableName)
                    {
                        lyr.Name = LocalLayerWallInfo.DisplayName;
                        lyr.Opacity = LocalLayerWallInfo.InitialOpicity;
                        this.LocalLayerWall = lyr;

                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalLayerWallInfo });

                    }
                    else if (LocalIsraeliSettlementInfo != null && LocalIsraeliSettlementInfo.TableName == tableName)
                    {
                        lyr.Name = LocalIsraeliSettlementInfo.DisplayName;
                        lyr.Opacity = LocalIsraeliSettlementInfo.InitialOpicity;
                        this.LocalIsraeliSettlement = lyr;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalIsraeliSettlementInfo });


                    }
                    else if (LocalIsraeliSetlmentOutpostEInfo != null && LocalIsraeliSetlmentOutpostEInfo.TableName == tableName)
                    {
                        lyr.Name = LocalIsraeliSetlmentOutpostEInfo.DisplayName;
                        lyr.Opacity = LocalIsraeliSetlmentOutpostEInfo.InitialOpicity;
                        this.LocalIsraeliSetlmentOutpostE = lyr;
                        layerOrderList.Add(new LayerOrderInfo() { Layer = lyr, LayerInfo = LocalIsraeliSetlmentOutpostEInfo });

                    }
                    else
                        lyr.Opacity = 1.0;
                }

                layerOrderList = layerOrderList.OrderBy(x => x.LayerInfo.Order).ToList();
                LayerCount = layerOrderList.Count;
                //HideLoading();
                //OpenStreetMapLayer openStreetMapLayer = new OpenStreetMapLayer();
                //MyMapView.Map.OperationalLayers.Add(openStreetMapLayer);
                foreach (var lo in layerOrderList)
                {
                    lo.Layer.Loaded += Layer_Loaded;
                    await lo.Layer.LoadAsync();
                    lo.Layer.LoadStatusChanged += Layer_LoadStatusChanged;

                    if (lo.LayerInfo.EnableLabeling)
                    {
                        var json = GetJsonLabelDef(lo.LayerInfo.FieldName, lo.LayerInfo.BorderLineColor, lo.LayerInfo.FontColor, lo.LayerInfo.FontSize, lo.LayerInfo.BorderLineSize);

                        lo.Layer.LabelsEnabled = true;

                        lo.Layer.LabelDefinitions.Clear();
                        lo.Layer.LabelDefinitions.Add(LabelDefinition.FromJson(json));
                    }

                    MyMapView.Map.OperationalLayers.Add(lo.Layer);
                }


                FillSymboligies();

                MyMapView.ViewpointChanged += MyMapView_ViewpointChanged;
                FeatureLayer layer = LocalLocalityLayer;


                if (layer != null)
                {
                    QueryParameters queryParams = new QueryParameters
                    {
                        WhereClause = string.Format("GOVCode   NOT IN ({0})", "-1")
                    };
                    if (layer.FeatureTable is GeodatabaseFeatureTable table)
                    {
                        IEnumerable<Feature> features = await table.QueryFeaturesAsync(queryParams);
                        IEnumerable<Feature> result = features;
                        features = (IEnumerable<Feature>)null;
                        //this._crewTargetOverlay = this.MyMapView.GraphicsOverlays["crewTargetOverlay"];
                        //this._crewTargetOverlay.Graphics.Clear();
                        foreach (Feature feature in result)
                        {
                            Feature f = feature;
                            Graphic g = new Graphic(f.Geometry, (IEnumerable<KeyValuePair<string, object>>)f.Attributes);
                            // this._crewTargetOverlay.Graphics.Add(g);
                            Viewpoint viewpoint1 = new Viewpoint(f.Geometry)
                            {

                            };

                            await this.MyMapView.SetViewpointAsync(viewpoint1);

                        }

                    }

                    layersList.ItemsSource = MyMapView.Map.OperationalLayers.Reverse().Where(x => x.Name != "العينة" && x.Name != "مواقع الأبنية" && x.Name != "Feature Collection");

                    //this.OnMapInitializedHandeler();

                }

            }
            catch
            {

            }
        }

        private void Layer_LoadStatusChanged(object sender, Esri.ArcGISRuntime.LoadStatusEventArgs e)
        {
            if (e.Status == Esri.ArcGISRuntime.LoadStatus.FailedToLoad)
            {
               // Debug.WriteLine(e.Status);
            }
        }

        private async void Layer_Loaded(object sender, EventArgs e)
        {
            if (sender is FeatureLayer)
            {
                var lyr = sender as FeatureLayer;

            }
            LoadedLayers++;

            if (LoadedLayers == LayerCount)
            {
                MyMapView.GeoViewTapped -= MyMapView_GeoViewTapped;
                MyMapView.GeoViewTapped += MyMapView_GeoViewTapped;

                // await LoadSamples();

                //await  LoadCurrentEA();
                //await  LoadStartBuilding();


                await LoadEA();
                //await LoadStartBuilding();
                //if(GeneralApplicationSettings.LocationForm.AppInfo.IsAreaSample == false)
                //    await FillSamplesBuildingColore(GeneralApplicationSettings.LocationForm);
                await LoadSamples();
            }

            HideLoading();

            //   LoadLayers(sender);


        }

        private async Task LoadLayers(object sender)
        {
            if (sender is FeatureLayer)
            {
                var lyr = sender as FeatureLayer;

            }
            LoadedLayers++;

            if (LoadedLayers == LayerCount)
            {
                MyMapView.GeoViewTapped -= MyMapView_GeoViewTapped;
                MyMapView.GeoViewTapped += MyMapView_GeoViewTapped;

                await LoadSamples();

                await LoadEA();
                //await LoadStartBuilding();
            }

            HideLoading();
        }

        private async void MyMapView_GeoViewTapped(object sender, Esri.ArcGISRuntime.Xamarin.Forms.GeoViewInputEventArgs e)
        {
            MyMapView.DismissCallout();

            if (MyMapView.GetCurrentViewpoint(ViewpointType.CenterAndScale).TargetScale > 8000)
            {
                return;
            }

            // StartLocationDisplay();
            GraphicsOverlay _graphicsOverlay = new GraphicsOverlay();
            GeoElement selectedPoint = null;
            if (CurrentMapOperation == MapOperation.Select || CurrentMapOperation == MapOperation.Info)
            {
                var result = await MyMapView.IdentifyLayersAsync(e.Position, 1, false);

                if (result.Count < 1)
                {
                    return;
                }


                var results = result.ToList();

                foreach (var r in results)
                {
                    if (r.LayerContent is FeatureLayer && r.LayerContent.Name == "مواقع الأبنية")
                    {
                        selectedPoint = r.GeoElements.FirstOrDefault();

                    }

                    else if (r.LayerContent is FeatureCollectionLayer)
                    {
                        foreach (var sLR in r.SublayerResults)
                        {
                            selectedPoint = sLR.GeoElements.FirstOrDefault();
                            if (selectedPoint != null) break;
                        }

                    }

                    if (selectedPoint != null) break;


                }

                if (selectedPoint == null)
                {

                    return;
                }


            }

            switch (CurrentMapOperation)
            {
                case MapOperation.Info:
                    {
                        bool polyGonFound = false;

                        var resultPolygon = await MyMapView.IdentifyLayerAsync(LocalBuildingPolygonLayer, e.Position, 2, false);

                        if (resultPolygon.GeoElements.Count > 0)
                        {
                            polyGonFound = true;
                        }

                        GeoElement selectedPolygon = null;
                        if (polyGonFound)
                        {
                            selectedPolygon = resultPolygon.GeoElements.First();
                        }
                        MapPoint mapLocation = e.Location;

                        // Project the user-tapped map point location to a geometry
                        Geometry myGeometry = GeometryEngine.Project(mapLocation, SpatialReferences.Wgs84);

                        // Convert to geometry to a traditional Lat/Long map point
                        MapPoint projectedLocation = (MapPoint)myGeometry;

                        // Format the display callout string based upon the projected map point (example: "Lat: 100.123, Long: 100.234")
                        string mapLocationDescription = string.Format("Lat: {0:F3} Long:{1:F3}", projectedLocation.Y, projectedLocation.X);

                        // Create a new callout definition using the formatted string
                        CalloutDefinition myCalloutDefinition = new CalloutDefinition("معلومات المبنى");

                        StringBuilder stringBuilder = new StringBuilder();

                        if (polyGonFound)
                        {
                            foreach (var dir in BuilingPolyFields)
                            {
                                stringBuilder.AppendLine($"{dir.Value}: {selectedPolygon.Attributes[dir.Key]}");
                            }
                        }


                        foreach (var dir in BuilingInfoFields)
                        {
                            if (selectedPoint.Attributes.ContainsKey(dir.Key))
                                stringBuilder.AppendLine($"{dir.Value}: {selectedPoint.Attributes[dir.Key]}");
                        }

                        stringBuilder.AppendLine($"الموقع: {mapLocationDescription}");

                        myCalloutDefinition.DetailText = stringBuilder.ToString();

                        //myCalloutDefinition.Icon= new RuntimeImage()
                        // Display the callout


                        MyMapView.ShowCalloutForGeoElement(selectedPoint, e.Position, myCalloutDefinition);

                        CurrentMapOperation = MapOperation.None;
                        btnInfoBuildingBtn.Source = GetInfoBuildingNormalImage();
                    }
                    break;
                case MapOperation.Select:
                    {
                        if (selectedPoint.Attributes.ContainsKey(BuildingCodeField))
                        {
                            var val = selectedPoint.Attributes[BuildingCodeField];

                            if (val != null && val.ToString().Trim().Length == 14)
                            {
                                var name = selectedPoint.Attributes[BuildingNameField];
                                var ID3 = selectedPoint.Attributes[BuildingEnumCode];

                                bool isValid = false;

                                string errorMsg =  "لا يمكن اختيار هذا المنبى خارج العينة";

                                if (ID3 != null)
                                {
                                    if (ID3.ToString().Length == 11)
                                    {

                                        if(ApplicationMainSettings.SamplesNotInBuildingsCount() == 0)
                                        {
                                            isValid = GeneralApplicationSettings.LocationForm.GetSamplesBuildingNo().Contains(val);
                                        }
                                        else
                                        {
                                            isValid = true;
                                        }

                                    }
                                }

                                if (isValid)
                                {
                                    await Navigation.PushAsync(new BuildingPage(val.ToString(), name?.ToString(),GeneralApplicationSettings.LocationForm.Locality , selectedPoint.Geometry));
                                }
                                else
                                {
                                    await DisplayAlert(GeneralMessages.Error, errorMsg, GeneralMessages.Cancel);
                                }

                                //await DisplayAlert("", val.ToString(), GeneralMessages.Cancel);
                                // ToastManager.ShortAlert(val.ToString());
                            }
                            else
                            {
                                ToastManager.LongAlert("لا يمكن اختيار هذا المبنى تابع!");
                            }


                            CurrentMapOperation = MapOperation.None;
                            selectBuildingBtn.Source = GetSelectBuildingNormalImage();

                            // Get the first identified graphic
                        }


                    }
                    break;
                case MapOperation.Add:
                    {
                        var result = await MyMapView.IdentifyLayersAsync(e.Position, 1, false);

                        GeoElement selectedEnumArea = null;

                        if (result.Count < 1)
                        {
                            return;
                        }

                        var results = result.ToList();

                        foreach (var r in results)
                        {
                            if (r.LayerContent is FeatureLayer && r.LayerContent.Name == "حدود منطقة العد")
                            {
                                selectedEnumArea = r.GeoElements.FirstOrDefault();

                            }

                            if (selectedEnumArea != null) break;
                        }

                        if (selectedEnumArea == null)
                        {
                            await DisplayAlert(GeneralMessages.Error, "لا يمكن  اضافة مبنى في هذا المكان", GeneralMessages.Cancel);
                            CurrentMapOperation = MapOperation.None;
                            btnAddNewBuilding.Source = GetAddBuildingNormalImage();
                            return;
                        }

                        else
                        {
                            var ID3 = selectedEnumArea.Attributes["EA_GEOCODE"];

                            bool isValid = false;

                            if (ID3 != null)
                            {
                                if (ID3.ToString().Length == 11)
                                {
                                    var intEnumArea = int.Parse(ID3.ToString().Substring(8, 3));
                                    isValid = true;// ID3.ToString() == GeneralApplicationSettings.LocationForm.GetEAFullCode();
                                }
                            }

                            if (!isValid)
                            {
                                await DisplayAlert(GeneralMessages.Error, "لا يمكن  اختيار هذا المبنى خارج منطقة العد", GeneralMessages.Cancel);
                                CurrentMapOperation = MapOperation.None;
                                btnAddNewBuilding.Source = GetAddBuildingNormalImage();
                                return;
                            }
                        }

                        if (GISCurrentLocation.CurrentX.HasValue && GISCurrentLocation.CurrentY.HasValue)
                        {
                            //Esri.ArcGISRuntime.Location.Location=new Esri.ArcGISRuntime.Location.Location()

                            MapPoint mapPoint = new MapPoint(GISCurrentLocation.CurrentX.Value, GISCurrentLocation.CurrentY.Value);
                            MapPoint mpLatLon = Esri.ArcGISRuntime.Geometry.GeometryEngine.Project(e.Location, SpatialReferences.Wgs84) as MapPoint;
                            double distance = GetDistance(this.MyMapView.LocationDisplay.Location.Position, mpLatLon);  //GeometryEngine.Distance(e.Location, mpLatLon);
                            int allowedDistance = 25;
                            if (Security.CurrentUserSettings.CurrentUser.UserName == "9991" || Security.CurrentUserSettings.CurrentUser.UserName == "8881")
                            {
                                allowedDistance = 99999999;
                            }
                            if (distance <= allowedDistance)
                            {
                                bool addNew = await DisplayAlert(GeneralMessages.Question, "هل  تريد الاستمرار في اضافة مبنى؟", GeneralMessages.Yes, GeneralMessages.No);

                                if (addNew)
                                {
                                    AddNewBuilding(e.Location, selectedEnumArea.Attributes["EA_GEOCODE"]?.ToString());
                                }

                            }

                            else
                            {
                                await DisplayAlert(GeneralMessages.Error, "أنت تبعد اكثر من  المسافة المسموحة", GeneralMessages.Cancel);
                            }

                        }

                        else
                        {
                            await DisplayAlert(GeneralMessages.Error, "لا يوجد احداثيات برجاء ضبط الاعدادات أولا", GeneralMessages.Cancel);
                        }

                        CurrentMapOperation = MapOperation.None;
                        btnAddNewBuilding.Source = GetAddBuildingNormalImage();

                    }
                    break;
                case MapOperation.Delete:
                    break;
                case MapOperation.selectBlock:
                    break;
                case MapOperation.following:
                    break;
                default:
                    break;
            }
        }

        private double GetDistance(MapPoint point1, MapPoint point2)
        {
            double currentDistance = 0;

            try
            {
                currentDistance = (Math.Round((6378.7 * Math.Acos(Math.Sin(point1.Y / 57.2958) * Math.Sin(point2.Y / 57.2958) + Math.Cos(point1.Y / 57.2958) * Math.Cos(point2.Y / 57.2958) * Math.Cos(point2.X / 57.2958 - point1.X / 57.2958))) * 1000));
                return currentDistance;
                //currentDistance = (Math.Round((6378.7 * Math.Acos(Math.Sin(point1.Y / 57.2958) * Math.Sin(point2.Y / 57.2958) + Math.Cos(point1.Y / 57.2958) * Math.Cos(point2.Y / 57.2958) * Math.Cos(point2.X / 57.2958 - (point2.X / 57.2958)))) * 1000));
                //return currentDistance;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private void MyMapView_ViewpointChanged(object sender, EventArgs e)
        {
            imgCampos.Rotation = -MyMapView.MapRotation;
        }





        #region  Symbology Functions

        private void FillSymboligies()
        {
            FillEnumerationAreasSymbology();
            //FillBuildingsSymbology();
            FillBuildingsSymbologyPoint(LocalBuildingLayer, Color.FromHex("#428190"));
            FillOtherLayerSymbology();

            FillStreetsSymbology();
            //DisplayLandmarksLabels();
            FillLandmarksSymbology();

            FillOriginalEnumerationAreasSymbology();

            FillLocalitySymbology();
            FillColoniesSymbology();
            FillSubColoniesSymbology();

            FillStartingPointSymbology();
        }

        private void FillSubColoniesSymbology()
        {
            try
            {
                FeatureLayer layer = LocalIsraeliSettlement;
                if (layer == null) return;
                SimpleLineSymbol myBlackSolidOutline = new SimpleLineSymbol
                {
                    Color = Color.Red,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 6.0
                };
                SimpleFillSymbol defaultSym = new SimpleFillSymbol()
                {
                    Color = Color.Black,
                    Outline = myBlackSolidOutline
                };
                UniqueValue uniqueVal1 = new UniqueValue("1", "1", (Symbol)defaultSym, "1");
                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();
                uniqueValRenderer.UniqueValues.Add(uniqueVal1);

                uniqueValRenderer.FieldNames.Add("GOVCODE");

                uniqueValRenderer.DefaultSymbol = (Symbol)defaultSym;

                layer.Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)uniqueValRenderer;
                myBlackSolidOutline = (SimpleLineSymbol)null;

            }
            catch
            {

            }
        }

        private void FillColoniesSymbology()
        {
            try
            {
                FeatureLayer layer = LocalIsraeliSettlement;
                SimpleLineSymbol myBlackSolidOutline = new SimpleLineSymbol
                {
                    Color = Color.Red,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 6.0
                };
                SimpleFillSymbol defaultSym = new SimpleFillSymbol()
                {
                    Color = Color.Black,
                    Outline = myBlackSolidOutline
                };
                UniqueValue uniqueVal1 = new UniqueValue("1", "1", defaultSym, "1");
                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();
                uniqueValRenderer.UniqueValues.Add(uniqueVal1);

                uniqueValRenderer.FieldNames.Add("GOVCODE");

                uniqueValRenderer.DefaultSymbol = defaultSym;

                layer.Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)uniqueValRenderer;

            }
            catch
            {

            }
        }

        private void FillLocalitySymbology()
        {
            try
            {
                FeatureLayer layer = LocalLocalityLayer;


                if (layer == null) return;
                SimpleLineSymbol myBlackSolidOutline = new SimpleLineSymbol
                {
                    Color = Color.DarkRed,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 6.0
                };
                SimpleFillSymbol defaultSym = new SimpleFillSymbol()
                {
                    Color = Color.Transparent,
                    Outline = myBlackSolidOutline
                };
                UniqueValue uniqueVal1 = new UniqueValue("1", "1", defaultSym, "1");
                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();
                uniqueValRenderer.UniqueValues.Add(uniqueVal1);

                uniqueValRenderer.FieldNames.Add("GOVCODE");

                uniqueValRenderer.DefaultSymbol = (Symbol)defaultSym;
                layer.Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)uniqueValRenderer;

            }
            catch
            {

            }
        }



        private void FillOriginalEnumerationAreasSymbology()
        {
            // throw new NotImplementedException();
        }


        private void FillStartingPointSymbology()
        {
            try
            {
                FeatureLayer layer = LocalStartPointLayer;
                if (layer == null) return;
                SimpleLineSymbol myBlackSolidOutline = new SimpleLineSymbol
                {
                    Color = Color.Red,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 5.0
                };
                SimpleMarkerSymbol defaultSym = new SimpleMarkerSymbol()
                {
                    Color = Color.GreenYellow,
                    Size = 20.0,
                    Outline = myBlackSolidOutline,
                    Style = SimpleMarkerSymbolStyle.Triangle
                };

                int num = 1;
                var uniqueVal1 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)defaultSym, num.ToString());

                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();
                uniqueValRenderer.UniqueValues.Add(uniqueVal1);
                uniqueValRenderer.FieldNames.Add("EA_GEOCODE");

                uniqueValRenderer.DefaultSymbol = (Symbol)defaultSym;

                layer.Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)uniqueValRenderer;
                myBlackSolidOutline = (SimpleLineSymbol)null;

            }
            catch
            {

            }
        }




        private async void FillLandmarksSymbology()
        {
            try
            {
                if (LocalLandmarksLayer == null) return;
                var dbpath = DependencyService.Get<IDatabaseSettings>().DatabaseFolderPath;


                var path = Path.Combine(dbpath, $"redstickpin.png");
                var stream = File.OpenRead(path);
                //byte[] pushImgdata = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RedPushPin.png"));
                PictureMarkerSymbol stickPinSymbol = await PictureMarkerSymbol.CreateAsync(stream);
                stickPinSymbol.Width = 24.0;
                stickPinSymbol.Height = 24.0;

                // PictureMarkerSymbol stickPinSymbol = pictureMarkerSymbol1;

                //PictureMarkerSymbol pictureMarkerSymbol2 = new PictureMarkerSymbol(RedStickpinURI)
                //{
                //    Width = 24.0,
                //    Height = 24.0,
                //    OffsetX = 0.0,
                //    OffsetY = 0.0
                //};

                SimpleRenderer myRenderer = new SimpleRenderer
                {
                    Symbol = stickPinSymbol
                };
                this.LocalLandmarksLayer.Renderer = myRenderer;

            }
            catch
            {

            }
        }



        private void FillStreetsSymbology()
        {
            try
            {
                if (LocalStreetsLayer == null) return;

                SimpleLineSymbol simpleLineSymbol = new SimpleLineSymbol
                {
                    Color = Color.YellowGreen,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 4.0
                };
                SimpleLineSymbol type1 = new SimpleLineSymbol()
                {
                    Width = 5.0,
                    Style = SimpleLineSymbolStyle.Solid,
                    Color = Color.FromHex("#5b0a0a")
                };
                SimpleLineSymbol type2 = new SimpleLineSymbol()
                {
                    Width = 4.0,
                    Style = SimpleLineSymbolStyle.Dash,
                    Color = Color.FromHex("#ff6c00")
                };
                SimpleLineSymbol type3 = new SimpleLineSymbol()
                {
                    Width = 5.0,
                    Style = SimpleLineSymbolStyle.DashDot,
                    Color = Color.Yellow
                };
                SimpleLineSymbol defaultSym = new SimpleLineSymbol()
                {
                    Width = 2.0,
                    Style = SimpleLineSymbolStyle.DashDot,
                    Color = Color.Yellow
                };
                int num = 1;
                var uniqueVal1 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)type1, num.ToString());
                num = 2;
                var uniqueVal2 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)type2, num.ToString());
                num = 3;
                var uniqueVal3 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)type3, num.ToString());
                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();
                uniqueValRenderer.UniqueValues.Add(uniqueVal1);
                uniqueValRenderer.UniqueValues.Add(uniqueVal2);
                uniqueValRenderer.UniqueValues.Add(uniqueVal3);

                uniqueValRenderer.FieldNames.Add("ROADTYPE_DOMAIN");

                uniqueValRenderer.DefaultSymbol = (Symbol)defaultSym;
                this.LocalStreetsLayer.Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)uniqueValRenderer;

            }
            catch
            {

            }
        }


        private void FillOtherLayerSymbology()
        {
            try
            {
                if (LocalOtherLayer == null) return;

                SimpleLineSymbol simpleLineSymbol = new SimpleLineSymbol
                {
                    Color = Color.Blue,
                    Style = SimpleLineSymbolStyle.DashDot,
                    Width = 6.0
                };
                SimpleLineSymbol sekka = new SimpleLineSymbol()
                {
                    Width = 6.0,
                    Style = SimpleLineSymbolStyle.Dash,

                    Color = Color.FromHex("#cc0d0d")
                };
                SimpleLineSymbol water = new SimpleLineSymbol()
                {
                    Width = 6.0,
                    Style = SimpleLineSymbolStyle.Dot,
                    Color = Color.FromHex("#0000ff")
                };
                SimpleLineSymbol stair = new SimpleLineSymbol()
                {
                    Width = 6.0,
                    Style = SimpleLineSymbolStyle.DashDot,
                    Color = Color.FromHex("#00ffce")
                };
                SimpleLineSymbol defaultSym = new SimpleLineSymbol()
                {
                    Width = 6.0,
                    Style = SimpleLineSymbolStyle.DashDot,
                    Color = Color.Blue
                };
                int num = 2;
                UniqueValue uniqueVal1 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)sekka, num.ToString());
                num = 3;
                UniqueValue uniqueVal2 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)water, num.ToString());
                num = 1;
                UniqueValue uniqueVal3 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)stair, num.ToString());
                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();
                uniqueValRenderer.UniqueValues.Add(uniqueVal1);
                uniqueValRenderer.UniqueValues.Add(uniqueVal2);
                uniqueValRenderer.UniqueValues.Add(uniqueVal3);

                uniqueValRenderer.FieldNames.Add("FEATURE_TYPE_DOMAIN");

                uniqueValRenderer.DefaultSymbol = defaultSym;
                this.LocalOtherLayer.Renderer = uniqueValRenderer;


            }
            catch
            {

            }
        }

        private void FillBuildingsSymbologyPoint(FeatureLayer layer, Color color)
        {
            try
            {
                if (layer == null) return;


                SimpleMarkerSymbol defaultSym = new SimpleMarkerSymbol()
                {
                    Color = color,
                    Style = SimpleMarkerSymbolStyle.Circle,
                    Size = 50.0
                };

                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();



                uniqueValRenderer.DefaultSymbol = (Symbol)defaultSym;
                layer.Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)uniqueValRenderer;




            }
            catch
            {

            }
        }

        private async void FillBuildingsPicturePoint(FeatureLayer layer, Color color)
        {
            try
            {
                if (layer == null) return;
                // System.Uri myPictureUri = new System.Uri("http://static.arcgis.com/images/Symbols/Transportation/SkullandCrossbones.png");






                var dbpath = DependencyService.Get<IDatabaseSettings>().DatabaseFolderPath;


                var path = Path.Combine(dbpath, $"balloon.png");
                var stream = File.OpenRead(path);


                var imgSym = await PictureMarkerSymbol.CreateAsync(stream);

                imgSym.Width = 70;
                imgSym.Height = 114;

                //var symbolUri = new Uri(
                //  "https://sampleserver6.arcgisonline.com/arcgis/rest/services/Recreation/FeatureServer/0/images/e82f744ebb069bb35b234b3fea46deae");

                //// Create new symbol using asynchronous factory method from uri.
                //PictureMarkerSymbol campsiteSymbol = new PictureMarkerSymbol(symbolUri)
                //{
                //    Width = 40,
                //    Height = 40
                //};

                //SimpleMarkerSymbol defaultSym = new SimpleMarkerSymbol()
                //{
                //    Color = color,
                //    Style = SimpleMarkerSymbolStyle.Circle,
                //    Size = 50.0
                //};

                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();



                uniqueValRenderer.DefaultSymbol = (Symbol)imgSym;
                layer.Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)uniqueValRenderer;




            }
            catch
            {

            }
        }






        private void FillBuildingsSymbology()
        {
            try
            {
                if (LocalBuildingLayer == null) return;

                SimpleLineSymbol myBlackSolidOutline = new SimpleLineSymbol
                {
                    Color = Color.Black,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 4.0
                };
                SimpleFillSymbol notCompletedSym = new SimpleFillSymbol()
                {
                    Color = Color.Yellow,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol completedSym = new SimpleFillSymbol()
                {
                    Color = Color.GreenYellow,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol notVisitedSym = new SimpleFillSymbol()
                {
                    Color = Color.AliceBlue,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol deletedSym = new SimpleFillSymbol()
                {
                    Color = Color.Red,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol updatedSym = new SimpleFillSymbol()
                {
                    Color = Color.BlueViolet,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol defaultSym = new SimpleFillSymbol()
                {
                    Color = Color.AliceBlue,
                    Outline = myBlackSolidOutline
                };
                int num = 1;
                UniqueValue uniqueVal1 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)notVisitedSym, num.ToString());
                num = 2;
                UniqueValue uniqueVal2 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)notCompletedSym, num.ToString());
                num = 3;
                UniqueValue uniqueVal3 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)completedSym, num.ToString());
                num = 4;
                UniqueValue uniqueVal4 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)deletedSym, num.ToString());

                UniqueValueRenderer uniqueValRenderer = new UniqueValueRenderer();
                uniqueValRenderer.FieldNames.Add("INCLOSESTATUS");
                uniqueValRenderer.UniqueValues.Add(uniqueVal1);
                uniqueValRenderer.UniqueValues.Add(uniqueVal2);
                uniqueValRenderer.UniqueValues.Add(uniqueVal3);
                uniqueValRenderer.UniqueValues.Add(uniqueVal4);
                uniqueValRenderer.DefaultSymbol = defaultSym;
                this.LocalBuildingPolygonLayer.Renderer = uniqueValRenderer;



            }
            catch
            {

            }
        }
        public void FillEnumerationAreasSymbology()
        {
            try
            {

                if (LocalBlockLayer == null) return;

                SimpleLineSymbol myBlackSolidOutline = new SimpleLineSymbol
                {
                    Color = Color.Black,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 4.0
                };
                SimpleFillSymbol notCompletedSym = new SimpleFillSymbol()
                {
                    Color = Color.Yellow,

                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol completedSym = new SimpleFillSymbol()
                {
                    Color = Color.DarkGreen,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol notVisitedSym = new SimpleFillSymbol()
                {
                    Color = Color.AliceBlue,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol confirmedSym = new SimpleFillSymbol()
                {
                    Color = Color.OrangeRed,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol returnedSym = new SimpleFillSymbol()
                {
                    Color = Color.BlueViolet,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol defaultSym = new SimpleFillSymbol()
                {
                    Color = Color.SkyBlue,
                    Outline = myBlackSolidOutline
                };
                int num = 1;


                UniqueValue uniqueVal1 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)notVisitedSym, num.ToString());
                num = 2;
                UniqueValue uniqueVal2 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)notCompletedSym, num.ToString());
                num = 3;
                UniqueValue uniqueVal3 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)completedSym, num.ToString());
                num = 5;
                UniqueValue uniqueVal4 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)confirmedSym, num.ToString());
                num = 6;
                UniqueValue uniqueVal5 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)returnedSym, num.ToString());

                UniqueValueRenderer valueRenderer = new UniqueValueRenderer();


                valueRenderer.UniqueValues.Add(uniqueVal1);
                valueRenderer.UniqueValues.Add(uniqueVal2);
                valueRenderer.UniqueValues.Add(uniqueVal3);
                valueRenderer.UniqueValues.Add(uniqueVal4);
                valueRenderer.UniqueValues.Add(uniqueVal5);




                valueRenderer.FieldNames.Add("INITIALSTA");

                valueRenderer.DefaultSymbol = defaultSym;
                this.LocalBlockLayer.Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)valueRenderer;

            }
            catch
            {

            }
        }


        public void FillEnumerationAreasSearchSymbology()
        {
            try
            {
                if (LocalBlockLayer == null) return;

                SimpleLineSymbol myBlackSolidOutline = new SimpleLineSymbol
                {
                    Color = Color.Black,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 4.0
                };
                SimpleFillSymbol notCompletedSym = new SimpleFillSymbol()
                {
                    Color = Color.Yellow,

                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol completedSym = new SimpleFillSymbol()
                {
                    Color = Color.DarkGreen,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol notVisitedSym = new SimpleFillSymbol()
                {
                    Color = Color.AliceBlue,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol confirmedSym = new SimpleFillSymbol()
                {
                    Color = Color.OrangeRed,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol returnedSym = new SimpleFillSymbol()
                {
                    Color = Color.BlueViolet,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol defaultSym = new SimpleFillSymbol()
                {
                    Color = Color.RosyBrown,
                    Outline = myBlackSolidOutline
                };
                int num = 1;


                UniqueValue uniqueVal1 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)notVisitedSym, num.ToString());
                num = 2;
                UniqueValue uniqueVal2 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)notCompletedSym, num.ToString());
                num = 3;
                UniqueValue uniqueVal3 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)completedSym, num.ToString());
                num = 5;
                UniqueValue uniqueVal4 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)confirmedSym, num.ToString());
                num = 6;
                UniqueValue uniqueVal5 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)returnedSym, num.ToString());

                UniqueValueRenderer valueRenderer = new UniqueValueRenderer();


                valueRenderer.UniqueValues.Add(uniqueVal1);
                valueRenderer.UniqueValues.Add(uniqueVal2);
                valueRenderer.UniqueValues.Add(uniqueVal3);
                valueRenderer.UniqueValues.Add(uniqueVal4);
                valueRenderer.UniqueValues.Add(uniqueVal5);




                valueRenderer.FieldNames.Add("INITIALSTA");

                valueRenderer.DefaultSymbol = defaultSym;
                featCollectionTable.Layers[0].Renderer = (Esri.ArcGISRuntime.Symbology.Renderer)valueRenderer;

            }
            catch
            {

            }
        }


        public void FillEnumerationAreasStartSymbology()
        {
            try
            {
                if (LocalBlockLayer == null) return;

                SimpleLineSymbol myBlackSolidOutline = new SimpleLineSymbol
                {
                    Color = Color.Black,
                    Style = SimpleLineSymbolStyle.Solid,
                    Width = 4.0
                };
                SimpleFillSymbol notCompletedSym = new SimpleFillSymbol()
                {
                    Color = Color.Yellow,

                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol completedSym = new SimpleFillSymbol()
                {
                    Color = Color.DarkGreen,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol notVisitedSym = new SimpleFillSymbol()
                {
                    Color = Color.Violet,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol confirmedSym = new SimpleFillSymbol()
                {
                    Color = Color.OrangeRed,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol returnedSym = new SimpleFillSymbol()
                {
                    Color = Color.BlueViolet,
                    Outline = myBlackSolidOutline
                };
                SimpleFillSymbol defaultSym = new SimpleFillSymbol()
                {
                    Color = Color.RosyBrown,
                    Outline = myBlackSolidOutline
                };
                int num = 1;


                UniqueValue uniqueVal1 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)notVisitedSym, num.ToString());
                num = 2;
                UniqueValue uniqueVal2 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)notCompletedSym, num.ToString());
                num = 3;
                UniqueValue uniqueVal3 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)completedSym, num.ToString());
                num = 5;
                UniqueValue uniqueVal4 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)confirmedSym, num.ToString());
                num = 6;
                UniqueValue uniqueVal5 = new UniqueValue(num.ToString(), num.ToString(), (Symbol)returnedSym, num.ToString());

                UniqueValueRenderer valueRenderer = new UniqueValueRenderer();


                valueRenderer.UniqueValues.Add(uniqueVal1);
                valueRenderer.UniqueValues.Add(uniqueVal2);
                valueRenderer.UniqueValues.Add(uniqueVal3);
                valueRenderer.UniqueValues.Add(uniqueVal4);
                valueRenderer.UniqueValues.Add(uniqueVal5);




                valueRenderer.FieldNames.Add("INITIALSTA");

                valueRenderer.DefaultSymbol = defaultSym;
                featEAStartCollectionTable.Layers[0].Renderer = valueRenderer;

            }
            catch
            {

            }
        }

        public async void StopLocationDisplay()
        {
            try
            {
                if (MyMapView.LocationDisplay.Started)
                {
                    var locationDataSource = MyMapView.LocationDisplay.DataSource;

                    try
                    {
                        await locationDataSource.StopAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                    //MyMapView.LocationDisplay.IsEnabled = true;

                }
            }
            catch
            {

            }
        }

        public async void StartLocationDisplay()
        {
            try
            {
                if (MyMapView.LocationDisplay.Started == false)
                {
                    var locationDataSource = MyMapView.LocationDisplay.DataSource;

                    try
                    {
                        await locationDataSource.StartAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                    //MyMapView.LocationDisplay.IsEnabled = true;

                }
            }
            catch
            {

            }
        }

        private void LocationDisplay_StatusChanged(object sender, Esri.ArcGISRuntime.Location.StatusChangedEventArgs e)
        {
            if (e.IsStarted == false)
            {
                StartLocationDisplay();
            }
        }



        #endregion




        public void zoomIN()
        {
            var scale = MyMapView.GetCurrentViewpoint(ViewpointType.CenterAndScale).TargetScale;
            if (scale > 10000)
            {
                scale -= 5000;
            }
            else
            {
                scale -= 1200;
            }

            if (scale < 100) scale = 100;

            MyMapView.SetViewpointScaleAsync(scale);




            // this.MyMapView.ZoomAsync(2.0);

        }

        public void zoomOut()
        {

            var scale = MyMapView.GetCurrentViewpoint(ViewpointType.CenterAndScale).TargetScale;
            if (scale < 10000)
            {
                scale += 1200;
            }
            else
            {
                scale += 5000;

            }

            if (scale > 50000) scale = 50000;

            MyMapView.SetViewpointScaleAsync(scale);
        }

        private void ImageLayer_LoadStatusChanged(object sender, Esri.ArcGISRuntime.LoadStatusEventArgs e)
        {

        }

        private void TileLayer_LoadStatusChanged(object sender, Esri.ArcGISRuntime.LoadStatusEventArgs e)
        {

        }

        private void LocationDisplay_LocationChanged(object sender, Esri.ArcGISRuntime.Location.Location e)
        {

            if (e.Position != null && e.Position != null)
            {
                GISCurrentLocation.CurrentX = e.Position.X;
                GISCurrentLocation.CurrentY = e.Position.Y;
            }

            else
            {

            }

            //lblLocation.Text = val;
        }

        private void StopButton_Clicked(object sender, EventArgs e)
        {

        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {

        }

        private void zoomInBtn_Clicked(object sender, EventArgs e)
        {
            zoomIN();
        }

        private void zoomOutBtn_Clicked(object sender, EventArgs e)
        {
            zoomOut();
        }

        private void navigationViewBtn_Clicked(object sender, EventArgs e)
        {
            this.MyMapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Navigation;
            MyMapView.SetViewpointScaleAsync(1200);
        }

        private void fullExtentBtn_Clicked(object sender, EventArgs e)
        {
            Viewpoint viewpoint = new Viewpoint((Esri.ArcGISRuntime.Geometry.Geometry)this.MyMapView.Map.Basemap.BaseLayers["TileMapLayer"].FullExtent);

            this.MyMapView.SetViewpointAsync(viewpoint);
            this.MyMapView.Rotation = 0.0;
        }

        private void layersBtn_Clicked(object sender, EventArgs e)
        {
            var visible = panelLegend.IsVisible;
            HideAllPopUpPanels();

            if (!visible)
            {
                panelOverlay.IsVisible = true;
                panelLegend.IsVisible = true;
            }

        }

        private void selectBuildingBtn_Clicked(object sender, EventArgs e)
        {
            ToastManager.ShortAlert("يرجى الضغط على المبنى المراد الدخول اليه");

            CurrentMapOperation = MapOperation.Select;

            if (selectBuildingBtn.Source.ToString() != "File: " + GetSelectBuildingActiveImage())
            {
                ResetActiveButtons();
                selectBuildingBtn.Source = GetSelectBuildingActiveImage();
            }
        }

        private void btnInfoBuildingBtn_Clicked(object sender, EventArgs e)
        {
            ToastManager.ShortAlert("يرجى الضغط على المبنى لاظهار معلوماته");


            CurrentMapOperation = MapOperation.Info;

            if (btnInfoBuildingBtn.Source.ToString() != "File: " + GetInfoBuildingActiveImage())
            {
                ResetActiveButtons();
                btnInfoBuildingBtn.Source = GetInfoBuildingActiveImage();
            }
        }

        private void ShowLoading()
        {
            MyMapView.IsVisible = false;
            btnInfoBuildingBtn.IsVisible = false;
            fullExtentBtn.IsVisible = false;
            layersBtn.IsVisible = false;
            zoomInBtn.IsVisible = false;
            zoomOutBtn.IsVisible = false;
            navigationViewBtn.IsVisible = false;
            selectBuildingBtn.IsVisible = false;
            panelLegend.IsVisible = false;
            btnFind.IsVisible = false;
            btnSamples.IsVisible = false;
            //btnUnknownSamples.IsVisible = false;
            imgLoading.IsVisible = true;
            btnAddNewBuilding.IsVisible = false;

            btnChart.IsVisible = false;
            btnBrightness.IsVisible = false;
            pnlMapLegand.IsVisible = false;

            HideAllPopUpPanels();

        }
        private void HideLoading()
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                MyMapView.IsVisible = true;
                btnInfoBuildingBtn.IsVisible = true;
                fullExtentBtn.IsVisible = true;
                layersBtn.IsVisible = true;
                zoomInBtn.IsVisible = true;
                zoomOutBtn.IsVisible = true;
                navigationViewBtn.IsVisible = true;
                selectBuildingBtn.IsVisible = true;
                btnFind.IsVisible = true;
                btnSamples.IsVisible = true;
                //  btnUnknownSamples.IsVisible = true;
                imgLoading.IsVisible = false;
                //imgLoading.WidthRequest = 10;
                btnAddNewBuilding.IsVisible = true;
                btnChart.IsVisible = true;

                btnBrightness.IsVisible = true;

                pnlMapLegand.IsVisible = true;

            });

        }

        private void btnFind_Clicked(object sender, EventArgs e)
        {
            var visible = panelFindLocation.IsVisible;
            HideAllPopUpPanels();

            if (!visible)
            {
                panelFindLocation.IsVisible = true;
                panelOverlay.IsVisible = true;

                if (cmbLayerId.SelectedIndex < 0)
                {
                    cmbLayerId.SelectedIndex = 0;
                }

                //if (featCollectionTable != null)
                //{
                //    if (!MyMapView.Map.OperationalLayers.Contains(featCollectionTable))
                //    {
                //        MyMapView.Map.OperationalLayers.Add(featCollectionTable);
                //    }
                //}
            }

        }

        private void btnUnKnownSamples_Clicked(object sender, EventArgs e)
        {


            // Navigation.PushAsync(new  UnknownSamplePage(CurrentLocality));
        }
        private void btnChangeLocation_Clicked(object sender, EventArgs e)
        {
            ChangeLocation();


        }



        private async void btnSettings_Clicked(object sender, EventArgs e)
        {
            UpdateSettingsPage updateSettings = new UpdateSettingsPage();
            await Navigation.PushAsync(updateSettings, true);
        }

        private void btnSamples_Clicked(object sender, EventArgs e)
        {
            SampleListPage sampleListPage = new SampleListPage(GeneralApplicationSettings.LocationForm.Locality);
            Navigation.PushAsync(sampleListPage);
        }



        private async void btnCollapse_Tapped(object sender, EventArgs e)
        {

            RelativeLayout pnlToolBar = this.FindByName<RelativeLayout>("pnlVerticalToolBar");
            Image btnCollapse = this.FindByName<Image>("btnCollapse");

            if (pnlToolBar.Opacity == 1)
            {
                bool result = await Task.Run(() =>
                {
                    pnlToolBar.FadeTo(0, 1000);
                    return btnCollapse.RelRotateTo(180, 1000);


                });


                if (result || !result)
                {
                    pnlToolBar.IsVisible = false;
                }





            }

            else if (pnlToolBar.Opacity == 0)
            {
                pnlToolBar.IsVisible = true;
                await Task.Run(() =>
                {
                    pnlToolBar.FadeTo(1, 1000);
                    btnCollapse.RelRotateTo(180, 1000);


                });



            }
        }

        private void btnNotVisied_Tapped(object sender, EventArgs e)
        {
            OnOffImage(btnNotVisited, "notvisited.png", "notvisited_off.png");

            ShowNotVisitedLayer = !ShowNotVisitedLayer;
            if (LocalSampleLayer != null)
            {
                if (LocalSampleLayer.FeatureCollection.Tables.Contains(LocalNotVisitedLayerTable))
                {
                    LocalSampleLayer.FeatureCollection.Tables.Remove(LocalNotVisitedLayerTable);
                    LocalNotVisitedLayer = null;
                }
                else
                {

                    LocalSampleLayer.FeatureCollection.Tables.Add(LocalNotVisitedLayerTable);
                    LocalNotVisitedLayer = LocalSampleLayer.Layers[LocalSampleLayer.Layers.Count - 1];

                }
            }

        }
        private void btnNotComplete_Tapped(object sender, EventArgs e)
        {

            OnOffImage(btnNotCompleted, "notcomplete.png", "notcomplete_off.png");
            ShowNotCompletedLayer = !ShowNotCompletedLayer;

            if (LocalSampleLayer != null)
            {
                if (LocalSampleLayer.FeatureCollection.Tables.Contains(LocalNotCompletedLayerTable))
                {
                    LocalSampleLayer.FeatureCollection.Tables.Remove(LocalNotCompletedLayerTable);

                    LocalNotCompletedLayer = null;

                }
                else
                {
                    LocalSampleLayer.FeatureCollection.Tables.Add(LocalNotCompletedLayerTable);
                    LocalNotCompletedLayer = LocalSampleLayer.Layers[LocalSampleLayer.Layers.Count - 1];

                }
            }
        }
        private void btnCompleted_Tapped(object sender, EventArgs e)
        {
            ShowCompletedLayer = !ShowCompletedLayer;
            OnOffImage(btnCompleted, "complete.png", "complete_off.png");

            if (LocalSampleLayer != null)
            {

                if (LocalSampleLayer.FeatureCollection.Tables.Contains(LocalCompletedLayerTable))
                {
                    LocalSampleLayer.FeatureCollection.Tables.Remove(LocalCompletedLayerTable);
                    LocalCompletedLayer = null;

                }
                else
                {
                    LocalSampleLayer.FeatureCollection.Tables.Add(LocalCompletedLayerTable);
                    LocalCompletedLayer = LocalSampleLayer.Layers[LocalSampleLayer.Layers.Count - 1];




                }

            }
        }
        private void btnNotSsample_Tapped(object sender, EventArgs e)
        {
            OnOffImage(btnNotSample, "notsample.png", "notsample_off.png");
            ShowAllLayers = !ShowAllLayers;

            LocalBuildingLayer.IsVisible = !LocalBuildingLayer.IsVisible;
        }

        private void OnOffImage(Image image, string onImage, string offImage)
        {
            var imgFile = ((FileImageSource)image.Source);
            bool isOff = imgFile.File.Contains("off");

            string imagePath = onImage;
            if (!isOff)
            {
                imagePath = offImage;
            }

            if (Device.RuntimePlatform == Device.UWP)
            {
                imgFile.File = $"images/{imagePath}";
            }
            else
            {
                imgFile.File = $"{imagePath}";
            }

        }


        private void HideAllPopUpPanels()
        {

            if (panelFindLocation.IsVisible && featCollectionTable != null)
            {
                MyMapView.Map.OperationalLayers.Remove(featCollectionTable);

            }
            panelLegend.IsVisible = false;
            panelChangeLocation.IsVisible = false;
            panelFindLocation.IsVisible = false;
            panelAddNewBuilding.IsVisible = false;
            panelChart.IsVisible = false;
            panelBrightness.IsVisible = false;
            panelSearchMultiResult.IsVisible = false;
            panelOverlay.IsVisible = false;

            //panelUnkNowSamples.IsVisible = false;
        }

        private async void btnLogo_Tapped(object sender, EventArgs e)
        {
            StackLayout pnlMapLegand2 = this.FindByName<StackLayout>("pnlMapLegand2");
            Image btnLogo = (Image)sender;

            if (pnlMapLegand2.Opacity == 1)
            {
                bool result = await Task.Run(() =>
                {
                    return pnlMapLegand2.FadeTo(0, 1000);



                });


                if (result || !result)
                {
                    pnlMapLegand2.IsVisible = false;
                }





            }

            else if (pnlMapLegand2.Opacity == 0)
            {
                pnlMapLegand2.IsVisible = true;
                await Task.Run(() =>
                {
                    pnlMapLegand2.FadeTo(1, 1000);



                });



            }
        }


        #region  Change Locality


        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            HideAllPopUpPanels();
        }

        private void FillLocationForm()
        {
            if (GeneralApplicationSettings.NeedUpdateLocs)
            {
                LocationForm_FillGovernorates();
            }

            SelectLocationFormGovPickerValue();
            SelectLocationFormLocalityPickerValue();
            //SelectLocationFormEnumPickerValue();
        }


        private void LocationForm_FillGovernorates()
        {
            var govs = new List<Governorate>();

            var db = DataBase.GetConnection();

            var govsCodes = db.Table<SampleInfo>().Select(x => x.ID01).Distinct().ToList();

            var govsGroups = db.Table<Governorate>().Where(x => govsCodes.Contains(x.Code)).GroupBy(x => x.Code).ToList();
            govs = govsGroups.Select(x => x.First()).ToList();

            if (!govs.Any())
            {
                cmbGov.IsEnabled = false;
            }
            else
            {
                cmbGov.IsEnabled = true;
            }

            cmbGov.ItemDisplayBinding = new Binding("Description");
            cmbGov.ItemsSource = govs;

            SelectLocationFormGovPickerValue();
        }

        private void LocationForm_FillLocalities()
        {
            Governorate gov = cmbGov.SelectedItem as Governorate;

            var localities = new List<Locality>();

            if (gov != null)
            {
                var db = DataBase.GetConnection();

                var localitiesCodes = db.Table<SampleInfo>().Where(x =>  x.ID01 == gov.Code).Select(x => x.ID02).Distinct().ToList();

                localities = db.Table<Locality>().Where(x => localitiesCodes.Contains(x.Code)).Distinct().ToList();
            }

            //if (!localities.Any())
            //{
            //    cmbLocality.IsEnabled = cmbEnumArea.IsEnabled = false;
            //}
            //else
            //{
            //    cmbLocality.IsEnabled = cmbEnumArea.IsEnabled = true;
            //}

            cmbLocality.ItemDisplayBinding = new Binding("Description");
            cmbLocality.ItemsSource = localities;

            SelectLocationFormLocalityPickerValue();
        }

        //private void LocationForm_FillEnumAreas()
        //{
        //    Governorate gov = cmbGov.SelectedItem as Governorate;
        //    Locality locality = cmbLocality.SelectedItem as Locality;

        //    var enumAreas = new List<string>();

        //    if ( gov != null && locality != null)
        //    {
        //        var db = DataBase.GetConnection();

        //        enumAreas = db.Table<SampleInfo>().Where(x =>  x.ID1 == gov.Code && x.E3 == locality.Code).Select(x => x.ID3).Distinct().Select(x => x.ToString()).ToList();
        //    }

        //    cmbEnumArea.ItemsSource = enumAreas;
        //    SelectLocationFormEnumPickerValue();
        //}

        private void cmbApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationForm_FillGovernorates();
            SetBtnOkEnableStatus();
        }

        private void cmbGovernorate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationForm_FillLocalities();
            SetBtnOkEnableStatus();
        }

        private void cmbLocality_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LocationForm_FillEnumAreas();
            SetBtnOkEnableStatus();
        }

        //private void cmbEnumArea_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SetBtnOkEnableStatus();
        //}

        private async void btnOk_Clicked(object sender, EventArgs e)
        {
            int records = await GetSamplesCountFromLocationForm();
            if (records == 0)
            {
                await DisplayAlert("", GeneralMessages.NoSamplesError, GeneralMessages.Ok);
                return;
            }

            if (GeneralApplicationSettings.LocationForm == null)
            {
                GeneralApplicationSettings.LocationForm = new LocationForm();
            }

            GeneralApplicationSettings.LocationForm.Governorate = (Governorate)cmbGov.SelectedItem;
            GeneralApplicationSettings.LocationForm.Locality = (Locality)cmbLocality.SelectedItem;
            //GeneralApplicationSettings.LocationForm.EnumArea = int.Parse(cmbEnumArea.SelectedItem.ToString());


            GeneralApplicationSettings.LocationForm.Init();

            HideAllPopUpPanels();
            LoadMap();
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            upateSettingsAIndicator.IsVisible = true;
            //btnUpdate.IsVisible = btnOk.IsVisible = false;

            if (await GeneralFunctions.UpdateSamples())
            {
                ToastManager.LongAlert(GeneralMessages.DownloadSuccess);
                LocationForm_FillGovernorates();
            }
            else
            {
                ToastManager.LongAlert(GeneralMessages.NoSamplesError);
            }

            SetBtnOkEnableStatus();

            upateSettingsAIndicator.IsVisible = false;
            //btnUpdate.IsVisible = btnOk.IsVisible = true;
        }

        private void SetBtnOkEnableStatus()
        {
            //if (cmbGov.SelectedItem == null || cmbLocality.SelectedItem == null || cmbEnumArea.SelectedItem == null)

            if (cmbGov.SelectedItem == null || cmbLocality.SelectedItem == null)
            {
                btnOk.IsEnabled = false;
                return;
            }

            btnOk.IsEnabled = true;
        }


        private void SelectLocationFormGovPickerValue()
        {
            if (GeneralApplicationSettings.LocationForm == null)
            {
                cmbGov.SelectedIndex = 0;
            }
            else
            {
                List<Governorate> governorates = cmbGov.ItemsSource as List<Governorate>;
                var selected = governorates.Where(x => x.Code == GeneralApplicationSettings.LocationForm.Governorate.Code).FirstOrDefault();
                if (selected != null) 
                {
                    cmbGov.SelectedItem = selected;
                }
                else
                {
                    cmbGov.SelectedIndex = 0;
                }
            }
        }

        private void SelectLocationFormLocalityPickerValue()
        {
            if (GeneralApplicationSettings.LocationForm == null)
            {
                cmbLocality.SelectedIndex = 0;
            }
            else
            {
                List<Locality> localities = cmbLocality.ItemsSource as List<Locality>;
                var selected = localities.Where(x => x.Code == GeneralApplicationSettings.LocationForm.Locality.Code).FirstOrDefault();
                if (selected != null)
                {
                    cmbLocality.SelectedItem = selected;
                }
                else
                {
                    cmbLocality.SelectedIndex = 0;
                }
            }
        }

        //private void SelectLocationFormEnumPickerValue()
        //{
        //    if (GeneralApplicationSettings.LocationForm == null)
        //    {
        //        cmbEnumArea.SelectedIndex = 0;
        //    }
        //    else
        //    {
        //        if (cmbEnumArea.ItemsSource.Contains(GeneralApplicationSettings.LocationForm.EnumArea.ToString()))
        //        {
        //            cmbEnumArea.SelectedItem = GeneralApplicationSettings.LocationForm.EnumArea.ToString();
        //        }
        //        else
        //        {
        //            cmbEnumArea.SelectedIndex = 0;
        //        }
        //    }

        //}


        private async Task<int> GetSamplesCountFromLocationForm()
        {
            //if (cmbGov.SelectedItem == null || cmbLocality.SelectedItem == null || cmbEnumArea.SelectedItem == null)

            if (cmbGov.SelectedItem == null || cmbLocality.SelectedItem == null)
            {
                return 0;
            }

            Governorate gov = cmbGov.SelectedItem as Governorate;
            Locality loc = cmbLocality.SelectedItem as Locality;

            //int enumArea = int.Parse(cmbEnumArea.SelectedItem.ToString());

            var db = await DataBase.GetAsyncConnection();
            //return await db.Table<SampleInfo>().CountAsync(x => x.ID1 == gov.Code && x.E3 == loc.Code && x.ID3 == enumArea);
            return await db.Table<SampleInfo>().CountAsync(x => x.ID01 == gov.Code && x.ID02 == loc.Code);
        }

        private void ChangeLocation()
        {
            var visible = panelChangeLocation.IsVisible;
            HideAllPopUpPanels();

            if (!visible)
            {
                FillLocationForm();
                panelChangeLocation.IsVisible = true;

                if (MyMapView.Map != null)
                {
                    panelOverlay.IsVisible = true;
                }
            }
        }




        #endregion



        #region Find Location

        private int LayerId = 0;
        private FeatureCollectionLayer featCollectionTable = null;
        private FeatureCollectionLayer featEAStartCollectionTable = null;
        private FeatureCollectionLayer featBStartCollectionTable = null;

        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            if (GeneralApplicationSettings.LocationForm == null) return;

            GeodatabaseFeatureTable table = null;
            string testSearch = txtSearch.Text.Replace(" ", "").Replace("%", "").Replace("_", "");
            string searchFormat = "";
            LayerId = 0;
            FeatureLayer layer = null;

            int minLength = 5;

            if (cmbLayerId.SelectedItem.ToString() == "مواقع الابنية")
            {

                table = LocalBuildingLayer.FeatureTable as GeodatabaseFeatureTable;
                searchFormat = $" ({BuildingNameField}   like '%{{0}}%'  or {BuildingOwnerField}   like '%{{0}}%')";
                LayerId = 1;
                layer = LocalBuildingLayer;
            }
            else if (cmbLayerId.SelectedItem.ToString() == "الابنية")
            {
                table = LocalBuildingPolygonLayer.FeatureTable as GeodatabaseFeatureTable;
                searchFormat = $" {BuildingPolyName}   like '%{{0}}%'";
                LayerId = 2;
                layer = LocalBuildingPolygonLayer;


            }
            else if (cmbLayerId.SelectedItem.ToString() == "مناطق العد")
            {
                table = LocalBlockLayer.FeatureTable as GeodatabaseFeatureTable;
                minLength = 1;
                bool result = int.TryParse(testSearch, out int numValue);
                searchFormat = $" {ENUMID}   = {{0}}";
                if (!result || (result && numValue <= 0) || (result && numValue >= 9999))
                {
                    ToastManager.LongAlert(" يجب ان تكون منطقة العد رقم صحيح بين 1 و 9999");
                    return;

                }

                LayerId = 3;

                layer = LocalBlockLayer;

            }

            if (featCollectionTable != null)
            {
                MyMapView.Map.OperationalLayers.Remove(featCollectionTable);
            }



            QueryParameters queryParams = new QueryParameters();
            LocalBuildingLayer.ClearSelection();
            LocalBuildingPolygonLayer.ClearSelection();
            LocalBlockLayer.ClearSelection();

            if (testSearch.Length >= minLength)
            {
                ShowLoading();
                try
                {


                    string search = txtSearch.Text.Replace("'", "''").Replace(" ", "%");
                    queryParams.WhereClause = string.Format(searchFormat, search);
                    if (table != null && layer != null)
                    {

                        FeatureQueryResult queryResult = await table.QueryFeaturesAsync(queryParams);

                        //Query the table to get all features


                        /// Create a new feature collection table from the result features
                        FeatureCollectionTable collectTable = new FeatureCollectionTable(queryResult);

                        // Create a feature collection and add the table
                        FeatureCollection featCollection = new FeatureCollection();
                        featCollection.Tables.Add(collectTable);

                        // Create a layer to display the feature collection, add it to the map's operational layers
                        featCollectionTable = new FeatureCollectionLayer(featCollection);
                        //featCollection.Loaded += FeatCollection_Loaded;

                        MyMapView.Map.OperationalLayers.Add(featCollectionTable);
                        await featCollectionTable.LoadAsync();

                        // LocalBuildingLayer.IsVisible = false;

                        if (LayerId == 1)
                        {

                            FillBuildingsSymbologyPoint(featCollectionTable.Layers[0], Color.Brown);
                            //featCollectionTable.MaxScale = 2000;
                        }
                        else if (LayerId == 2)
                        {
                            featCollectionTable.MaxScale = 2000;
                        }
                        else if (LayerId == 3)
                        {
                            FillEnumerationAreasSearchSymbology();
                            featCollectionTable.Opacity = 0.6;
                            featCollectionTable.MaxScale = 3000;

                        }

                        //MyMapView.SetViewpointGeometryAsync();


                        //Cast the QueryResult to a List so the results can be interrogated.
                        List<Feature> features = queryResult.ToList();


                        if (features.Any())
                        {

                            // Create an envelope.
                            EnvelopeBuilder envBuilder = new EnvelopeBuilder(SpatialReferences.WebMercator);

                            layer.SelectFeatures(features);

                            features.ForEach((feature) => envBuilder.UnionOf(feature.Geometry.Extent));

                            // Zoom to the extent of the selected feature(s).


                            if (features.Count > 1 && LayerId != 3) // For Buildings only
                            {
                                List<BuildingInfo> searchResult = new List<BuildingInfo>();

                                foreach (var f in features)
                                {
                                    searchResult.Add(new BuildingInfo()
                                    {
                                        Name = f.Attributes[BuildingNameField] != null ? f.Attributes[BuildingNameField].ToString() : string.Empty,

                                        Owner = f.Attributes[BuildingOwnerField] != null ? f.Attributes[BuildingOwnerField].ToString() : string.Empty,

                                        NumberOfFloors = f.Attributes[BuildingNumberOfFloorsField] != null ? f.Attributes[BuildingNumberOfFloorsField].ToString() : string.Empty,

                                        BuildingCode = f.Attributes[BuildingCodeField] != null ? f.Attributes[BuildingCodeField].ToString() : string.Empty,

                                        BuildingKey = f.Attributes[BuildingKey] != null ? f.Attributes[BuildingKey].ToString() : string.Empty
                                    });
                                }

                                searchResultCount.Text = "نتيجة البحث" + "(" + features.Count + ")";
                                searchResultDataGrid.ItemsSource = searchResult;

                                panelFindLocation.IsVisible = false;

                                panelOverlay.IsVisible = true;
                                panelSearchMultiResult.IsVisible = true;

                                //await MyMapView.SetViewpointGeometryAsync(envBuilder.ToGeometry());
                            }

                            else
                            {
                                await MyMapView.SetViewpointGeometryAsync(envBuilder.ToGeometry());
                                await MyMapView.SetViewpointScaleAsync(1000);
                            }
                            ToastManager.LongAlert($"عدد نتائج البحث هو {features.Count()}");



                        }
                        else
                        {
                            ToastManager.ShortAlert("لا يوجد نتائج لبحثك برجاء محاولة كلمة بحث اخرى");

                        }
                    }

                }

                catch
                {
                    ToastManager.ShortAlert("حدث خطاء اثناء تنفيذ البحث");
                }

                HideLoading();
            }

            else
            {
                ToastManager.ShortAlert("كلمة البحث يجب ان تكون اكبر من 5 احرف");
            }

        }



        #endregion


        #region  SetStartPoint
        private async Task LoadEA()
        {
            //GeodatabaseFeatureTable table = null;

            //LayerId = 0;
            //FeatureLayer layer = null;
            //table = LocalBlockLayer.FeatureTable as GeodatabaseFeatureTable;

            //var enumsString = string.Join(",", GeneralApplicationSettings.LocationForm.EnumArea);

            //string WhereClause = $" {ENUMID}  in ({enumsString})";
            //// minLength = 1;


            //LayerId = 3;

            //layer = LocalBlockLayer;



            //if (featEAStartCollectionTable != null)
            //{
            //    MyMapView.Map.OperationalLayers.Remove(featEAStartCollectionTable);
            //}



            //QueryParameters queryParams = new QueryParameters();
            ////LocalBuildingLayer.ClearSelection();
            ////LocalBuildingPolygonLayer.ClearSelection();
            ////LocalBlockLayer.ClearSelection();


            //// ShowLoading();
            //try
            //{



            //    queryParams.WhereClause = WhereClause;
            //    if (table != null && layer != null)
            //    {

            //        FeatureQueryResult queryResult = await table.QueryFeaturesAsync(queryParams);

            //        //Query the table to get all features


            //        /// Create a new feature collection table from the result features
            //        FeatureCollectionTable collectTable = new FeatureCollectionTable(queryResult);

            //        // Create a feature collection and add the table
            //        FeatureCollection featCollection = new FeatureCollection();
            //        featCollection.Tables.Add(collectTable);

            //        // Create a layer to display the feature collection, add it to the map's operational layers
            //        featEAStartCollectionTable = new FeatureCollectionLayer(featCollection);
            //        //featCollection.Loaded += FeatCollection_Loaded;

            //        MyMapView.Map.OperationalLayers.Add(featEAStartCollectionTable);
            //        await featEAStartCollectionTable.LoadAsync();

            //        // LocalBuildingLayer.IsVisible = false;


            //        FillEnumerationAreasStartSymbology();
            //        featEAStartCollectionTable.Opacity = 0.8;
            //        featEAStartCollectionTable.MaxScale = 2500;



            //        //MyMapView.SetViewpointGeometryAsync();



            //        //Cast the QueryResult to a List so the results can be interrogated.
            //        List<Feature> features = queryResult.ToList();



            //        if (features.Any())
            //        {

            //            // Create an envelope.
            //            EnvelopeBuilder envBuilder = new EnvelopeBuilder(SpatialReferences.WebMercator);

            //            layer.SelectFeatures(features);

            //            features.ForEach((feature) => envBuilder.UnionOf(feature.Geometry.Extent));

            //            // Zoom to the extent of the selected feature(s).

            //            if (features.Count > 1)
            //            {
            //                //await MyMapView.SetViewpointGeometryAsync(envBuilder.ToGeometry());
            //            }

            //            else
            //            {

            //                Device.BeginInvokeOnMainThread(async () =>
            //                {
            //                    await MyMapView.SetViewpointGeometryAsync(envBuilder.ToGeometry());
            //                    if (GeneralApplicationSettings.LocationForm.GetLocalityFullCode().StartsWith("24"))
            //                    {
            //                        await MyMapView.SetViewpointScaleAsync(1500);
            //                    }
            //                    else
            //                    {
            //                        await MyMapView.SetViewpointScaleAsync(3000);
            //                    }
            //                });

            //            }

            //            //  ToastManager.LongAlert($"عدد نتائج البحث هو {features.Count()}");



            //        }
            //        else
            //        {
            //            // ToastManager.ShortAlert("لا يوجد نتائج لبحثك برجاء محاولة كلمة بحث اخرى");

            //        }
            //    }

            //}

            //catch
            //{
            //    ToastManager.ShortAlert("حدث خطاء اثناء تنفيذ البحث");
            //}

            //// HideLoading();


        }

        GraphicsOverlay overlay = null;

        // Add created overlay to the MapView

        //private async Task LoadStartBuilding()
        //{
        //    GeodatabaseFeatureTable table = null;
        //    // string testSearch = txtSearch.Text.Replace(" ", "").Replace("%", "").Replace("_", "");
        //    string searchFormat = "";
        //    LayerId = 0;
        //    FeatureLayer layer = null;



        //    table = LocalBuildingLayer.FeatureTable as GeodatabaseFeatureTable;
        //    searchFormat = $" {BuildingCodeField}   like '%{{0}}%'";
        //    LayerId = 1;
        //    layer = LocalBuildingLayer;


        //    if (overlay != null)
        //    {
        //        MyMapView.GraphicsOverlays.Remove(overlay);
        //    }

        //    overlay = new GraphicsOverlay();


        //    // Add graphics using different source types


        //    if (featBStartCollectionTable != null)
        //    {
        //        MyMapView.Map.OperationalLayers.Remove(featBStartCollectionTable);
        //    }



        //    QueryParameters queryParams = new QueryParameters();
        //    LocalBuildingLayer.ClearSelection();
        //    LocalBuildingPolygonLayer.ClearSelection();
        //    LocalBlockLayer.ClearSelection();


        //    // ShowLoading();
        //    try
        //    {


        //        string search = GeneralApplicationSettings.LocationForm.GetStartBuildingFullCode(); //txtSearch.Text.Replace("'", "''").Replace(" ", "%");
        //        queryParams.WhereClause = string.Format(searchFormat, search);
        //        if (table != null && layer != null)
        //        {

        //            FeatureQueryResult queryResult = await table.QueryFeaturesAsync(queryParams);

        //            //Query the table to get all features


        //            /// Create a new feature collection table from the result features
        //            FeatureCollectionTable collectTable = new FeatureCollectionTable(queryResult);

        //            // Create a feature collection and add the table
        //            FeatureCollection featCollection = new FeatureCollection();
        //            featCollection.Tables.Add(collectTable);

        //            // Create a layer to display the feature collection, add it to the map's operational layers
        //            featBStartCollectionTable = new FeatureCollectionLayer(featCollection);
        //            //featCollection.Loaded += FeatCollection_Loaded;

        //            MyMapView.Map.OperationalLayers.Add(featBStartCollectionTable);
        //            await featBStartCollectionTable.LoadAsync();

        //            // LocalBuildingLayer.IsVisible = false;


        //            FillBuildingsPicturePoint(featBStartCollectionTable.Layers[0], Color.Brown);
        //            //featCollectionTable.MaxScale = 2000;


        //            //MyMapView.SetViewpointGeometryAsync();



        //            //Cast the QueryResult to a List so the results can be interrogated.
        //            List<Feature> features = queryResult.ToList();



        //            if (features.Any())
        //            {

        //                // Create an envelope.
        //                EnvelopeBuilder envBuilder = new EnvelopeBuilder(SpatialReferences.WebMercator);

        //                layer.SelectFeatures(features);

        //                features.ForEach((feature) => envBuilder.UnionOf(feature.Geometry.Extent));

        //                // Zoom to the extent of the selected feature(s).

        //                if (features.Count > 1)
        //                {
        //                    //await MyMapView.SetViewpointGeometryAsync(envBuilder.ToGeometry());
        //                }

        //                else
        //                {
        //                    Device.BeginInvokeOnMainThread(async () =>
        //                    {
        //                        await MyMapView.SetViewpointGeometryAsync(envBuilder.ToGeometry());
        //                        await MyMapView.SetViewpointScaleAsync(1000);

        //                    });

        //                }

        //                // ToastManager.LongAlert($"عدد نتائج البحث هو {features.Count()}");



        //            }
        //            else
        //            {
        //                // ToastManager.ShortAlert("لا يوجد نتائج لبحثك برجاء محاولة كلمة بحث اخرى");

        //            }
        //        }

        //    }

        //    catch
        //    {
        //        //  ToastManager.ShortAlert("حدث خطاء اثناء تنفيذ البحث");
        //    }

        //    // HideLoading();


        //}
        #endregion

        #region LoadSamples

        FeatureCollectionLayer LocalSampleLayer;
        FeatureLayer LocalCompletedLayer;
        FeatureLayer LocalNotCompletedLayer;
        FeatureLayer LocalNotVisitedLayer;
        FeatureLayer LocalBuildingSamplesLayer;

        FeatureCollectionTable LocalCompletedLayerTable;
        FeatureCollectionTable LocalNotCompletedLayerTable;
        FeatureCollectionTable LocalNotVisitedLayerTable;
        FeatureCollectionTable LocalBuildingSamplesLayerTable;


        private async Task LoadSamples()
        {
            if (GeneralApplicationSettings.LocationForm == null) return;
            if (LocalSampleLayer != null)
            {
                MyMapView.Map.OperationalLayers.Remove(LocalSampleLayer);
                // MyMapView.Map.OperationalLayers.Clear();
            }

            GeneralApplicationSettings.LocationForm.LoadSamplesVisits();
            var samples = GeneralApplicationSettings.LocationForm.Samples;
            var groupedSamples = samples.Where(x=> x.GetBuildingFullCode() != string.Empty).GroupBy(x => x.GetBuildingFullCode()).ToList();

            var completed = new List<string>();
            var notCompleted = new List<string>();
            var notVisited = new List<string>();

            groupedSamples.ForEach(x =>
            {
                if (x.Where(y => y.Visit == null).Count() == 0 && x.Where(y => y.Visit.IsComplete).Count() == x.Count())
                {
                    completed.Add(x.FirstOrDefault().GetBuildingFullCode());
                }
                else if (x.Where(y => y.Visit != null).Count() != 0)
                {
                    notCompleted.Add(x.FirstOrDefault().GetBuildingFullCode());
                }
                else
                {
                    notVisited.Add(x.FirstOrDefault().GetBuildingFullCode());
                }
            });

            var compbnosString = string.Join(",", completed);
            var notCompbnosString = string.Join(",", notCompleted);
            var notVisitedbnosString = string.Join(",", notVisited);

            FeatureCollection featCollection = new FeatureCollection();
            var table = LocalBuildingLayer.FeatureTable;

            QueryParameters compqueryParams = new QueryParameters() { WhereClause = $" {BuildingCodeField}   in ({compbnosString})" };
            FeatureQueryResult compqueryResult = await table.QueryFeaturesAsync(compqueryParams);
            /// Create a new feature collection table from the result features
            LocalCompletedLayerTable = new FeatureCollectionTable(compqueryResult);
            LocalCompletedLayerTable.DisplayName = "المنجز";
            featCollection.Tables.Add(LocalCompletedLayerTable);
            await LocalCompletedLayerTable.LoadAsync();

            QueryParameters notCompqueryParams = new QueryParameters() { WhereClause = $" {BuildingCodeField}   in ({notCompbnosString})" };
            FeatureQueryResult notCompqueryResult = await table.QueryFeaturesAsync(notCompqueryParams);
            /// Create a new feature collection table from the result features
            LocalNotCompletedLayerTable = new FeatureCollectionTable(notCompqueryResult);
            LocalNotCompletedLayerTable.DisplayName = "غير مكتمل";
            featCollection.Tables.Add(LocalNotCompletedLayerTable);
            await LocalNotCompletedLayerTable.LoadAsync();

            QueryParameters notVisitedqueryParams = new QueryParameters() { WhereClause = $" {BuildingCodeField}   in ({notVisitedbnosString})" };
            FeatureQueryResult notVisitedqueryResult = await table.QueryFeaturesAsync(notVisitedqueryParams);
            /// Create a new feature collection table from the result features
            LocalNotVisitedLayerTable = new FeatureCollectionTable(notVisitedqueryResult);
            LocalNotVisitedLayerTable.DisplayName = "غير مزار";
            featCollection.Tables.Add(LocalNotVisitedLayerTable);
            await LocalNotVisitedLayerTable.LoadAsync();

            // Create a layer to display the feature collection, add it to the map's operational layers
            LocalSampleLayer = new FeatureCollectionLayer(featCollection);
            LocalSampleLayer.Name = "العينة";
            await LocalSampleLayer.LoadAsync();

            try
            {
                MyMapView.Map.OperationalLayers.Insert(MyMapView.Map.OperationalLayers.Count -1, LocalSampleLayer);

                LocalCompletedLayer = LocalSampleLayer.Layers[0];
                LocalNotCompletedLayer = LocalSampleLayer.Layers[1];
                LocalNotVisitedLayer = LocalSampleLayer.Layers[2];


                #region Remaining Sample
                    var visitsBuildingNo = samples.Where(x => x.Visit != null).Select(x => x.Visit).ToList().Select(x => x.BuildingNo).Distinct().ToList();
                    var samplesBuildings = GeneralApplicationSettings.LocationForm.GetSamplesBuildingNo().Where(x => !visitsBuildingNo.Contains(x)).ToList();

                    var samplesbnosString = string.Join(",", samplesBuildings);
                    QueryParameters samplesqueryParams = new QueryParameters() { WhereClause = $" {BuildingCodeField}   in ({samplesbnosString})" };
                    FeatureQueryResult samplesqueryResult = await table.QueryFeaturesAsync(samplesqueryParams);
                    /// Create a new feature collection table from the result features
                    LocalBuildingSamplesLayerTable = new FeatureCollectionTable(samplesqueryResult);
                    LocalBuildingSamplesLayerTable.DisplayName = "العينة المسموحة";
                    featCollection.Tables.Add(LocalBuildingSamplesLayerTable);
                    await LocalBuildingSamplesLayerTable.LoadAsync();

                    LocalBuildingSamplesLayer = LocalSampleLayer.Layers[3];
                    FillBuildingsSymbologyPoint(LocalSampleLayer.Layers[3], Color.Brown);
                #endregion

                FillBuildingsSymbologyPoint(LocalSampleLayer.Layers[2], Color.FromHex("#ea2b2b"));
                FillBuildingsSymbologyPoint(LocalSampleLayer.Layers[1], Color.FromHex("#f1e411"));
                FillBuildingsSymbologyPoint(LocalSampleLayer.Layers[0], Color.FromHex("#32ae27"));

                if (!ShowCompletedLayer)
                {
                    LocalSampleLayer.FeatureCollection.Tables.Remove(LocalCompletedLayerTable);
                    // LocalCompletedLayer = null;
                }
                if (!ShowNotCompletedLayer)
                {
                    LocalSampleLayer.FeatureCollection.Tables.Remove(LocalNotCompletedLayerTable);
                    // LocalNotCompletedLayer = null;

                }
                if (!ShowNotVisitedLayer)
                {
                    LocalSampleLayer.FeatureCollection.Tables.Remove(LocalNotVisitedLayerTable);
                    // LocalNotVisitedLayer = null;
                }

                if (!ShowAllLayers)
                {
                    LocalBuildingLayer.IsVisible = false;
                }

                if (!string.IsNullOrEmpty(CurrentBuildingNo))
                {
                    await SelectBuildingAsync(CurrentBuildingNo, LocalSampleLayer);
                    CurrentBuildingNo = null;
                }


            } // try
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }


        #endregion


        #region Add new Building


        private MapPoint CurrentAddBuildingMapPoint { get; set; }
        private Feature LastBuilding { get; set; }


        private async void AddNewBuilding(MapPoint location, string enumCode)
        {
            if (string.IsNullOrWhiteSpace(enumCode)) return;
            QueryParameters queryParams = new QueryParameters();
            string search = txtSearch.Text.Replace("'", "''").Replace(" ", "%");
            string searchFormat = $" {BuildingEnumCode}   = '{enumCode}'";
            queryParams.WhereClause = string.Format(searchFormat, search);
            var table = LocalBuildingLayer.FeatureTable;
            if (table != null)
            {


                FeatureQueryResult queryResult = await table.QueryFeaturesAsync(queryParams);
                Feature lastFeature = null;

                lastFeature = queryResult.OrderBy(x => x.Attributes[BuildingCodeField]).LastOrDefault();

                if (lastFeature != null)
                {
                    LastBuilding = lastFeature;
                    int num = Convert.ToInt32(LastBuilding.Attributes[BuildingKey]);

                    // string enumCode = LastBuilding.Attributes[BuildingEnumCode].ToString();
                    num++;

                    var ID4 = enumCode + num.ToString("000");

                    //if (cmbBuildingStatus.Items.Count() == 0)
                    //{

                    //    var domain = table.Fields.Where(x => x.Name == "BUILDTYPE_DOMAIN").First().Domain as CodedValueDomain;
                    //    var data = domain.CodedValues.ToList().Select(x => new OptionInfo() { Description = x.Name, Id = Convert.ToInt32(x.Code) }).ToList(); ;
                    //    data.Insert(0, OptionInfo.Default);
                    //    cmbBuildingStatus.ItemsSource = data;
                    //    cmbBuildingStatus.ItemDisplayBinding = new Binding("Description");
                    //    cmbBuildingStatus.SelectedIndex = 0;
                    //}

                    txtBuildingCode.Text = ID4;
                    CurrentAddBuildingMapPoint = location;
                    var visible = panelAddNewBuilding.IsVisible;
                    HideAllPopUpPanels();

                    txtOwner.Text = null;
                    txtAddBuildingPassowrd.Text = null;
                    txtNumberOfFloors.Text = null;
                    //cmbBuildingStatus.SelectedIndex = 0;

                    if (!visible)
                    {
                        //FillLocality();
                        panelOverlay.IsVisible = true;
                        panelAddNewBuilding.IsVisible = true;
                    }

                }

                else
                {
                    await DisplayAlert(GeneralMessages.Error, "لا يمكن اضافة مبنى يرجى مراجعة المشرف", GeneralMessages.Cancel);
                }

            }
        }

        private async Task AddNewBuildingToDB()
        {
            if (await IsValidBuilding())
            {
                bool success = false;
                try
                {


                    ArcGISFeatureTable buildingTable = LocalBuildingLayer.FeatureTable as ArcGISFeatureTable;


                    if (buildingTable == null)
                        throw new ApplicationException("Birds table was not found in the local geodatabase.");
                    Feature building = buildingTable.CreateFeature();

                    var editableFields = building.FeatureTable.Fields.Where(x => x.IsEditable == true).ToList();
                    foreach (var field in editableFields)
                    {


                        building.Attributes[field.Name] = LastBuilding.Attributes[field.Name];
                    }

                    int num = Convert.ToInt32(LastBuilding.Attributes[BuildingKey]);
                    string enumCode = LastBuilding.Attributes[BuildingEnumCode].ToString();
                    num++;

                    var ID4 = enumCode + num.ToString("000");
                    building.Attributes[BuildingOwner] = txtOwner.Text;
                    building.Attributes[BuildingNumberOfFloors] = txtNumberOfFloors.Text;
                    building.Attributes[BuildingKey] = num.ToString();
                    building.Attributes[BuildingCodeField] = txtBuildingCode.Text;
                    //var buildingDomain = (cmbBuildingStatus.SelectedItem as OptionInfo);

                    //var domain = buildingTable.Fields.Where(x => x.Name == "BUILDTYPE_DOMAIN").First().Domain as CodedValueDomain;
                    //CodedValue codedValue = null;
                    //foreach(var  cd  in domain.CodedValues)
                    //{
                    //    if(cd.Code.Equals(buildingDomain.Id))
                    //    {
                    //        codedValue = cd;
                    //        break;
                    //    }
                    //}

                    // var bd = domain.CodedValues.Where(x => x.Code == (object)buildingDomain.Id).First();
                    //building.Attributes["BUILDTYPE_DOMAIN"] = codedValue.Code;
                    //building.Attributes["BUILDTYPE_DOMAIN_DESC"] = buildingDomain.Description;

                    building.Geometry = CurrentAddBuildingMapPoint;
                    Geometry normalizedPoint1 = GeometryEngine.NormalizeCentralMeridian(CurrentAddBuildingMapPoint);
                    MapPoint projectedCenter1 = GeometryEngine.Project(normalizedPoint1, SpatialReferences.Wgs84) as MapPoint;

                    var currentBuildingX = projectedCenter1.X.ToString();
                    var currentBuildingY = projectedCenter1.Y.ToString();


                    building.Attributes["GPS_X"] = currentBuildingX;
                    building.Attributes["GPS_Y"] = currentBuildingY;


                    //building.Attributes["BULDINGGEOCODE"] = DateTime.Now.Ticks.ToString();


                    await buildingTable.AddFeatureAsync(building);


                    // Building buildingInfo = new Building() { ID4 = ID4, CreatedBy = Security.CurrentUserSettings.CurrentUser.UserName, CreatedDate = DateTime.Now, DomainId = buildingDomain.Id, IsSent = false, NumberOfFloor = Convert.ToInt32(txtNumberOfFloors.Text), Owner = txtOwner.Text, GPS_X = projectedCenter1.X, GPS_Y = projectedCenter1.Y };

                    var db = DataBase.GetConnection();


                    await LoadSamples();
                    // db.InsertOrReplace(buildingInfo);
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                }


                if (!success)
                {
                    await DisplayAlert(GeneralMessages.Error, GeneralMessages.SaveNotSuccess, GeneralMessages.Cancel);
                }

                else
                {
                    HideAllPopUpPanels();
                    ToastManager.LongAlert(GeneralMessages.DataSavedSuccessfully);
                }

            }
        }
        //private async Task AddNewBuildingToDB()
        //{
        //    if (await IsValidBuilding())
        //    {
        //        bool success = false;
        //        try
        //        {




        //            ArcGISFeatureTable buildingTable = LocalBuildingLayer.FeatureTable as ArcGISFeatureTable;


        //            if (buildingTable == null)
        //                throw new ApplicationException("Birds table was not found in the local geodatabase.");
        //            Feature building = buildingTable.CreateFeature();

        //            var editableFields = building.FeatureTable.Fields.Where(x => x.IsEditable == true).ToList();
        //            foreach (var field in editableFields)
        //            {


        //                building.Attributes[field.Name] = LastBuilding.Attributes[field.Name];
        //            }

        //            int num = Convert.ToInt32(LastBuilding.Attributes[BuildingKey]);
        //            string enumCode = LastBuilding.Attributes[BuildingEnumCode].ToString();
        //            num++;

        //            var ID4 = enumCode + num.ToString("000");
        //            building.Attributes[BuildingOwner] = txtOwner.Text;
        //            building.Attributes[BuildingNumberOfFloors] = txtNumberOfFloors.Text;
        //            building.Attributes[BuildingKey] = num.ToString();
        //            building.Attributes[BuildingCodeField] = txtBuildingCode.Text;
        //           // var buildingDomain = (cmbBuildingStatus.SelectedItem as OptionInfo);

        //            //var domain = buildingTable.Fields.Where(x => x.Name == "BUILDTYPE_DOMAIN").First().Domain as CodedValueDomain;
        //            //CodedValue codedValue = null;
        //            //foreach(var  cd  in domain.CodedValues)
        //            //{
        //            //    if(cd.Code.Equals(buildingDomain.Id))
        //            //    {
        //            //        codedValue = cd;
        //            //        break;
        //            //    }
        //            //}

        //            //// var bd = domain.CodedValues.Where(x => x.Code == (object)buildingDomain.Id).First();
        //            //building.Attributes["BUILDTYPE_DOMAIN"] = codedValue.Code;
        //            //building.Attributes["BUILDTYPE_DOMAIN_DESC"] = buildingDomain.Description;

        //            building.Geometry = CurrentAddBuildingMapPoint;
        //            Geometry normalizedPoint1 = GeometryEngine.NormalizeCentralMeridian(CurrentAddBuildingMapPoint);
        //            MapPoint projectedCenter1 = GeometryEngine.Project(normalizedPoint1, SpatialReferences.Wgs84) as MapPoint;

        //            var currentBuildingX = projectedCenter1.X.ToString();
        //            var currentBuildingY = projectedCenter1.Y.ToString();


        //            building.Attributes["GPS_X"] = currentBuildingX;
        //            building.Attributes["GPS_Y"] = currentBuildingY;


        //            building.Attributes["BULDINGGEOCODE"] = DateTime.Now.Ticks.ToString();


        //            await buildingTable.AddFeatureAsync(building);



        //            Building buildingInfo = new Building() { ID4 = ID4, CreatedBy = Security.CurrentUserSettings.CurrentUser.UserName, CreatedDate = DateTime.Now, DomainId =-1, IsSent = false, NumberOfFloor = Convert.ToInt32(txtNumberOfFloors.Text), Owner = txtOwner.Text, GPS_X = projectedCenter1.X, GPS_Y = projectedCenter1.Y };

        //            var db = DataBase.GetConnection();

        //            db.InsertOrReplace(buildingInfo);
        //            success = true;





        //        }
        //        catch (Exception ex)
        //        {
        //            success = false;
        //        }


        //        if (!success)
        //        {
        //            await DisplayAlert(GeneralMessages.Error, GeneralMessages.SaveNotSuccess, GeneralMessages.Cancel);
        //        }

        //        else
        //        {
        //            HideAllPopUpPanels();
        //            ToastManager.LongAlert(GeneralMessages.DataSavedSuccessfully);
        //        }

        //    }
        //}

        private async Task<bool> IsValidBuilding()
        {
            bool valid = true;
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(txtOwner.Text))
            {
                valid = false;
                stringBuilder.AppendLine("- اسم المبنى");
            }

            if (string.IsNullOrWhiteSpace(txtNumberOfFloors.Text))
            {
                valid = false;
                stringBuilder.AppendLine("- عدد الطوابق");
            }
            else
            {
                bool isNumber = int.TryParse(txtNumberOfFloors.Text, out int numberOfFloors);

                if (!isNumber || (isNumber && (numberOfFloors < 0 || numberOfFloors > 20)))
                {
                    valid = false;
                    stringBuilder.AppendLine("- عدد الطوابق يجب ان  يكون عدد صحيح ما بين 1 و 20");
                }


            }

            //if (cmbBuildingStatus.SelectedIndex==0)
            //{
            //    valid = false;
            //    stringBuilder.AppendLine("حالة المبنى");
            //}

            if (string.IsNullOrWhiteSpace(txtAddBuildingPassowrd.Text))
            {
                valid = false;
                stringBuilder.AppendLine("- كلمة المرور الخاصة باضافة مبنى");
            }

            else if (txtAddBuildingPassowrd.Text != "951753")
            {
                valid = false;
                stringBuilder.AppendLine("- كلمة المرور الخاصة باضافة مبنى غير صحيحة");
            }



            if (!valid)
            {
                await DisplayAlert(GeneralMessages.Error, stringBuilder.ToString(), GeneralMessages.Cancel);
            }

            return valid;

        }
        #endregion

        private void btnAddNew_Clicked(object sender, EventArgs e)
        {
            ToastManager.ShortAlert("يرجى اختيار الموقع المناسب لاضافة مبنى");

            CurrentMapOperation = MapOperation.Add;

            if (btnAddNewBuilding.Source.ToString() != "File: " + GetAddBuildingActiveImage())
            {
                ResetActiveButtons();
                btnAddNewBuilding.Source = GetAddBuildingActiveImage();
            }
        }

        //BtnChart_Clicked
        private async void BtnChart_Clicked(object sender, EventArgs e)
        {


            var visible = panelChart.IsVisible;

            HideAllPopUpPanels();

            if (!visible)
            {

                //FillLocality();

                await FillChartData();
            }

        }
        private async void CmbSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            await FillChartData();
        }
        private async Task FillChartData()
        {
            var db = await DataBase.GetAsyncConnection();

            List<Visit> completeVisit,notCompletedVisits;

            //completeVisit = await db.Table<Visit>().Where(x => x.IsComplete && x.ID1 == GeneralApplicationSettings.LocationForm.Governorate.Code && x.E3 == GeneralApplicationSettings.LocationForm.Locality.Code && x.ID3 == GeneralApplicationSettings.LocationForm.EnumArea).ToListAsync();


            //notCompletedVisits = await db.Table<Visit>().Where(x => x.IsComplete == false && x.ID1 == GeneralApplicationSettings.LocationForm.Governorate.Code && x.E3 == GeneralApplicationSettings.LocationForm.Locality.Code && x.ID3 == GeneralApplicationSettings.LocationForm.EnumArea).ToListAsync();

            completeVisit = await db.Table<Visit>().Where(x => x.IsComplete && x.ID01 == GeneralApplicationSettings.LocationForm.Governorate.Code && x.ID02 == GeneralApplicationSettings.LocationForm.Locality.Code).ToListAsync();


            notCompletedVisits = await db.Table<Visit>().Where(x => x.IsComplete == false && x.ID01 == GeneralApplicationSettings.LocationForm.Governorate.Code && x.ID02 == GeneralApplicationSettings.LocationForm.Locality.Code).ToListAsync();

            List<Visit> allVisits = new List<Visit>();
            allVisits.AddRange(completeVisit);
            allVisits.AddRange(notCompletedVisits);

            List<string> allVisitsIDSAM = allVisits.Select(x => x.ID00).ToList();


            int completedCount = completeVisit.Count;
            int notCompletedCount = notCompletedVisits.Count;
            int remainsCount = GeneralApplicationSettings.LocationForm.Samples.Where(x => !allVisitsIDSAM.Contains(x.ID00)).Count();

            if (completedCount == 0 && notCompletedCount == 0 && remainsCount == 0)
            {
                await DisplayAlert("", " لا يوجد بيانات متاحة للعرض", GeneralMessages.Ok);
                return;
            }

            List<Color> colors = new List<Color>();
            var data = new ObservableCollection<ChartDataPoint>();

            if (notCompletedCount != 0)
            {
                data.Add(new ChartDataPoint("غير مكتمل", notCompletedCount));
                colors.Add(Colors.Yellow);
            }

            if (completedCount != 0)
            {
                data.Add(new ChartDataPoint("منجز", completedCount));
                colors.Add(Colors.Green);
            }

            if (remainsCount != 0)
            {
                data.Add(new ChartDataPoint("المتبقي", remainsCount));
                colors.Add(Colors.Red);
            }

            pieChart.ItemsSource = data;
            pieChart.ColorModel.CustomBrushes = colors;
         
            panelChart.IsVisible = true;
            panelOverlay.IsVisible = true;
        }

        private void btnCancelAddBuilding_Clicked(object sender, EventArgs e)
        {
            HideAllPopUpPanels();
        }



        private async void btnAddNewBuilding_Clicked(object sender, EventArgs e)
        {
            await AddNewBuildingToDB();
        }


        protected override bool OnBackButtonPressed()
        {
            return true;
            //return base.OnBackButtonPressed();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ((ArcGISTiledLayer)MyMapView.Map.Basemap.BaseLayers[0]).Brightness = e.NewValue;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var visible = panelBrightness.IsVisible;

            HideAllPopUpPanels();

            if (!visible)
            {


                sliderBrightness.Value = ((ArcGISTiledLayer)MyMapView.Map.Basemap.BaseLayers[0]).Brightness;
                //sliderContrast.Value = ((ArcGISTiledLayer)MyMapView.Map.Basemap.BaseLayers[0]).Contrast;
                sliderOpacity.Value = ((ArcGISTiledLayer)MyMapView.Map.Basemap.BaseLayers[0]).Opacity;

                panelBrightness.IsVisible = true;
                panelOverlay.IsVisible = true;
            }
        }

        private void BtnClosebrightness_Clicked(object sender, EventArgs e)
        {

            HideAllPopUpPanels();
        }

        private void SliderContrast_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ((ArcGISTiledLayer)MyMapView.Map.Basemap.BaseLayers[0]).Contrast = e.NewValue;
        }

        private void SliderOpacity_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            LocalBlockLayer.Opacity = 1 - e.NewValue;
            ((ArcGISTiledLayer)MyMapView.Map.Basemap.BaseLayers[0]).Opacity = e.NewValue;
        }


        private async void ShowSearchResultBuilding_Clicked(object sender, EventArgs e)
        {
            BuildingInfo buildingInfo = (sender as Button).BindingContext as BuildingInfo;
            if (buildingInfo == null) return;

            if (string.IsNullOrEmpty(buildingInfo.BuildingCode))
            {
                await DisplayAlert("", "هذا المبنى غير تعدادي", GeneralMessages.Ok);
                return;
            }

           await SelectBuildingAsync(buildingInfo.BuildingCode, featCollectionTable);
        }


        private string GetInfoBuildingNormalImage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return "buildinginfo.png";
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                return "images/buildinginfo.png";
            }

            return string.Empty;
        }

        private string GetInfoBuildingActiveImage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return "buildinginfo_active.png";
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                return "images/buildinginfo_active.png";
            }

            return string.Empty;
        }

        private string GetSelectBuildingNormalImage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return "buildingvisit.png";
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                return "images/buildingvisit.png";
            }

            return string.Empty;
        }

        private string GetSelectBuildingActiveImage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return "buildingvisit_active.png";
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                return "images/buildingvisit_active.png";
            }

            return string.Empty;
        }


        private string GetAddBuildingNormalImage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return "addnewbuilding.png";
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                return "images/addnewbuilding.png";
            }

            return string.Empty;
        }
        private string GetAddBuildingActiveImage()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return "addnewbuilding_active.png";
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                return "images/addnewbuilding_active.png";
            }

            return string.Empty;
        }


        private void ResetActiveButtons()
        {
            if (selectBuildingBtn.Source.ToString() != "File: " + GetSelectBuildingNormalImage())
                selectBuildingBtn.Source = GetSelectBuildingNormalImage();

            if (btnInfoBuildingBtn.Source.ToString() != "File: " + GetInfoBuildingNormalImage())
                btnInfoBuildingBtn.Source = GetInfoBuildingNormalImage();

            if (btnAddNewBuilding.Source.ToString() != "File: " + GetAddBuildingNormalImage())
                btnAddNewBuilding.Source = GetAddBuildingNormalImage();
        }


    }
}