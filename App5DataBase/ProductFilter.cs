using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace App5DataBase 
{
    public class ProductFilter : Filter //e nefolosita,o pot sterge
    {
        static   RecyclerAdapter adapter;
        static Product product;
        public List<Product> productList;
        static JavaList<Product> productFilterList;
        public ProductFilter productFilter;
        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            // throw new NotImplementedException();
          
            long magazinId = Long.ParseLong(constraint.ToString());
            FilterResults results = new FilterResults();
            if (magazinId > 0)
            {
              JavaList<Product> filterList = new JavaList<Product>();
                for (int i = 0; i < productFilterList.Count; i++)
                {

                    if ((productFilterList.ElementAt<Product>(i).magazinId) == magazinId)
                    {

                        Product product = productFilterList.ElementAt<Product>(i);
                        filterList.Add(product);
                    }
                }

                results.Count = filterList.Count;
                results.Values = filterList;

            }
            else
            {

                results.Count = productFilterList.Count;
                results.Values = productFilterList;

            }
            return results;

        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            //  throw new NotImplementedException();
            adapter.setProduct((JavaList<Product>)results.Values);
            adapter.NotifyDataSetChanged();
        }

        public static ProductFilter newInstance(JavaList<Product> currentList, RecyclerAdapter adapter)
        {
            ProductFilter.adapter = adapter;
            ProductFilter.productFilterList = currentList;
            return new ProductFilter();
        }
    }


}