using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace App5DataBase
{
    [Activity(Label = "SinglePhoto")]
    public class SinglePhotoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SinglePhoto);

            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageViewSingle);
            Button buttonBack = FindViewById<Button>(Resource.Id.btnBack);

           Bitmap photo=GalleryActivity.mPhotoAlbum[Intent.Extras.GetInt("photo")]; //putem apela lista mPhtoAlbum pentru ca e public static, nu trebuie sa o creem din nou

            //nu putem deserializa ->nu functioneaza
   //         byte[] bytes = Intent.Extras.GetByteArray("photo"); //ii iau datele
   //         using (MemoryStream ms=new MemoryStream(bytes))
   //         {
   //             BinaryFormatter formattter = new BinaryFormatter();
   //             List<Bitmap> output_images =
   //         (List<Bitmap>)formattter.Deserialize(ms);
   
   //         }
             imageView.SetImageBitmap(photo); //pun in imageView imaginea

            // Create your application here
            buttonBack.Click += ButtonBack_Click;

            //slide between images

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            ImageAdpter adapter = new ImageAdpter(this);
            viewPager.Adapter = adapter;

        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //Intent previousActivity = new Intent(this, typeof(GalleryActivity));
            //StartActivity(previousActivity);

            //ca sa dau back doar trebuie sa dau finish la activitate
            Finish();
        }

        
    }

    //slide between images
    public class ImageAdpter : PagerAdapter
    {
        private Context context;
        public ImageAdpter(Context context)
        {
            this.context = context;
        }
        public override int Count => GalleryActivity.mPhotoAlbum.Count;

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            //throw new NotImplementedException();
            return view == ((ImageView)@object);
        }

        public override Java.Lang.Object InstantiateItem(View container, int position)
        {
            ImageView imageView = new ImageView(context);
            imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            // Bitmap photo = GalleryActivity.mPhotoAlbum[GetItemPosition("photo")];
           // int lastposition = GalleryActivity.mPhotoAlbum.IndexOf(Bitmap );
            imageView.SetImageBitmap(GalleryActivity.mPhotoAlbum[position]);
            ((ViewPager)container).AddView(imageView, 0);
            return imageView;
            // return base.InstantiateItem(container, position);
        }
       
        public override void DestroyItem(View container, int position, Java.Lang.Object @object)
        {

            //base.DestroyItem(container, position, @object);
            ((ViewPager)container).RemoveView((ImageView)@object);
        }


    }
}