﻿using Android.App;
using Android.Content.PM;
using Plugin.Fingerprint;
using Android.OS;
using Plugin.CurrentActivity;

namespace ManagPassWord;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        CrossFingerprint.SetCurrentActivityResolver(() => this);
    }
}
