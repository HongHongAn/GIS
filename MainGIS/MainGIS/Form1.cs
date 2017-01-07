using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainGIS
{
    public partial class Form1 : Form
    {
        public ILayer pGlobalFeatureLayer;
        public IEnvelope IEnvelopepEnvelope;
        public Form1()
        {
            InitializeComponent();
        }

        private void openMxdFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMxd();
        }

        private string OpenMxd()
        {
            string MxdPath = "";
            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "Open Mxd File";
            OpenMXD.InitialDirectory = "E:\\ArcGIS10.2\\DeveloperKit10.2\\Samples\\data\\California";
            OpenMXD.Filter = "Map Documents (*.mxd)|*.mxd";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                MxdPath = OpenMXD.FileName;
                axMapControl1.LoadMxFile(MxdPath);
            }
            return MxdPath;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenShape();
        }

        private string[] OpenShape()
        {
            string[] ShpFile = new string[2];
            OpenFileDialog OpenShapeFile = new OpenFileDialog();
            OpenShapeFile.Title = "Open Shape File";
            OpenShapeFile.InitialDirectory = "E:\\ArcGIS10.2\\DeveloperKit10.2\\Samples\\data\\California";
            OpenShapeFile.Filter = "Shape File (*.shp)|*.shp";
            if (OpenShapeFile.ShowDialog() == DialogResult.OK)
            {
                string ShapePath = OpenShapeFile.FileName;
                //利用“\\”将文件路径分成两部分
                int Position = ShapePath.LastIndexOf("\\");
                string FilePath = ShapePath.Substring(0, Position);
                string ShpName = ShapePath.Substring(Position + 1);
                ShpFile[0] = FilePath;
                ShpFile[1] = ShpName;
                axMapControl1.AddShapeFile(FilePath, ShpName);
            }
            return ShpFile;
        }

        private void axTOCControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnMouseDownEvent e)
        {
            if (axMapControl1.LayerCount > 0)
            {
                esriTOCControlItem pItem = new esriTOCControlItem();
                pGlobalFeatureLayer = new FeatureLayerClass();
                IBasicMap pBasicMap = new MapClass();
                object pOther = new object();
                object pIndex = new object();
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref pBasicMap, ref pGlobalFeatureLayer, ref pOther, ref pIndex);
            }
            if (e.button == 2)
            {
                contextMenuStrip1.Show(axTOCControl1, e.x, e.y);
            }
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAttributeTable Ft = new frmAttributeTable(pGlobalFeatureLayer as IFeatureLayer);
            Ft.Show();
        }

        private void copyToPageLayout()
        {
            IObjectCopy objectCopy = new ObjectCopyClass();
            object copyFromMap = axMapControl1.Map;
            object copyMap = objectCopy.Copy(copyFromMap);
            object copyToMap = axPageLayoutControl1.ActiveView.FocusMap;
            objectCopy.Overwrite(copyMap, ref copyToMap);
        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            copyToPageLayout();
        }

        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            IActiveView activeVieww = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            IDisplayTransformation displayTransformation = activeVieww.ScreenDisplay.DisplayTransformation;
            displayTransformation.VisibleBounds = axMapControl1.Extent;
            axPageLayoutControl1.ActiveView.Refresh();
            copyToPageLayout();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            this.axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
        }

        /// <summary>
        /// Open MDB
        /// </summary>
        /// <param name="_pMDBName"></param>
        /// <returns></returns>
        public IWorkspace GetMDBWorkspace(String _pMDBName)
        {
            IWorkspaceFactory pWsFac = new AccessWorkspaceFactoryClass();
            IWorkspace pWs = pWsFac.OpenFromFile(_pMDBName, 0);
            return pWs;
        }

        /// <summary>
        /// Open GDB
        /// </summary>
        /// <param name="_pGDBName"></param>
        /// <returns></returns>
        public IWorkspace GetGDBWorkspace(String _pGDBName)
        {
            IWorkspaceFactory pWsFac = new FileGDBWorkspaceFactoryClass();
            IWorkspace pWs = pWsFac.OpenFromFile(_pGDBName, 0);
            return pWs;
        }

        /// <summary>
        /// 获取个人数据库的路径
        /// </summary>
        /// <returns></returns>
        public string WsPath()
        {
            string WsFileName = "";
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "个人数据库(MDB)|*.mdb";
            DialogResult DialogR = OpenFile.ShowDialog();
            if (DialogR == DialogResult.Cancel)
            {
            }
            else
            {
                WsFileName = OpenFile.FileName;
            }
            return WsFileName;
        }

        private void openMDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOpenMDB openMDB = new frmOpenMDB();
            openMDB.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.axMapControl1.Map.LayerCount; i++)
            {
                if (this.axMapControl1.Map.get_Layer(i) == pGlobalFeatureLayer)
                {
                    this.axMapControl1.DeleteLayer(i);
                }
            }
        }

        private void openRasterDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// return RasterWorkspace
        /// </summary>
        /// <param name="pWsName"></param>
        /// <returns></returns>
        IRasterWorkspace GetRasterWorkspace(string pWsName)
        {
            try
            {
                IWorkspaceFactory pWorkFact = new RasterWorkspaceFactoryClass();
                return pWorkFact.OpenFromFile(pWsName, 0) as IRasterWorkspace;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// return RasterDataset
        /// </summary>
        /// <param name="pFolderName"></param>
        /// <param name="pFileName"></param>
        /// <returns></returns>
        IRasterDataset OpenFileRasterDataset(string pFolderName, string pFileName)
        {
            IRasterWorkspace pRasterWorkspace = GetRasterWorkspace(pFolderName);
            IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(pFileName);
            return pRasterDataset;
        }
        /*
        /// <summary>
        /// 打开栅格目录中的一个数据
        /// </summary>
        /// <param name="pCatalog"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        IRasterDataset GetRasterCatalogItem(IRasterCatalog pCatalog,pObjectID)
        {
            //栅格目录继承了IFeatureClass
            IFeatureClass pFeatureClass = (IFeatureClass)pCatalog;
            IRasterCatalogItem pRasterCatalogItem = (IRasterCatalogItem)pFeatureClass.GetFeature(pObjectID);
            return pRasterCatalogItem.RasterDataset;
        }
        */
        /// <summary>
        /// 打开镶嵌数据集
        /// </summary>
        /// <param name="pFGDBPath"></param>
        /// <param name="pMDame"></param>
        /// <returns></returns>
        IMosaicDataset GetMosicDataset(string pFGDBPath, string pMDame)
        {
            IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
            IWorkspace pFgdbWorkspace = pWorkspaceFactory.OpenFromFile(pFGDBPath, 0);
            IMosaicWorkspaceExtensionHelper pMosaicExentionHelper = new MosaicWorkspaceExtensionHelperClass();
            IMosaicWorkspaceExtension pMosaicExtention = pMosaicExentionHelper.FindExtension(pFgdbWorkspace);
            return pMosaicExtention.OpenMosaicDataset(pMDame);
        }
        /// <summary>
        /// 创建一个镶嵌数据集
        /// </summary>
        /// <param name="pFGDBPath"></param>
        /// <param name="pMDame"></param>
        /// <param name="pSrs"></param>
        /// <returns></returns>
        IMosaicDataset CreateMosaicDataset(string pFGDBPath, string pMDame, ISpatialReference pSrs)
        {
            IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
            IWorkspace pFgdbWorkspace = pWorkspaceFactory.OpenFromFile(pFGDBPath, 0);
            ICreateMosaicDatasetParameters pCreationPars = new CreateMosaicDatasetParametersClass();
            pCreationPars.BandCount = 3;
            pCreationPars.PixelType = rstPixelType.PT_UCHAR;
            IMosaicWorkspaceExtensionHelper pMosaicExentionHelper = new MosaicWorkspaceExtensionHelperClass();
            IMosaicWorkspaceExtension pMosaicExtention = pMosaicExentionHelper.FindExtension(pFgdbWorkspace);
            return pMosaicExtention.CreateMosaicDataset(pMDame, pSrs, pCreationPars, "");
        }
        /// <summary>
        /// 根据图层的名称获取图层的方法
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        private ILayer GetLayer(IMap pMap, string LayerName)
        {
            IEnumLayer pEnunLayer;
            pEnunLayer = pMap.get_Layers(null, false);
            pEnunLayer.Reset();
            ILayer pRetureLayer;
            pRetureLayer = pEnunLayer.Next();
            while (pRetureLayer != null)
            {
                if (pRetureLayer.Name == LayerName)
                {
                    break;
                }
                pRetureLayer = pEnunLayer.Next();
            }
            return pRetureLayer;
        }

        /// <summary>
        /// 输出结果为一张表，这张表有三个字段，其中面ID为面要素数据的FID
        /// 个数用于记录这个面包含的点的个数
        /// </summary>
        /// <param name="_TablePath"></param>
        /// <param name="_TableName"></param>
        /// <returns></returns>
        public ITable CreateTable(string _TablePath, string _TableName)
        {
            IWorkspaceFactory pWks = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pFwk = pWks.OpenFromFile(_TablePath, 0) as IFeatureWorkspace;
            //用于记录面中的ID;
            IField pFieldID = new FieldClass();
            IFieldEdit pFieldIID = pFieldID as IFieldEdit;
            pFieldIID.Type_2 = esriFieldType.esriFieldTypeInteger;
            pFieldIID.Name_2 = "面ID";
            //用于记录个数的;
            IField pFieldCount = new FieldClass();
            IFieldEdit pFieldICount = pFieldCount as IFieldEdit;
            pFieldICount.Type_2 = esriFieldType.esriFieldTypeInteger;
            pFieldICount.Name_2 = "个数";
            //用于添加表中的必要字段
            ESRI.ArcGIS.Geodatabase.IObjectClassDescription objectClassDescription =
            new ESRI.ArcGIS.Geodatabase.ObjectClassDescriptionClass();
            IFields pTableFields = objectClassDescription.RequiredFields;
            IFieldsEdit pTableFieldsEdit = pTableFields as IFieldsEdit;
            pTableFieldsEdit.AddField(pFieldID);
            pTableFieldsEdit.AddField(pFieldCount);
            ITable pTable = pFwk.CreateTable(_TableName, pTableFields, null, null,
            "");
            return pTable;
        }
        /// <summary>
        /// 统计需要的数据
        /// 第一个参数为面数据，第二个参数为点数据，第三个为输出的表
        /// </summary>
        /// <param name="_pPolygonFClass"></param>
        /// <param name="_pPointFClass"></param>
        /// <param name="_pTable"></param>
        public void StatisticPointCount(IFeatureClass _pPolygonFClass, IFeatureClass _pPointFClass, ITable _pTable)
        {
            IFeatureCursor pPolyCursor = _pPolygonFClass.Search(null, false);
            IFeature pPolyFeature = pPolyCursor.NextFeature();
            while (pPolyFeature != null)
            {
                IGeometry pPolGeo = pPolyFeature.Shape;
                int Count = 0;
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = pPolGeo;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                //spatialFilter.WhereClause = "矿种=" + "'煤'";
                IFeatureCursor pPointCur = _pPointFClass.Search(spatialFilter, false);
                if (pPointCur != null)
                {
                    IFeature pPointFeature = pPointCur.NextFeature();
                    while (pPointFeature != null)
                    {
                        pPointFeature = pPointCur.NextFeature();
                        Count++;
                    }
                }
                if (Count != 0)
                {
                    IRow pRow = _pTable.CreateRow();
                    pRow.set_Value(1, pPolyFeature.get_Value(0));
                    pRow.set_Value(2, Count);
                    pRow.Store();
                }
                pPolyFeature = pPolyCursor.NextFeature();
            }
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="_pSWorkspaceFactory"></param>
        /// <param name="_pSWs"></param>
        /// <param name="_pSName"></param>
        /// <param name="_pTWorkspaceFactory"></param>
        /// <param name="_pTWs"></param>
        /// <param name="_pTName"></param>
        public void ConvertFeatureClass(IWorkspaceFactory _pSWorkspaceFactory, string _pSWs, string _pSName, IWorkspaceFactory _pTWorkspaceFactory, string _pTWs, string _pTName)
        {
            //Open the source and target workspace
            IWorkspace pSWorkspace = _pSWorkspaceFactory.OpenFromFile(_pSWs, 0);
            IWorkspace pTWorkspace = _pTWorkspaceFactory.OpenFromFile(_pTWs, 0);
            IFeatureWorkspace pFtWs = pSWorkspace as IFeatureWorkspace;
            IFeatureClass pSourceFeatureClass = pFtWs.OpenFeatureClass(_pSName);
            IDataset pSDataset = pSourceFeatureClass as IDataset;
            IFeatureClassName pSourceFeatureClassName = pSDataset.FullName as IFeatureClassName;

            IDataset pTDataset = (IDataset)pTWorkspace;
            IName pTDatasetName = pTDataset.FullName;
            IWorkspaceName pTargetWorkspaceName = (IWorkspaceName)pTDatasetName;
            IFeatureClassName pTargetFeatureClassName = new FeatureClassNameClass();
            IDatasetName pTargetDatasetName = (IDatasetName)pTargetFeatureClassName;
            pTargetDatasetName.Name = _pTName;
            pTargetDatasetName.WorkspaceName = pTargetWorkspaceName;
            // 创建字段检查对象
            IFieldChecker pFieldChecker = new FieldCheckerClass();
            IFields sourceFields = pSourceFeatureClass.Fields;
            IFields pTargetFields = null;
            IEnumFieldError pEnumFieldError = null;
            pFieldChecker.InputWorkspace = pSWorkspace;
            pFieldChecker.ValidateWorkspace = pTWorkspace;
            // 验证字段
            pFieldChecker.Validate(sourceFields, out pEnumFieldError, out pTargetFields);
            if (pEnumFieldError != null)
            {
                // Handle the errors in a way appropriate to your application.
                Console.WriteLine("Errors were encountered during field validation.");
            }
            String pShapeFieldName = pSourceFeatureClass.ShapeFieldName;
            int pFieldIndex = pSourceFeatureClass.FindField(pShapeFieldName);
            IField pShapeField = sourceFields.get_Field(pFieldIndex);
            IGeometryDef pTargetGeometryDef = pShapeField.GeometryDef;
            // 创建要素转换对象
            IFeatureDataConverter pFDConverter = new FeatureDataConverterClass();
            IEnumInvalidObject pEnumInvalidObject = pFDConverter.ConvertFeatureClass(pSourceFeatureClassName, null, null, pTargetFeatureClassName, pTargetGeometryDef, pTargetFields, "", 1000, 0);
            // Check for errors.
            IInvalidObjectInfo pInvalidInfo = null;
            pEnumInvalidObject.Reset();
            while ((pInvalidInfo = pEnumInvalidObject.Next()) != null)
            {
                // Handle the errors in a way appropriate to the application.
                Console.WriteLine("Errors occurred for the following feature: {0}", pInvalidInfo.InvalidObjectID);
            }

        }

        /// <summary>
        /// 创建一个Point对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private IPoint ConstructPoint(double x, double y)
        {
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(x, y);
            return pPoint;
        }

        private object pMissing = Type.Missing;
        /// <summary>
        /// 构建MultiPoint几何对象
        /// </summary>
        /// <returns></returns>
        public IGeometry GetMultipointGeometry()
        {
            const double MultipointPointCount = 25;
            IPointCollection pPointCollection = new MultipointClass();
            for (int i = 0; i < MultipointPointCount; i++)
            {
                pPointCollection.AddPoint(GetPoint(), ref pMissing, ref pMissing);
            }
            return pPointCollection as IGeometry;
        }
        private IPoint GetPoint()
        {
            const double Min = -10;
            const double Max = 10;
            Random pRandom = new Random();
            double x = Min + (Max - Min) * pRandom.NextDouble();
            double y = Min + (Max - Min) * pRandom.NextDouble();
            return ConstructPoint(x, y);
        }

        /// <summary>
        /// Polyline 函数
        /// </summary>
        /// <returns></returns>
        public IGeometry GetPolylineGeometry()
        {

            const double PathCount = 3;
            const double PathVertexCount = 3;
            IGeometryCollection pGeometryCollection = new PolylineClass();
            for (int i = 0; i < PathCount; i++)
            {
                IPointCollection pPointCollection = new PathClass();
                for (int j = 0; j < PathVertexCount; j++)
                {
                    pPointCollection.AddPoint(GetPoint(), ref pMissing, ref pMissing);
                }
                pGeometryCollection.AddGeometry(pPointCollection as IGeometry, ref pMissing,
                ref pMissing);
            }
            return pGeometryCollection as IGeometry;
        }

        /// <summary>
        /// 通过点构造面函数
        /// </summary>
        /// <param name="pPointCollection"></param>
        /// <returns></returns>
        public IPolygon CreatePolygonByPoints(IPointCollection pPointCollection)
        {
            IGeometryBridge2 pGeometryBridge2 = new GeometryEnvironmentClass();
            IPointCollection4 pPolygon = new PolygonClass();
            WKSPoint[] pWKSPoint = new WKSPoint[pPointCollection.PointCount];
            for (int i = 0; i < pPointCollection.PointCount; i++)
            {
                pWKSPoint[i].X = pPointCollection.get_Point(i).X;
                pWKSPoint[i].Y = pPointCollection.get_Point(i).Y;
            }
            pGeometryBridge2.SetWKSPoints(pPolygon, ref pWKSPoint);
            IPolygon pPoly = pPolygon as IPolygon;
            pPoly.Close();
            return pPoly;
        }

        /// <summary>
        /// 平头缓冲
        /// </summary>
        /// <param name="pLline1"></param>
        /// <param name="pBufferDis"></param>
        /// <returns></returns>
        private IPolygon FlatBuffer(IPolyline pLline1, double pBufferDis)
        {
            object o = System.Type.Missing;
            //分别对输入的线平移两次（正方向和负方向）
            IConstructCurve pCurve1 = new PolylineClass();
            pCurve1.ConstructOffset(pLline1, pBufferDis, ref o, ref o);
            IPointCollection pCol = pCurve1 as IPointCollection;
            IConstructCurve pCurve2 = new PolylineClass();
            pCurve2.ConstructOffset(pLline1, -1 * pBufferDis, ref o, ref o);
            //把第二次平移的线的所有节点翻转
            IPolyline pline2 = pCurve2 as IPolyline;
            pline2.ReverseOrientation();
            //把第二条的所有节点放到第一条线的IPointCollection里面
            IPointCollection pCol2 = pline2 as IPointCollection;
            pCol.AddPointCollection(pCol2);
            //用面去初始化一个IPointCollection
            IPointCollection pPointCol = new PolygonClass();
            pPointCol.AddPointCollection(pCol);
            //把IPointCollection转换为面
            IPolygon pPolygon = pPointCol as IPolygon;
            //简化节点次序
            pPolygon.SimplifyPreserveFromTo();
            return pPolygon;
        }

        /// <summary>
        /// 等距离打断线
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <param name="inPoints"></param>
        /// <returns></returns>
        private IEnumGeometry MakeMultiPoints(IPolyline pGeometry, int inPoints)
        {
            IConstructGeometryCollection pConGeoCollection = new GeometryBagClass();
            pConGeoCollection.ConstructDivideEqual(pGeometry, inPoints, esriConstructDivideEnum.esriDivideIntoPolylines);
            IEnumGeometry pEnumGeometry = pConGeoCollection as IEnumGeometry;
            return pEnumGeometry;
        }

        /// <summary>
        /// 同一基准面的坐标转换
        /// </summary>
        /// <param name="pPoint"></param>
        /// <param name="pBool"></param>
        /// <returns></returns>
        private IPoint GetpProjectPoint(IPoint pPoint, bool pBool)
        {
            ISpatialReferenceFactory pSpatialReferenceEnvironemnt = new SpatialReferenceEnvironment();
            ISpatialReference pFormSpatialReference = pSpatialReferenceEnvironemnt.CreateGeographicCoordinateSystem((int)esriSRGeoCS3Type.esriSRGeoCS_Xian1980);
            ISpatialReference pToSpatialReference = pSpatialReferenceEnvironemnt.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_Zone_34);//西安80
            if (pBool == true)//球面转平面
            {
                IGeometry pGeo = (IGeometry)pPoint;
                pGeo.SpatialReference = pFormSpatialReference;
                pGeo.Project(pToSpatialReference);
                return pPoint;
            }
            else //平面转球面
            {
                IGeometry pGeo = (IGeometry)pPoint;
                pGeo.SpatialReference = pToSpatialReference;
                pGeo.Project(pFormSpatialReference);
                return pPoint;
            }
        }

        /// <summary>
        /// 进行Intersect操作
        /// </summary>
        /// <param name="_pFtClass"></param>
        /// <param name="_pFtOverlay"></param>
        /// <param name="_FilePath"></param>
        /// <param name="_pFileName"></param>
        /// <returns></returns>
        public IFeatureClass Intsect(IFeatureClass _pFtClass, IFeatureClass _pFtOverlay, string _FilePath, string _pFileName)
        {
            IFeatureClassName pOutPut = new FeatureClassNameClass();
            pOutPut.ShapeType = _pFtClass.ShapeType;
            pOutPut.ShapeFieldName = _pFtClass.ShapeFieldName;
            pOutPut.FeatureType = esriFeatureType.esriFTSimple;
            //set output location and feature class name
            IWorkspaceName pWsN = new WorkspaceNameClass();
            pWsN.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory";
            pWsN.PathName = _FilePath;
            //也可以用这种方法，IName 和IDataset的用法
            /* IWorkspaceFactory pWsFc = new ShapefileWorkspaceFactoryClass();
            IWorkspace pWs = pWsFc.OpenFromFile(_FilePath， 0);
            IDataset pDataset = pWs as IDataset;
            IWorkspaceName pWsN = pDataset.FullName as IWorkspaceName;
            */
            IDatasetName pDatasetName = pOutPut as IDatasetName;
            pDatasetName.Name = _pFileName;
            pDatasetName.WorkspaceName = pWsN;
            IBasicGeoprocessor pBasicGeo = new BasicGeoprocessorClass();
            IFeatureClass pFeatureClass = pBasicGeo.Intersect(_pFtClass as ITable, false, _pFtOverlay as ITable, false, 0.1, pOutPut);
            return pFeatureClass;
        }
    }
}
