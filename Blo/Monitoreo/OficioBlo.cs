using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Monitoreo;
using Novacode;
using System.Drawing;
using System.Globalization;

namespace Blo.Monitoreo
{
    public class OficioBlo : GenericBlo<MON_OFICIO>, IOficioBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IOficioDao _oficioDao;
        private IDiaFestivoDao _diaFestivoDao;
        private IContactoInstitucionDao _contactoInstitucionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public OficioBlo(IOficioDao oficioDao, IDiaFestivoDao diaFestivoDao, IContactoInstitucionDao contactoInstitucionDao)
            : base(oficioDao)
        {
            _oficioDao = oficioDao;
            _diaFestivoDao = diaFestivoDao;
            _contactoInstitucionDao = contactoInstitucionDao;
        }

        /// <summary>
        /// Estructura de datos usada para la creacion del documento de oficios
        /// permite que el metodo "ImprimirOficio" retorne estos objetos
        /// </summary>
        public struct DatosImprimir
        {
            public DocX DocX;
            public string NumeroOficio;
        }


        /// <summary>
        ///  Metodo que calcula los dias habiles en un rango de fechas 
        /// </summary>
        /// <param name="oficio">Entidad de MON_OFICIO</param>
        public void GetDiasHabiles(ref MON_OFICIO oficio)
        {
            List<DateTime> listDiasFestivos = new List<DateTime>();
            oficio.DIAS_HABILES = 0;

            if (oficio.FECHA_RECIBIDO_UIF != null && oficio.FECHA_RECIBIDO != null)
            {
                DateTime fechaInicial = oficio.FECHA_RECIBIDO;
                DateTime fechaFinal = oficio.FECHA_RECIBIDO_UIF.Value;
                int diferenciaFechas = Math.Abs((fechaFinal - fechaInicial).Days);

                //Obteniendo lista de días festivos
                foreach (var item in _diaFestivoDao.GetAll())
                {
                    int diferenciaFechas_festivo = Math.Abs((item.FECHA_FIN - item.FECHA_INICIO).Days);
                    int contador = 0;

                    do
                    {
                        listDiasFestivos.Add(item.FECHA_INICIO.AddDays(contador)); contador++;
                    } while (contador <= diferenciaFechas_festivo);
                }

                //Recorrer el rengo de fechas (FECHA_RECIBIDO Y FECHA_RECIBIDO_UIF)
                //Ademas de excluir los sabados y domingo excluye los dias festivos
                for (int i = 1; i <= diferenciaFechas; i++)
                    if (fechaInicial.AddDays(i).DayOfWeek != DayOfWeek.Saturday &&
                        fechaInicial.AddDays(i).DayOfWeek != DayOfWeek.Sunday)
                        if (!listDiasFestivos.Where(x => x == fechaInicial.AddDays(i)).Any())
                            oficio.DIAS_HABILES += 1;

            }

        }


        /// <summary>
        /// Obtiene la fecha maxima que la oficialia tiene para entregar la documentación
        /// </summary>
        /// <param name="oficio">Entidad de MON_OFICIO</param>
        public void GetFechaMaxima(ref MON_OFICIO oficio)
        {
            List<DateTime> listDiasFestivos = new List<DateTime>();
            int diasHabiles = 1;
            int totalDias = 1;

            if (oficio.FECHA_RECIBIDO != null)
            {
                //Obteniendo lista de días festivos
                foreach (var item in _diaFestivoDao.GetAll())
                {
                    int diferenciaFechas_festivo = Math.Abs((item.FECHA_FIN - item.FECHA_INICIO).Days);
                    int contador = 0;

                    do
                    {
                        listDiasFestivos.Add(item.FECHA_INICIO.AddDays(contador)); contador++;
                    } while (contador <= diferenciaFechas_festivo);
                }

                DateTime fechaInicial = oficio.FECHA_RECIBIDO;

                //Recorrer el rengo de fechas (FECHA_RECIBIDO Y FECHA_RECIBIDO_UIF)
                //Ademas de excluir los sabados y domingo excluye los dias festivos
                while (diasHabiles <= oficio.DIAS_MAX)
                {
                    if (fechaInicial.AddDays(totalDias).DayOfWeek != DayOfWeek.Saturday &&
                        fechaInicial.AddDays(totalDias).DayOfWeek != DayOfWeek.Sunday)
                        if (!listDiasFestivos.Where(x => x == fechaInicial.AddDays(totalDias)).Any())
                            diasHabiles += 1;

                    totalDias += 1;
                }

                oficio.FECHA_MAXIMA_UIF = fechaInicial.AddDays(totalDias - 1);
            }
        }


        /// <summary>
        /// Metodo que obtiene el estado del OFICIO
        /// por este se conce si fue entregado a tiempo o no
        /// </summary>
        /// <param name="oficio">Entidad de MON_OFICIO</param>
        public void GetCumplimiento(ref MON_OFICIO oficio)
        {
            oficio.CUMPLIMIENTO = oficio.DIAS_HABILES - oficio.DIAS_MAX;
        }


        /// <summary>
        /// Metodo que permite imprimir oficio deacuerdo a plantilla de Microsoft-Word
        /// </summary>
        /// <param name="id">Identificador único de  MON_OFICIO</param>
        public DatosImprimir ImprimirOficio(long id, string path)
        {
            try
            {
                DocX doc = DocX.Load(path);
                Dictionary<string, string> keysValues = new Dictionary<string, string>();
                MON_OFICIO oficio = new MON_OFICIO();
                List<MON_OFICIO_PERSONA> listPersonas = new List<MON_OFICIO_PERSONA>();
                MON_CONTACTO_INSTITUCION destinatario = new MON_CONTACTO_INSTITUCION();
                MON_CONTACTO_INSTITUCION remitente = new MON_CONTACTO_INSTITUCION();


                if (id != 0)
                {
                    oficio = _oficioDao.GetById(id, true);
                    destinatario = _contactoInstitucionDao.GetById(oficio.ID_CONTACTO_INSTITUCION, true);
                    remitente = _contactoInstitucionDao.GetById(oficio.ID_CONTACTO_INSTITUCION_OFICIAL, true);

                    //string fechaEnLetras = CultureInfo.InvariantCulture.TextInfo.
                    //                                   ToTitleCase(DateTime.Now.ToString("dddd dd De MMMM Del yyyy"));


                    keysValues["[DIA]"] = oficio.FECHA_RESPUESTA_UIF.Day.ToString();
                    keysValues["[MES]"] = oficio.FECHA_RESPUESTA_UIF.ToString("MMMM");
                    keysValues["[ANO]"] = oficio.FECHA_RESPUESTA_UIF.Year.ToString();
                    keysValues["[TITULO]"] = destinatario.LIS_CAT_TITULOS.DESCRIPCION;
                    keysValues["[CONTACTO_OFICIO]"] = destinatario.NOMBRE;
                    keysValues["[CARGO_OFICIO]"] = destinatario.MON_CARGO_INSTITUCION.NOMBRE;
                    keysValues["[UNIDAD]"] = destinatario.MON_CAT_UNIDAD.DESCRIPCION;
                    keysValues["[INSTITUCION_OFICIO]"] = destinatario.MON_CARGO_INSTITUCION.MON_CAT_INSTITUCION.NOMBRE;
                    keysValues["[NUMOFICIO]"] = oficio.NUMERO_OFICIO;
                    keysValues["[NUMREFERENCIA]"] = oficio.REFERENCIA;
                    keysValues["[FECHAOFICIO]"] = oficio.FECHA_OFICIO.ToString("dd/MM/yyyy");
                    keysValues["[FECHARECEPCION]"] = oficio.FECHA_RECIBIDO.ToString("dd/MM/yyyy");
                    keysValues["[FECHAINVESTIGACIONINI]"] = oficio.FECHA_INVESTIGACION_INI == null ?
                                                            "desde su apertura" :
                                                            "del " + oficio.FECHA_INVESTIGACION_INI.Value.ToString("dd/MM/yyyy");
                    keysValues["[FECHAINVESTIGACIONFIN]"] = oficio.FECHA_INVESTIGACION_FIN == null ?
                                                            "hasta la fecha actual" :
                                                            "al " + oficio.FECHA_INVESTIGACION_FIN.Value.ToString("dd/MM/yyyy");


                    listPersonas = oficio.MON_OFICIO_PERSONA.ToList();

                    keysValues["[COMENTARIO]"] = string.IsNullOrEmpty(oficio.COMENTARIO) ? "" : oficio.COMENTARIO; ;
                    keysValues["[CONTACTO]"] = remitente.NOMBRE.ToUpper();
                    keysValues["[CARGO]"] = remitente.MON_CARGO_INSTITUCION.NOMBRE.ToUpper();
                    keysValues["[INSTITUCION]"] = remitente.MON_CARGO_INSTITUCION.MON_CAT_INSTITUCION.NOMBRE.ToUpper();

                    //Agregar valores al documento
                    foreach (var x in keysValues) doc.ReplaceText(x.Key, x.Value);

                    //Crear tabla de personas
                    Table t = doc.Tables[0];
                    Table invoice_table = CrearTabla(t, ref doc, listPersonas);
                    t.Remove();

                    //Retornar datos
                    DatosImprimir datosImprimir = new DatosImprimir();
                    datosImprimir.DocX = doc;
                    datosImprimir.NumeroOficio = oficio.NUMERO_OFICIO;

                    return datosImprimir;
                }
                else
                    throw new System.ArgumentException("Error al tratar de obtener los datos con id = 0");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al imprimir oficio", ex);
            }
        }


        /// <summary>
        /// Metodo que permite crear una nueva tabla dentro de un documento Word
        /// </summary>
        /// <param name="tabla">Tabla que esta en la plantilla</param>
        /// <param name="document">Documento Word pre-cargado</param>
        /// <param name="listPersonas">lista de personas que se agregaran a la nueva tabla</param>
        /// <returns></returns>
        private Table CrearTabla(Table tabla, ref DocX document, List<MON_OFICIO_PERSONA> listPersonas)
        {
            try
            {
                int row = 1;
                string textTablaTitulo0 = null;
                string textTablaTitulo1 = null;
                string textTablaTitulo2 = null;
                string textTablaTitulo3 = null;

                //Reemplaza una tabla con la nueva deficion de esta
                //es aqui el inicio de la nueva tabla
                Table newTabla = tabla.InsertTableAfterSelf(listPersonas.Count + 1, 4);
                newTabla.Design = TableDesign.TableGrid;

                #region ===================== Formato de tabla
                //Definicio  de formato para la nueva tabla
                Formatting tabla_fomato = new Formatting();
                tabla_fomato.Bold = true;
                tabla_fomato.FontFamily = new FontFamily("Calibri");
                tabla_fomato.Size = 10;
                #endregion

                #region ===================== Titulos de tabla
                //Deficion de titulos para la tabla
                newTabla.Rows[0].Cells[0].FillColor = tabla.Rows[0].Cells[0].FillColor;
                newTabla.Rows[0].Cells[1].FillColor = tabla.Rows[0].Cells[1].FillColor;
                newTabla.Rows[0].Cells[2].FillColor = tabla.Rows[0].Cells[2].FillColor;
                newTabla.Rows[0].Cells[3].FillColor = tabla.Rows[0].Cells[3].FillColor;

                foreach (var item in tabla.Rows[0].Cells[0].Paragraphs.ToList())
                    textTablaTitulo0 += item.Text;
                newTabla.Rows[0].Cells[0].Paragraphs.First().InsertText(textTablaTitulo0.ToUpper(), false, tabla_fomato);
                newTabla.Rows[0].Cells[0].Paragraphs.First().Alignment = Alignment.center;

                foreach (var item in tabla.Rows[0].Cells[1].Paragraphs.ToList())
                    textTablaTitulo1 += item.Text;
                newTabla.Rows[0].Cells[1].Paragraphs.First().InsertText(textTablaTitulo1.ToUpper(), false, tabla_fomato);
                newTabla.Rows[0].Cells[1].Paragraphs.First().Alignment = Alignment.center;

                foreach (var item in tabla.Rows[0].Cells[2].Paragraphs.ToList())
                    textTablaTitulo2 += item.Text;
                newTabla.Rows[0].Cells[2].Paragraphs.First().InsertText(textTablaTitulo2.ToUpper(), false, tabla_fomato);
                newTabla.Rows[0].Cells[2].Paragraphs.First().Alignment = Alignment.center;

                foreach (var item in tabla.Rows[0].Cells[3].Paragraphs.ToList())
                    textTablaTitulo3 += item.Text;
                newTabla.Rows[0].Cells[3].Paragraphs.First().InsertText(textTablaTitulo3.ToUpper(), false, tabla_fomato);
                newTabla.Rows[0].Cells[3].Paragraphs.First().Alignment = Alignment.center;

                #endregion

                foreach (var item in listPersonas)
                {
                    SQLBDEntities _SQLBDEntities = new SQLBDEntities();
                    string tipoDocumento = null ;
                    string dui = null;
                    string nit = null;
                    string otroDocumento = null;


                    if (item.TIPO_DOCUMENTO != null)
                        tipoDocumento = _SQLBDEntities.VIEW_TIPO_DOCUMENTO.Where(x=>x.CODIGO_TIPO_IDENTIFICACION==item.TIPO_DOCUMENTO).First().DESCRIPCION;

                    dui = item.DUI == null? "" : "DUI " + item.DUI; 
                    nit = item.NIT == null? "" : "\nNIT " + item.NIT;
                    otroDocumento = item.NUMERO_DOCUMENTO== null || item.TIPO_DOCUMENTO==null? "" : "\n" + tipoDocumento + " " + item.NUMERO_DOCUMENTO;

                    tabla_fomato.Bold = false;
                    Paragraph cell_0 = newTabla.Rows[row].Cells[0].Paragraphs.First();
                    cell_0.InsertText(row.ToString(), false, tabla_fomato);

                    tabla_fomato.Bold = true;
                    Paragraph cell_1 = newTabla.Rows[row].Cells[1].Paragraphs.First();
                    cell_1.InsertText(item.NOMBRE + (string.IsNullOrEmpty(item.GENERALES)?"":", ") + item.GENERALES, false, tabla_fomato);

                    tabla_fomato.Bold = false;
                    
                    Paragraph cell_2 = newTabla.Rows[row].Cells[2].Paragraphs.First();
                    cell_2.InsertText(dui + nit + otroDocumento,false, tabla_fomato);

                    Paragraph cell_3 = newTabla.Rows[row].Cells[3].Paragraphs.First();
                    cell_3.InsertText(item.RESULTADO, false, tabla_fomato);
                    cell_3.Alignment = Alignment.both;


                    if ((item.NUMERO_DOCUMENTO == null || item.TIPO_DOCUMENTO == null) && item.DUI == null && item.NIT == null)
                    {
                        Cell cell = newTabla.Rows[row].Cells[2];
                        cell.SetBorder(TableCellBorderType.TopLeftToBottomRight, new Border { Size = BorderSize.four, Color = Color.Black });
                        cell.SetBorder(TableCellBorderType.TopRightToBottomLeft, new Border { Size = BorderSize.four, Color = Color.Black });
                    }

                    row++;
                }

                //Copiar de la plantilla el tamaño de las columnas
                newTabla.SetColumnWidth(0, tabla.GetColumnWidth(0));
                newTabla.SetColumnWidth(1, tabla.GetColumnWidth(1));
                newTabla.SetColumnWidth(2, tabla.GetColumnWidth(2));
                newTabla.SetColumnWidth(3, tabla.GetColumnWidth(3));


                return tabla;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al crear tabla de personas");
            }
        }


        /// <summary>
        /// Metodo que permite obtener una lista de oficios pendientes de recibir una respuesta de la UIF.
        /// Ademas realiza la paginación de los datos, permite buscar numero de oficio o referencia y busca
        /// datos de las personas como lo son nombre y documentos el resultado sera una lista de oficios.
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de oficios</returns>
        public IQueryable<dynamic> GetOficios(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                return _oficioDao.GetOficios(out total, page, limit, sortBy, direction, searchString);
            }
            catch (Exception e)
            {
                log.Error("Error al obtener lista de oficios: " + e);
                throw new Exception("Error al obtener lista de oficios");
            }
        }


        /// <summary>
        /// Metodo que permite obtener una lista de oficios Procesados y con respuesta UIF.
        /// Ademas realiza la paginación de los datos, permite buscar numero de oficio o referencia y busca
        /// datos de las personas como lo son nombre y documentos el resultado sera una lista de oficios.
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de oficios Historicos</returns>
        public IQueryable<dynamic> GetHistorialOficios(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                return _oficioDao.GetHistorialOficios(out total, page, limit, sortBy, direction, searchString);
            }
            catch (Exception e)
            {
                log.Error("Error al obtener los oficios historicos: " + e);
                throw new Exception("Error al obtener los oficios historicos");
            }
        }


        /// <summary>
        /// Metodo que permite obtener el total de oficios pendientes de procesar y
        /// donde la fecha actual mas los dias aplicados en configuración sea mayor
        /// o igual a la fecha maxima UIF. Entonces sera notificada en el sistema.
        /// </summary>
        /// <returns>Int total de oficios</returns>
        public int GetTotalNotificarOficio()
        {
            try
            {
                return _oficioDao.GetTotalNotificarOficio();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Error al obtener el total de oficios a notificar");
            }
        }

        /// <summary>
        /// Metodo que obtiene una lista de oficios PENDIENTES de procesar, 
        /// con fecha de recibido UIF igual a null y filtradas por un rango de fecha
        /// en este caso seria el establecido por el plugin del calendario.
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de oficios</returns>
        public List<MON_OFICIO> GetCalendarioOficios(DateTime fechaIni, DateTime fechaFin)
        {
            List<MON_OFICIO> lista = new List<MON_OFICIO>();
            try
            {
                lista = _oficioDao.GetCalendarioOficios(fechaIni, fechaFin);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de oficios", e);
            }

            return lista;
        }


        /// <summary>
        /// Metodo que obtiene una lista de oficios en HISTORIAL, 
        /// con fecha de recibido UIF igual a null y filtradas por un rango de fecha
        /// en este caso seria el establecido por el plugin del calendario.
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de oficios</returns>
        public List<MON_OFICIO> GetHistorialCalendarioOficios(DateTime fechaIni, DateTime fechaFin)
        {
            List<MON_OFICIO> lista = new List<MON_OFICIO>();
            try
            {
                lista = _oficioDao.GetHistorialCalendarioOficios(fechaIni, fechaFin);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de oficios", e);
            }

            return lista;
        }

    }
}
