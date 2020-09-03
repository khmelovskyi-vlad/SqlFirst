using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                var blogs = db.Blogs
                    .Where(b => b.Rating > 3)
                    .OrderBy(b => b.Url)
                    .ToList();
            }
            using (var db = new BloggingContext())
            {
                var strategy = db.Database.CreateExecutionStrategy();

                strategy.Execute(() =>
                {
                    using (var context = new BloggingContext())
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/dotnet" });
                            context.SaveChanges();

                            context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/visualstudio" });
                            context.SaveChanges();

                            transaction.Commit();
                        }
                    }
                });
            }
            Console.ReadKey();
        }
    }
}
