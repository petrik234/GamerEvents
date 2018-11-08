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
using GamerEvents.DBModel;

namespace GamerEvents
{
    [Activity(Label = "Profile")]
    public class Profile : Activity
    {
        Button btnMain;
        Button btnProfile;
        Button btnMap;
        Button btnCreate;
        Button btnSave;

        TextView etEmail;
        TextView etName;
        TextView etAge;
        TextView etCity;
        TextView etConfig;

        User currentUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.profile);

            SettingsManager sm = new SettingsManager();
            int.TryParse(sm.LoadLocalFile("userid"), out int id);
            currentUser = User.GetById(id);

            etEmail = FindViewById<TextView>(Resource.Id.etEmail);
            etEmail.SetFocusable(ViewFocusability.NotFocusable);
            etName  = FindViewById<TextView>(Resource.Id.etName);
            etAge = FindViewById<TextView>(Resource.Id.etAge);
            etCity = FindViewById<TextView>(Resource.Id.etCity);
            etConfig = FindViewById<TextView>(Resource.Id.etKonfig);

            etEmail.Text = currentUser.email;
            etName.Text = currentUser.realname;
            etAge.Text = currentUser.age.ToString();
            etCity.Text = currentUser.city;
            etConfig.Text = currentUser.konfig;

            btnSave = FindViewById<Button>(Resource.Id.btnSave);

            btnMain = FindViewById<Button>(Resource.Id.btnMain);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            btnMap = FindViewById<Button>(Resource.Id.btnMap);
            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);

            btnSave.Click += BtnSave_Click;     

            btnMain.Click += BtnMain_Click;
            btnProfile.Click += BtnProfile_Click;
            btnMap.Click += BtnMap_Click;
            btnCreate.Click += BtnCreate_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            //az age intre alakítása
            int.TryParse(etAge.Text, out int age);
            User modifiedUser = new User()
            {
                userid = currentUser.userid,
                email = currentUser.email,
                realname = etName.Text,
                age = age,
                city = etCity.Text,
                konfig = etConfig.Text
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