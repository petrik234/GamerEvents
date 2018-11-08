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
    [Activity(Label = "Event")]
    public class DetailsEvent : Activity
    {
        Button btnMain;
        Button btnProfile;
        Button btnMap;
        Button btnCreate;

        TextView tvGame;
        TextView tvStartDate;
        TextView tvLocation;
        TextView tvDetails;
        TextView tvMaxMember ;
        Button btnSubscribe;

        Event currentEvent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.detailsEvent);
            
            SettingsManager sm = new SettingsManager();
            int.TryParse(sm.LoadLocalFile("eventid"), out int id);
            currentEvent = Event.GetById(id);
            
            tvGame = FindViewById<TextView>(Resource.Id.tvGame);
            tvStartDate = FindViewById<TextView>(Resource.Id.tvStartDate);
            tvLocation = FindViewById<TextView>(Resource.Id.tvLocation);
            tvDetails = FindViewById<TextView>(Resource.Id.tvDetails);
            tvMaxMember = FindViewById<TextView>(Resource.Id.tvMaxMember);

            tvGame.Text = currentEvent.game;
            tvStartDate.Text = currentEvent.startdate;
            tvLocation.Text = currentEvent.location;
            tvDetails.Text = currentEvent.details;
            tvMaxMember.Text = currentEvent.userlimit.ToString();

            btnSubscribe = FindViewById<Button>(Resource.Id.btnSubscribe);

            btnMain = FindViewById<Button>(Resource.Id.btnMain);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            btnMap = FindViewById<Button>(Resource.Id.btnMap);
            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);

            btnSubscribe.Click += BtnSubscribe_Click;          
            btnMain.Click += BtnMain_Click;
            btnProfile.Click += BtnProfile_Click;
            btnMap.Click += BtnMap_Click;
            btnCreate.Click += BtnCreate_Click;
        }

        private void BtnSubscribe_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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