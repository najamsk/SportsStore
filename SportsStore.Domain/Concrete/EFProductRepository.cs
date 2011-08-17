using System.Linq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Data.Entity;
using System.Data;
using System.Xml;
using System.Data.Entity.Infrastructure;
using System.Text;



namespace SportsStore.Domain.Concrete {

    public class EFProductRepository : IProductRepository {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Product> Products {
            get {                          
                
                using (var writer = new XmlTextWriter(@"d:/SportsStore.edmx", Encoding.Default))
                {
                    EdmxWriter.WriteEdmx(context, writer);
                }
                
                return context.Products;             
            }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
            }
            //context.ChangeObjectState(product, EntityState.Modified); 
            //context.Products.FirstOrDefault(p=>p.ProductID == product.ProductID);
            context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }
        
    }

    public class EFDbContext : DbContext {
        public DbSet<Product> Products { get; set; }
    }
}