using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
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
    public partial class frmOpenMDB : Form
    {
        public frmOpenMDB()
        {
            InitializeComponent();
        }

        private void btGetFeature_Click(object sender, EventArgs e)
        {
            string WsName = WsPath();
            if (WsName != "")
            {
                IWorkspaceFactory pWsFt = new AccessWorkspaceFactoryClass();
                IWorkspace pWs = pWsFt.OpenFromFile(WsName,0);
                IEnumDataset pEDataset = pWs.get_Datasets(esriDatasetType.esriDTAny);
                IDataset pDataset = pEDataset.Next();
                while (pDataset != null)
                {
                    if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        FeatureClassBox.Items.Add(pDataset.Name);
                    }
                    //如果是数据集
                    else if (pDataset.Type == esriDatasetType.esriDTFeatureDataset)
                    {
                        IEnumDataset pESubDataset = pDataset.Subsets;
                        IDataset pSubDataset = pESubDataset.Next();
                        while (pSubDataset != null)
                        {
                            FeatureClassBox.Items.Add(pSubDataset.Name);
                            pSubDataset = pESubDataset.Next();
                        }
                    }
                    pDataset = pEDataset.Next();
                }
 
            }
            FeatureClassBox.Text = FeatureClassBox.Items[0].ToString();
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
        /// <summary>
        /// 判断要素是否被编辑
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        public bool ISEdit(IFeatureClass pFeatureClass)
        {
            IDatasetEdit pDataEdit = pFeatureClass as IDatasetEdit;
            return pDataEdit.IsBeingEdited();
        }

    }
}
