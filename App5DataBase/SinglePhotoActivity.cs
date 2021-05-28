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
using Android.Views;
using Android.Widget;

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
}