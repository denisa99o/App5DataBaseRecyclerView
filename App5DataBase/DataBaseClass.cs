using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace App5DataBase
{
   public class DataBaseClass
    {
        SQLiteConnection db;
     
        public DataBaseClass()
        {
            string dbPath = Path.Combine(
              Globals.RootDirectory, "denisa.db"
                   );
            db = new SQLiteConnection(dbPath);
            db.CreateTable<Product>();
            

            db.CreateTable<Magazin>();
            

        }

 public void addProduct(Product product)
    {
            db.Insert(product);
    }
     public void addMagazin(Magazin magazin)
       {
            db.Insert(magazin);
      }
        public List<Product> getAllProducts()
        {
            return db.Query<Product>("select * from Product");
        }

        public List<Magazin> getAllMagazin()
        {
            return db.Query<Magazin>("select * from Magazin");
        }

        public void deleteProduct(Product product)
        {
            db.Delete(product);
        }

        public void insertProduct(Product product)
        {
            db.Insert(product);
        }

        //public string insertProducts(string _id, string Name, string Cantity)
        //{
        //    db.Query<Product>("Insert into Product([Id],[Name],[Cantity]) values ('"+_id+ "','" + Name + "','" + Cantity + "')");

        //    return
        //}

        public void updateProduct(Product product)
        {
            db.Update(product);
           
        }

    }
    [Table("Product")]
    public class ProductTable
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(8)]
        public string Name { get; set; }
        public string Cantity { get; set; }
    }
    
    public class Product : ProductTable
    {
        [Ignore]
        public int Position { get; set; }
    }


    [Table("Magazin")]
    public class Magazin
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(8)]
        public string Name { get; set; }
        public string Locatie { get; set; }

    }
    
}