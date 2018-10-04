using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V7.App;
using Android.Widget;

using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace GamerEvents
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText etEmail;
        private EditText etPass;
        private Button btnOK;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            etEmail = FindViewById<EditText>(Resource.Id.TextEmail);
            etPass = FindViewById<EditText>(Resource.Id.TextPass);
            btnOK = FindViewById<Button>(Resource.Id.buttonOk);

            btnOK.Click += BtnOK_Click;

     

        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(@"http://pte-ttk.wscdev.hu/team4/insert.php");
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("email",etEmail.Text);
            parameters.Add("password",etPass.Text);

            wc.UploadValuesCompleted += Wc_UploadValuesCompleted;
            wc.UploadValuesAsync(uri,parameters);



        }

        private void Wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {


            //btnOK.Text = e.Result.ToString(); ;
        }
    }
}