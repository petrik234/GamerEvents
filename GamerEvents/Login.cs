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
using GamerEvents.DBModel;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms;
using Android.Gms.Plus;
using Android.Gms.Plus.Model.People;
using Android.Content;
using System.Security.Cryptography;

namespace GamerEvents
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class Login : AppCompatActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
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

            SetContentView(Resource.Layout.login);
            
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
            btnGoogleSignIn = FindViewById<SignInButton>(Resource.Id.sign_in_button);


            btnOK.Click += BtnOK_Click;
            btnGoogleSignIn.Click += BtnGoogleSignIn_Click;


        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (etEmail.Text == string.Empty || etPass.Text == string.Empty)
            {
                //hibakezelés
                return;
            }

            User formUser = new User
            {
                email = etEmail.Text,
                password = etPass.Text
            };
                       
            User userResult = User.GetUserByEmail(formUser.email);
            if (userResult.email != null)
            {
                byte[] encodedPassword = new UTF8Encoding().GetBytes(etPass.Text);

                byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

                string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

                if (userResult.password == encoded )
                {
                    //bejelentkezett
                    GoToMainPage(userResult.userid);
                }
                else
                {
                    //sikertelen bejelentkezéss
                }
            }
            else
            {
                if (User.CreateNewUser(formUser))
                {
                    //regisztrált
                    //lekérjük az idjét
                    User loggedUser = User.GetUserByEmail(formUser.email);
                    GoToMainPage(loggedUser.userid);
                }
                else
                {
                    //sikertelen regisztráció
                }
           }
        }

        private void GoToMainPage(int userId)
        {
            SettingsManager sm = new SettingsManager();
            sm.WriteLocalFile("userájdi", userId.ToString());

            //string asd = sm.LoadLocalFile("userájdi");

            Intent intent = new Intent(this, typeof(Main));
            this.StartActivity(intent);
            this.Finish();
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
                //plus user regisztrálás

                

               
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
                catch
                {
                    intentInProgress = false;
                    googleApiClient.Connect();

                }
            }
        }
    }
}