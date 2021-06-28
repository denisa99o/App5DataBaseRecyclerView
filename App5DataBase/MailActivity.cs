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
using MimeKit;
using MailKit.Net.Smtp;
//send mail via application-trimit mail direct din aplicatie
namespace App5DataBase
{
    [Activity(Label = "MailActivity")]
    public class MailActivity : Activity
    {
        EditText editFrom, editTo, editSubject, editMessage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Mail);

             editFrom = FindViewById<EditText>(Resource.Id.editFrom);
             editTo = FindViewById<EditText>(Resource.Id.editTo);
             editSubject = FindViewById<EditText>(Resource.Id.editSubject);
             editMessage = FindViewById<EditText>(Resource.Id.editMessage);
            Button btnSend = FindViewById<Button>(Resource.Id.btnSend);

            btnSend.Click += BtnSend_Click;

        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            View view = (View)sender;
            new MailAsyncTask(this).Execute(); 
        }

        class MailAsyncTask : AsyncTask
        {
            string username = "mail-id or username", password = "password", host = "smtp.gmail.com";
            int port = 25; //portul este by default
            MailActivity mailActivity;
            ProgressDialog progressDialog;

            public MailAsyncTask(MailActivity activity)
            {
                mailActivity = activity;
                progressDialog = new ProgressDialog(mailActivity);
                progressDialog.SetMessage("Sending...");
                progressDialog.SetCancelable(false);
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                progressDialog.Show();
            }

            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("From", mailActivity.editFrom.Text));
                    message.To.Add(new MailboxAddress("To", mailActivity.editTo.Text));
                    message.Subject = mailActivity.editSubject.Text;

                    message.Body = new TextPart("plain")
                    {
                        Text = mailActivity.editMessage.Text
                    };

                    using (var client = new SmtpClient())
                    {
                        // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        client.Connect(host, port, false);

                        // Note: only needed if the SMTP server requires authentication
                        client.Authenticate(username, password);
                        client.Send(message);
                        client.Disconnect(true);
                    }
                    return "Successfully Sent";
                }
                catch (System.Exception ex)
                {
                    return ex.Message;
                }
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                progressDialog.Dismiss();
                mailActivity.editFrom.Text = null;
                mailActivity.editTo.Text = null;
                mailActivity.editSubject.Text = null;
                mailActivity.editMessage.Text = null;
                Toast.MakeText(mailActivity, "Email Succesfully Sent...", ToastLength.Short).Show();
            }
        }
    }

 
}