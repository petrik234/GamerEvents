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
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.IO;
using GamerEvents.Model;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms;
using Android.Gms.Plus;
using Android.Gms.Plus.Model.People;
using Android.Content;

namespace GamerEvents
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private GoogleApiClient googleApiClient;
        private EditText etEmail;
        private EditText etPass;
        private Button btnOK;
        private SignInButton btnGoogleSignIn;

        private ConnectionResult connectionResult;
        private bool intentInProgress;
        private bool signInClicked;
        private bool infoPopulated;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            
            GoogleApiClient.Builder builder = new GoogleApiClient.Builder(this);
            builder.AddConnectionCallbacks(this);
            builder.AddOnConnectionFailedListener(this);
            builder.AddApi(PlusClass.API);
            builder.AddScope(PlusClass.ScopePlusProfile);
            builder.AddScope(PlusClass.ScopePlusLogin);
            googleApiClient = builder.Build();


            etEmail = FindViewById<EditText>(Resource.Id.TextEmail);
            etPass = FindViewById<EditText>(Resource.Id.TextPass);
            btnOK = FindViewById<Button>(Resource.Id.buttonOk);
            

            btnOK.Click += BtnOK_Click;

            btnGoogleSignIn = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            btnGoogleSignIn.Click += BtnGoogleSignIn_Click;


        }

        private void BtnOK_Click(object sender, EventArgs e)
        {

            User user = new User
            {
                email = etEmail.Text,
                password = etPass.Text
            };

            //search for existed email/pass

            string url = "https://pte-ttk.wscdev.hu/team4/user/read_one.php?email=" + user.email;


            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            string reply = client.UploadString(url, "");

            User userResult = JsonConvert.DeserializeObject<User>(reply);


            /*
            //beszúrás
            string jsonString = JsonConvert.SerializeObject(user);
            url = "https://pte-ttk.wscdev.hu/team4/user/create.php";
          

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            string reply = client.UploadString(url, jsonString);

            bool messageResult = JsonConvert.DeserializeObject<bool>(reply);

            btnOK.Text = messageResult.ToString();

            */
        }


        private void BtnGoogleSignIn_Click(object sender, EventArgs e)
        {
            if (!googleApiClient.IsConnecting)
            {
                signInClicked = true;
                ResolveSignInError();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            googleApiClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (googleApiClient.IsConnected)
            {
                googleApiClient.Disconnect();
            }
        }
        //base-t töröltük
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 0)
            {
                if (resultCode != Result.Ok)
                {
                    signInClicked = false;
                }

                intentInProgress = false;

                if (!googleApiClient.IsConnecting)
                {
                    googleApiClient.Connect();
                }
            }

        }
        public void OnConnected(Bundle connectionHint)
        {
            //Successful log in hooray!!
            signInClicked = false;

            if (infoPopulated)
            {
                //No need to populate info again
                return;
            }

            if (PlusClass.PeopleApi.GetCurrentPerson(googleApiClient) != null)
            {
                IPerson plusUser = PlusClass.PeopleApi.GetCurrentPerson(googleApiClient);
               

               
                infoPopulated = true;
            }
        }

        public void OnConnectionSuspended(int cause)
        {
            throw new NotImplementedException();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!intentInProgress)
            {
                //Store the ConnectionResult so that we can use it later when the user clicks 'sign-in;
                connectionResult = result;

                if (signInClicked)
                {
                    //The user has already clicked 'sign-in' so we attempt to resolve all
                    //errors until the user is signed in, or the cancel
                    ResolveSignInError();
                }
            }
        }


        private void ResolveSignInError()
        {
            if (googleApiClient.IsConnected)
            {
                return;
            }
            if (connectionResult.HasResolution)
            {
                try
                {
                    intentInProgress = true;
                    StartIntentSenderForResult(connectionResult.Resolution.IntentSender, 0, null, 0, 0, 0);
                }
                catch (Android.Content.IntentSender.SendIntentException e)
                {
                    intentInProgress = false;
                    googleApiClient.Connect();

                }
            }
        }    
    }
}