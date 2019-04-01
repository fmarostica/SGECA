using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;

namespace SGECA.ColaDeImpresion
{
    public partial class FrmLibroIva : Form, SGECA.LogManager.IObserver, SGECA.LogManager.ISubject
    {
        #region Observer Pattern
        private List<object> Observers = new List<object>();

        /// <summary>
        /// Método encargado de recibir notificaciones del subscriptor donde  ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void UpdateState(LogManager.IMensaje mensaje)
        {
            Notify(mensaje);
        }

        /// <summary>
        /// Método encargado de notificar al subscriptor que ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void Notify(LogManager.IMensaje mensaje)
        {
           
            // Recorremos cada uno de los observadores para notificarles el evento.
            foreach (LogManager.IObserver observer in this.Observers)
            {
                // Indicamos a cada uno de los subscriptores la actualización del 
                // estado (evento) producido.
                observer.UpdateState(mensaje);
            }

           

        } // Notify



        /// <summary>
        /// Método encargado de agregar un observador para que el subscriptor le 
        /// pueda notificar al subscriptor el evento.
        /// </summary>
        /// <param name="observer">Interfaz IObserver que indica el observador.</param>
        public void Subscribe(LogManager.IObserver observer)
        {
            if (!this.Observers.Contains(observer))
                // Agregamos el subscriptor a la lista de subscriptores del publicador.
                this.Observers.Add(observer);
        } // Subscribe


        /// <summary>
        /// Método encargado de eliminar un observador para que el subscriptor no le 
        /// notifique ningún evento más al que era su subscriptor.
        /// </summary>
        /// <param name="observer">Interfaz IObserver que indica el observador.</param>
        public void Unsubscribe(LogManager.IObserver observer)
        {
            // Eliminamos el subscriptor de la lista de subscriptores del publicador.
            this.Observers.Remove(observer);
        }


        // Unsubscribe

        #endregion

        public FrmLibroIva()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        public void mostrar(int id)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DateTime desde = DateTime.Now, hasta = DateTime.Now;
                string titulo = "", folio = "";

                MySqlConnection cone = new MySqlConnection(ConfigurationManager.ConnectionStrings["ce"].ConnectionString);
                MySqlCommand cmdlvt = new MySqlCommand("select * from libroivaventas where lvt_id = @id ", cone);
                cmdlvt.Parameters.AddWithValue("id", id);

                cone.Open();
                MySqlDataReader dr = cmdlvt.ExecuteReader();
                if (dr.Read())
                {
                    DateTime.TryParse(dr["lvt_desde"].ToString(), out desde);
                    DateTime.TryParse(dr["lvt_hasta"].ToString(), out hasta);
                    hasta = hasta.AddMinutes(1439).AddSeconds(59).AddMilliseconds(99);
                    titulo = dr["lvt_titulo"].ToString() ;
                    folio = dr["lvt_folio"].ToString();
                }
                cone.Close();

                if (titulo == "")
                    titulo = "Libro IVA Ventas";

                if (folio == "")
                    folio = "0";

                List<object> trabajosPendientes = new List<object>();
                MySqlCommand cmd = new MySqlCommand("select * from vw_libroivaventas where cen_fecha >= @desde and cen_fecha <= @hasta", cone);
                cmd.Parameters.AddWithValue("desde", desde);
                cmd.Parameters.AddWithValue("hasta", hasta);

                cone.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                cone.Close();

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\reports\LibroIvaVentas.rdlc"; // bind reportviewer with .rdlc
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Titulo", titulo));
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Folio", folio));
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Desde", desde.ToString("dd/MM/yyyy")));
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Hasta", hasta.ToString("dd/MM/yyyy")));
                reportViewer1.LocalReport.Refresh();
                reportViewer1.RefreshReport(); // refresh report
                this.Cursor = Cursors.Default;

                this.ShowDialog();
            }
            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.FrmLibroIva",
                                            "mostrar",
                                            0,
                                            "Error al intentar mostrar el documento.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);
                this.Close();
            }

            this.Cursor = Cursors.Default;
        }
    }
}
