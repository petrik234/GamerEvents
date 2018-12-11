using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GamerEvents.DBModel;

namespace GamerEvents
{
    class EventAdapter : RecyclerView.Adapter
    {
        public Event[] events;

        public EventAdapter(Event[] events)
        {
            this.events = events;
        }

        public event EventHandler<int> ItemClick;

        public override int ItemCount
        {
            get { return events.Length; }
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            EventViewHolder vh = holder as EventViewHolder;

            // Load the photo image resource from the photo album:
            /*Uri imgUri = new Uri("android.resource://my.package.name/"); //+ R.drawable.image);
            vh.Image.SetImageURI(null); //SetImageResource(events[position].imageid);*/
            vh.Image.SetImageResource(events[position].imageid);
            //vh.Image.SetImageResource(2130903042);

            // Load the photo caption from the photo album:
            vh.Caption.Text = events[position].game;
            vh.Details.Text = events[position].details;
            vh.Date.Text = events[position].startdate.ToString("yyyy-MM-dd  HH:mm");//.Substring(0, events[position].startdate.Length-3);
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.EventCardView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            EventViewHolder vh = new EventViewHolder(itemView, OnClick);
            return vh;
        }
    }
}