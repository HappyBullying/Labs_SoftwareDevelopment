using System;

using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Content.Res;
using Android.Support.V4.App;
using Android.Graphics;
using System.Threading;

namespace Lab_4
{

    [BroadcastReceiver(Label = "HellApp Widget")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]
    class AppWidget : AppWidgetProvider
    {

        public static string curDate = "";
        public static int year, month, dayOfMonth, started;
        public static long days;
        public static int counter = 0;
        public PendingIntent service = null;
        public Timer myTimer = null;
        private static int Period = 5000;

        public static uint SecondsLeft
        {
            get
            {
                return _SecondsLeft;
            }
            set
            {
                _SecondsLeft = value;
            }
        }

        static volatile uint _SecondsLeft = 0;

        public override void OnEnabled(Context context)
        {
            base.OnEnabled(context);
        }


        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            base.OnUpdate(context, appWidgetManager, appWidgetIds);
            foreach (int id in appWidgetIds)
            {
                startTimer(context, appWidgetManager, appWidgetIds);
                UpdateWidget(context, appWidgetManager, id);
            }
        }
        public override void OnDeleted(Context context, int[] appWidgetIds)
        {
            base.OnDeleted(context, appWidgetIds);
            stopTimer(); //Остановка таймера
        }

        public override void OnDisabled(Context context)
        {
            base.OnDisabled(context);
            stopTimer();
        }

        private void startTimer(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            if (myTimer == null)
                myTimer = new System.Threading.Timer((object state) =>
                {
                    for (int i = 0; i < appWidgetIds.Length; i++)
                        UpdateWidget(context, appWidgetManager, appWidgetIds[i]);
                }, null, 0, Period);
            else
                myTimer.Change(0, Period);
        }

        private void stopTimer()
        {
            myTimer.Change(0, Timeout.Infinite);
        }

        public static void UpdateWidget(Context context, AppWidgetManager appWidgetManager, int widgetID)
        {
            RemoteViews widgetView = new RemoteViews(context.PackageName, Resource.Layout.layout1);

            _SecondsLeft -= 5;
            TimeSpan differ = (DateTime.Now.AddSeconds(_SecondsLeft) - DateTime.Now);

            widgetView.SetTextViewText(2131296412, "Осталось целых дней: " + Math.Floor((DateTime.Now.AddSeconds(_SecondsLeft) - DateTime.Now).TotalDays) + "\nОсталось минут: " + (int)differ.TotalMinutes);
            
            if (_SecondsLeft < 0)
            {
                notificationEventStarted(context, widgetID);
            }


            Intent configIntent = new Intent(context, Java.Lang.Class.FromType(typeof(CalendarActivity)));
            configIntent.SetAction(AppWidgetManager.ActionAppwidgetConfigure);
            configIntent.PutExtra(AppWidgetManager.ExtraAppwidgetId, widgetID);
            PendingIntent pIntent = PendingIntent.GetActivity(context, widgetID, configIntent, 0);


            widgetView.SetOnClickPendingIntent(2131296411, pIntent);
            appWidgetManager.UpdateAppWidget(widgetID, widgetView);
        }

       

        public static void notificationEventStarted(Context context, int widgetID)
        {
            const int NOTIFY_ID = 101;
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                Resources res = context.Resources;
                NotificationChannel notificationChannel = new NotificationChannel("EventStarted", "Событие наступило", NotificationManager.ImportanceDefault);
                NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(notificationChannel);
                NotificationCompat.Builder builder = new NotificationCompat.Builder(context, "EventStarted").SetSmallIcon(Resource.Drawable.ic_launcher_background).SetContentTitle("Напоминание").SetContentText("СОБЫТИЕ НАСТУПИЛО")
                        .SetLargeIcon(BitmapFactory.DecodeResource(res, Resource.Drawable.ic_launcher_background))
                        .SetTicker("БЫСТРЕЕ!")
                        .SetWhen((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds)
                        .SetAutoCancel(true);
                notificationManager.Notify(NOTIFY_ID, builder.Build());
            }
            else
            {
                Intent notificationIntent = new Intent(context, Java.Lang.Class.FromType(typeof(CalendarActivity)));
                PendingIntent contentIntent = PendingIntent.GetActivity(context,
                        0, notificationIntent,
                        PendingIntentFlags.CancelCurrent);

                Resources res = context.Resources;

                // до версии Android 8.0 API 26
                NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

                builder.SetContentIntent(contentIntent)
                            .SetSmallIcon(Resource.Drawable.ic_launcher_background)
                            .SetContentTitle("Напоминание")
                            .SetContentText("СОБЫТИЕ НАСТУПИЛО")
                            .SetLargeIcon(BitmapFactory.DecodeResource(res, Resource.Drawable.ic_launcher_background))
                            .SetTicker("БЫСТРЕЕ!")
                            .SetWhen((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds)
                            .SetAutoCancel(true); // автоматически закрыть уведомление после нажатия

                NotificationManager notificationManager =
                        (NotificationManager)context.GetSystemService(Context.NotificationService);
                // Альтернативный вариант
                // Notification
                // ManagerCompat notificationManager = NotificationManagerCompat.from(this);
                notificationManager.Notify(NOTIFY_ID, builder.Build());
            }
        }

    }
}