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
        Button btnSubscribe;

        TextView tvGame;
        TextView tvStartDate;
        TextView tvLocation;
        TextView tvDetails;
        TextView tvMaxMember ;

        Subscribe[] allSub;

        Event currentEvent;
        User currentUser;

        bool isCurrentUserSubscribed;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.detailsEvent);
            
            SettingsManager sm = new SettingsManager();
            int.TryParse(sm.LoadLocalFile("eventid"), out int eid);
            currentEvent = Event.GetById(eid);
            int.TryParse(sm.LoadLocalFile("userid"), out int uid);
            currentUser = User.GetById(uid);
            allSub = Subscribe.GetAll();
            tvGame = FindViewById<TextView>(Resource.Id.tvGame);
            tvStartDate = FindViewById<TextView>(Resource.Id.tvStartDate);
            tvLocation = FindViewById<TextView>(Resource.Id.tvLocation);
            tvDetails = FindViewById<TextView>(Resource.Id.tvDetails);
            tvMaxMember = FindViewById<TextView>(Resource.Id.tvMaxMember);

            tvGame.Text = currentEvent.game;
            tvStartDate.Text = currentEvent.startdate.ToString("yyyy-MM-dd  HH:mm");
            tvLocation.Text = currentEvent.location;
            tvDetails.Text = currentEvent.details;
            tvMaxMember.Text = string.Format("{0} / {1}", (allSub is null) ? 0.ToString() : allSub.Where(x => x.eventid == currentEvent.eventid).Count().ToString() , currentEvent.userlimit.ToString());

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

            if (allSub is null)
            {
                isCurrentUserSubscribed = false;
            }
            else
            {
                isCurrentUserSubscribed = (allSub.Where(x => x.userid == currentUser.userid && x.eventid == currentEvent.eventid).Count() == 0 ) ? false : true;
            }

            btnSubscribe.Text = (isCurrentUserSubscribed) ? "Leiratkozás" : "Feliratkozás";

        }

        private void BtnSubscribe_Click(object sender, EventArgs e)
        {
            

            if (isCurrentUserSubscribed)
            {
                int currentSubId = allSub.Where(x => x.userid == currentUser.userid && x.eventid == currentEvent.eventid).Select(x => x.subid).First(); ;
                
                //leiratkozunk
                if (Subscribe.deleteById(currentSubId))
                {
                    Toast.MakeText(this, "Sikeresen leiratkoztál!", ToastLength.Short).Show();
                    isCurrentUserSubscribed = false;
                    btnSubscribe.Text = "Feliratkozás";
                    allSub = Subscribe.GetAll();
                    if (allSub is null)
                    {
                        tvMaxMember.Text = "0 /" + currentEvent.userlimit.ToString();
                    }
                    else
                    {
                        tvMaxMember.Text = string.Format("{0} / {1}", allSub.Where(x => x.eventid == currentEvent.eventid).Count(), currentEvent.userlimit.ToString());
                    }
                }
                else
                {
                    Toast.MakeText(this, "Nem sikerült leiratkozni!", ToastLength.Short).Show();
                }
                
            }
            else
            {
                if (!(allSub is null))
                {
                    if (allSub.Where(x => x.eventid == currentEvent.eventid).Count() >= currentEvent.userlimit)
                    {
                        Toast.MakeText(this, "Az esemény jelenleg be van telve!", ToastLength.Short).Show();
                        return;
                    }
                }
                
                // feliratkozunk
                Subscribe subscribe = new Subscribe()
                {
                    userid = currentUser.userid,
                    eventid = currentEvent.eventid
                };

                if (Subscribe.CreateNew(subscribe))
                {
                    btnSubscribe.Text = "Leiratkozás";
                    isCurrentUserSubscribed = true;
                    Toast.MakeText(this, "Sikeresen feliratkoztál!", ToastLength.Short).Show();
                    allSub = Subscribe.GetAll();
                    tvMaxMember.Text = string.Format("{0} / {1}", allSub.Where(x => x.eventid == currentEvent.eventid).Count(), currentEvent.userlimit.ToString());
                }
                else
                {
                    Toast.MakeText(this, "Nem sikerült feliratkozni!", ToastLength.Short).Show();
                }
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