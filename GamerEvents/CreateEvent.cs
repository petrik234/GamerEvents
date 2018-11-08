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
    [Activity(Label = "CreateEvent")]
    public class CreateEvent : Activity
    {
        Button btnMain;
        Button btnProfile;
        Button btnMap;
        Button btnCreate;


        private EditText createGame;
        private EditText createTime;
        private EditText createLocation;
        private EditText createDescription;
        private EditText createNumber;
        private Button createEventsB;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.createEvent);

            btnMain = FindViewById<Button>(Resource.Id.btnMain);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            btnMap = FindViewById<Button>(Resource.Id.btnMap);
            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            
            btnMain.Click += BtnMain_Click;
            btnProfile.Click += BtnProfile_Click;
            btnMap.Click += BtnMap_Click;
            btnCreate.Click += BtnCreate_Click;

            createGame = FindViewById<EditText>(Resource.Id.cEinputGame);
            createTime = FindViewById<EditText>(Resource.Id.cEinputTime);
            createLocation = FindViewById<EditText>(Resource.Id.cEinputLocation);
            createDescription = FindViewById<EditText>(Resource.Id.cEinputDescription);
            createNumber = FindViewById<EditText>(Resource.Id.cEnumber);

            createEventsB = FindViewById<Button>(Resource.Id.createEventsButton);


            createEventsB.Click += createEventsButton_Click;

        }



        private void createEventsButton_Click(object sender, EventArgs e)
        {
            if (createGame.Text == string.Empty || 
                //createTime.Text == string.Empty || 
                createLocation.Text == string.Empty ||
                createDescription.Text == string.Empty )
            {
                //hibakezelés
                return;
            }

            SettingsManager sm = new SettingsManager();
            string userid = sm.LoadLocalFile("userájdi");


            Event formEvent = new Event
            {


                //ownerid = userid.Text,
                //date = createTime.Text,
                location = createLocation.Text,
                game = createGame.Text,
                details = createDescription.Text
                //userlimit = createNumber

            };




            if (Event.CreateNewEvent(formEvent))
            {
                //event felvétele sikeresen megtörtént

            }
            else
            {
                //Nem sikerült az event felvétel
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