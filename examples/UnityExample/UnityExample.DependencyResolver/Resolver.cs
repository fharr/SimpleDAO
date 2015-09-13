using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.DAL;
using UnityExample.DAL.EntityFramework;
using UnityExample.DAL.EntityFramework.Mapping;

namespace UnityExample.DependencyResolver
{
    public class Resolver
    {
        private static IUnityContainer Container { get; set; }

        static Resolver()
        {
            Container = new UnityContainer()
                .RegisterType<IUnitOfWork, UnitOfWork>();
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
