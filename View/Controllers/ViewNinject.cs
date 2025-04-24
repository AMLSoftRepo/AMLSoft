using Ninject.Modules;
using Ninject.Web.Common;
using System.Web;
using System.Security.Principal;
using Blo.Alertas;
using Blo.Seguridad;
using Blo.Monitoreo;
using Blo.Listas;
using Blo.Matriz;
using Blo.Perfiles;
using Blo.Reportes;

namespace View.Controllers
{
    public class ViewNinject : NinjectModule
    {
        public override void Load()
        {
            //Alertas
            Bind<IContactoAlertaBlo>().To<ContactoAlertaBlo>();
            Bind<ITipoAlertaBlo>().To<TipoAlertaBlo>();
            Bind<INotificacionAlertaBlo>().To<NotificacionAlertaBlo>();
            Bind<IOperacionIrregularBlo>().To<OperacionIrregularBlo>();
            Bind<IAlertaBlo>().To<AlertaBlo>();
            Bind<IDatosAdicionalesTransaccionBlo>().To<DatosAdicionalesTransaccionBlo>();
            Bind<IAlertaListaPersonaBlo>().To<AlertaListaPersonaBlo>();
            Bind<IAlertaPerfilTransaccionalBlo>().To<AlertaPerfilTransaccionalBlo>();
            Bind<IBloqueoBlo>().To<BloqueoBlo>();

            //Seguridad
            Bind<IAccesoUsuarioBlo>().To<AccesoUsuarioBlo>();
            Bind<IModuloBlo>().To<ModuloBlo>();
            Bind<IOpcionBlo>().To<OpcionBlo>();
            Bind<IParametroBlo>().To<ParametroBlo>();
            Bind<IRolBlo>().To<RolBlo>();
            Bind<IRolUsuarioBlo>().To<RolUsuarioBlo>();
            Bind<IEmpresaBlo>().To<EmpresaBlo>();
            Bind<IPermisoBlo>().To<PermisoBlo>();
            Bind<IRolPermisoBlo>().To<RolPermisoBlo>();
            Bind<ICargaDatosBlo>().To<CargaDatosBlo>();

            //Perfiles
            Bind<IFactorBlo>().To<FactorBlo>();
            Bind<IConfiguracionFactorBlo>().To<ConfiguracionFactorBlo>();
            Bind<ICalificacionFactorBlo>().To<CalificacionFactorBlo>();
            Bind<IHistorialPerfilBlo>().To<HistorialPerfilBlo>();
            Bind<ITipoCalificacionBlo>().To<TipoCalificacionBlo>();
            Bind<ICoincidenciaListaBlo>().To<CoincidenciaListaBlo>();

            //Monitoreo
            Bind<ICatFinancieraBlo>().To<CatFinancieraBlo>();
            Bind<ICatInstitucionBlo>().To<CatInstitucionBlo>();
            Bind<IContactoInstitucionBlo>().To<ContactoInstitucionBlo>();
            Bind<ICargoInstitucionBlo>().To<CargoInstitucionBlo>();
            Bind<IAgenciaBancariaBlo>().To<AgenciaBancariaBlo>();
            Bind<IOficioBlo>().To<OficioBlo>();
            Bind<IPersonasOficioBlo>().To<PersonasOficioBlo>();
            Bind<IDiaFestivoBlo>().To<DiaFestivoBlo>();
            Bind<IConfiguracionOficioBlo>().To<ConfiguracionOficioBlo>();
            Bind<IRosBlo>().To<RosBlo>();
            Bind<IPersonasRosBlo>().To<PersonasRosBlo>();
            Bind<IOperacionesRosBlo>().To<OperacionesRosBlo>();
            Bind<IAdjuntosRosBlo>().To<AdjuntosRosBlo>();
            Bind<IClientePEPBlo>().To<ClientePEPBlo>();
            Bind<ICatUnidadInstitucionBlo>().To<CatUnidadInstitucionBlo>();
            Bind<IEscalaCalificacionBlo>().To<EscalaCalificacionBlo>();

            //Listas
            Bind<IParametroONUSDNBlo>().To<ParametroONUSDNBlo>();
            Bind<ISDNBlo>().To<SDNBlo>();
            Bind<ISDNAkaBlo>().To<SDNAkaBlo>();
            Bind<ISDNDireccionBlo>().To<SDNDireccionBlo>();
            Bind<ISDNDocumentoBlo>().To<SDNDocumentoBlo>();
            Bind<ISDNNacionalidadBlo>().To<SDNNacionalidadBlo>();
            Bind<IONUBlo>().To<ONUBlo>();
            Bind<IONUAliasBlo>().To<ONUAliasBlo>();
            Bind<IONUDireccionBlo>().To<ONUDireccionBlo>();
            Bind<IONUDocumentoBlo>().To<ONUDocumentoBlo>();
            Bind<IPEPBlo>().To<PEPBlo>();
            Bind<IPEPCargoBlo>().To<PEPCargoBlo>();
            Bind<IPEPRelacionBlo>().To<PEPRelacionBlo>();
            Bind<IGeneralBlo>().To<GeneralBlo>();
            Bind<IGeneralPersonalizadaBlo>().To<GeneralPersonalizadaBlo>();
            Bind<IPersonalizadaBlo>().To<PersonalizadaBlo>();
            Bind<IPersonalizadaDocumentoBlo>().To<PersonalizadaDocumentoBlo>();
            Bind<IPersonalizadaDireccionBlo>().To<PersonalizadaDireccionBlo>();
            Bind<IPersonalizadaAliasBlo>().To<PersonalizadaAliasBlo>();
            Bind<IExentoBlo>().To<ExentoBlo>();
            Bind<ICatOrganoBlo>().To<CatOrganoBlo>();
            Bind<ICatTituloBlo>().To<CatTituloBlo>();
            Bind<ICatEntidadBlo>().To<CatEntidadBlo>();
            Bind<ICatGrupoBlo>().To<CatGrupoBlo>();
            Bind<IPaisGrupoBlo>().To<PaisGrupoBlo>();


            //Matriz
            Bind<ICatProbabilidadOcurrenciaBlo>().To<CatProbabilidadOcurrenciaBlo>();
            Bind<ICatTipoControlBlo>().To<CatTipoControlBlo>();
            Bind<ICatUnidadBlo>().To<CatUnidadBlo>();
            Bind<ICatAgenciaBlo>().To<CatAgenciaBlo>();
            Bind<ICatFactorRiesgoBlo>().To<CatFactorRiesgoBlo>();
            Bind<ICatTipoRiesgoBlo>().To<CatTipoRiesgoBlo>();
            Bind<ICatAutomatizacionBlo>().To<CatAutomatizacionBlo>();
            Bind<ICatDisenoBlo>().To<CatDisenoBlo>();
            Bind<ICatDocumentacionBlo>().To<CatDocumentacionBlo>();
            Bind<ICatFrecuenciaBlo>().To<CatFrecuenciaBlo>();
            Bind<ICatMezclaBlo>().To<CatMezclaBlo>();
            Bind<ICatSeveridadBlo>().To<CatSeveridadBlo>();
            Bind<ICausaRiesgoBlo>().To<CausaRiesgoBlo>();
            Bind<IControlBlo>().To<ControlBlo>();
            Bind<IRiesgoBlo>().To<RiesgoBlo>();
            Bind<ISeveridadBlo>().To<SeveridadBlo>();
            Bind<IControlEventoBlo>().To<ControlEventoBlo>();
            Bind<IEventoRiesgoBlo>().To<EventoRiesgoBlo>();
            Bind<IMatrizBlo>().To<MatrizBlo>();
            Bind<IMatrizControlBlo>().To<MatrizControlBlo>();
            Bind<IMatrizControlEventoBlo>().To<MatrizControlEventoBlo>();
            Bind<IMatrizEventoRiesgoBlo>().To<MatrizEventoRiesgoBlo>();

            //Reportes
            Bind<IReportesBlo>().To<ReportesBlo>();

        }
    }
}