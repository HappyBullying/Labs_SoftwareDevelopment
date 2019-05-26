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

namespace Lab1
{
    class Adapter1 : BaseAdapter<Element>
    {

        private Context context;
        private Android.Graphics.Bitmap Cbitmap;
        public List<Element> Numbers = null;


        public Adapter1(Context context, List<Element> Numbers)
        {
            this.context = context;
            this.Numbers = Numbers;
        }

        public override int Count
        {
            get { return Numbers.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Element this[int position]
        {
            get { return Numbers[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.ElementDesign, null, false);
            }
            TextView text = view.FindViewById<TextView>(Resource.Id.TextElem);
            text.Text = Numbers[position].Text;
            ImageView IMGV = view.FindViewById<ImageView>(Resource.Id.EmageElem);
            IMGV.SetImageResource(Numbers[position].CBitmap);
            view.SetBackgroundColor(Numbers[position].CurrentColor);
            return view;
        }


        


    }
}