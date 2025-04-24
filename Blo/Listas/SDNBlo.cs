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
    public class SDNBlo : GenericBlo<LIS_SDN>, ISDNBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ISDNDao _sdnDao;
        private ISDNAkaDao _sdnAkaDao;
        private ISDNDireccionDao _sdnDireccionDao;
        private ISDNDocumentoDao _sdnDocumentoDao;
        private ISDNNacionalidadDao _sdnNacionalidadDao;
        private ICargaDatosDao _cargaDatosDao;
        private ICargaDatosDao _errorCargaDatosDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public SDNBlo(ISDNDao sdnDao, ISDNAkaDao sdnAkaDao, ISDNDireccionDao sdnDireccionDao, ISDNDocumentoDao sdnDocumentoDao,
            ISDNNacionalidadDao sdnNacionalidadDao, ICargaDatosDao cargaDatosDao, ICargaDatosDao errorCargaDatosDao)
            : base(sdnDao)
        {
            _sdnDao = sdnDao;
            _sdnAkaDao = sdnAkaDao;
            _sdnDireccionDao = sdnDireccionDao;
            _sdnDocumentoDao = sdnDocumentoDao;
            _sdnNacionalidadDao = sdnNacionalidadDao;
            _cargaDatosDao = cargaDatosDao;
            _errorCargaDatosDao = errorCargaDatosDao;
        }

        public void SaveListaSDN(string ubicacionXML)
        {
            _sdnDao.ResetID("LIS_SDN_NACIONALIDAD");
            _sdnDao.ResetID("LIS_SDN_DOCUMENTO");
            _sdnDao.ResetID("LIS_SDN_DIRECCION");
            _sdnDao.ResetID("LIS_SDN_AKA");
            _sdnDao.ResetID("LIS_SDN");
            //SDN
            string sdnUid = "";
            string sdnTipo = "";
            string sdnNombres = "";
            string sdnApellidos = "";
            string sdnProgramas = "";
            //DOCUMENTO
            string docTipo = "";
            string docNumero = "";
            string docPais = "";
            //AKA
            string akaTipo = "";
            string akaCategoria = "";
            string akaNombres = "";
            string akaApellidos = "";
            //DIRECCION
            string dirDireccion = "";
            string dirEstadoProvincia = "";
            string dirCiudad = "";
            string dirPais = "";
            //NACIONALIDAD
            string nacPais = "";
            bool nacPrincipal = false;

            try
            {
                int totalRegistros = 0;
                SEG_CARGAR_DATOS editCargaDatos = new SEG_CARGAR_DATOS();
                XmlDocument doc = new XmlDocument();
               

                //PROCESO 1 DE 4
                editCargaDatos = _cargaDatosDao.GetById(SEG_CARGAR_DATOS.ID_SDN);
                editCargaDatos.TOTAL_REGISTROS = totalRegistros;
                editCargaDatos.ESTADO = SEG_CARGAR_DATOS.ESTADO_ENPROCESO;
                editCargaDatos.MENSAJE = "Obteniendo archivo para la carga";
                _cargaDatosDao.Save(editCargaDatos);

                doc.Load(ubicacionXML);

                XmlNodeList sdnEntrys = doc.GetElementsByTagName("sdnEntry");

                //PROCESO 2 DE 4
                editCargaDatos.MENSAJE = "Calculando total registros";
                _cargaDatosDao.Save(editCargaDatos);

                foreach (XmlNode nodo in sdnEntrys)
                    totalRegistros++;

                //PROCESO 3 DE 4
                editCargaDatos.TOTAL_REGISTROS = totalRegistros;
                editCargaDatos.MENSAJE = "Cargando datos en el sistema";
                _cargaDatosDao.Save(editCargaDatos);


                foreach (XmlNode nodo in sdnEntrys)
                {
                    using (var scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        XmlNodeList nodoLista = nodo.ChildNodes;
                        //guardar SDN
                        foreach (XmlNode nodo2 in nodoLista)
                        {
                            //SWITCH PARA TABLA SDN
                            switch (nodo2.Name)
                            {
                                case "uid":
                                    if (nodo2.InnerText != null)
                                    {
                                        sdnUid = nodo2.InnerText;
                                    }
                                    break;

                                case "firstName":
                                    if (nodo2.InnerText != null)
                                    {
                                        sdnNombres = nodo2.InnerText;
                                    }
                                    break;

                                case "lastName":
                                    if (nodo2.InnerText != null)
                                    {
                                        sdnApellidos = nodo2.InnerText;
                                    }
                                    break;

                                case "sdnType":
                                    if (nodo2.InnerText != null)
                                    {
                                        sdnTipo = nodo2.InnerText;
                                    }
                                    break;

                                case "programList":
                                    XmlNodeList programaLista = nodo2.ChildNodes;
                                    foreach (XmlNode programa in programaLista)
                                    {
                                        if (programa.InnerText != null)
                                        {
                                            sdnProgramas += programa.InnerText + " ";
                                        }
                                    }
                                    break;
                            }
                        }

                        //GUARDAR TABLA SDN
                        LIS_SDN sdn = new LIS_SDN();
                        sdn.UID = Int32.Parse(sdnUid);
                        sdn.TIPO = sdnTipo;
                        sdn.NOMBRES = sdnNombres;
                        sdn.APELLIDOS = sdnApellidos;
                        sdn.PROGRAMAS = sdnProgramas;
                        _sdnDao.SaveNoTrack(sdn);
                        //SDN LIMPIAR VARIABLES
                        sdnUid = "";
                        sdnTipo = "";
                        sdnNombres = "";
                        sdnApellidos = "";
                        sdnProgramas = "";

                        //guardar HIJOS
                        foreach (XmlNode nodo2 in nodoLista)
                        {
                            switch (nodo2.Name)
                            {
                                //SDN_DOCUMENTO
                                case "idList":
                                    XmlNodeList idLista = nodo2.ChildNodes;
                                    foreach (XmlNode id in idLista)
                                    {
                                        XmlNodeList idLista2 = id.ChildNodes;
                                        foreach (XmlNode id2 in idLista2)
                                        {
                                            switch (id2.Name)
                                            {
                                                case "idType":
                                                    if (id2.InnerText != null)
                                                    {
                                                        docTipo = id2.InnerText;
                                                    }
                                                    break;

                                                case "idNumber":
                                                    if (id2.InnerText != null)
                                                    {
                                                        docNumero = id2.InnerText;
                                                    }
                                                    break;

                                                case "idCountry":
                                                    if (id2.InnerText != null)
                                                    {
                                                        docPais = id2.InnerText;
                                                    }
                                                    break;
                                            }
                                        }
                                        //GUARDAR
                                        LIS_SDN_DOCUMENTO sdnDocumento = new LIS_SDN_DOCUMENTO();
                                        sdnDocumento.ID_LIS_SDN = sdn.ID;
                                        sdnDocumento.TIPO = docTipo;
                                        sdnDocumento.NUMERO = docNumero;
                                        sdnDocumento.PAIS = docPais;
                                        _sdnDocumentoDao.SaveNoTrack(sdnDocumento);
                                        //LIMPIAR VARIABLES
                                        docTipo = "";
                                        docNumero = "";
                                        docPais = "";
                                    }
                                    break; //idList

                                //SDN_AKA
                                case "akaList":
                                    XmlNodeList akaLista = nodo2.ChildNodes;
                                    foreach (XmlNode aka in akaLista)
                                    {
                                        XmlNodeList akaLista2 = aka.ChildNodes;
                                        foreach (XmlNode aka2 in akaLista2)
                                        {
                                            switch (aka2.Name)
                                            {
                                                case "type":
                                                    if (aka2.InnerText != null)
                                                    {
                                                        akaTipo = aka2.InnerText;
                                                    }
                                                    break;

                                                case "category":
                                                    if (aka2.InnerText != null)
                                                    {
                                                        akaCategoria = aka2.InnerText;
                                                    }
                                                    break;

                                                case "firstName":
                                                    if (aka2.InnerText != null)
                                                    {
                                                        akaNombres = aka2.InnerText;
                                                    }
                                                    break;

                                                case "lastName":
                                                    if (aka2.InnerText != null)
                                                    {
                                                        akaApellidos = aka2.InnerText;
                                                    }
                                                    break;
                                            }
                                        }
                                        //GUARDAR
                                        LIS_SDN_AKA sdnAka = new LIS_SDN_AKA();
                                        sdnAka.ID_LIS_SDN = sdn.ID;
                                        sdnAka.TIPO = akaTipo;
                                        sdnAka.CATEGORIA = akaCategoria;
                                        sdnAka.NOMBRES = akaNombres;
                                        sdnAka.APELLIDOS = akaApellidos;
                                        _sdnAkaDao.SaveNoTrack(sdnAka);
                                        //LIMPIAR VARIABLES
                                        akaTipo = "";
                                        akaCategoria = "";
                                        akaNombres = "";
                                        akaApellidos = "";
                                    }
                                    break; //akaList

                                //SDN_DIRECCION
                                case "addressList":
                                    XmlNodeList addressLista = nodo2.ChildNodes;
                                    foreach (XmlNode address in addressLista)
                                    {
                                        XmlNodeList addressLista2 = address.ChildNodes;
                                        foreach (XmlNode address2 in addressLista2)
                                        {
                                            switch (address2.Name)
                                            {
                                                case "address1":
                                                    if (address2.InnerText != null)
                                                    {
                                                        dirDireccion = address2.InnerText;
                                                    }
                                                    break;

                                                case "address2":
                                                    if (address2.InnerText != null)
                                                    {
                                                        dirDireccion += address2.InnerText;
                                                    }
                                                    break;

                                                case "address3":
                                                    if (address2.InnerText != null)
                                                    {
                                                        dirDireccion += address2.InnerText;
                                                    }
                                                    break;

                                                case "stateOrProvince":
                                                    if (address2.InnerText != null)
                                                    {
                                                        dirEstadoProvincia = address2.InnerText;
                                                    }
                                                    break;

                                                case "city":
                                                    if (address2.InnerText != null)
                                                    {
                                                        dirCiudad = address2.InnerText;
                                                    }
                                                    break;

                                                case "country":
                                                    if (address2.InnerText != null)
                                                    {
                                                        dirPais = address2.InnerText;
                                                    }
                                                    break;
                                            }
                                        }
                                        //GUARDAR
                                        LIS_SDN_DIRECCION sdnDireccion = new LIS_SDN_DIRECCION();
                                        sdnDireccion.ID_LIS_SDN = sdn.ID;
                                        sdnDireccion.DIRECCION = dirDireccion;
                                        sdnDireccion.ESTADO_PROVINCIA = dirEstadoProvincia;
                                        sdnDireccion.CIUDAD = dirCiudad;
                                        sdnDireccion.PAIS = dirPais;
                                        _sdnDireccionDao.SaveNoTrack(sdnDireccion);
                                        //LIMPIAR VARIABLES
                                        dirDireccion = "";
                                        dirEstadoProvincia = "";
                                        dirCiudad = "";
                                        dirPais = "";
                                    }
                                    break; //addressList

                                //SDN_NACIONALIDAD
                                case "citizenshipList":
                                    XmlNodeList citizenshipLista = nodo2.ChildNodes;
                                    foreach (XmlNode citizenship in citizenshipLista)
                                    {
                                        XmlNodeList citizenshipLista2 = citizenship.ChildNodes;
                                        foreach (XmlNode citizenship2 in citizenshipLista2)
                                        {
                                            switch (citizenship2.Name)
                                            {
                                                case "country":
                                                    if (citizenship2.InnerText != null)
                                                    {
                                                        nacPais = citizenship2.InnerText;
                                                    }
                                                    break;

                                                case "mainEntry":
                                                    if (citizenship2.InnerText != null)
                                                    {
                                                        if (citizenship2.InnerText == "true")
                                                            nacPrincipal = true;
                                                    }
                                                    break;
                                            }
                                        }
                                        //GUARDAR
                                        LIS_SDN_NACIONALIDAD sdnNacionalidad = new LIS_SDN_NACIONALIDAD();
                                        sdnNacionalidad.ID_LIS_SDN = sdn.ID;
                                        sdnNacionalidad.PAIS = nacPais;
                                        sdnNacionalidad.PRINCIPAL = nacPrincipal;
                                        _sdnNacionalidadDao.SaveNoTrack(sdnNacionalidad);
                                        //LIMPIAR VARIABLES
                                        nacPais = "";
                                        nacPrincipal = false;
                                    }
                                    break; //citizenshipList
                            }// SWITCH
                        }
                        scope.Complete();
                    } //TransactionScope
                }

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
                errorCargaDatos = _cargaDatosDao.GetById(SEG_CARGAR_DATOS.ID_SDN);
                errorCargaDatos.ESTADO = SEG_CARGAR_DATOS.ESTADO_ERROR;
                errorCargaDatos.MENSAJE = ex.Message;
                _errorCargaDatosDao.Save(errorCargaDatos);
            }
        }
    }
}
