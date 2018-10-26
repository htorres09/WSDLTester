using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using WSDLTester.Classes;


namespace WSDLTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            string bandera = "C", ruta = @"C:\Users\hector.torresg\source\repos\WSDLTester\WSDLTester\Docs\";
            int c = 0, i = 0, n = 0, p = 0;
            int numPruebas = 0, numExitos = 0, numFallos = 0;
            StringBuilder log = new StringBuilder();
            string[] lista;
            List<string> matriculas = new List<string>();
            List<Alumno> alumnos = new List<Alumno>();

            obj.Encabezado();
            lista = obj.GetLineas(@"C:\Users\hector.torresg\source\repos\WSDLTester\WSDLTester\Docs\MatriculasPruebas.csv");

            Console.WriteLine("[" + (i++) + "]:\t... Cargando Listado de Matriculas ... ");
            matriculas = obj.GetMatriculas(lista);
            Console.WriteLine("[" + (i++) + "]:\t... Matriculas cargadas {0} ... ", matriculas.Count);
            Console.WriteLine("[" + (i++) + "]:\t... Cargando Datos de Alumnos ... ");
            alumnos = obj.GetInfoAlumnos(lista);
            Console.WriteLine("[" + (i++) + "]:\t... Datos cargados {0} ... ", alumnos.Count);

            while (bandera == "C")
            {
                Console.WriteLine("[" + (i++) + "]:\tCuantas veces desea repetir la prueba: ");
                n = obj.GetNumPruebas();
                Console.WriteLine("[" + (i++) + "]:\t... Numero de pruebas a realizar {0} ... ", n);
                Console.WriteLine("[" + (i++) + "]:\t... Se realizaran: {0} peticiones ...", (matriculas.Count * n));

                while (c < n)
                {
                    foreach (string matricula in matriculas)
                    {
                        numPruebas++;
                        Console.WriteLine("[" + (i++) + "]:\t ... Prueba matricula {0}", matricula);
                        log.AppendLine("Fecha: " + DateTime.Now);
                        log.AppendLine("        >>> Petición: [" + (p++) + "], Matricula: " + matricula + ", Estatus: ");
                        try
                        {
                            Console.WriteLine("\t ... Enviando petición al servidor ...");
                            obj.InvokeService(1, matricula);
                            log.Append("200");
                            numExitos++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\t ... Error: " + ex.Message);
                            log.Append("        " + ex.Message);
                            numFallos++;
                        }
                        log.AppendLine();
                    }
                    c++;
                }

                do
                {
                    if (bandera != "C" && bandera != "s")
                        Console.WriteLine("\t\tCaracter no valido.");
                    Console.WriteLine("[" + (i++) + "]:\tEscriba C para continuar. Escriba S para salir.");
                    bandera = Console.ReadLine().ToUpper();
                } while (bandera != "C" && bandera != "S");

                if (bandera == "C")
                    c = 0;
                else
                {
                    DateTime _fecha = (Convert.ToDateTime(DateTime.Now.ToString()));
                    log.Append("-------------------- Reporte de Pruebas: " + _fecha.ToString() + " --------------------");
                    log.AppendLine();
                    log.AppendLine("Numero de Matriculas: " + matriculas.Count);
                    log.AppendLine("Numero de peticiones SOAP: " + numPruebas);
                    log.AppendLine("Peticiones exitosas:" + numExitos);
                    log.AppendLine("Peticiones fallidas:" + numFallos);
                    log.Append("---------------------------------------------------------------------------------------");
                    File.AppendAllText(ruta + "log-" + _fecha.Year.ToString() + "-" + _fecha.Month + ".txt", log.ToString());
                    Console.WriteLine("---Finalizando---");
                    break;
                }
            }
        }

        /// <summary>
        /// Impresión de cabecera de programa.
        /// </summary>
        public void Encabezado()
        {
            Console.WriteLine("Programa Prueba de Peticiones SOAP:");
            Console.WriteLine("\t Archivo de Lista: MatriculasPruebas.csv");
            Console.WriteLine("\t URL Request: http://148.234.19.240:8080/wsatest/wsa1");
            Console.WriteLine("\t SOPA Action: urn:SIASE:RegCodice");
            Console.WriteLine("\n------------------------------------------------------------\n");
        }

        /// <summary>
        /// Obtiene el numero de pruebas a realizar y valida que se trate de un numero entero.
        /// </summary>
        /// <returns>Numero entero</returns>
        public int GetNumPruebas()
        {
            int num = 0;
            bool bandera = true;
            do
            {
                if (Int32.TryParse(Console.ReadLine(), out num))
                    bandera = false;
                else
                {
                    Console.WriteLine("\t ... Caracter no valido.");
                    Console.WriteLine("\tVuelva a teclear el numero: ");
                }
            } while (bandera);
            return num;
        }

        /// <summary>
        /// Obtiene el numero de renglones del archivo.
        /// </summary>
        /// <param name="ruta">Ruta al archivo para cargar las matriculas</param>
        /// <returns></returns>
        public string[] GetLineas(string ruta)
        {
            string[] renglones = File.ReadAllLines(ruta);
            if (renglones != null || renglones.Length != 0)
                return renglones;
            else
                return null;
        }

        /// <summary>
        /// Obtiene el listado de matriculas
        /// </summary>
        /// <param name="renglones">Arreglo de caracteres</param>
        /// <returns></returns>
        public List<string> GetMatriculas(string[] renglones)
        {
            Char delimite = ',';
            List<string> lista = new List<string>();
            try
            {
                foreach (var linea in renglones)
                {
                    string[] datos = linea.Split(delimite);
                    if (datos[0] != "Matricula")
                        lista.Add(datos[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\t\tError: ");
                Console.Write(ex.Message);
                Console.ReadKey();
            }
            return lista;
        }

        /// <summary>
        /// Obtiene la información de los alumnos en el archivo
        /// </summary>
        /// <param name="renglones">Arreglo de Caracteres</param>
        /// <returns></returns>
        public List<Alumno> GetInfoAlumnos(string[] renglones)
        {
            Char delimite = ',';
            List<Alumno> lista = new List<Alumno>();

            try
            {
                foreach (var linea in renglones)
                {
                    string[] datos = linea.Split(delimite);
                    //Alumno => Matricula, ApellidoPaterno, ApellidoMaterno, Nombre, CveDependencia, CveUnidad, NombreDependencia
                    //Alumno alumno = new Alumno(datos[0], datos[1], datos[2], datos[3], datos[4], datos[5], datos[6]);
                    if (datos[0] != "Matricula")
                        lista.Add(new Alumno(datos[0], datos[1], datos[2], datos[3], datos[4], datos[5], datos[6]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\t\tError: ");
                Console.Write(ex.Message);
                Console.ReadKey();
            }
            return lista;
        }

        /// <summary>
        /// Invoca al servicio SOAP
        /// Carga el XML SOAP Envelop
        /// </summary>
        /// <param name="tipo">Parametro de tipo de cuenta</param>
        /// <param name="matricula">Matricula para realizar consulta</param>
        public void InvokeService(int tipo, string matricula)
        {
            HttpWebRequest request = CreateSOAPWebRequest();
            XmlDocument SOAPReqBody = new XmlDocument();

            SOAPReqBody.LoadXml(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <Envelop xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <Body>
                        <es_regcodice01 xmlns=""urn:SIASE:RegCodice:RegCodice"">
                            <TipoId>" + tipo + @"</TipoId>
                            <Id>" + matricula + @"</Id>
                        </es_regcodice01>
                    </Body>
                </Envelop>");

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            using (WebResponse ServiceRes = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(ServiceRes.GetResponseStream()))
                {
                    var ServiceResult = rd.ReadToEnd();

                    Console.WriteLine(ServiceResult);
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Crear Request SOAP
        /// </summary>
        /// <returns></returns>
        public HttpWebRequest CreateSOAPWebRequest()
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"http://148.234.19.240:8080/wsatest/wsa1");
            Req.Headers.Add(@"SOAPAction:urn:SIASE:RegCodice");
            Req.ContentType = "text/xml; charset=\"utf-8\"";
            Req.Accept = "*/*";
            Req.Method = "POST";
            return Req;
        }
    }
}
