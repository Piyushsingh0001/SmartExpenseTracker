using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
#if IOS
using UserNotifications;
using ObjCRuntime;
using UIKit;
using Foundation;
#endif

namespace SmartExpenseTracker.Services;

public class NotificationService
{
    private int _notifId = 100;

    public async Task SendAsync(string title, string message)
    {
        var request = new NotificationRequest
        {
            NotificationId = _notifId++,
            Title = title,
            Description = message,
            Android = new AndroidOptions
            {
                ChannelId = "budget_alerts",
                Priority = AndroidPriority.High
            }
        };
        await LocalNotificationCenter.Current.Show(request);
    }
}
