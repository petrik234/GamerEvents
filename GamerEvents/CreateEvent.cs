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
using Android.Gms.Location.Places;
using Android.Gms.Location;
using Android.Gms.Location.Places.UI;

namespace GamerEvents
{
    [Activity(Label = "CreateEvent")]
    public class CreateEvent : Activity
    {
        private static readonly int PLACE_PICKER_REQUEST = 1;
        Button btnMain;
        Button btnProfile;
        Button btnMap;
        Button btnCreate;

        private Button btnPickPlace;
        private EditText createDescription;
        private EditText createNumber;
        private Button createEventsB;
  
        private TextView dateDisplay;
        private TextView timeDisplay;

        DateTime selectedDateTime = new DateTime();
        string selectedGame = string.Empty;
        string selectedLocation = string.Empty;
        double selectedLon = 0f;
        double selectedLat = 0f;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.createEvent);

            

            //startActivityForResult(builder.build(this), PLACE_PICKER_REQUEST);

            btnMain = FindViewById<Button>(Resource.Id.btnMain);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            btnMap = FindViewById<Button>(Resource.Id.btnMap);
            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);


            btnMain.Click += BtnMain_Click;
            btnProfile.Click += BtnProfile_Click;
            btnMap.Click += BtnMap_Click;
            btnCreate.Click += BtnCreate_Click;

            dateDisplay = FindViewById<TextView>(Resource.Id.dateDisplay);
            timeDisplay = FindViewById<TextView>(Resource.Id.timeDisplay);
            btnPickPlace = FindViewById<Button>(Resource.Id.btnPickPlace);
            createDescription = FindViewById<EditText>(Resource.Id.cEinputDescription);
            createNumber = FindViewById<EditText>(Resource.Id.cEnumber);


            createEventsB = FindViewById<Button>(Resource.Id.createEventsButton);


            createEventsB.Click += createEventsButton_Click;
            dateDisplay.Click += _dateSelectButton_Click;
            timeDisplay.Click += TimeDisplay_Click;
            btnPickPlace.Click += BtnGetLoc_Click;
            


            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

        }

        private void BtnGetLoc_Click(object sender, EventArgs e)
        {

            var builder = new PlacePicker.IntentBuilder();
            StartActivityForResult(builder.Build(this), PLACE_PICKER_REQUEST);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == PLACE_PICKER_REQUEST && resultCode == Result.Ok)
            {
                GetPlaceFromPicker(data);
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void GetPlaceFromPicker(Intent data)
        {
            var placePicked = PlacePicker.GetPlace(this, data);
            selectedLocation = placePicked?.NameFormatted?.ToString();
            btnPickPlace.Text = selectedLocation;
            selectedLon = placePicked.LatLng.Longitude;
            selectedLat = placePicked.LatLng.Latitude;

        }
    

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            selectedGame = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }

        private void TimeDisplay_Click(object sender, EventArgs e)
        { 
            TimePickerFragment frag = TimePickerFragment.NewInstance(
            delegate (DateTime time)
            {
                timeDisplay.Text = time.ToString("HH:mm");
                selectedDateTime = new DateTime(selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, time.Hour, time.Minute, 0);
               });

            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void _dateSelectButton_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                dateDisplay.Text = date.ToString("yyyy-MM-dd");
                selectedDateTime = new DateTime(date.Year, date.Month, date.Day, selectedDateTime.Hour, selectedDateTime.Minute, 0);
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void createEventsButton_Click(object sender, EventArgs e)
        {
            

            if (dateDisplay.Text == string.Empty ||
                selectedLocation == string.Empty ||
                createDescription.Text == string.Empty ||
                timeDisplay.Text == string.Empty)
            {
                //hibakezelés
                Toast.MakeText(this, "Valamit nem töltöttél ki!", ToastLength.Short).Show();
                return;
            }

            SettingsManager sm = new SettingsManager();
            string userid = sm.LoadLocalFile("userid");
            int uid = Convert.ToInt32(userid);
            int cNumber = Convert.ToInt32(createNumber.Text);



            Event formEvent = new Event
            {
                ownerid = uid,
                startdate = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                location = selectedLocation,
                game = selectedGame,
                details = createDescription.Text,
                userlimit = cNumber,
                lat = selectedLat,
                lon = selectedLon
            };




            if (Event.CreateNewEvent(formEvent))
            {
                //event felvétele sikeresen megtörtént
                Toast.MakeText(this, "Az eventet sikeresen létrehoztad!", ToastLength.Short).Show();
                createEventsB.Enabled = false;
                Handler h = new Handler();
                void myAction()
                {
                    Intent intent = new Intent(this, typeof(Main));
                    this.StartActivity(intent);
                    this.Finish();
                }
                h.PostDelayed(myAction, 1500);
                
                
            }
            else
            {
                //Nem sikerült az event felvétel
                Toast.MakeText(this, "Az event felvétele nem sikerült!", ToastLength.Short).Show();
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