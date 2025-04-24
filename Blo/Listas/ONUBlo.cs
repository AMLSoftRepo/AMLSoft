using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Listas;
using Dao.Seguridad;
using System.Transactions;
using System.Xml;

namespace Blo.Listas
{
    public class ONUBlo : GenericBlo<LIS_ONU>, IONUBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IONUDao _onuDao;
        private IONUAliasDao _onuAliasDao;
        private IONUDireccionDao _onuDireccionDao;
        private IONUDocumentoDao _onuDocumentoDao;
        private ICargaDatosDao _cargaDatosDao;
        private ICargaDatosDao _errorCargaDatosDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ONUBlo(IONUDao onuDao, IONUAliasDao onuAliasDao, IONUDireccionDao onuDireccionDao,
            IONUDocumentoDao onuDocumentoDao, ICargaDatosDao cargaDatosDao, ICargaDatosDao errorCargaDatosDao)
            : base(onuDao)
        {
            _onuDao = onuDao;
            _onuAliasDao = onuAliasDao;
            _onuDireccionDao = onuDireccionDao;
            _onuDocumentoDao = onuDocumentoDao;
            _cargaDatosDao = cargaDatosDao;
            _errorCargaDatosDao = errorCargaDatosDao;
        }

        public void SaveListaONU(string ubicacionXML)
        {
            _onuDao.ResetID("LIS_ONU_DOCUMENTO");
            _onuDao.ResetID("LIS_ONU_DIRECCION");
            _onuDao.ResetID("LIS_ONU_ALIAS");
            _onuDao.ResetID("LIS_ONU");
            //ONU
            int onuDataId = 0;
            string onuTipo = "";
            int onuVersion = 0;
            string onuPrimerNombre = "";
            string onuSegundoNombre = "";
            string onuTercerNombre = "";
            string onuCuartoNombre = "";
            string onuTipoLista = "";
            string onuReferencia = "";
            DateTime onuListadoEl = new DateTime();
            string onuComentarios = "";
            string onuNacionalidad = "";
            //ALIAS
            string aliasCalidad = "";
            string aliasNombre = "";
            string aliasCiudad = "";
            string aliasPais = "";
            string aliasNota = "";
            //DIRECCION
            string dirCalle = "";
            string dirCiudad = "";
            string dirEstadoProvincia = "";
            string dirPais = "";
            string dirNota = "";
            //DOCUMENTO
            string docTipo = "";
            string docNumero = "";
            string docPais = "";
            string docNota = "";

            try
            {
                int totalRegistros = 0;
                SEG_CARGAR_DATOS editCargaDatos = new SEG_CARGAR_DATOS();
                XmlDocument doc = new XmlDocument();

                //PROCESO 1 DE 4
                editCargaDatos = _cargaDatosDao.GetById(SEG_CARGAR_DATOS.ID_ONU);
                editCargaDatos.TOTAL_REGISTROS = totalRegistros;
                editCargaDatos.ESTADO = SEG_CARGAR_DATOS.ESTADO_ENPROCESO;
                editCargaDatos.MENSAJE = "Obteniendo archivo para la carga";
                _cargaDatosDao.Save(editCargaDatos);

                doc.Load(ubicacionXML);

                //PROCESO 2 DE 4
                editCargaDatos.MENSAJE = "Calculando total registros";
                _cargaDatosDao.Save(editCargaDatos);


                XmlNodeList onuIndividuals = doc.GetElementsByTagName("INDIVIDUALS");
                XmlNodeList onuEntities = doc.GetElementsByTagName("ENTITIES");


                //Obtener el numero de registros  para esta carga de datos
                foreach (XmlNode individual in onuIndividuals)
                    foreach (XmlNode nodo in individual.ChildNodes)
                        totalRegistros++;


                foreach (XmlNode entidades in onuEntities)
                    foreach (XmlNode nodo in entidades.ChildNodes)
                        totalRegistros++;


                //PROCESO 3 DE 4
                editCargaDatos.TOTAL_REGISTROS = totalRegistros;
                editCargaDatos.MENSAJE = "Cargando datos en el sistema";
                _cargaDatosDao.Save(editCargaDatos);


                //INDIVIDUO
                foreach (XmlNode individual in onuIndividuals)
                {
                    onuTipo = "Individuo";
                    //using (var scope = new TransactionScope(TransactionScopeOption.Required))
                    //{
                    XmlNodeList nodoLista = individual.ChildNodes;
                    //guardar ONU
                    foreach (XmlNode nodo in nodoLista)
                    {
                        XmlNodeList nodoLista2 = nodo.ChildNodes;
                        //guardar ONU
                        foreach (XmlNode nodo2 in nodoLista2)
                        {
                            //SWITCH PARA TABLA ONU
                            switch (nodo2.Name)
                            {
                                case "DATAID":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuDataId = int.Parse(nodo2.InnerText);
                                    }
                                    break;

                                case "VERSIONNUM":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuVersion = int.Parse(nodo2.InnerText);
                                    }
                                    break;

                                case "FIRST_NAME":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuPrimerNombre = nodo2.InnerText;
                                    }
                                    break;

                                case "SECOND_NAME":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuSegundoNombre = nodo2.InnerText;
                                    }
                                    break;

                                case "THIRD_NAME":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuTercerNombre = nodo2.InnerText;
                                    }
                                    break;

                                case "FOURTH_NAME":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuCuartoNombre = nodo2.InnerText;
                                    }
                                    break;

                                case "UN_LIST_TYPE":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuTipoLista = nodo2.InnerText;
                                    }
                                    break;

                                case "REFERENCE_NUMBER":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuReferencia = nodo2.InnerText;
                                    }
                                    break;

                                case "LISTED_ON":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuListadoEl = DateTime.Parse(nodo2.InnerText);
                                    }
                                    break;

                                case "COMMENTS1":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuComentarios = nodo2.InnerText;
                                    }
                                    break;

                                case "NATIONALITY":
                                    XmlNodeList nacionalidadLista = nodo2.ChildNodes;
                                    foreach (XmlNode nacionalidad in nacionalidadLista)
                                    {
                                        if (nacionalidad.InnerText != null)
                                        {
                                            onuNacionalidad += nacionalidad.InnerText + " ";
                                        }
                                    }
                                    break;
                            }
                        }//NODOS INDIVIDUO
                        //GUARDAR TABLA ONU
                        LIS_ONU onu = new LIS_ONU();
                        onu.DATA_ID = onuDataId;
                        onu.TIPO = onuTipo;
                        onu.VERSION_NUM = onuVersion;
                        onu.FIRST_NAME = onuPrimerNombre;
                        onu.SECOND_NAME = onuSegundoNombre;
                        onu.THIRD_NAME = onuTercerNombre;
                        onu.FOURTH_NAME = onuCuartoNombre;
                        onu.UN_LIST_TYPE = onuTipoLista;
                        onu.REFERENCE_NUMBER = onuReferencia;
                        onu.LISTED_ON = onuListadoEl;
                        onu.COMMENTS = onuComentarios;
                        onu.NATIONALITY = onuNacionalidad;
                        _onuDao.SaveNoTrack(onu);
                        //SDN LIMPIAR VARIABLES
                        onuDataId = 0;
                        onuVersion = 0;
                        onuPrimerNombre = "";
                        onuSegundoNombre = "";
                        onuTercerNombre = "";
                        onuCuartoNombre = "";
                        onuTipoLista = "";
                        onuReferencia = "";
                        onuListadoEl = new DateTime();
                        onuComentarios = "";
                        onuNacionalidad = "";

                        //guardar HIJOS
                        foreach (XmlNode nodo2 in nodoLista2)
                        {
                            switch (nodo2.Name)
                            {
                                //ONU_ALIAS
                                case "INDIVIDUAL_ALIAS":
                                    XmlNodeList aliasLista = nodo2.ChildNodes;
                                    foreach (XmlNode nodoAlias in aliasLista)
                                    {
                                        switch (nodoAlias.Name)
                                        {
                                            case "QUALITY":
                                                if (nodoAlias.InnerText != null)
                                                {
                                                    aliasCalidad = nodoAlias.InnerText;
                                                }
                                                break;

                                            case "ALIAS_NAME":
                                                if (nodoAlias.InnerText != null)
                                                {
                                                    aliasNombre = nodoAlias.InnerText;
                                                }
                                                break;

                                            case "CITY_OF_BIRTH":
                                                if (nodoAlias.InnerText != null)
                                                {
                                                    aliasCiudad = nodoAlias.InnerText;
                                                }
                                                break;

                                            case "COUNTRY_OF_BIRTH":
                                                if (nodoAlias.InnerText != null)
                                                {
                                                    aliasPais = nodoAlias.InnerText;
                                                }
                                                break;

                                            case "NOTE":
                                                if (nodoAlias.InnerText != null)
                                                {
                                                    aliasNota = nodoAlias.InnerText;
                                                }
                                                break;
                                        }
                                    }
                                    //GUARDAR
                                    if (aliasNombre != "")
                                    {
                                        LIS_ONU_ALIAS onuAlias = new LIS_ONU_ALIAS();
                                        onuAlias.ID_LIS_ONU = onu.ID;
                                        onuAlias.QUALITY = aliasCalidad;
                                        onuAlias.ALIAS_NAME = aliasNombre;
                                        onuAlias.CITY_OF_BIRTH = aliasCiudad;
                                        onuAlias.COUNTRY_OF_BIRTH = aliasPais;
                                        onuAlias.NOTE = aliasNota;
                                        _onuAliasDao.SaveNoTrack(onuAlias);
                                    }
                                    //LIMPIAR VARIABLES
                                    aliasCalidad = "";
                                    aliasNombre = "";
                                    aliasCiudad = "";
                                    aliasPais = "";
                                    aliasNota = "";
                                    break; //INDIVIDUAL_ALIAS

                                //ONU_DIRECCION
                                case "INDIVIDUAL_ADDRESS":
                                    XmlNodeList addressLista = nodo2.ChildNodes;
                                    foreach (XmlNode nodoAddress in addressLista)
                                    {
                                        switch (nodoAddress.Name)
                                        {
                                            case "STREET":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirCalle = nodoAddress.InnerText;
                                                }
                                                break;

                                            case "CITY":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirCiudad = nodoAddress.InnerText;
                                                }
                                                break;

                                            case "STATE_PROVINCE":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirEstadoProvincia = nodoAddress.InnerText;
                                                }
                                                break;

                                            case "COUNTRY":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirPais = nodoAddress.InnerText;
                                                }
                                                break;

                                            case "NOTE":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirNota = nodoAddress.InnerText;
                                                }
                                                break;
                                        }
                                    }
                                    //GUARDAR
                                    if ((dirPais != "") || (dirNota != ""))
                                    {
                                        LIS_ONU_DIRECCION onuDireccion = new LIS_ONU_DIRECCION();
                                        onuDireccion.ID_LIS_ONU = onu.ID;
                                        onuDireccion.STREET = dirCalle;
                                        onuDireccion.CITY = dirCiudad;
                                        onuDireccion.STATE_PROVINCE = dirEstadoProvincia;
                                        onuDireccion.COUNTRY = dirPais;
                                        onuDireccion.NOTE = dirNota;
                                        _onuDireccionDao.SaveNoTrack(onuDireccion);
                                    }
                                    //LIMPIAR VARIABLES
                                    dirCalle = "";
                                    dirCiudad = "";
                                    dirEstadoProvincia = "";
                                    dirPais = "";
                                    dirNota = "";
                                    break; //INDIVIDUAL_ADDRESS

                                //ONU_DOCUMENTO
                                case "INDIVIDUAL_DOCUMENT":
                                    XmlNodeList documentLista = nodo2.ChildNodes;
                                    foreach (XmlNode nodoDocument in documentLista)
                                    {
                                        switch (nodoDocument.Name)
                                        {
                                            case "TYPE_OF_DOCUMENT":
                                                if (nodoDocument.InnerText != null)
                                                {
                                                    docTipo = nodoDocument.InnerText;
                                                }
                                                break;

                                            case "NUMBER":
                                                if (nodoDocument.InnerText != null)
                                                {
                                                    docNumero += nodoDocument.InnerText;
                                                }
                                                break;

                                            case "ISSUING_COUNTRY":
                                                if (nodoDocument.InnerText != null)
                                                {
                                                    docPais = nodoDocument.InnerText;
                                                }
                                                break;

                                            case "NOTE":
                                                if (nodoDocument.InnerText != null)
                                                {
                                                    docNota = nodoDocument.InnerText;
                                                }
                                                break;
                                        }
                                    }
                                    //GUARDAR
                                    if ((docTipo != "") || (docNumero != ""))
                                    {
                                        LIS_ONU_DOCUMENTO onuDocumento = new LIS_ONU_DOCUMENTO();
                                        onuDocumento.ID_LIS_ONU = onu.ID;
                                        onuDocumento.TYPE = docTipo;
                                        onuDocumento.NUMBER = docNumero;
                                        onuDocumento.COUNTRY = docPais;
                                        onuDocumento.NOTE = docNota;
                                        _onuDocumentoDao.SaveNoTrack(onuDocumento);
                                    }
                                    //LIMPIAR VARIABLES
                                    docTipo = "";
                                    docNumero = "";
                                    docPais = "";
                                    docNota = "";
                                    break; //INDIVIDUAL_DOCUMENT
                            }// SWITCH
                        }
                    }
                    //scope.Complete();
                    //} //TransactionScope
                }//INDIVIDUO


                //ENTIDAD
                foreach (XmlNode entidad in onuEntities)
                {
                    onuTipo = "Entidad";
                    //using (var scope2 = new TransactionScope(TransactionScopeOption.Required))
                    //{
                    XmlNodeList nodoLista = entidad.ChildNodes;
                    //guardar ONU
                    foreach (XmlNode nodo in nodoLista)
                    {
                        XmlNodeList nodoLista2 = nodo.ChildNodes;
                        //guardar ONU
                        foreach (XmlNode nodo2 in nodoLista2)
                        {
                            //SWITCH PARA TABLA ONU
                            switch (nodo2.Name)
                            {
                                case "DATAID":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuDataId = int.Parse(nodo2.InnerText);
                                    }
                                    break;

                                case "VERSIONNUM":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuVersion = int.Parse(nodo2.InnerText);
                                    }
                                    break;

                                case "FIRST_NAME":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuPrimerNombre = nodo2.InnerText;
                                    }
                                    break;

                                case "SECOND_NAME":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuSegundoNombre = nodo2.InnerText;
                                    }
                                    break;

                                case "THIRD_NAME":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuTercerNombre = nodo2.InnerText;
                                    }
                                    break;

                                case "FOURTH_NAME":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuCuartoNombre = nodo2.InnerText;
                                    }
                                    break;

                                case "UN_LIST_TYPE":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuTipoLista = nodo2.InnerText;
                                    }
                                    break;

                                case "REFERENCE_NUMBER":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuReferencia = nodo2.InnerText;
                                    }
                                    break;

                                case "LISTED_ON":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuListadoEl = DateTime.Parse(nodo2.InnerText);
                                    }
                                    break;

                                case "COMMENTS1":
                                    if (nodo2.InnerText != null)
                                    {
                                        onuComentarios = nodo2.InnerText;
                                    }
                                    break;
                            }
                        }//NODOS INDIVIDUO
                        //GUARDAR TABLA ONU
                        LIS_ONU onu = new LIS_ONU();
                        onu.DATA_ID = onuDataId;
                        onu.TIPO = onuTipo;
                        onu.VERSION_NUM = onuVersion;
                        onu.FIRST_NAME = onuPrimerNombre;
                        onu.SECOND_NAME = onuSegundoNombre;
                        onu.THIRD_NAME = onuTercerNombre;
                        onu.FOURTH_NAME = onuCuartoNombre;
                        onu.UN_LIST_TYPE = onuTipoLista;
                        onu.REFERENCE_NUMBER = onuReferencia;
                        onu.LISTED_ON = onuListadoEl;
                        onu.COMMENTS = onuComentarios;
                        onu.NATIONALITY = onuNacionalidad;
                        _onuDao.SaveNoTrack(onu);
                        //SDN LIMPIAR VARIABLES
                        onuDataId = 0;
                        onuVersion = 0;
                        onuPrimerNombre = "";
                        onuSegundoNombre = "";
                        onuTercerNombre = "";
                        onuCuartoNombre = "";
                        onuTipoLista = "";
                        onuReferencia = "";
                        onuListadoEl = new DateTime();
                        onuComentarios = "";
                        onuNacionalidad = "";

                        //guardar HIJOS
                        foreach (XmlNode nodo2 in nodoLista2)
                        {
                            switch (nodo2.Name)
                            {
                                //ONU_ALIAS
                                case "ENTITY_ALIAS":
                                    XmlNodeList aliasLista = nodo2.ChildNodes;
                                    foreach (XmlNode nodoAlias in aliasLista)
                                    {
                                        switch (nodoAlias.Name)
                                        {
                                            case "QUALITY":
                                                if (nodoAlias.InnerText != null)
                                                {
                                                    aliasCalidad = nodoAlias.InnerText;
                                                }
                                                break;

                                            case "ALIAS_NAME":
                                                if (nodoAlias.InnerText != null)
                                                {
                                                    aliasNombre = nodoAlias.InnerText;
                                                }
                                                break;
                                        }
                                    }
                                    //GUARDAR
                                    if (aliasNombre != "")
                                    {
                                        LIS_ONU_ALIAS onuAlias = new LIS_ONU_ALIAS();
                                        onuAlias.ID_LIS_ONU = onu.ID;
                                        onuAlias.QUALITY = aliasCalidad;
                                        onuAlias.ALIAS_NAME = aliasNombre;
                                        onuAlias.CITY_OF_BIRTH = aliasCiudad;
                                        onuAlias.COUNTRY_OF_BIRTH = aliasPais;
                                        onuAlias.NOTE = aliasNota;
                                        _onuAliasDao.SaveNoTrack(onuAlias);
                                    }
                                    //LIMPIAR VARIABLES
                                    aliasCalidad = "";
                                    aliasNombre = "";
                                    aliasCiudad = "";
                                    aliasPais = "";
                                    aliasNota = "";
                                    break; //ENTITY_ALIAS

                                //ONU_DIRECCION
                                case "ENTITY_ADDRESS":
                                    XmlNodeList addressLista = nodo2.ChildNodes;
                                    foreach (XmlNode nodoAddress in addressLista)
                                    {
                                        switch (nodoAddress.Name)
                                        {
                                            case "STREET":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirCalle = nodoAddress.InnerText;
                                                }
                                                break;

                                            case "CITY":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirCiudad = nodoAddress.InnerText;
                                                }
                                                break;

                                            case "STATE_PROVINCE":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirEstadoProvincia = nodoAddress.InnerText;
                                                }
                                                break;

                                            case "COUNTRY":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirPais = nodoAddress.InnerText;
                                                }
                                                break;

                                            case "NOTE":
                                                if (nodoAddress.InnerText != null)
                                                {
                                                    dirNota = nodoAddress.InnerText;
                                                }
                                                break;
                                        }
                                    }
                                    //GUARDAR
                                    if ((dirPais != "") || (dirNota != ""))
                                    {
                                        LIS_ONU_DIRECCION onuDireccion = new LIS_ONU_DIRECCION();
                                        onuDireccion.ID_LIS_ONU = onu.ID;
                                        onuDireccion.STREET = dirCalle;
                                        onuDireccion.CITY = dirCiudad;
                                        onuDireccion.STATE_PROVINCE = dirEstadoProvincia;
                                        onuDireccion.COUNTRY = dirPais;
                                        onuDireccion.NOTE = dirNota;
                                        _onuDireccionDao.SaveNoTrack(onuDireccion);
                                    }
                                    //LIMPIAR VARIABLES
                                    dirCalle = "";
                                    dirCiudad = "";
                                    dirEstadoProvincia = "";
                                    dirPais = "";
                                    dirNota = "";
                                    break; //ENTITY_ADDRESS                                    
                            }// SWITCH
                        }
                    }
                    //scope2.Complete();
                    //} //TransactionScope
                }//ENTIDAD


                //PROCESO 4 DE 4
                editCargaDatos.ESTADO = SEG_CARGAR_DATOS.ESTADO_LISTO;
                editCargaDatos.MENSAJE = "Proceso finalizado";
                _cargaDatosDao.Save(editCargaDatos);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                //PROCESO 4 DE 4
                SEG_CARGAR_DATOS errorCargaDatos = new SEG_CARGAR_DATOS();
                errorCargaDatos = _cargaDatosDao.GetById(SEG_CARGAR_DATOS.ID_ONU);
                errorCargaDatos.ESTADO = SEG_CARGAR_DATOS.ESTADO_ERROR;
                errorCargaDatos.MENSAJE = ex.Message;
                _errorCargaDatosDao.Save(errorCargaDatos);
            }
        }
    }
}
