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

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.MyButton);
			button.Text = "Show notification in 5s";
			button.Click += delegate {
				ScheduleNotification();
			};
		}

		public void ScheduleNotification() {
			Intent alarmIntent = new Intent(this, typeof(AlarmReceiver));

			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);

			//schedule notification for 2s out
			alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 5000, pendingIntent);
		}
	}

	[BroadcastReceiver]
	public class AlarmReceiver : BroadcastReceiver {
		public override void OnReceive(Context context, Intent intent) {
			var notIntent = new Intent(context, typeof(MainActivity));
			var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
			var manager = NotificationManager.FromContext(context);

			var builder = new Notification.Builder(context)
				.SetSmallIcon(Resource.Drawable.Icon)
				.SetContentIntent(contentIntent)
				.SetContentText("Sweet")
				.SetContentTitle("Dude")
				.SetAutoCancel(true)
				.SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis());

			var notification = builder.Build();
			manager.Notify(0, notification);
		}
	}
}

