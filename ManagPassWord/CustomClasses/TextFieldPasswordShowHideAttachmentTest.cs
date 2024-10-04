using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.Shapes;
using UraniumUI.Extensions;
using UraniumUI.Material.Controls;
using UraniumUI.Pages;
using UraniumUI.Resources;
using UraniumUI.Views;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace ManagPassWord.CustomClasses
{
    public interface INotification
    {
        Task ShowNotification(string message);
    }
    public class ToastNotification : INotification
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ToastDuration duration;
        double textSize;
        public ToastNotification() : this(ToastDuration.Short, 14)
        {

        }
        public ToastNotification(ToastDuration duration, double textSize)
        {
            this.duration = duration;
            this.textSize = textSize;
        }
        public async Task ShowNotification(string message)
        {
            string text = $"{message} ";
            var toast = Toast.Make(text, duration, textSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
    public class TextFieldPasswordShowHideAttachmentTest: StatefulContentView
    {
        public TextField TextField { get; protected set; }

        public TextFieldPasswordShowHideAttachmentTest()
        {
            VerticalOptions = LayoutOptions.Center;
            Padding = new Thickness(5, 0);
            Margin = new Thickness(0, 0, 5, 0);
            TappedCommand = new Command(SwitchPassword);
        }

        protected override void OnParentSet()
        {
            TextField = this.FindInParents<TextField>();
            if (TextField == null)
            {
                return;
            }

            UpdateIcon();
        }

        protected virtual void SwitchPassword(object parameter)
        {
            if (TextField is null)
            {
                UpdateIcon();
                return;
            }

            TextField.IsPassword = !TextField.IsPassword;
            UpdateIcon();
        }

        protected void UpdateIcon()
        {
            if (TextField is null)
            {
                Content = null;

                return;
            }

            Content = TextField.IsPassword ? GetPathFromData(UraniumShapes.Eye) : GetPathFromData(UraniumShapes.EyeSlash);
        }

        private Path GetPathFromData(Geometry data)
        {
            return new Path
            {
                Fill = ColorResource.GetColor("OnBackground", "OnBackgroundDark", Colors.DarkGray).WithAlpha(.5f),
                VerticalOptions = LayoutOptions.Center,
                Data = data,
            };
        }
    }
    public class TextFieldPasswordShowHideAttachmentCopy : StatefulContentView
    {
        public TextField TextField { get; protected set; }
        INotification notification;

        public TextFieldPasswordShowHideAttachmentCopy()
        {
            VerticalOptions = LayoutOptions.Center;
            Padding = new Thickness(5, 0);
            Margin = new Thickness(0, 0, 10, 0);
            notification = new ToastNotification();
            TappedCommand = new Command(OnCopy);
        }

        protected override void OnParentSet()
        {
            TextField = this.FindInParents<TextField>();
            if (TextField == null)
            {
                return;
            }

            UpdateIcon();
        }

        protected virtual async void OnCopy(object parameter)
        {
            if (TextField is null)
            {
                UpdateIcon();
                return;
            }

            await Clipboard.Default.SetTextAsync(TextField.Text);
            await notification.ShowNotification("Copied");
        }

        protected void UpdateIcon()
        {
            if (TextField is null)
            {
                Content = null;

                return;
            }

            Content = GetPathFromData(UraniumShapes.ExclamationCircle);
        }

        private Path GetPathFromData(Geometry data)
        {
            return new Path
            {
                Fill = ColorResource.GetColor("OnBackground", "OnBackgroundDark", Colors.DarkGray).WithAlpha(.5f),
                VerticalOptions = LayoutOptions.Center,
                Data = data,
            };
        }
    }

}
