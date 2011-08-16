using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Infrastructure.Concrete;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using Moq;
using System.Linq;
using System.Collections.Generic;
using SportsStore.Domain.Concrete;

namespace SportsStore.WebUI.Infrastructure
{


    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext,
            Type controllerType)
        {

            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            // put additional bindings here
            var prods = new Product[] { 
                new Product{ProductID =1, Name ="Mangos", Category="Fruit", Description="Summer gift", Price=12M},
                new Product{ProductID =2, Name ="Apples", Category="Fruit", Description="spring gift", Price=20M},
                new Product{ProductID =3, Name ="Nike Joggers", Category="Sports", Description="football fever", Price=13M},
                new Product{ProductID =4, Name ="Calculator", Category="Accounting", Description="japaniiii", Price=52M},
                new Product{ProductID =5, Name ="PC", Category="Computers", Description="I am PC", Price=92M},
                new Product{ProductID =6, Name ="MAC", Category="Computers", Description="I am  Mac", Price=120M}
            };
                     
            //Mocking IProduct and setting what will its Products property will return
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(prods.AsQueryable());

            //Registering the Mock object with IProductRepository
            //ninjectKernel.Bind<IProductRepository>().ToConstant(mock.Object);
            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile
                = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            ninjectKernel.Bind<IOrderProcessor>()
            .To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);

            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}