using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AlarmNotifications {
	[Activity(Label = "AlarmNotifications", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			Button button = FindViewById<Button>(Resource.Id.MyButton);
			Button recurringButton = FindViewById<Button>(Resource.Id.buttonRecurring);

			button.Click += delegate {
				ScheduleNotification();
			};

			recurringButton.Click += delegate {
				ScheduleRecurringNotification();
			};
		}

		public void ScheduleNotification() {
			Intent alarmIntent = new Intent(this, typeof(AlarmReceiver));

			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);

			//schedule notification for 5s out
			alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 5000, pendingIntent);
		}

		public void ScheduleRecurringNotification() {
			Intent alarmIntent = new Intent(this, typeof(AlarmReceiver));

			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);

			Java.Util.Calendar calendar = Java.Util.Calendar.GetInstance(Java.Util.Locale.Us);
			calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
			calendar.Set(Java.Util.CalendarField.HourOfDay, DateTime.Now.Hour);
			calendar.Set(Java.Util.CalendarField.Minute, DateTime.Now.Minute);
			
			//schedule repeating notification for every 5 seconds
			alarmManager.SetRepeating(AlarmType.RtcWakeup, 0, 5000, pendingIntent);
		}
	}

	[BroadcastReceiver]
	public class AlarmReceiver : BroadcastReceiver {
		public override void OnReceive(Context context, Intent intent) {
			var notIntent = new Intent(context, typeof(MainActivity));
			var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.UpdateCurrent);
			var manager = NotificationManager.FromContext(context);

			var builder = new Notification.Builder(context)
				.SetSmallIcon(Resource.Drawable.Icon)
				.SetContentIntent(contentIntent)
				.SetContentText("Sweet")
				.SetContentTitle("Dude")
				.SetAutoCancel(true);

			var notification = builder.Build();
			manager.Notify(0, notification);
		}
	}
}

