using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SGECA.ControlesComunes
{
    public partial class CtrlSupEdicion : UserControl, Comun.Interfaces.IObserverEventoBotonera
    {
        public event agregar Agregar;
        public event actualizar Actualizar;
        public event eliminar Eliminar;
        public event cancelar Cancelar;
        public event deshacer Deshacer;
        public event editar Editar;
        public event cerrar Cerrar;
        public event habilitar Habilitar;
        public event deshabilitar Deshabilitar;
        public event exportar Exportar;


        public delegate void cerrar(object sender, EventArgs e);
        public delegate void editar(object sender, EventArgs e);
        public delegate void agregar(object sender, EventArgs e);
        public delegate void actualizar(object sender, EventArgs e);
        public delegate void eliminar(object sender, EventArgs e);
        public delegate void cancelar(object sender, EventArgs e);
        public delegate void deshacer(object sender, EventArgs e);
        public delegate void habilitar(object sender, EventArgs e);
        public delegate void deshabilitar(object sender, EventArgs e);
        public delegate void exportar(object sender, EventArgs e);
       
        LogManager.Mensaje UltimoMensaje { get; set; }

        #region Observer Pattern
        private List<object> Observers = new List<object>();
        #endregion

        public CtrlSupEdicion()
        {
            InitializeComponent();
        }

        #region getSetBotonera

        public bool BotonAgregar
        {
            get { return tsbAgregar.Enabled; }
            set { tsbAgregar.Enabled = value; }

        }

        public bool BotonEditar
        {
            get { return tsbEditar.Enabled; }
            set { tsbEditar.Enabled = value; }

        }

        public bool BotonCancelar
        {
            get { return tsbCancelar.Enabled; }
            set { tsbCancelar.Enabled = value; }

        }

        public bool BotonEliminar
        {
            get { return tsbEliminar.Enabled; }
            set { tsbEliminar.Enabled = value; }

        }

        public bool BotonDeshacer
        {
            get { return tsbDeshacer.Enabled; }
            set { tsbDeshacer.Enabled = value; }

        }

        public bool BotonActualizar
        {
            get { return tsbActualizar.Enabled; }
            set { tsbActualizar.Enabled = value; }

        }

        public bool BotonCerrar
        {
            get { return tsbCerrar.Enabled; }
            set { tsbCerrar.Enabled = value; }

        }

        public bool BotonHabilitar
        {
            get { return tsbHabilitar.Enabled;}
            set { tsbHabilitar.Enabled = value; }

        }

        public bool BotonDeshabilitar
        {
            get { return tsbDeshabilitar.Enabled; }
            set { tsbDeshabilitar.Enabled = value; }

        }


        public bool BotonExportar
        {
            get { return tsbExportar.Enabled; }
            set { tsbExportar.Enabled = value; }

        }

        #endregion

        #region Click botonera

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            eventoEditar(e);
        }

        private void tsbAgregar_Click(object sender, EventArgs e)
        {
            eventoAgregar(e);
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            eventoActualizar(e);
        }

        private void tsbEliminar_Click(object sender, EventArgs e)
        {
            eventoEliminar(e);
        }

        private void tsbHabilitar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Recuerde grabar para hacer estos cambios permanentes", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            eventoHabilitar( e);
        }

        private void tsbDeshabilitar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Recuerde grabar para hacer estos cambios permanentes", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            eventoDeshabilitar(e);
        }

        void tsbDeshacer_Click(object sender, System.EventArgs e)
        {
            eventoDeshacer(e);
        }

        private void tsbCancelar_Click(object sender, EventArgs e)
        {
            eventoCancelar(e);
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            eventoCerrar(sender, e);
        }
        #endregion

        #region eventosClickBotonera

        private void eventoEditar(EventArgs e)
        {
            if (Editar != null)
                Editar(this, e);
        }

        private void eventoAgregar(EventArgs e)
        {
            if (Agregar != null)
                Agregar(this, e);
        }

        private void eventoActualizar(EventArgs e)
        {
            if (Actualizar != null)
                Actualizar(this, e);
        }

        private void eventoEliminar(EventArgs e)
        {
            Comun.MessageBox msjBox = new Comun.MessageBox();

            if (Eliminar != null && (msjBox.MostrarMessageBoxConfirmacion("Esta seguro que " +
                "desea eliminar el registro? ", Comun.Enums.
                EMessageBoxTitulo.Confirmación.ToString())) == true)
                Eliminar(this, e);
        }

        private void eventoHabilitar(EventArgs e)
        {
            if (Habilitar != null)
                Habilitar(this, e);
        }

        private void eventoDeshabilitar(EventArgs e)
        {
            if (Deshabilitar != null)
                Deshabilitar(this, e);
        }

        private void eventoDeshacer(System.EventArgs e)
        {
            if (Deshacer != null)
                Deshacer(this, e);
        }

        private void eventoCancelar(EventArgs e)
        {
            if (Cancelar != null)
                Cancelar(this, e);
        }

        private void eventoCerrar(object sender, EventArgs e)
        {
            if (Cerrar != null)
                Cerrar(sender, e);
        }

        #endregion

        /// <summary>
        /// Método encargado de notificar a la toolbar que evento ha sido disparado 
        /// desde el teclado.
        /// </summary>
        /// <param name="objeto">object</param>
        /// <param name="evento">LogManager.IEventoBotonera de tipo KeyEventArgs</param>
        public void UpdateState(object objeto, System.Windows.Forms.KeyEventArgs evento)
        {
           
            if (evento != null)
                try
                {
                    manejoTeclado(objeto, evento);
                }
                catch (Exception ex)
                {
                    UltimoMensaje =DAL.GestionErrores.obtenerError(ex);

                    Notify(UltimoMensaje);
                }
        }

        /// <summary>
        /// Método encargado de manejar ToolBar por teclado. 
        /// </summary>
        public void manejoTeclado(object objeto, KeyEventArgs key)
        {

            switch (key.KeyCode)
            {
                case Keys.F4:
                    if (BotonEditar)
                    {
                        eventoEditar(new EventArgs());
                    }
                    break;
                case Keys.F5:
                    if (BotonAgregar)
                    {
                        eventoAgregar(new EventArgs());
                    }
                    break;
                case Keys.F6:
                    if (BotonActualizar)
                    {
                        eventoActualizar(new EventArgs());
                    }
                    break;
                case Keys.F7:
                    if (BotonEliminar)
                    {
                        eventoEliminar(new EventArgs());
                    }
                    break;
                case Keys.F8:
                    if (BotonDeshabilitar)
                    {
                        eventoDeshabilitar(new EventArgs());
                    }
                    break;
                case Keys.F9:
                    if (BotonHabilitar)
                    {
                        eventoHabilitar(new EventArgs());
                    }
                    break;
                case Keys.F10:
                    if (BotonDeshacer)
                    {
                        eventoDeshacer(new EventArgs());
                    }
                    break;
                case Keys.F11:
                    if (BotonCancelar)
                    {
                        eventoCancelar(new EventArgs());
                    }
                    break;
            }

        }

        #region Observer Pattern

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
            // Agregamos el subscriptor a la lista de subscriptores del publicador.
            if (!Observers.Contains(observer))
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
        } // Unsubscribe

        #endregion

        /// <summary>
        /// Método encargado de notificar un mensaje a través del método "Notify".
        /// </summary>
        public void UpdateState(LogManager.IMensaje mensaje)
        {
            Notify(mensaje);
        }

        private void tsbExportar_Click(object sender, EventArgs e)
        {
            if (Exportar != null)
                Exportar(this, new EventArgs());
        }

    }
}
