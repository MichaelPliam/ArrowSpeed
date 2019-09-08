using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace ArrowSpeed
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText _et1, _et2, _et3, _et4;
        TextView _tv7, _tv9, _tv11;
        Button _btnCalc, _btnNext;

        // inputs
        double ibo;
        double drawlen;
        double drawwt;
        double arrowwt;
        // outputs
        double fps;
        double ke;
        double mv;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _et1 = (EditText)FindViewById(Resource.Id.edit1);
            _et2 = (EditText)FindViewById(Resource.Id.edit2);
            _et3 = (EditText)FindViewById(Resource.Id.edit3);
            _et4 = (EditText)FindViewById(Resource.Id.edit4);

            _tv7 = (TextView)FindViewById(Resource.Id.textView7);
            _tv9 = (TextView)FindViewById(Resource.Id.textView9);
            _tv11 = (TextView)FindViewById(Resource.Id.textView11);
            _btnCalc = (Button)FindViewById(Resource.Id.calc);
            _btnNext = (Button)FindViewById(Resource.Id.next);


            /*
                        _et1.Text = string.Empty;
                        _et2.Text = string.Empty;
                        _et3.Text = string.Empty;
                        _et4.Text = string.Empty;

                        _tv7.Text = string.Empty;
                        _tv9.Text = string.Empty;
                        _tv11.Text = string.Empty;
            */


            _btnCalc.Click += delegate
            {
                ibo = drawlen = drawwt = arrowwt = 0.0;
                bool b1 = Double.TryParse(_et1.Text, out ibo);
                bool b2 = Double.TryParse(_et2.Text, out drawlen);
                bool b3 = Double.TryParse(_et3.Text, out drawwt);
                bool b4 = Double.TryParse(_et4.Text, out arrowwt);

                if (!b1 || !b2 || !b3 || !b4)
                {
                    // DisplayAlert("Data Input Error", "The data is either missing or invalid.\nPlease try again.", "OK");
                    string message = "The data is either missing or invalid.\nPlease try again.";
                    Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();

                    return;
                }

                System.Diagnostics.Debug.WriteLine(ibo);
                System.Diagnostics.Debug.WriteLine(drawlen);
                Debug.WriteLine(drawwt);
                Debug.WriteLine(arrowwt);

                // calculate
                fps = FPS(ibo, drawlen, drawwt, arrowwt);
                ke = KE(fps, arrowwt);
                mv = MV(fps, arrowwt);

                fps = Math.Round(fps, 0);
                ke = Math.Round(ke, 0);
                mv = Math.Round(mv, 2);

                // check results
                Debug.WriteLine(String.Format("\n{0} fps", fps));
                Debug.WriteLine(String.Format("\n{0} lb-ft2/sec2", ke));
                Debug.WriteLine(String.Format("\n{0} lb-ft or slugs", mv));

                // ouput results 
                _tv7.Text = String.Format("{0}", fps);
                _tv9.Text = String.Format("{0}", ke);
                _tv11.Text = String.Format("{0}", mv);

            };

            _btnNext.Click += delegate
            {
                Debug.WriteLine("You clicked the Next button!");
                // go to the next Activity
            };

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// http://bestcompoundbowsource.com/whats-bows-real-speed/
        /// Draw Length - for every 1"< 30" subtract 10 FPS
        /// Draw Weight - for every 10 lbs < 70 lbs subtract 15 - 20 FPS
        ///	Arrow Weight - for every 5 grains > 350 grains subtract 1.5 FPS from IBO
        ///	Xtra String Accessories - subtract 5 - 6 FPS
        ///	Human Release Factor - subtract 2 - 3 FPS
        private double FPS(double IBO, double drawlen, double drawwt, double arrowwt)
        {
            double fps = IBO - (30.0 - drawlen) * 10.0;
            fps = fps - 20.0 * (70.0 - drawwt) / 10.0;
            fps = fps - 1.5 * (arrowwt - 350.0) / 5.0;
            fps = fps - 6.0;
            fps = fps - 3.0;
            return fps;
        }

        private double KE(double speed, double weight)
        {
            // KE = 1/2 m * v^2
            // 1 grain = 0.000142857 pound
            // 1 pound = 6999.999691 grains
            // double vfps = speed;
            // double wtgr = weight;
            // double wtlb = wtgr * 0.000142857;
            // correlates well with https://www.realtree.com/kinetic-energy-and-momentum-calculator
            // also with http://archerycalculator.com/estimate-bow-speed/
            double ke = speed * speed * weight / 450800;
            //double ke = vfps * vfps * wtlb / 2.0;
            return ke;
        }

        private double MV(double speed, double weight)
        {
            // correlates well with https://www.realtree.com/kinetic-energy-and-momentum-calculator
            // also with http://archerycalculator.com/estimate-bow-speed/
            double mv = speed * weight / 225400;
            return mv;
        }



        private void DisplayAlert(string v1, string v2, string v3)
        {
            //throw new NotImplementedException();
           // AlertDialog dlg = new Android.App.AlertDialog();
        }

 
    }
}