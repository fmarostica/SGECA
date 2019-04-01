using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SGECA.Comun
{
    public class Parametros
    {

        private static string rutaLog = "";
        private static string rutaEntrada = "";
        private static string rutaSalida = "";
        private static string rutaPlantillas = "";
        private static string rutaAlarmas = "";
        private static string rutaConfig = "";
        private static string rutaBackup = "";
        private static string rutaReposicion = "";
        private static string lenguajePorDefecto = "";
        //private static string puesto = "";
        //private static string envioPorDefecto = "";
        //private static string recepcionPorDefecto = "";
        //private static string baudiosPorDefecto = "";
        //private static string tiempoEntreEnviosPorDefecto = "";
        private static string origenDeDatos = "";
        //private static string stockExternoConnectionString = "";
        //private static string stockExternoConsulta = "";

        private static int anchoCaracter = -1;
        private static int backupPedidos = -1;
        private static int habilitaLog = -1;
        private static int modoPrueba = -1;
        public int par_CantidadRegistrosConsultas { get; set; }
        private static string tipoDBMS = "";

        public DateTime ConvierteFecha(string Fecha, string Formato)
        {

            System.Globalization.CultureInfo oCultura = new System.Globalization.CultureInfo("es-es");
            oCultura.DateTimeFormat.FullDateTimePattern = Formato;
            return DateTime.ParseExact(Fecha, Formato, oCultura);
        }

        //public string ObtenerCadena(string Cadena)
        //{

        //    string _CadenaOrig = Cadena.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u'); ;
        //    Cadena = _CadenaOrig.Replace(" ", "").Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u');
        //    System.Resources.ResourceManager rMan;
        //    switch (LenguajePorDefecto)
        //    {
        //        case "EN":
        //            rMan = Comun.Properties.English.ResourceManager;
        //            break;
        //        case "PT":
        //            rMan = Comun.Properties.Portuguese.ResourceManager;
        //            break;
        //        default:
        //            rMan = Comun.Properties.Spanish.ResourceManager;
        //            break;
        //    }
        //    string miCadena = rMan.GetString(Cadena);

        //    if (miCadena == null || miCadena.Length == 0)
        //    {
        //        Cadena = Regex.Replace(_CadenaOrig, @"\w+", new MatchEvaluator(CapitalizeString));

        //        miCadena = rMan.GetString(Cadena.Replace(" ", ""));

        //        if (miCadena == null || miCadena.Length == 0)
        //        {
        //            miCadena = LenguajePorDefecto + "_" + Cadena;
        //        }


        //    }

        //    return miCadena;
        //}


        public static string limpiarEspaciosDobles(string cadena)
        {

            while (cadena.Contains("  "))
                cadena = cadena.Replace("  ", " ");

            return cadena;
        }

        static string CapitalizeString(Match matchString)
        {

            string strTemp = matchString.ToString();

            strTemp = char.ToUpper(strTemp[0]) + strTemp.Substring(1, strTemp.Length - 1).ToLower();

            return strTemp;

        }

        //public object ObtenerObjeto(string NombreObjeto)
        //{
        //    System.Resources.ResourceManager rMan;
        //    switch (LenguajePorDefecto)
        //    {
        //        case "EN":
        //            rMan = Comun.Properties.English.ResourceManager;
        //            break;
        //        case "PT":
        //            rMan = Comun.Properties.Portuguese.ResourceManager;
        //            break;
        //        default:
        //            rMan = Comun.Properties.Spanish.ResourceManager;
        //            break;
        //    }
        //    return rMan.GetObject(NombreObjeto + LenguajePorDefecto);
        //}

        //public string StockExternoConsulta
        //{
        //    get
        //    {
        //        stockExternoConsulta = obtenerString("StockExternoConsulta", stockExternoConsulta);
        //        return stockExternoConsulta;
        //    }
        //}

        //public string StockExternoConnectionString
        //{
        //    get
        //    {
        //        stockExternoConnectionString = obtenerString("StockExternoConnectionString", stockExternoConnectionString);
        //        return stockExternoConnectionString;
        //    }
        //}

        public static string RutaEntrada
        {
            get
            {

                rutaEntrada = obtenerString("RutaEntrada", rutaEntrada);
                if (!IOutil.verificarCarpetaExistenteYEscribible(rutaEntrada))
                {
                    rutaEntrada = null;
                }
                return rutaEntrada;
            }


        }

        public static string RutaSalida
        {
            get
            {

                rutaSalida = obtenerString("RutaSalida", rutaSalida);
                if (!IOutil.verificarCarpetaExistenteYEscribible(rutaSalida))
                {
                    rutaSalida = null;
                }
                return rutaSalida;
            }


        }

        public static string RutaPlantillas
        {
            get
            {

                rutaPlantillas = obtenerString("RutaPlantillas", rutaPlantillas);
                if (!IOutil.verificarCarpetaExistenteYEscribible(rutaPlantillas))
                {
                    rutaPlantillas = null;
                }
                return rutaPlantillas;
            }


        }

        public static bool HabilitaLog
        {
            get
            {
                if (habilitaLog != -1)
                    if (habilitaLog == 0)
                        return false;
                    else
                        return true;

                ExeConfigurationFileMap oExeConfigurationFileMap = new ExeConfigurationFileMap();
                oExeConfigurationFileMap.ExeConfigFilename = @"C:\NeoPicking\NeoPicking.config";
                Configuration oConfig = ConfigurationManager.OpenMappedExeConfiguration(oExeConfigurationFileMap, ConfigurationUserLevel.None);
                AppSettingsSection oAppSettingsSection = oConfig.AppSettings;
                bool fHabilitaLog = false;
                try
                {
                    fHabilitaLog = (oAppSettingsSection.Settings["HabilitaLog"].Value.ToString().ToLower() == "si") ? true : false;
                }
                catch { }

                if (fHabilitaLog)
                    habilitaLog = 1;
                else
                    habilitaLog = 0;

                return fHabilitaLog;
            }

        }


        public static bool ModoPrueba
        {
            get
            {
                if (modoPrueba != -1)
                    if (modoPrueba == 0)
                        return false;
                    else
                        return true;

                ExeConfigurationFileMap oExeConfigurationFileMap = new ExeConfigurationFileMap();
                oExeConfigurationFileMap.ExeConfigFilename = @"C:\NeoPicking\NeoPicking.config";
                Configuration oConfig = ConfigurationManager.OpenMappedExeConfiguration(oExeConfigurationFileMap, ConfigurationUserLevel.None);
                AppSettingsSection oAppSettingsSection = oConfig.AppSettings;
                bool fModoPrueba = true;
                try
                {
                    fModoPrueba = (oAppSettingsSection.Settings["ModoPrueba"].Value.ToString().ToLower() == "si") ? true : false;
                }
                catch { }

                if (fModoPrueba)
                    modoPrueba = 1;
                else
                    modoPrueba = 0;

                return fModoPrueba;
            }

        }

        public static bool BackupPedidos
        {
            get
            {
                if (backupPedidos != -1)
                    if (backupPedidos == 0)
                        return false;
                    else
                        return true;
                ExeConfigurationFileMap oExeConfigurationFileMap = new ExeConfigurationFileMap();
                oExeConfigurationFileMap.ExeConfigFilename = @"C:\NeoPicking\NeoPicking.config";
                Configuration oConfig = ConfigurationManager.OpenMappedExeConfiguration(oExeConfigurationFileMap, ConfigurationUserLevel.None);
                AppSettingsSection oAppSettingsSection = oConfig.AppSettings;

                bool bkPedidos = false;
                try
                {
                    bkPedidos = (oAppSettingsSection.Settings["BackupPedidos"].Value.ToString().ToLower() == "si") ? true : false;
                }
                catch { }

                if (bkPedidos)
                    backupPedidos = 1;
                else
                    backupPedidos = 0;

                return bkPedidos;
            }

        }


        public static string RutaLog
        {
            get
            {
                rutaLog = obtenerString("RutaLog", rutaLog);
                if (!IOutil.verificarCarpetaExistenteYEscribible(rutaLog))
                {
                    rutaLog = null;
                }
                return rutaLog;
            }

        }

        public static string OrigenDeDatos
        {
            get
            {
                origenDeDatos = obtenerString("OrigenDeDatos", origenDeDatos);
                return origenDeDatos;
            }

        }


        public static bool VibracionTodos
        {
            get
            {
                ExeConfigurationFileMap oExeConfigurationFileMap = new ExeConfigurationFileMap();
                oExeConfigurationFileMap.ExeConfigFilename = @"C:\NeoPicking\NeoPicking.config";
                Configuration oConfig = ConfigurationManager.OpenMappedExeConfiguration(oExeConfigurationFileMap, ConfigurationUserLevel.None);
                AppSettingsSection oAppSettingsSection = oConfig.AppSettings;
                try
                {
                    if (oAppSettingsSection.Settings["VibracionTodos"].Value.ToString().ToLower() == "si")
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }

            }
        }

        public static string RutaAlarmas
        {
            get
            {
                rutaAlarmas = obtenerString("RutaAlarmas", rutaAlarmas);
                if (!IOutil.verificarCarpetaExistenteYEscribible(rutaAlarmas))
                {
                    rutaAlarmas = null;
                }
                return rutaAlarmas;
            }

        }

        private static string obtenerString(string clave, string variable)
        {
            if (variable != "")
                return variable;

            string sValor = null;

            ExeConfigurationFileMap oExeConfigurationFileMap = new ExeConfigurationFileMap();
            oExeConfigurationFileMap.ExeConfigFilename = @"C:\NeoPicking\NeoPicking.config";
            Configuration oConfig = ConfigurationManager.OpenMappedExeConfiguration(oExeConfigurationFileMap, ConfigurationUserLevel.None);
            AppSettingsSection oAppSettingsSection = oConfig.AppSettings;
            try
            {
                sValor = oAppSettingsSection.Settings[clave].Value.ToString().ToUpper();
            }
            catch
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("No se encontro la clave \"" +
                    clave + "\" en el archivo " + oExeConfigurationFileMap.ExeConfigFilename
                    , Comun.Enums.EMessageBoxTitulo.Error.ToString());
            }
            variable = sValor;
            return sValor;
        }

        public static string RutaConfig
        {
            get
            {
                rutaConfig = obtenerString("RutaConfig", rutaConfig);
                if (!IOutil.verificarCarpetaExistenteYEscribible(rutaConfig))
                {
                    rutaConfig = null;
                }
                return rutaConfig;
            }

        }

        public static string RutaBackup
        {
            get
            {
                rutaBackup = obtenerString("RutaBackup", rutaBackup);
                if (!IOutil.verificarCarpetaExistenteYEscribible(rutaBackup))
                {
                    rutaBackup = null;
                }
                return rutaBackup;
            }

        }

        public static string RutaReposicion
        {
            get
            {
                rutaReposicion = obtenerString("RutaReposicion", rutaReposicion);
                if (!IOutil.verificarCarpetaExistenteYEscribible(rutaReposicion))
                {
                    rutaReposicion = null;
                }
                return rutaReposicion;
            }

        }



        public static string LenguajePorDefecto
        {
            get
            {
                lenguajePorDefecto = obtenerString("LenguajePorDefecto", lenguajePorDefecto);
                return lenguajePorDefecto;
            }

        }

        public static bool HacerBackupDatabase
        {
            get
            {
                ExeConfigurationFileMap oExeConfigurationFileMap = new ExeConfigurationFileMap();
                oExeConfigurationFileMap.ExeConfigFilename = @"C:\NeoPicking\NeoPicking.config";
                Configuration oConfig = ConfigurationManager.OpenMappedExeConfiguration(oExeConfigurationFileMap, ConfigurationUserLevel.None);
                AppSettingsSection oAppSettingsSection = oConfig.AppSettings;

                bool sHacerBackupDatabase = false;
                try
                {

                    sHacerBackupDatabase = (oAppSettingsSection.Settings["HacerBackupDatabase"].Value.ToString().ToLower() == "si") ? true : false;
                }
                catch
                {
                    Comun.MessageBox msjBox = new Comun.MessageBox();
                    msjBox.MostrarMessageBox("Parámetro faltante o incorrecto: HacerBackupDatabase\n" +
                     "Verifique la siguiente línea en c:\\NeoPicking\\NeoPicking.config\n" +
                     "<add key=\"HacerBackupDatabase\" value=\"valorParámetro\" />", 
                        Comun.Enums.EMessageBoxTitulo.Error.ToString());

                }
                return sHacerBackupDatabase;
            }

        }

        public static bool VerificarActualizaciones
        {
            get
            {
                ExeConfigurationFileMap oExeConfigurationFileMap = new ExeConfigurationFileMap();
                oExeConfigurationFileMap.ExeConfigFilename = @"C:\NeoPicking\NeoPicking.config";
                Configuration oConfig = ConfigurationManager.OpenMappedExeConfiguration(oExeConfigurationFileMap, ConfigurationUserLevel.None);
                AppSettingsSection oAppSettingsSection = oConfig.AppSettings;

                bool sVerificarActualizaciones = false;
                try
                {
                    sVerificarActualizaciones = (oAppSettingsSection.Settings["VerificarActualizaciones"].Value.ToString().ToLower() == "si") ? true : false;
                }
                catch
                {
                    Comun.MessageBox msjBox = new Comun.MessageBox();
                    msjBox.MostrarMessageBox("Parámetro faltante o incorrecto: VerificarActualizaciones\n" +
                     "Verifique la siguiente línea en c:\\NeoPicking\\NeoPicking.config\n" +
                     "<add key=\"VerificarActualizaciones\" value=\"valorParámetro\" />", 
                        Comun.Enums.EMessageBoxTitulo.Error.ToString());
                }
                return sVerificarActualizaciones;
            }

        }

        public static bool PrenderAlarma
        {
            get
            {
                ExeConfigurationFileMap oExeConfigurationFileMap = new ExeConfigurationFileMap();
                oExeConfigurationFileMap.ExeConfigFilename = @"C:\NeoPicking\NeoPicking.config";
                Configuration oConfig = ConfigurationManager.OpenMappedExeConfiguration(oExeConfigurationFileMap, ConfigurationUserLevel.None);
                AppSettingsSection oAppSettingsSection = oConfig.AppSettings;


                bool bPrenderAlarma = true;
                try
                {
                    bPrenderAlarma = (oAppSettingsSection.Settings["PrenderAlarma"].Value.ToString().ToLower() == "si") ? true : false;
                }
                catch { }
                return bPrenderAlarma;
            }

        }

        public static int AnchoCaracter
        {
            get
            {
                string ancho = "";
                if (anchoCaracter != -1)
                    ancho = anchoCaracter.ToString();
                anchoCaracter = int.Parse(obtenerString("AnchoCaracter", ancho));
                return anchoCaracter;

            }
        }

        //public static string Puesto
        //{
        //    get
        //    {
        //        puesto = obtenerString("Puesto", puesto);
        //        return puesto;
        //    }
        //}


        public static TiposDBMS TipoDBMS
        {
            get
            {

                tipoDBMS = obtenerString("TipoDBMS", tipoDBMS);

                TiposDBMS tipo = TiposDBMS.MSSql;

                switch (tipoDBMS.ToLower())
                {
                    case "mysql":
                        tipo = TiposDBMS.MySQL;
                        tipoDBMS = "MySQL";
                        break;
                    case "mssql":
                        tipo = TiposDBMS.MSSql;
                        tipoDBMS = "MSSql";
                        break;
                    default:
                        break;
                }

                return tipo;
            }
        }


        //public static string EnvioPorDefecto
        //{
        //    get
        //    {
        //        envioPorDefecto = obtenerString("EnvioPorDefecto", envioPorDefecto);
        //        return envioPorDefecto;
        //    }
        //}

        //public static string RecepcionPorDefecto
        //{
        //    get
        //    {
        //        recepcionPorDefecto = obtenerString("RecepcionPorDefecto", recepcionPorDefecto);
        //        return recepcionPorDefecto;
        //    }
        //}

        //public static string BaudiosPorDefecto
        //{
        //    get
        //    {
        //        baudiosPorDefecto = obtenerString("BaudiosPorDefecto", baudiosPorDefecto);
        //        return baudiosPorDefecto;
        //    }
        //}

        //public static string TiempoEntreEnviosPorDefecto
        //{
        //    get
        //    {
        //        tiempoEntreEnviosPorDefecto = obtenerString("TiempoEntreEnviosPorDefecto", tiempoEntreEnviosPorDefecto);
        //        return tiempoEntreEnviosPorDefecto;
        //    }
        //}


        public static bool IsNumeric(object Valor)
        {
            long iValor;
            if (Valor is DBNull || Valor == null)
                return false;
            else
                return long.TryParse(Valor.ToString(), out iValor);

        }

        public static bool verificaDirectorio(string Path)
        {
            try
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Path);

                if (!di.Exists)
                {
                    di.Create();
                }

                return true;

            }
            catch
            {
                return false;
            }

        }




        public void Log(string Origen, string Cadena)
        {
            if (Parametros.HabilitaLog)
            {
                FileInfo oFile = new FileInfo(RutaLog + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + "-log.txt");
                StreamWriter oWrite = oFile.AppendText();
                oWrite.WriteLine(DateTime.Now.ToLongTimeString() + " - Origen:" + Origen + " - Mensaje:" + Cadena);
                oWrite.Close();
            }
        }

        public void Temporizador(int Milisegundos)
        {
            DateTime dInicio, dSep, dDemora;
            TimeSpan dSepara = new TimeSpan();

            dDemora = DateTime.Now;
            dInicio = DateTime.Now;
            dSepara = new TimeSpan();

            while (dSepara.TotalMilliseconds < Milisegundos)
            {
                dSep = DateTime.Now;
                dSepara = dSep - dInicio;
            }

        }



        /// <summary> Returns True if so. </summary>
        public bool tThereIsAnInstanceOfThisProgramAlreadyRunning()
        { return tThereIsAnInstanceOfThisProgramAlreadyRunning(false); }


        public bool tThereIsAnInstanceOfThisProgramAlreadyRunning(bool
        tToActivateThePreviousInstance)
        {
            System.Diagnostics.Process oThisProcess;
            System.Diagnostics.Process[] aoProcList;

            oThisProcess = System.Diagnostics.Process.GetCurrentProcess();
            aoProcList = System.Diagnostics.Process.GetProcessesByName
            (oThisProcess.ProcessName); // At least 1.

            if (aoProcList.Length == 1)
                return false; // There's just the current process.

            if (tToActivateThePreviousInstance)
                for (uint i = 0; i < aoProcList.Length; i++)
                    if (aoProcList[i] != oThisProcess)
                    {
                        // Activate the previous instance.
                        //!! AppActivate() is a Vb function.
                        // Q. How is it called in C# ?
                        // A. Add a reference to VBA. Import it. Call it.
                        // Q. Is there a better way ?
                        // A. Calling Herfried ?? Come in Herfried ??
                        break;
                    }

            return true;
        }

        public string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }


        //public void SeteaTextos(System.Windows.Forms.Control Control)
        //{
        //    foreach (System.Windows.Forms.Control oControl in Control.Controls)
        //    {
        //        if (oControl.HasChildren)
        //            SeteaTextos(oControl);

        //        if (oControl.Name.Substring(0, 1) == "_")
        //        {
        //            oControl.Text = ObtenerCadena(oControl.Name.Substring(4, oControl.Name.Length - 4));
        //        }
        //    }
        //}

        public static bool verificarConfiguracion()
        {
            bool retorno = true;
            if (Parametros.RutaAlarmas == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Ruta mal configurada: Alarmas\n" +
                                    "Verifique la siguiente línea en c:\\NeoPicking\\" +
                                    " NeoPicking.config\n" +
                                    "<add key=\"RutaAlarmas\" value=\"valorRuta\" />", 
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());

                retorno = false;
            }

            if (Parametros.RutaBackup == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Ruta mal configurada: Backup\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" + 
                                 " NeoPicking.config\n" +
                                 "<add key=\"RutaBackup\" value=\"valorRuta\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());
                retorno = false;
            }


            if (Parametros.RutaConfig == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Ruta mal configurada: Config\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" + 
                                 " NeoPicking.config\n" +
                                 "<add key=\"RutaConfig\" value=\"valorRuta\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());

                retorno = false;
            }

            if (Parametros.RutaLog == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Ruta mal configurada: Log\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" +
                                 " NeoPicking.config\n" +
                                 "<add key=\"RutaLog\" value=\"valorRuta\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());
                retorno = false;
            }

            if (Parametros.RutaEntrada == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Ruta mal configurada: Entrada\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" +
                                 " NeoPicking.config\n" +
                                 "<add key=\"RutaEntrada\" value=\"valorRuta\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());
                retorno = false;
            }


            if (Parametros.RutaSalida == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Ruta mal configurada: Salida\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" +
                                 " NeoPicking.config\n" +
                                 "<add key=\"RutaSalida\" value=\"valorRuta\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());
                retorno = false;
            }


            if (Parametros.RutaPlantillas == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Ruta mal configurada: Plantillas\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" +
                                 " NeoPicking.config\n" +
                                 "<add key=\"RutaPlantillas\" value=\"valorRuta\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());
                retorno = false;
            }

            if (Parametros.RutaReposicion == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Ruta mal configurada: Reposición\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" +
                                 " NeoPicking.config\n" +
                                 "<add key=\"RutaReposicion\" value=\"valorRuta\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());

                retorno = false;
            }


            //if (Parametros.EnvioPorDefecto == null)
            //{
            //    Comun.MessageBox msjBox = new Comun.MessageBox();
            //    msjBox.MostrarMessageBox("Parámetro faltante: EnvioPorDefecto\n" +
            //                     "Verifique la siguiente línea en c:\\NeoPicking\\" +
            //                     " NeoPicking.config\n" +
            //                     "<add key=\"EnvioPorDefecto\" value=\"valorParámetro\" />",
            //                        Comun.Enums.
            //        EMessageBoxTitulo.Información.ToString());
            //    retorno = false;
            //}



            //if (Parametros.RecepcionPorDefecto == null)
            //{
            //    Comun.MessageBox msjBox = new Comun.MessageBox();
            //    msjBox.MostrarMessageBox("Parámetro faltante: EnvioPorDefecto\n" +
            //                     "Verifique la siguiente línea en c:\\NeoPicking\\" +
            //                     " NeoPicking.config\n" +
            //                     "<add key=\"RecepcionPorDefecto\" value=\"valorParámetro\" />",
            //                        Comun.Enums.
            //        EMessageBoxTitulo.Información.ToString()); 
            //    retorno = false;
            //}



            //if (Parametros.BaudiosPorDefecto == null)
            //{
            //    Comun.MessageBox msjBox = new Comun.MessageBox();
            //    msjBox.MostrarMessageBox("Parámetro faltante: EnvioPorDefecto\n" +
            //                     "Verifique la siguiente línea en c:\\NeoPicking\\" +
            //                     " NeoPicking.config\n" +
            //                     "<add key=\"BaudiosPorDefecto\" value=\"valorParámetro\" />",
            //                        Comun.Enums.
            //        EMessageBoxTitulo.Información.ToString());
            //    retorno = false;
            //}

            //if (Parametros.TiempoEntreEnviosPorDefecto == null)
            //{
            //    Comun.MessageBox msjBox = new Comun.MessageBox();
            //    msjBox.MostrarMessageBox("Parámetro faltante: EnvioPorDefecto\n" +
            //                     "Verifique la siguiente línea en c:\\NeoPicking\\" +
            //                     " NeoPicking.config\n" +
            //                     "<add key=\"TiempoEntreEnviosPorDefecto\" value=" + 
            //                     " \"valorParámetro\" />",
            //                        Comun.Enums.
            //        EMessageBoxTitulo.Información.ToString());
            //    retorno = false;
            //}

            if (Parametros.LenguajePorDefecto == null)
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Parámetro faltante: EnvioPorDefecto\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" +
                                 " NeoPicking.config\n" +
                                 "<add key=\"LenguajePorDefecto\" value=" +
                                 " \"valorParámetro\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());
                retorno = false;
            }

            if (Parametros.OrigenDeDatos != "TEXTO" && Parametros.OrigenDeDatos != "DB")
            {
                Comun.MessageBox msjBox = new Comun.MessageBox();
                msjBox.MostrarMessageBox("Parámetro faltante: OrigenDeDatos\n" +
                                 "Verifique la siguiente línea en c:\\NeoPicking\\" + 
                                 " NeoPicking.config\n" +
                                 "<add key=\"OrigenDeDatos\" value=\"valorParámetro\" />",
                                    Comun.Enums.
                    EMessageBoxTitulo.Información.ToString());
                retorno = false;
            }

            int puesto = 0;

            //if (!int.TryParse(Parametros.Puesto, out puesto) || puesto < 1 || puesto > 99)
            //{
            //    Comun.MessageBox msjBox = new Comun.MessageBox();
            //    msjBox.MostrarMessageBox("Parámetro faltante o incorrecto : Puesto\n" +
            //                     "Verifique la siguiente línea en c:\\NeoPicking\\ " + 
            //                     " NeoPicking.config\n" +
            //                     "<add key=\"OrigenDeDatos\" value=\"Puesto\" />",
            //                        Comun.Enums.
            //        EMessageBoxTitulo.Información.ToString());
            //    retorno = false;
            //}

            return retorno;



        }

    }
}
