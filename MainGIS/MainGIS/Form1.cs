using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
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
            IWorkspace pFgdbWorkspace = pWorkspaceFactory.OpenFromFile(pFGDBPath,0);
            IMosaicWorkspaceExtensionHelper pMosaicExentionHelper = new MosaicWorkspaceExtensionHelperClass();
            IMosaicWorkspaceExtension pMosaicExtention = pMosaicExentionHelper.FindExtension(pFgdbWorkspace);
            return pMosaicExtention.OpenMosaicDataset(pMDame);
        }


    }
}
