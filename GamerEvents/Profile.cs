using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GamerEvents.DBModel;

namespace GamerEvents
{
    [Activity(Label = "Profile")]
    public class Profile : Activity
    {
        private bool _isModifyOn = false;

        Button btnMain;
        Button btnProfile;
        Button btnMap;
        Button btnCreate;
        Button btnSave;
        Button btnModify;
        Button btnLogout;

        TextView etEmail;
        TextView etName;
        TextView etAge;
        TextView etCity;
        TextView etConfig;
        TextView etPass;
        TextView etPass2;

        TableRow trPass;
        TableRow trPass2;


        User currentUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.profile);

            SettingsManager sm = new SettingsManager();
            int.TryParse(sm.LoadLocalFile("userid"), out int id);
            currentUser = User.GetById(id);

            etEmail = FindViewById<TextView>(Resource.Id.etEmail);
            etName  = FindViewById<TextView>(Resource.Id.etName);
            etAge = FindViewById<TextView>(Resource.Id.etAge);
            etCity = FindViewById<TextView>(Resource.Id.etCity);
            etConfig = FindViewById<TextView>(Resource.Id.etKonfig);
            etPass = FindViewById<TextView>(Resource.Id.etPass);
            etPass2 = FindViewById<TextView>(Resource.Id.etPass2);

            trPass = FindViewById<TableRow>(Resource.Id.trPass);
            trPass2 = FindViewById<TableRow>(Resource.Id.trPass2);

            etEmail.Text = currentUser.email;
            etName.Text = currentUser.realname;
            etAge.Text = currentUser.age.ToString();
            etCity.Text = currentUser.city;
            etConfig.Text = currentUser.konfig;

            etEmail.Enabled = false;
            etName.Enabled = false;
            etAge.Enabled = false;
            etCity.Enabled = false;
            etConfig.Enabled = false;

            btnLogout = FindViewById<Button>(Resource.Id.btnLogout);
            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnModify = FindViewById<Button>(Resource.Id.btnModify);

            btnMain = FindViewById<Button>(Resource.Id.btnMain);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            btnMap = FindViewById<Button>(Resource.Id.btnMap);
            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);

            btnSave.Click += BtnSave_Click;
            btnModify.Click += BtnModify_Click;
            btnLogout.Click += BtnLogout_Click;

            btnMain.Click += BtnMain_Click;
            btnProfile.Click += BtnProfile_Click;
            btnMap.Click += BtnMap_Click;
            btnCreate.Click += BtnCreate_Click;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {

            SettingsManager sm = new SettingsManager();
            sm.DeleteLocalFile("userid");
            Intent intent = new Intent(this, typeof(Login));
            this.StartActivity(intent);
            this.Finish();
        }

        private void BtnModify_Click(object sender, EventArgs e)
        {
            _isModifyOn = !_isModifyOn;

            etEmail.Enabled = !etEmail.Enabled;
            etName.Enabled = !etName.Enabled;
            etAge.Enabled = !etAge.Enabled;
            etCity.Enabled = !etCity.Enabled;
            etConfig.Enabled = !etConfig.Enabled;

            if (_isModifyOn)
            {
                btnModify.Text = "Szerkesztés ki";
                btnSave.Visibility = ViewStates.Visible;
                trPass.Visibility = ViewStates.Visible;
                trPass2.Visibility = ViewStates.Visible;
            }
            else
            {
                btnModify.Text = "Szerkesztés";
                btnSave.Visibility = ViewStates.Invisible;
                trPass.Visibility = ViewStates.Invisible;
                trPass2.Visibility = ViewStates.Invisible;
            }
        }



        private void BtnSave_Click(object sender, EventArgs e)
        {
            string codedPw = string.Empty;
            bool error = false;
            //az age intre alakítása
            int.TryParse(etAge.Text, out int age);
            if (age > 150)
            {
                Toast.MakeText(this, "Csak nem vagy 150-nél idősebb!", ToastLength.Long).Show();
                error = true;
            }
            if(User.GetByEmail(etEmail.Text) != null && etEmail.Text != currentUser.email)
            {
                error = true;
                Toast.MakeText(this, "Ez az email már foglalt", ToastLength.Long).Show();
            }
            if (etPass.Text != etPass2.Text)
            {
                error = true;
                Toast.MakeText(this, "A két jelszó nem egyezik meg!", ToastLength.Long).Show();
            }
            else if(etPass.Text != string.Empty)
            {
                byte[] encodedPassword = new UTF8Encoding().GetBytes(etPass.Text);

                byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

                codedPw = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            }
            if (error)
            {
                return;
            }
            User modifiedUser = new User()
            {
                userid = currentUser.userid,
                email = etEmail.Text,
                realname = etName.Text,
                age = age,
                city = etCity.Text,
                konfig = etConfig.Text,
                password = (codedPw == string.Empty ) ? currentUser.password : codedPw
            };

            if (User.UpdateById(modifiedUser))
            {
                Toast.MakeText(this, "A módosítás sikeres volt!", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "A módosítás sikertelen volt!", ToastLength.Long).Show();
            }
        }

        private void BtnMain_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Main));
            this.StartActivity(intent);
            this.Finish();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(CreateEvent));
            this.StartActivity(intent);
            this.Finish();
        }

        private void BtnMap_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Map));
            this.StartActivity(intent);
            this.Finish();
        }

        private void BtnProfile_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Profile));
            this.StartActivity(intent);
            this.Finish();
        }
    }
}