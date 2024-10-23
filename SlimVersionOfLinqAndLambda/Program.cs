using SlimVersionOfLinqAndLambda.Models;
using SlimVersionOfLinqAndLambda.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlimVersionOfLinqAndLambda
{
    internal class Program
    {
        static void Main(string[] args)
        {
			try
			{
                DoTask();
			}
			catch (Exception ex)
			{

                Console.WriteLine(ex.Message);
            }
            finally { Console.ReadLine(); }
        }

        private static void DoTask()
        {
            ProductRepo pRepo=new ProductRepo();
            var products=pRepo.GetProducts();
            CategoryRepo cRepo=new CategoryRepo();
            var cats=cRepo.GetCategoryList();
            ProductModelRepo pMRepo=new ProductModelRepo();
            var models=pMRepo.GetModels();

            Console.ForegroundColor=ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("===========LambdaList===========");
            Console.WriteLine("---------------------------------");
            var lambdaList=products.Join(cats, p=>p.ProductCategoryID, c=>c.ProductCategoryID, (p,c)=>new {p,c})
                                   .Join(models, pc=>pc.p.ProductModelID, m=>m.ProductModelID, (pc,m)=>new { pc, m })
                                   .Select (n=>new 
                                   {
                                       PID=n.pc.p.ProductID,
                                       PName=n.pc.p.Name,
                                       MName=n.pc.p.Name,
                                       CatName=n.pc.c.Name,
                                       Cost = n.pc.p.StandardCost


                                   });
            foreach (var item in lambdaList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("==========Linq=========");
            Console.WriteLine("------------------------");
            var linqList = from p in products
                           join c in cats on p.ProductCategoryID equals c.ProductCategoryID
                           join m in models on p.ProductModelID equals m.ProductModelID
                           select new 
                           {
                                PID=p.ProductID,
                                PName=p.Name,
                                MName=m.Name,
                                CName=c.Name,
                                SPrice=p.StandardCost
                           };
            foreach (var item in linqList) 
            {
                Console.WriteLine(item);
            }
                          


        }
    }
}
