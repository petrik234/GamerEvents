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

namespace GamerEvents
{
    [Activity(Label = "Main")]
    public class Main : Activity
    {
        Button btnMain;
        Button btnProfile;
        Button btnMap;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main);

            btnMain = FindViewById<Button>(Resource.Id.btnMain);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            btnMap = FindViewById<Button>(Resource.Id.btnMap);

            btnMain.Click += BtnMain_Click;
            btnProfile.Click += BtnProfile_Click;
            btnMap.Click += BtnMap_Click;
        }

        private void BtnMain_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Main));
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