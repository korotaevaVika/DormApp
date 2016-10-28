using DormApp.Domain;
using DormApp.Domain.Interfaces;

namespace DormApplication.Infrastructure
{
    public class Bindings : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument("context", new DormApp.Entities.Dormitory_Entities());
        }
    }
}