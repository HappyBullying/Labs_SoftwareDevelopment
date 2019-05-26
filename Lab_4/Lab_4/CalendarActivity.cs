using System;

using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using Java.Text;
using Java.Util;

namespace Lab_4
{
    [Activity(Label = "CalendarActivity")]
    public class CalendarActivity : Activity
    {
        Activity contextActivity;
        int widgetID = AppWidgetManager.InvalidAppwidgetId;
        Intent resultValue;
        static volatile uint Seconds = 0;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // извлекаем ID конфигурируемого виджета
            Intent intent = Intent;
            Bundle extras = intent.Extras;
            if (extras != null)
            {
                widgetID = extras.GetInt(AppWidgetManager.ExtraAppwidgetId, AppWidgetManager.InvalidAppwidgetId);
            }
            if (widgetID == AppWidgetManager.InvalidAppwidgetId)
            {
                Finish();
            }

            // формируем intent ответа
            resultValue = new Intent();
            resultValue.PutExtra(AppWidgetManager.ExtraAppwidgetId, widgetID);

            // отрицательный ответ
            SetResult(Result.Canceled, resultValue);

            SetContentView(Resource.Layout.calendar_layout);
            Button button = FindViewById<Button>(Resource.Id.choose_button);
            contextActivity = this;
            CalendarView calendar = FindViewById<CalendarView>(Resource.Id.calendarView1);
            if (AppWidget.curDate != null && !AppWidget.curDate.Equals(""))
            {
                Calendar calendarW = Calendar.GetInstance(Java.Util.TimeZone.Default);
                calendarW.Set(Calendar.Year, AppWidget.year);
                calendarW.Set(Calendar.Month, AppWidget.month - 1);
                calendarW.Set(Calendar.DayOfMonth, AppWidget.dayOfMonth);
                long milliTime = calendarW.TimeInMillis;
                calendar.SetDate(milliTime, false, false);
            }



            calendar.DateChange += (s, e) =>
            {
                long curLongDate = (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                Calendar calendarW = Calendar.GetInstance(Java.Util.TimeZone.Default);
                calendarW.Set(Calendar.Year, e.Year);
                calendarW.Set(Calendar.Month, e.Month);
                calendarW.Set(Calendar.DayOfMonth, e.DayOfMonth);
                calendarW.Set(CalendarField.Hour, 0);
                calendarW.Set(CalendarField.Minute, 0);
                calendarW.Set(CalendarField.Second, 0);
                calendarW.Set(CalendarField.Millisecond, 0);



                long differ = calendarW.TimeInMillis - curLongDate;

                int year2 = e.Year, month2 = e.Month, dayOfMonth2 = e.DayOfMonth;
                if (differ < 0)
                {
                    Toast toast = Toast.MakeText(contextActivity, "Вы не можете выбрать дату, которая раньше текущей", ToastLength.Long);
                    toast.Show();
                    string date = new SimpleDateFormat("dd/MM/yyyy").Format(Calendar.GetInstance(Java.Util.TimeZone.Default).Time);
                    string[] array = date.Split('/');
                    Calendar calendarNow = Calendar.GetInstance(Java.Util.TimeZone.Default);
                    year2 = Convert.ToInt32(array[2]);
                    month2 = Convert.ToInt32(array[1]) - 1;
                    dayOfMonth2 = Convert.ToInt32(array[0]);
                    calendarNow.Set(Calendar.Year, year2);
                    calendarNow.Set(Calendar.Month, month2);
                    calendarNow.Set(Calendar.DayOfMonth, dayOfMonth2);
                    calendarNow.Set(Calendar.Hour, 0);
                    calendarNow.Set(Calendar.Minute, 0);
                    calendarNow.Set(Calendar.Second, 0);
                    calendarNow.Set(Calendar.Millisecond, 0);
                    long milliTime = calendarNow.TimeInMillis;
                    calendar.SetDate(milliTime, false, false);
                }
                var y = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(calendarW.TimeInMillis);
                Seconds = (uint)(new DateTime(1970, 1, 1, 0,0,0).AddMilliseconds(calendarW.TimeInMillis) - DateTime.Now).TotalSeconds;
            };

            button.Click += (s, e) =>
            {
                SetResult(Result.Ok, resultValue);
                AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(contextActivity);
                
                AppWidget.SecondsLeft = Seconds;
                AppWidget.UpdateWidget(contextActivity, appWidgetManager, widgetID);
                Finish();
            };
        }
    }
}