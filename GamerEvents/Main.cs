using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GamerEvents.DBModel;

namespace GamerEvents
{
    [Activity(Label = "Main")]
    public class Main : Activity
    {
        Button btnMain;
        Button btnProfile;
        Button btnMap;
        Button btnCreate;
        RecyclerView rvEvent;
        RecyclerView.LayoutManager mLayoutManager;
        EventAdapter eventAdapter;

        Event[] events;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            events = Event.GetAll().Where(x => x.startdate > DateTime.Now).OrderBy(x => x.startdate).ToArray();


            eventAdapter = new EventAdapter(events);

            eventAdapter.ItemClick += OnItemClick;

            SetContentView(Resource.Layout.main);

            btnMain = FindViewById<Button>(Resource.Id.btnMain);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            btnMap = FindViewById<Button>(Resource.Id.btnMap);
            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            rvEvent = FindViewById<RecyclerView>(Resource.Id.rvEvent);
            
            btnMain.Click += BtnMain_Click;
            btnProfile.Click += BtnProfile_Click;
            btnMap.Click += BtnMap_Click;
            btnCreate.Click += BtnCreate_Click;

            rvEvent.SetAdapter(eventAdapter);
            mLayoutManager = new LinearLayoutManager(this);
            rvEvent.SetLayoutManager(mLayoutManager);

        }

        void OnItemClick(object sender, int position)
        {
            SettingsManager sm = new SettingsManager();
            sm.WriteLocalFile("eventid", events[position].eventid.ToString());

            Intent intent = new Intent(this, typeof(DetailsEvent));
            this.StartActivity(intent);
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