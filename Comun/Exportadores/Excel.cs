using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SGECA.Comun.Exportadores
{
    public class Excel
    {

        public bool exporta(IList datos)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Archivo Excel |*.xls";
                saveFileDialog1.Title = "Grabar un archivo de formato Excel";
                saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.
                if (saveFileDialog1.FileName == "")
                    return false;


                System.IO.StreamWriter excelDoc;

                excelDoc = new System.IO.StreamWriter(saveFileDialog1.FileName);
                const string startExcelXML = "<xml version>\r\n<Workbook " +
                      "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
                      " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
                      "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
                      "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
                      "office:spreadsheet\">\r\n <Styles>\r\n " +
                      "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
                      "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
                      "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
                      "\r\n <Protection/>\r\n </Style>\r\n " +
                      "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
                      "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
                      "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
                      " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
                      "ss:ID=\"Decimal\">\r\n <NumberFormat " +
                      "ss:Format=\"0.0000\"/>\r\n </Style>\r\n " +
                      "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
                      "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
                      "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
                      "ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n " +
                      "</Styles>\r\n ";
                const string endExcelXML = "</Workbook>";

                int rowCount = 0;
                int sheetCount = 1;

                excelDoc.Write(startExcelXML);
                excelDoc.Write("<Worksheet ss:Name=\"Hoja" + sheetCount + "\">");
                excelDoc.Write("<Table>");
                excelDoc.Write("<Row>");
                //for (int x = 0; x < source.Tables[0].Columns.Count; x++) REEMPLAZO

                Dictionary<string, string> objHeaders = new Dictionary<string, string>();

                PropertyInfo[] headerInfo = datos[0].GetType().GetProperties();

                foreach (PropertyInfo item in headerInfo)
                {
                    excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                    //excelDoc.Write(source.Tables[0].Columns[x].ColumnName);
                    excelDoc.Write(item.Name);
                    excelDoc.Write("</Data></Cell>");
                }

                excelDoc.Write("</Row>");
                //foreach (DataRow x in source.Tables[0].Rows) REEMPLAZO
                foreach (var x in datos)
                {
                    rowCount++;
                    //------si el numero de filas > 64000 cse crea una nueva pagina.
                    if (rowCount == 64000)
                    {
                        rowCount = 0;
                        sheetCount++;
                        excelDoc.Write("</Table>");
                        excelDoc.Write(" </Worksheet>");
                        excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                        excelDoc.Write("<Table>");
                    }
                    //-----------------------------------------------------------
                    excelDoc.Write("<Row>"); //ID=" + rowCount + "
                    //for (int y = 0; y < source.Tables[0].Columns.Count; y++)
                    foreach (PropertyInfo item in headerInfo)
                    {
                        string valor = x.GetType().GetProperty(item.Name).GetValue(x, null).ToString();

                        if(item.PropertyType.Name == "Boolean" )
                        {
                            valor =((Boolean)x.GetType().GetProperty(item.Name).GetValue(x, null)) ? "Si" : "No";
                        }
                        string XMLstring = valor;
                      
                        XMLstring = XMLstring.Trim();
                        XMLstring = XMLstring.Replace("&", "&");
                        XMLstring = XMLstring.Replace(">", ">");
                        XMLstring = XMLstring.Replace("<", "<");
                        excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                       "<Data ss:Type=\"String\">");
                        excelDoc.Write(XMLstring);
                        excelDoc.Write("</Data></Cell>");
                    }
                    excelDoc.Write("</Row>");
                }
                excelDoc.Write("</Table>");
                excelDoc.Write(" </Worksheet>");
                excelDoc.Write(endExcelXML);
                excelDoc.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
    }
}
