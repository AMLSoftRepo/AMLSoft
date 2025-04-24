using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Model;
using log4net;
using System.Threading;
using System.Collections;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Linq.Expressions;
using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;

namespace Dao
{
    /// <summary>
    /// Clase generica que implementa las interfaces generales de acceso a 
    /// datos que permite los métodos generales de: CRUD (Create, Read, Update
    /// y Delete)
    /// </summary>
    /// <typeparam name="T">Parámetro que define el objeto generico sobre el 
    /// cual opera la clase en tiempo de ejecución.</typeparam>
    public abstract class GenericDao<T> : IGenericDao<T>, IDisposable where T : BaseEntity
    {

        /// <summary>
        /// Propiedad para administrar el mecanismo de logeo 
        /// de informacion y fallas
        /// </summary>
        public static readonly ILog log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Propiedad que representa el objeto principal de acceso a datos
        /// </summary>
        protected SQLBDEntities _SQLBDEntities = new SQLBDEntities();


        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos.
        /// </summary>
        public GenericDao()
        {
        }


        /// <summary>
        /// Metodo para validar si la conexion a la base de datos es correcta
        /// </summary>
        /// <returns>True: si la conexion a la base de datos es correcta, false: 
        /// en caso contrario.</returns>
        public bool DatabaseIsValid()
        {
            DbConnection conn = _SQLBDEntities.Database.Connection;
            try
            {
                //Validar la conexion
                conn.Open();
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception("La conexión a la base de datos ha fallado");
            }

            try
            {
                // Validar acceso a datos, verificar si el esquema se cargo correctamente
                SEG_PARAMETRO u = (SEG_PARAMETRO)_SQLBDEntities.Set<SEG_PARAMETRO>().FirstOrDefault();
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception("El esquema EntityFramework no válido");
            }

            //Validar las otras bases de datos
            return true;
        }


        /// <summary>
        /// Metodo que nos permite crear una condicion en 
        /// consultas lambda o linq con objetos genericos
        /// </summary>
        /// <param name="Value">valor</param>
        /// <param name="paramName">Nombre del campo o propiedad de la entidad</param>
        /// <returns>Retorna una expresion compatible para metodos genericos</returns>
        public static Expression<Func<T, bool>> WhereClause(object Value, String paramName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            Expression<Func<T, bool>> condition = Expression.Lambda<Func<T, bool>>
                (
                Expression.Equal
                    (
                        Expression.Property(parameter, paramName),
                        Expression.Constant(Value)
                    ),
                parameter
                );

            return condition;
        }


        /// <summary>
        /// Método generico que permite obtener el total de registros
        /// entidad
        /// </summary>
        /// <returns>numero de registros</returns>
        public virtual int GetCount()
        {
            int total = 0;
            T t = (T)Activator.CreateInstance(typeof(T));
            SQLBDEntities _Entities = new SQLBDEntities();

            try
            {
                total = _Entities.Set<T>().Count();
            }
            catch (Exception e)
            {
                log.Error("Error obteniendo total de objetos: " + t.GetType() + " - " + e.StackTrace);
                throw new Exception("Error obteniendo total de objetos: " + t.ToString());
            }

            return total;
        }


        /// <summary>
        /// Método generico que permite obtener todos los elementos de una 
        /// entidad
        /// </summary>
        /// <returns>un listado de objetos del tipo especificado</returns>
        public virtual IList<T> GetAll(bool relaciones = false)
        {
            List<T> list = new List<T>();
            T t = (T)Activator.CreateInstance(typeof(T));
            SQLBDEntities _Entities = new SQLBDEntities();

            try
            {
                _Entities.Configuration.ProxyCreationEnabled = relaciones;
                list = _Entities.Set<T>().AsNoTracking().ToList();
            }
            catch (Exception e)
            {
                log.Error("Error obteniendo listado de objetos: " + t.GetType() + " - " + e.StackTrace);
                throw new Exception("Error obteniendo listado de objetos: " + t.ToString());
            }

            return list;
        }


        /// <summary>
        /// Método generico que permite obtener todos los elementos de una 
        /// entidad paginados ademas de retornar el total de registros existentes
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <returns>Lista de datos con paginación</returns>
        public virtual IList<T> GetDatosGrid(out int total, int? page, int? limit, string sortBy, string direction, bool relaciones = false)
        {
            T t = (T)Activator.CreateInstance(typeof(T));

            try
            {
                SQLBDEntities _Entities = new SQLBDEntities();
                List<T> list = new List<T>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                _Entities.Configuration.ProxyCreationEnabled = relaciones;

                list = _Entities.Set<T>()
                    .AsNoTracking()
                    .OrdenarGrid(sortBy, direction)
                    .Skip(start)
                    .Take(limit.Value)
                    .ToList();

                total = _Entities.Set<T>().Count();

                return list;
            }
            catch (Exception e)
            {
                log.Error("Error obteniendo listado de objetos: " + t.GetType() + " - " + e.StackTrace);
                throw new Exception("Error obteniendo listado de objetos: " + t.ToString());
            }
        }

        /// <summary>
        /// Método generico que permite obtener el detelle de una entidad
        /// filtrados  por el identificador del padre, paginados 
        /// ademas de retornar el total de registros existentes
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="nombreId">Nombre del campo identificador del padre</param>
        /// <param name="idPadre">Identificador del padre</param>
        /// <returns>Lista de datos con paginación</returns>
        public virtual IList<T> GetDatosDetalle(out int total, int? page, int? limit, string sortBy, string direction, string nombreId, object idPadre, bool relaciones = false)
        {
            T t = (T)Activator.CreateInstance(typeof(T));

            try
            {
                SQLBDEntities _Entities = new SQLBDEntities();
                Expression<Func<T, bool>> condition = WhereClause(idPadre, nombreId);
                List<T> list = new List<T>();
                int start = (page.Value - 1) * limit.Value;
                sortBy = sortBy == null ? "ID" : sortBy;
                direction = direction == null ? "asc" : direction;
                total = 0;

                _Entities.Configuration.ProxyCreationEnabled = relaciones;

                list = _Entities.Set<T>()
                    .AsNoTracking()
                    .Where(condition)
                    .OrdenarGrid(sortBy, direction)
                    .Skip(start)
                    .Take(limit.Value)
                    .ToList();

                total = _Entities.Set<T>().Where(condition).Count();

                return list;
            }
            catch (Exception e)
            {
                log.Error("Error obteniendo listado de objetos: " + t.GetType() + " - " + e.StackTrace);
                throw new Exception("Error obteniendo listado de objetos: " + t.ToString());
            }
        }


        /// <summary>
        /// Método generico que permite obtener el detelle de una entidad
        /// filtrados  por el identificador del padre
        /// </summary>
        /// <param name="nombreId">Nombre del campo identificador del padre</param>
        /// <param name="idPadre">Identificador del padre</param>
        /// <returns></returns>
        public virtual IList<T> GetDatosDetalle(string nombreId, object idPadre, bool relaciones = false)
        {
            T t = (T)Activator.CreateInstance(typeof(T));

            try
            {
                SQLBDEntities _Entities = new SQLBDEntities();
                Expression<Func<T, bool>> condition = WhereClause(idPadre, nombreId);
                List<T> list = new List<T>();


                _Entities.Configuration.ProxyCreationEnabled = relaciones;

                list = _Entities.Set<T>()
                    .AsNoTracking()
                    .Where(condition)
                    .ToList();

                return list;
            }
            catch (Exception e)
            {
                log.Error("Error obteniendo listado de objetos: " + t.GetType() + " - " + e.StackTrace);
                throw new Exception("Error obteniendo listado de objetos: " + t.ToString());
            }
        }


        /// <summary>
        /// Método genérico que permite obtener una instancia de la entidad
        /// especificada a traves de su respectivo Id
        /// </summary>
        /// <param name="_id">Identificador unico de la instancia que se 
        /// necesita obtener</param>
        /// <returns></returns>
        public virtual T GetById(object _id, bool relaciones = false)
        {
            T t = (T)Activator.CreateInstance(typeof(T));
            SQLBDEntities _Entities = new SQLBDEntities();

            try
            {
                _Entities.Configuration.ProxyCreationEnabled = relaciones;
                t = (T)_Entities.Set<T>().Find(_id);
            }
            catch (Exception e)
            {
                log.Error("Error obteniendo objeto: " + t.GetType() + _id + " - " + e.StackTrace);
                throw new Exception("Error obteniendo objeto: " + t.ToString());
            }

            return t;
        }


        /// <summary>
        /// Método genérico que permite agrear una nueva instancia a la entidad
        /// especificada
        /// </summary>
        /// <param name="entity">Nueva instancia a ser persistida en la base de 
        /// datos</param>
        public virtual void Save(T entity)
        {
            try
            {
                bool nuevoRegistro = EsRegistroNuevo(entity);
                _SQLBDEntities.Set<T>().Add(entity);

                if (nuevoRegistro)
                {
                    //Guardar cambios
                    _SQLBDEntities.SaveChanges();

                    //Generar el registro de cambios
                    SaveTrack(entity,
                            _SQLBDEntities.Entry(entity).CurrentValues, null, "Agregar");
                }
                else
                {
                    //Update
                    //Establecer el estado a Modificado
                    _SQLBDEntities.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                    //Generar el registro de cambios
                    SaveTrack(entity,
                            _SQLBDEntities.Entry(entity).CurrentValues,
                            _SQLBDEntities.Entry(entity).GetDatabaseValues(),
                            "Actualizar");
                }


                //Persistir cambios a la base de datos
                _SQLBDEntities.SaveChanges();

            }
            catch (Exception e)
            {
                log.Error("Error guardando objeto: " + entity.ToString() + e);

                string mensaje = "Error guardando objeto: " + entity.ToString();

                mensaje = e.ToString().Contains("IX_UNIQUE_VALOR_") ? "No puede guardar valores que ya existen." : mensaje;
                mensaje = e.ToString().Contains("IX_UNIQUE_NIVEL_") ? "No puede guardar niveles que ya existen." : mensaje;
                mensaje = e.ToString().Contains("IX_UNIQUE_DESC_") ? "El registro ya existe, favor validar." : mensaje;
                mensaje = e.ToString().Contains("IX_UNIQUE_CLIENTE") ? "El cliente ya existe." : mensaje;
                mensaje = e.ToString().Contains("IX_UNIQUE_SECUENCIA_") ? "La transacción ya tiene datos adicionales" : mensaje;

                throw new Exception(mensaje);
            }

        }


        /// <summary>
        /// Método genérico que permite agrear una nueva instancia a la entidad
        /// especificada. En este no se incluye la auditoria
        /// </summary>
        /// <param name="entity">Nueva instancia a ser persistida en la base de datos</param>
        public virtual void SaveNoTrack(T entity)
        {
            try
            {
                bool nuevoRegistro = EsRegistroNuevo(entity);
                _SQLBDEntities.Set<T>().Add(entity);

                if (nuevoRegistro)
                {
                    //Guardar cambios
                    _SQLBDEntities.SaveChanges();
                }
                else
                {
                    //Update
                    //Establecer el estado a Modificado
                    _SQLBDEntities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                }

                //Persistir cambios a la base de datos
                _SQLBDEntities.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error("Error guardando objeto: " + entity.ToString() + e);
                throw new Exception("Error guardando objeto: " + entity.ToString());
            }
        }


        /// <summary>
        /// Permite cargar de manera eficiente de grandes cantidades de datos
        /// una tabla de SQL Server con datos de otra fuentes
        /// </summary>
        /// <param name="list">Lista de datos</param>
        /// <param name="TabelName">Nombre de la tabla de destino</param>
        public virtual void InsertData(List<T> list, string TabelName)
        {
            try
            {
                string conn = _SQLBDEntities.Database.Connection.ConnectionString;
                DataTable dt = new DataTable(TabelName);

                dt = ConvertToDataTable(list);

                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    bulkcopy.BulkCopyTimeout = 60 /*segundos*/ * 30/*minutos*/;
                    bulkcopy.DestinationTableName = TabelName;
                    bulkcopy.WriteToServer(dt);
                };
            }
            catch (Exception e)
            {
                log.Error("Error guardando objeto: " + TabelName + e);
                throw new Exception("Error guardando objeto: " + TabelName);
            }
        }


        /// <summary>
        /// Meto que permite copiar los nombres de las propiedades de la lista,
        /// Ademas de esto permite copiar los datos a un objeto "DataTable"
        /// </summary>
        /// <typeparam name="T">Tipo de patametro</typeparam>
        /// <param name="data">Lista de datos</param>
        /// <returns>Objeto DataTable</returns>
        private static DataTable ConvertToDataTable(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.Name != "PrimaryKey" && !prop.PropertyType.IsAbstract && !prop.PropertyType.IsClass)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.Name != "PrimaryKey" && !prop.PropertyType.IsAbstract && !prop.PropertyType.IsClass)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }


        /// <summary>
        /// Metodo generico que permite cambiar el estado de una entidad 
        /// </summary>
        /// <param name="_id">Identificador unico de la instancia que sera 
        /// removida de la base de datos.</param>
        //public virtual void Inactivo(object _id)
        //{
        //    T entity = null;
        //    try
        //    {
        //        entity = GetById(_id);
        //        entity.GetType().GetProperty("INACTIVO").SetValue(entity, true, null);

        //        //Update
        //        //Establecer el estado a Modificado
        //        _SQLBDEntities.Entry(entity).State = System.Data.Entity.EntityState.Modified;

        //        //Generar el registro de cambios
        //        SaveTrack(entity,
        //                  _SQLBDEntities.Entry(entity).CurrentValues,
        //                  _SQLBDEntities.Entry(entity).GetDatabaseValues(),
        //                  "Eliminar");

        //        _SQLBDEntities.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        log.Error("Error removiendo objeto: "
        //            + entity.GetType() + " : " + _id + " - " + e.StackTrace);
        //        throw new Exception("Error removiendo objeto: " + entity.ToString());
        //    }
        //}


        /// <summary>
        /// Metodo generico que permite eliminar una instancia de la entidad 
        /// especificada.
        /// </summary>
        /// <param name="_id">Identificador unico de la instancia que sera 
        /// removida de la base de datos.</param>
        public virtual void Remove(object _id)
        {
            T entity = null;
            try
            {
                //Eliminar el objeto
                entity = GetById(_id);
                _SQLBDEntities.Entry(entity).State = System.Data.Entity.EntityState.Deleted;

                //Generar el registro de cambios
                SaveTrack(entity,
                        _SQLBDEntities.Entry(entity).OriginalValues, null, "Eliminar");

                _SQLBDEntities.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error("Error removiendo objeto: "
                    + entity.GetType() + " : " + _id + " - " + e.StackTrace);
                throw new Exception("No es posible eliminar el registro, posiblemente esté relacionado con otros registros. Favor validar.");
            }
        }


        /// <summary>
        /// Método genérico que permite eliminar una instancia de la entidad 
        /// especificada, validando si el usuario y la opcion permite el 
        /// proceso de eliminacion. En este no se incluye la auditoria
        /// </summary>
        /// <param name="_id"></param>
        public virtual void RemoveNoTrack(object _id)
        {
            T entity = null;
            try
            {
                //Eliminar el objeto
                entity = GetById(_id);
                _SQLBDEntities.Entry(entity).State = System.Data.Entity.EntityState.Deleted;

                _SQLBDEntities.SaveChanges();
            }
            catch (Exception e)
            {
                log.Error("Error removiendo objeto: "
                    + entity.GetType() + " : " + _id + " - " + e.StackTrace);
                throw new Exception("Error removiendo objeto." + entity.ToString());
            }
        }


        /// <summary>
        /// Permite reseteaar el ID de una tabla
        /// </summary>
        /// <param name="nombreTabla"></param>
        public void ResetID(string nombreTabla)
        {
            try
            {
                _SQLBDEntities.Database.ExecuteSqlCommand("DELETE FROM " + nombreTabla.ToUpper());
                _SQLBDEntities.Database.ExecuteSqlCommand("DBCC CHECKIDENT('" + nombreTabla.ToUpper() + "', RESEED, 0)");
            }
            catch (Exception e)
            {
                log.Error("Error reseteando el ID: " + e);
                throw new Exception("Error reseteando el ID de la tabla: " + nombreTabla.ToUpper());
            }
        }


        /// <summary>
        /// Permite ejecutar consultas SQL
        /// </summary>
        /// <param name="sql">consulta sql</param>
        public void EjecutarQuerySQL(string sql)
        {
            try
            {
                _SQLBDEntities.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                log.Error("Error al tratar de ejecutar consulta sql: " + e);
                throw new Exception("Error al tratar de ejecutar consulta sql: " + sql.ToUpper());
            }
        }


        /// <summary>
        /// Metodo para determinar si una entidad es nueva en la base de datos
        /// o ya existe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>verdadero, si el registro es completamente nuevo, falso 
        /// en caso contrario</returns>
        public bool EsRegistroNuevo(T entity)
        {

            List<object> keys = (List<object>)entity.GetType()
                .GetProperty("PrimaryKey").GetValue(entity, null);
            bool esNuevo = true;

            //Si la entidad extiende a Entity utiliza pk long
            if (entity is Entity<int> || entity is Entity<long>)
                foreach (object k in keys)
                    return ((k is long || k is int) && Convert.ToUInt64(k) == 0);

            return esNuevo;

        }


        /// <summary>
        /// Permite validar los permisos del un suario por el rol
        /// </summary>
        /// <param name="codigo">código del permiso</param>
        public void ValidarPermiso(string codigo)
        {
            //String usuarioIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            String usuarioIdentity = "Global16";
            List<int> idPermisos = new List<int>();
            List<SEG_PERMISO> listPermisos = new List<SEG_PERMISO>();
            SQLBDEntities _Entities = new SQLBDEntities();

            try
            {
                if (usuarioIdentity.Contains("\\"))
                    usuarioIdentity = usuarioIdentity.Split(new string[] { "\\" }, 2, StringSplitOptions.None)[1];

                idPermisos = _Entities.SEG_ROL_USUARIO.AsNoTracking()
                                           .Where(x => x.USUARIO == usuarioIdentity)
                                           .FirstOrDefault().SEG_ROL.SEG_ROL_PERMISO
                                           .Select(x => x.ID_PERMISO)
                                           .ToList();

                listPermisos = _Entities.SEG_PERMISO.AsNoTracking().Where(x => idPermisos.Contains(x.ID) && x.CODIGO == codigo).ToList();

                if (!listPermisos.Any())
                {
                    var permiso = _Entities.SEG_PERMISO.Where(x => x.CODIGO == codigo).ToList();
                    if (permiso.Any())
                        throw new Exception("No tiene permiso para: " + permiso.FirstOrDefault().DESCRIPCION);
                    else
                        throw new Exception("No tiene permiso para: " + codigo);
                }

            }
            catch (Exception e)
            {
                log.Error("Error validando permisos por ROL: " + e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Permite validar los permisos para actualizar o agregar 
        /// registros esto por rol de usuario
        /// </summary>
        /// <param name="id">Identificador para</param>
        public void ValidarSave(long id)
        {
            //String usuarioIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            String usuarioIdentity = "Global16";
            List<int> idPermisos = new List<int>();
            List<SEG_PERMISO> listPermisos = new List<SEG_PERMISO>();
            SQLBDEntities _Entities = new SQLBDEntities();
            string codigo = null;

            try
            {
                if (id == 0)
                    codigo = SEG_PERMISO.AGREGAR;
                else
                    codigo = SEG_PERMISO.EDITAR;

                if (usuarioIdentity.Contains("\\"))
                    usuarioIdentity = usuarioIdentity.Split(new string[] { "\\" }, 2, StringSplitOptions.None)[1];

                idPermisos = _Entities.SEG_ROL_USUARIO.AsNoTracking()
                                           .Where(x => x.USUARIO == usuarioIdentity)
                                           .FirstOrDefault().SEG_ROL.SEG_ROL_PERMISO
                                           .Select(x => x.ID_PERMISO)
                                           .ToList();

                listPermisos = _Entities.SEG_PERMISO.Where(x => idPermisos.Contains(x.ID) && x.CODIGO == codigo).ToList();

                if (!listPermisos.Any())
                {
                    var permiso = _Entities.SEG_PERMISO.Where(x => x.CODIGO == codigo).ToList();
                    if (permiso.Any())
                        throw new Exception("No tiene permiso para: " + permiso.FirstOrDefault().DESCRIPCION);
                    else
                        throw new Exception("No tiene permiso para: " + codigo);
                }

            }
            catch (Exception e)
            {
                log.Error("Error validando permisos para el metodo SAVE: " + e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Registra todos los cambios realizados a una entidad 
        /// los cambios seran calculados a partir
        /// de los atributos que contenga cada entidad.
        /// </summary>
        /// <param name="entity">Instancia generica de para la que se requiere 
        /// el control de cambios</param>
        /// <param name="current">Listado de propiedades con los valores actuales</param>
        /// <param name="old">Listado de propiedades con los valores anteriores al cambio</param>
        /// <param name="accion">Define que accion se realiza sobre un objeto determinado</param>
        public void SaveTrack(T entity, DbPropertyValues current, DbPropertyValues old, string accion)
        {
            List<SEG_LOG_AUDITORIA> cambios = new List<SEG_LOG_AUDITORIA>();
            List<object> keys = new List<object>();
            String tableName = "";
            String identityName = "";

            try
            {
                //string[] props = (string[])entity.GetType().GetProperty("Atributos").GetValue(entity, null);
                var props = entity.GetType().GetProperties().ToList();
                keys = (List<object>)entity.GetType().GetProperty("PrimaryKey").GetValue(entity, null);
                tableName = ObjectContext.GetObjectType(entity.GetType()).Name;
                identityName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                foreach (var p in props)
                {
                    string OldValue = "";
                    string NewValue = "";

                    //Validar que la propiedad exista
                    try
                    {
                        try
                        {
                            if (old == null)
                            {
                                NewValue = current[p.Name] == null ? "" : current[p.Name].ToString();
                            }
                            else
                            {
                                OldValue = old[p.Name] == null ? "" : old[p.Name].ToString();
                                NewValue = current[p.Name] == null ? "" : current[p.Name].ToString();
                            }
                        }
                        catch (Exception)
                        {
                        }


                        if (!OldValue.Equals(NewValue))
                        {
                            SEG_LOG_AUDITORIA auditoria = new SEG_LOG_AUDITORIA();

                            if (identityName.Contains("\\"))
                                identityName = identityName.Split(new string[] { "\\" }, 2, StringSplitOptions.None)[1];

                            auditoria.CAMPO = p.Name;
                            auditoria.VALOR_ANTERIOR = OldValue.ToString();
                            auditoria.VALOR_ACTUAL = NewValue.ToString();
                            auditoria.FECHA_HORA = DateTime.Now;
                            auditoria.TABLA = tableName;
                            auditoria.USUARIO = identityName;
                            auditoria.ACCION = accion;
                            auditoria.ID_ORIGEN = long.Parse(string.Join("|", keys));


                            _SQLBDEntities.Set<SEG_LOG_AUDITORIA>().Add(auditoria);
                            _SQLBDEntities.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Info("La propiedad: " + p.Name + " no existe para la entidad: " + tableName, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error guardando trak :: " + tableName + " :: " + keys.ToString() + " :: " + ex);
            }
        }


        /// <summary>
        /// Metodo que permite liberar los recursos asigandos
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Metodo que permite liberar los recursos asigandos
        /// </summary>
        /// <param name="disposing">Indica si se libera el recurso</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_SQLBDEntities != null)
                {
                    _SQLBDEntities.Dispose();
                    _SQLBDEntities = null;
                }
            }
        }


    }

}
