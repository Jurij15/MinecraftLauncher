﻿using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using WinUIEx.Messaging;

namespace MinecraftLauncherUniversal.Services
{
    public class NotificationService
    {
        public static void SendSimpleToast(string title, string message, double ExpirationTime)
        {
            var toast1 = new AppNotificationBuilder()
                .AddText(title)
                .AddText(message)
                .BuildNotification();

            var xmlPayload = new string($@"
        <toast>    
            <visual>    
                <binding template=""ToastGeneric"">    
                    <text>{title}</text>
                    <text>{message}</text>    
                </binding>
            </visual>  
        </toast>");

            var toast = new AppNotification(xmlPayload);
            toast.Expiration = DateTimeOffset.Now.AddSeconds(ExpirationTime);

            if (Globals.ToastFailedInit)
            {
                Log.Error("Failed to show a notification with title " + title + " and message " + message + " that was set to expire in " + ExpirationTime.ToString());
            }
            else
            {
                try
                {
                    AppNotificationManager.Default.Show(toast);
                }
                catch (Exception)
                {
                    Log.Error("Failed to show a notification with title " + title + " and message " + message + " that was set to expire in " + ExpirationTime.ToString());
                    throw;
                }
            }
        }
    }
}
