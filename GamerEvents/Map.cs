using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GamerEvents.DBModel;


namespace GamerEvents
{
    [Activity(Label = "Map")]
    public class Map : AppCompatActivity, IOnMapReadyCallback
    {
        Button btnMain;
        Button btnProfile;
        Button btnMap;
        Button btnCreate;

        MapFragment mapFragment;

        Event[] events;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.map);

            btnMain = FindViewById<Button>(Resource.Id.btnMain);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            btnMap = FindViewById<Button>(Resource.Id.btnMap);
            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            
            btnMain.Click += BtnMain_Click;
            btnProfile.Click += BtnProfile_Click;
            btnMap.Click += BtnMap_Click;
            btnCreate.Click += BtnCreate_Click;

            

            mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map2);
            mapFragment.GetMapAsync(this);
        }

        public void OnMapReady(GoogleMap map)
        {
            map.MapType = GoogleMap.MapTypeTerrain;

            map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(46.0774,18.6222), 5.9f));

          

            events = Event.GetAll();

            foreach (Event item in events)
            {
                // itt minegyik elemnek létrehozol egy pontot item.lat, item.lon tulajdonságokban vannak az értékek
                MarkerOptions markerOpt = new MarkerOptions();
                markerOpt.SetPosition(new LatLng(item.lat, item.lon));
                markerOpt.SetTitle(item.game);

                map.AddMarker(markerOpt);

                //most csak a példa kedvéért minden ciklusba belerakom az aktuális lat-ot a btnMain szövegébe azaz az uccsót fogja kiírni a képernyőre a main button szövegére
                //btnMain.Text = item.lat.ToString();
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