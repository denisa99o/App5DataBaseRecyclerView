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

            db.CreateTable<Depozit>();
            

        }

 public void addProduct(Product product)
    {
            db.Insert(product);
    }
     public void addMagazin(Magazin magazin)
       {
            db.Insert(magazin);
      }

        public void addDepozit(Depozit depozit)
        {
            db.Insert(depozit);
        }

        public List<Depozit> getAllDepozite()
        {
            return db.Query<Depozit>("select * from Depozit");
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

        public List<Magazin> GetMagazins()
        {
            return db.Query<Magazin>("select * from Magazin");
        }

        public Magazin GetMagazinById(int id)
        {
            return db.Get<Magazin>(id);
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
        public override string ToString()
        {
            return Name;
        }
        public int magazinId { get; set; }

        [Ignore]
        public bool Checked { get; set; } = false; //folosit pentru stergere multipla



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
        [MaxLength(8),Unique]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
        public string Locatie { get; set; }

       

    }


    [Table("Depozit")]
    public class Depozit
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(8)]
        public string Name { get; set; }

        public double Longitudine { get; set; }
        public double Latitudine { get; set; }

    }

   
    
}