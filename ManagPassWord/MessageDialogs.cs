using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagPassWord
{
    public static class MessageDialogs
    {
        public static async Task ShowToast(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //string text = "This is a Toast";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(message, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
