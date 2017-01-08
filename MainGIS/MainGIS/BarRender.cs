using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGIS
{
    class BarRender
    {
        /// <summary>
        /// 柱状图
        /// </summary>
        /// <param name="pMapcontrol"></param>
        /// <param name="pFtLayer"></param>
        /// <param name="pFieldName1"></param>
        /// <param name="pFieldName2"></param>
        public BarRender(AxMapControl pMapcontrol, IFeatureLayer pFtLayer, String pFieldName1, string pFieldName2)
        {
            IGeoFeatureLayer pGeoFeatureLayer = pFtLayer as IGeoFeatureLayer;
            pGeoFeatureLayer.ScaleSymbols = true;
            IFeatureClass pFeatureClass = pFtLayer.FeatureClass;
            //定义柱状图渲染组建对象
            IChartRenderer pChartRenderer = new ChartRendererClass();
            //定义渲染字段对象并给字段对象实例化为pChartRenderer
            IRendererFields pRendererFields;
            pRendererFields = (IRendererFields)pChartRenderer;
            //向渲染字段对象中添加字段--- 待补充自定义添加
            pRendererFields.AddField(pFieldName1, pFieldName1);
            pRendererFields.AddField(pFieldName2, pFieldName2);
            ITable pTable = pGeoFeatureLayer as ITable;
            int[] pFieldIndecies = new int[2];
            pFieldIndecies[0] = pTable.FindField(pFieldName1);
            pFieldIndecies[1] = pTable.FindField(pFieldName2);
            IDataStatistics pDataStat = new DataStatisticsClass();
            IFeatureCursor pFtCursor = pFtLayer.FeatureClass.Search(null, false);
            pDataStat.Cursor = pFtCursor as ICursor;
            pDataStat.Field = pFieldName2;
            double pMax = pDataStat.Statistics.Maximum;
            // 定义并设置渲染时用的chart marker symbol
            IBarChartSymbol pBarChartSymbol = new BarChartSymbolClass();
            pBarChartSymbol.Width = 6;
            IChartSymbol pChartSymbol;
            pChartSymbol = pBarChartSymbol as IChartSymbol;
            IMarkerSymbol pMarkerSymbol;
            pMarkerSymbol = (IMarkerSymbol)pBarChartSymbol;
            IFillSymbol pFillSymbol;
            //设置pChartSymbol的最大值
            pChartSymbol.MaxValue = pMax;
            // 设置bars的最大高度
            pMarkerSymbol.Size = 80;
            //下面给每一个bar设置符号
            //定义符号数组
            ISymbolArray pSymbolArray = (ISymbolArray)pBarChartSymbol;
            //添加第一个符号
            pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = GetRGBColor(193, 252, 179) as IColor;
            pSymbolArray.AddSymbol(pFillSymbol as ISymbol);
            //添加第二个符号
            pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = GetRGBColor(145, 55, 251) as IColor;
            pSymbolArray.AddSymbol(pFillSymbol as ISymbol);
            pChartRenderer.ChartSymbol = pChartSymbol as IChartSymbol;
            //pChartRenderer.Label = "AREA";
            pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = GetRGBColor(239, 228, 190);
            pChartRenderer.BaseSymbol = (ISymbol)pFillSymbol;
            pChartRenderer.CreateLegend();
            pChartRenderer.UseOverposter = false;
            //将柱状图渲染对象与渲染图层挂钩
            pGeoFeatureLayer.Renderer = (IFeatureRenderer)pChartRenderer;
            //刷新地图和TOOCotrol
            IActiveView pActiveView = pMapcontrol.ActiveView as IActiveView;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }
        public IRgbColor GetRGBColor(int r, int g, int b)
        {
            IRgbColor pRGB;
            pRGB = new RgbColorClass();
            pRGB.Red = r;
            pRGB.Green = g;
            pRGB.Blue = b;
            return pRGB;
        }
    }
}
