using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;

namespace Lab1
{
    [Activity(Label = "activity_main", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private bool isResumed = false;
        private volatile bool isOnBackPressed = false;
        System.Threading.Thread WaitThread = null;
        Android.Content.Intent INTENT = null;
        static readonly String IsResumed = "is resumed";
        static readonly String IsOnBackPressed = "is on back pressed";


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            ImageView MainImage = FindViewById<ImageView>(Resource.Id.MainImage);
            INTENT = new Android.Content.Intent(this, typeof(SecondActivity));
            MainImage.SetImageResource(Resource.Drawable.carrot);

        }


        protected override void OnResume()
        {
            base.OnResume();
            if (!isResumed)
            {
                WaitThread = new System.Threading.Thread(() =>
                {
                    System.Threading.Thread.Sleep(2000);
                    RunOnUiThread(() =>
                    {
                        StartActivity(INTENT);
                        Finish();
                    });
                });
                WaitThread.Start();
                isResumed = true;
            }
        }


        public override void OnSaveInstanceState(Bundle outState, PersistableBundle outPersistentState)
        {
            outState.PutBoolean(IsResumed, isResumed);
            outState.PutBoolean(IsOnBackPressed, isOnBackPressed);
            base.OnSaveInstanceState(outState);
        }

        public override void OnRestoreInstanceState(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnRestoreInstanceState(savedInstanceState, persistentState);
            isResumed = savedInstanceState.GetBoolean(IsResumed);
            isOnBackPressed = savedInstanceState.GetBoolean(IsOnBackPressed);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            isOnBackPressed = true;
        }
    }
}