using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SGECA.Comun
{
    public class IOutil
    {
        public static bool verificarCarpetaExistenteYEscribible(string carpeta)
        {
            if (carpeta == null)
                return false;
            bool retorno = true;

            try
            {

                DirectoryInfo di = new DirectoryInfo(carpeta);
                if (!di.Exists)
                {
                    retorno = false;
                    throw new DirectoryNotFoundException("Excepción! La carpeta: " + carpeta + " no existe.");
                }

                try
                {
                    StreamWriter sw = new StreamWriter(carpeta + "test.pik");
                    sw.WriteLine(".");
                    sw.Close();
                    FileInfo fi = new FileInfo(carpeta + "test.pik");
                    fi.Delete();
                    Console.WriteLine("Carpeta : " + carpeta + " OK");
                }
                catch (Exception ex)
                {
                    retorno = false;
                    throw new IOException("Excepción de entrada/salida en la carpeta: " + carpeta, ex);
                }

            }
            catch (NullReferenceException ex)
            {
                retorno = false;
                throw new ArgumentNullException("Excepción! Carpeta no especificada.", ex);
            }



            return retorno;
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public static bool moverArchivoProcesadoAOtraCarpeta(FileInfo nombreArchivo, string carpetaDestino)
        {
            
            //fArchivo.CopyTo(clsUtil.RutaPedidos + @"\" + fArchivo.Name + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".bpk", true)
            try 
            {
                //se verifica que el archivo a mover exista y este disponible
                if (!IsFileLocked(nombreArchivo))
                {
                    //se verifica que la carpeta destino exista, de lo contrario la crea
                    if (!System.IO.Directory.Exists(carpetaDestino))
                    {
                        System.IO.Directory.CreateDirectory(carpetaDestino);
                    }

                    //mueve el archivo a la otra carpeta
                    string destino = carpetaDestino + @"\" + nombreArchivo.Name ;
                    System.IO.File.Move(nombreArchivo.FullName.ToString(), destino);
                    return true;
                }
                else
                {
                    //mensaje de error la carpeta destino o el archivo no existen o esatn inaccesibles
                    return false;                
                }
               
            }
            catch (Exception ex)
            {
             return false;
            }
        
        
        }

    }
}
