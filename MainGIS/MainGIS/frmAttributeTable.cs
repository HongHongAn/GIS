using ESRI.ArcGIS.Carto;
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
    public partial class frmAttributeTable : Form
    {
        IFeatureLayer pFeatureLayer;
        ILayer pLayer;
        public frmAttributeTable(ILayer pLyr)
        {
            InitializeComponent();
            pLayer = pLyr;
        }
        public static string ParseFieldType(esriFieldType fieldType)
        {
            switch (fieldType)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DataTine";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.String";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";
                default:
                    return "System.String";
            }
        }

        public void Itable2Dable()
        {
            pFeatureLayer = pLayer as IFeatureLayer;
            IFields pFields;
            pFields = pFeatureLayer.FeatureClass.Fields;
            dataGridView1.ColumnCount = pFields.FieldCount;
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                string fldName = pFields.get_Field(i).Name;
                dataGridView1.Columns[i].Name = fldName;
                dataGridView1.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
            }
            IFeatureCursor pFeatureCursor;
            pFeatureCursor = pFeatureLayer.FeatureClass.Search(null, false);

            IFeature pFeature;
            pFeature = pFeatureCursor.NextFeature();

            while (pFeature != null)
            {
                string[] fldValue = new string[pFields.FieldCount];
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string fldName;
                    fldName = pFields.get_Field(i).Name;
                    if (fldName == pFeatureLayer.FeatureClass.ShapeFieldName)
                    {
                        fldValue[i] = Convert.ToString(pFeature.Shape.GeometryType);
                    }
                    else
                        fldValue[i] = Convert.ToString(pFeature.get_Value(i));
                }
                dataGridView1.Rows.Add(fldValue);
                pFeature = pFeatureCursor.NextFeature();
            }
        }

        private void frmAttributeTable_Load(object sender, EventArgs e)
        {
            Itable2Dable();
        }
    }
}
