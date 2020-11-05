using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Game.Core;
using Microsoft.Xna.Framework;

namespace Game.Android
{
    /// <summary>
    /// The main game activity
    /// </summary>
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        Theme = "@style/SplashTheme",
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Landscape | ScreenOrientation.ReverseLandscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class MainActivity : AndroidGameActivity
    {
        /// <summary>
        /// Called when the activity is started
        /// </summary>
        /// <param name="bundle">bundle; unused</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var game = new TheGame(isTouchEnabledDevice: true, isMobileDevice: true);
            game.SavegameFolder = this.FilesDir.AbsolutePath;

            var view = game.Services.GetService(typeof(View)) as View;

            this.SetContentView(view);
            game.Run();
        }
    }
}
