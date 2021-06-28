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
//apelez gmail si intru din aplicatie in gmail
namespace App5DataBase
{
    [Activity(Label = "Mail2Activity")]
    public class Mail2Activity : Activity
    {
        EditText editTo2, editSubject2, editMessage2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Mail2);
           
             editTo2 = FindViewById<EditText>(Resource.Id.editTo2);
             editSubject2 = FindViewById<EditText>(Resource.Id.editSubject2);
             editMessage2 = FindViewById<EditText>(Resource.Id.editMessage2);
            Button btnSend2 = FindViewById<Button>(Resource.Id.btnSend2);

            btnSend2.Click += BtnSend2_Click;

        }

        private void BtnSend2_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Intent email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraEmail, new string[] { editTo2.Text.ToString() });
            email.PutExtra(Intent.ExtraSubject, editSubject2.Text.ToString());
            email.PutExtra(Intent.ExtraText, editMessage2.Text.ToString());
            email.SetType("message/rfc822"); //the message content type->indicates that the body contains an encapsulated message, with the syntax of an RCF 822 message
            StartActivity(Intent.CreateChooser(email, "Send Email Via"));
        }
    }
}