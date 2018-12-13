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
using System.Threading;
using Xamarin.Auth;
using Xamarin_FacebookAuth.Authentication;
using Xamarin_FacebookAuth.Services;
using System.Threading.Tasks;
using System.Net.Mail;

namespace GamerEvents
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class Login : AppCompatActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IFacebookAuthenticationDelegate
    {
        private GoogleApiClient googleApiClient;
        private EditText etEmail;
        private EditText etPass;
        private Button btnOK;
        private SignInButton btnGoogleSignIn;

        private FacebookAuthenticator _auth;
        private string loggedEmail = "";
        private string loggedName = "";

        private ConnectionResult connectionResult;
        private bool intentInProgress;
        private bool signInClicked;
        private bool infoPopulated;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.login);

            //skip login
            //GoToMainPage(1); 

            _auth = new FacebookAuthenticator("306666026619877", "email", this);

            var facebookLoginButton = FindViewById<Button>(Resource.Id.facebookLoginButton);
            facebookLoginButton.Click += OnFacebookLoginButtonClicked;

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

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            var authenticator = _auth.GetAuthenticator();
            var intent = authenticator.GetUI(this);
            StartActivity(intent);
        }

        public async void OnAuthenticationCompleted(FacebookOAuthToken token)
        {
            // Retrieve the user's email address
            var facebookService = new FacebookService();
            loggedEmail = await facebookService.GetEmailAsync(token.AccessToken);
            loggedName = await facebookService.GetNameAsync(token.AccessToken);

            User formUser = new User
            {
                email = loggedEmail,
                password = ""
            };

            User userResult = User.GetByEmail(formUser.email);
            if (userResult != null)
            {
                GoToMainPage(userResult.userid);
            }
            else
            {
                if (User.CreateNew(formUser))
                {
                    Toast.MakeText(this, "Sikeres facebook bejelentkezés", ToastLength.Long).Show();
                    //regisztrált
                    //lekérjük az idjét
                    User loggedUser = User.GetByEmail(formUser.email);
                    Handler h = new Handler();
                    Action myAction = () =>
                    {
                        GoToMainPage(loggedUser.userid);
                    };

                    h.PostDelayed(myAction, 2000);

                }
                else
                {
                    //sikertelen regisztráció
                    Toast.MakeText(this, "Sikertelen facebook bejelentkezés", ToastLength.Short).Show();
                }
            }

                // Display it on the UI
                /*var facebookButton = FindViewById<Button>(Resource.Id.facebookLoginButton);
                facebookButton.Text = $"Connected with {loggedEmail}";*/
        }

        public void OnAuthenticationCanceled()
        {
            new Android.App.AlertDialog.Builder(this).SetTitle("Authentication canceled").SetMessage("You didn't completed the authentication process").Show();
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            new Android.App.AlertDialog.Builder(this).SetTitle(message).SetMessage(exception?.ToString()).Show();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                MailAddress m = new MailAddress(etEmail.Text);
            }
            catch (FormatException)
            {
                Toast.MakeText(this, "Helytelen email formátum", ToastLength.Long).Show();
                return;
            }

            if ( etPass.Text.Length < 6)
            {
                Toast.MakeText(this, "A jelszó 6 karekter vagy\nannál hosszabb legyen!", ToastLength.Long).Show();
                return;
            }

            User formUser = new User
            {
                email = etEmail.Text,
                password = etPass.Text
            };

            User userResult = User.GetByEmail(formUser.email);
            if (userResult != null)
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
                    Toast.MakeText(this, "Sikertelen Bejelentkezés\nRossz a jelszó!", ToastLength.Long).Show();
                }
            }
            else
            {
                if (User.CreateNew(formUser))
                {
                    Toast.MakeText(this, "Sikeres regisztráció", ToastLength.Long).Show();
                    //regisztrált
                    //lekérjük az idjét
                    User loggedUser = User.GetByEmail(formUser.email);
                    Handler h = new Handler();
                    Action myAction = () =>
                    {
                        GoToMainPage(loggedUser.userid);
                    };

                    h.PostDelayed(myAction, 2000);
                    
                }
                else
                {
                    //sikertelen regisztráció
                    Toast.MakeText(this, "Sikertelen regisztráció\nAz email foglalt!", ToastLength.Short).Show();
                }
           }
        }

        private void GoToMainPage(int userId)
        {
            SettingsManager sm = new SettingsManager();
            sm.WriteLocalFile("userid", userId.ToString());

            //string asd = sm.LoadLocalFile("userid");

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