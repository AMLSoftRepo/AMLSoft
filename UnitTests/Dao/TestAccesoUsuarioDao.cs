using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dao.Seguridad;
using Model;

namespace UnitTests.Dao
{
    [TestClass]
    public class TestAccesoUsuarioDao
    {
        /// <summary>
        /// Propiedad que referencia e objeto de acceso a datos.
        /// </summary>
        private AccesoUsuarioDao _accesoUsuarioDao;

        /// <summary>
        /// Iniciar los objetos necesarios para la realización de las pruebas
        /// </summary>
        [TestInitialize]
        public void UnitTestInitialize()
        {
            _accesoUsuarioDao = new AccesoUsuarioDao();
        }

        /// <summary>
        /// Metodo para probar obtener un elemento por su ID
        /// </summary>
        [TestMethod]
        public void GetById()
        {
            var l = _accesoUsuarioDao.GetById(1);
        }

        /// <summary>
        /// Metodo para probar Insertar un elemento 
        /// </summary>
        [TestMethod]
        public void Save()
        {
            //SEG_ACCESO_USUARIO neww= new SEG_ACCESO_USUARIO();

            //neww.ID_MODULO = 1;
            //neww.ID_OPCION = 6;
            //neww.ID_ROL = 1;
            //neww.ACCESO = true;

            //_accesoUsuarioDao.Save(neww);
        }

        /// <summary>
        /// Metodo para probar la actualizacion de un elemento
        /// </summary>
        [TestMethod]
        public void Update()
        {
            //SEG_ACCESO_USUARIO update = new SEG_ACCESO_USUARIO();

            //update = _accesoUsuarioDao.GetById(14);

            //update.ACCESO = false;

            //_accesoUsuarioDao.Save(update);
        }

        /// <summary>
        /// Metodo para probar la eliminacion de un elemento
        /// </summary>
        [TestMethod]
        public void Delete()
        {
            //_accesoUsuarioDao.Remove(15);
        }

    }
}
