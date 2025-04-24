using Dao.Alertas;
using Dao.Seguridad;
using Dao.Monitoreo;
using Dao.Listas;
using Dao.Matriz;
using Dao.Reportes;
using Dao.Perfiles;
using Ninject.Modules;

namespace BLO
{
    public class NinjectBlo : NinjectModule
    {
        public override void Load()
        {
            //Alertas
            Bind<IContactoAlertaDao>().To<ContactoAlertaDao>();
            Bind<ITipoAlertaDao>().To<TipoAlertaDao>();
            Bind<INotificacionAlertaDao>().To<NotificacionAlertaDao>();
            Bind<IOperacionIrregularDao>().To<OperacionIrregularDao>();
            Bind<IAlertaDao>().To<AlertaDao>();
            Bind<IDatosAdicionalesTransaccionDao>().To<DatosAdicionalesTransaccionDao>();
            Bind<IAlertaListaPersonaDao>().To<AlertaListaPersonaDao>();
            Bind<IAlertaPerfilTransaccionalDao>().To<AlertaPerfilTransaccionalDao>();
            Bind<IBloqueoDao>().To<BloqueoDao>();

            //Seguridad
            Bind<IAccesoUsuarioDao>().To<AccesoUsuarioDao>();
            Bind<IModuloDao>().To<ModuloDao>();
            Bind<IOpcionDao>().To<OpcionDao>();
            Bind<IParametroDao>().To<ParametroDao>();
            Bind<IRolDao>().To<RolDao>();
            Bind<IRolUsuarioDao>().To<RolUsuarioDao>();
            Bind<IEmpresaDao>().To<EmpresaDao>();
            Bind<IPermisoDao>().To<PermisoDao>();
            Bind<IRolPermisoDao>().To<RolPermisoDao>();
            Bind<ICargaDatosDao>().To<CargaDatosDao>();

            //Perfiles
            Bind<IFactorDao>().To<FactorDao>();
            Bind<IConfiguracionFactorDao>().To<ConfiguracionFactorDao>();
            Bind<ICalificacionFactorDao>().To<CalificacionFactorDao>();
            Bind<IHistorialPerfilDao>().To<HistorialPerfilDao>();
            Bind<ITipoCalificacionDao>().To<TipoCalificacionDao>();
            Bind<ICoincidenciaListaDao>().To<CoincidenciaListaDao>();

            //Monitoreo
            Bind<ICatFinancieraDao>().To<CatFinancieraDao>();
            Bind<ICatInstitucionDao>().To<CatInstitucionDao>();
            Bind<IContactoInstitucionDao>().To<ContactoInstitucionDao>();
            Bind<ICargoInstitucionDao>().To<CargoInstitucionDao>();
            Bind<IAgenciaBancariaDao>().To<AgenciaBancariaDao>();
            Bind<IOficioDao>().To<OficioDao>();
            Bind<IPersonasOficioDao>().To<PersonasOficioDao>();
            Bind<IDiaFestivoDao>().To<DiaFestivoDao>();
            Bind<IConfiguracionOficioDao>().To<ConfiguracionOficioDao>();
            Bind<IRosDao>().To<RosDao>();
            Bind<IPersonasRosDao>().To<PersonasRosDao>();
            Bind<IOperacionesRosDao>().To<OperacionesRosDao>();
            Bind<IAdjuntosRosDao>().To<AdjuntosRosDao>();
            Bind<IClientePEPDao>().To<ClientePEPDao>();
            Bind<ICatUnidadInstitucionDao>().To<CatUnidadInstitucionDao>();
            Bind<IEscalaCalificacionDao>().To<EscalaCalificacionDao>();

            //Listas
            Bind<IParametroONUSDNDao>().To<ParametroONUSDNDao>();
            Bind<ISDNDao>().To<SDNDao>();
            Bind<ISDNAkaDao>().To<SDNAkaDao>();
            Bind<ISDNDireccionDao>().To<SDNDireccionDao>();
            Bind<ISDNDocumentoDao>().To<SDNDocumentoDao>();
            Bind<ISDNNacionalidadDao>().To<SDNNacionalidadDao>();
            Bind<IONUDao>().To<ONUDao>();
            Bind<IONUAliasDao>().To<ONUAliasDao>();
            Bind<IONUDireccionDao>().To<ONUDireccionDao>();
            Bind<IONUDocumentoDao>().To<ONUDocumentoDao>();
            Bind<IPEPDao>().To<PEPDao>();
            Bind<IPEPCargoDao>().To<PEPCargoDao>();
            Bind<IPEPRelacionDao>().To<PEPRelacionDao>();
            Bind<IGeneralDao>().To<GeneralDao>();
            Bind<IGeneralPersonalizadaDao>().To<GeneralPersonalizadaDao>();
            Bind<IPersonalizadaDao>().To<PersonalizadaDao>();
            Bind<IPersonalizadaDocumentoDao>().To<PersonalizadaDocumentoDao>();
            Bind<IPersonalizadaDireccionDao>().To<PersonalizadaDireccionDao>();
            Bind<IPersonalizadaAliasDao>().To<PersonalizadaAliasDao>();
            Bind<IExentoDao>().To<ExentoDao>();
            Bind<ICatOrganoDao>().To<CatOrganoDao>();
            Bind<ICatTituloDao>().To<CatTituloDao>();
            Bind<ICatEntidadDao>().To<CatEntidadDao>();
            Bind<ICatGrupoDao>().To<CatGrupoDao>();
            Bind<IPaisGrupoDao>().To<PaisGrupoDao>();

            //Matriz
            Bind<ICatProbabilidadOcurrenciaDao>().To<CatProbabilidadOcurrenciaDao>();
            Bind<ICatTipoControlDao>().To<CatTipoControlDao>();
            Bind<ICatUnidadDao>().To<CatUnidadDao>();
            Bind<ICatAgenciaDao>().To<CatAgenciaDao>();
            Bind<ICatFactorRiesgoDao>().To<CatFactorRiesgoDao>();
            Bind<ICatTipoRiesgoDao>().To<CatTipoRiesgoDao>();
            Bind<ICatAutomatizacionDao>().To<CatAutomatizacionDao>();
            Bind<ICatDisenoDao>().To<CatDisenoDao>();
            Bind<ICatDocumentacionDao>().To<CatDocumentacionDao>();
            Bind<ICatFrecuenciaDao>().To<CatFrecuenciaDao>();
            Bind<ICatMezclaDao>().To<CatMezclaDao>();
            Bind<ICatSeveridadDao>().To<CatSeveridadDao>();
            Bind<ICausaRiesgoDao>().To<CausaRiesgoDao>();
            Bind<IControlDao>().To<ControlDao>();
            Bind<IRiesgoDao>().To<RiesgoDao>();
            Bind<ISeveridadDao>().To<SeveridadDao>();
            Bind<IControlEventoDao>().To<ControlEventoDao>();
            Bind<IEventoRiesgoDao>().To<EventoRiesgoDao>();
            Bind<IMatrizDao>().To<MatrizDao>();
            Bind<IMatrizControlDao>().To<MatrizControlDao>();
            Bind<IMatrizControlEventoDao>().To<MatrizControlEventoDao>();
            Bind<IMatrizEventoRiesgoDao>().To<MatrizEventoRiesgoDao>();

            //Reportes
            Bind<IReportesDao>().To<ReportesDao>();

        }
    }
}