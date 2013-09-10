using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.DAL.Concrete;
using ESRGC.DLLR.EARN.Domain.DAL;

namespace ESRGC.DLLR.EARN.Infrastructure
{
  public class NinjectDependencyResolver: IDependencyResolver
  {
    IKernel _kernel;
    public NinjectDependencyResolver() {
      _kernel = new StandardKernel();
      addBindings();
    }

    void addBindings() {
      _kernel
        .Bind<IWorkUnit>()
        .To<WorkUnit>()
        .WithConstructorArgument("context", new DomainContext());
    }

    public object GetService(Type serviceType) {
      return _kernel.TryGet(serviceType);
    }

    public IEnumerable<object> GetServices(Type serviceType) {
      return _kernel.GetAll(serviceType);
    }
  }
}