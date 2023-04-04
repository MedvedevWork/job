using Android.OS;
using Android.Widget;
using System.Data.SqlTypes;
using System.Text;
using System.Xml;

namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {

        string[] parser = new string[5];
        string[] answers = new string[5];
        ListView lw_id;
        string[] items;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            get(null, null);
        }

        async private void get(Object sender, EventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //TextView txt = FindViewById<TextView>(Resource.Id.text);
            lw_id = FindViewById<ListView>(Resource.Id.lw_id);

            string result = null;

            string url = "https://yastatic.net/market-export/_/partner/help/YML.xml";
            XmlTextReader reader = null;

            string oid1 = "", oid2 = "";

            reader = new XmlTextReader(url);

            while (reader.Read())
            {
                switch (reader.Name) 
                {
                    case "offers": // Вывод первого id
                      
                        oid2 += reader.ReadOuterXml();
                        int d = oid2.IndexOf("id"); // ответ 28

                        for (int i = d+4; i < d+9; i++)
                        {
                            oid1 += oid2.ElementAt(i);
                        }

                        answers[0] = oid1;

                        // Вывод остальных id

                        for (int q = 1; q < 5; q++)
                        {
                            string[] parser2 = oid2.Split("</offer>", 2);

                            string p21 = parser2[1];

                            string[] parser3 = p21.Split("<offer", 2);

                            //result += oid1 + parser2[1]; // шестой символ первая цифра

                            oid2 = parser3[1];
                            oid1 = "";

                            int d2 = oid2.IndexOf("id");

                            //result += answers[0] + "****" + parser3[1];

                            //d = 26;

                            for (int a = d2 +4; a < d2 +9; a++)
                            {
                               oid1 += oid2.ElementAt(a);
                            }

                            answers[q] = oid1;
                        }

                        for (int res = 0; res < 5; res++)
                        {
                            result += answers[res] + " ";
                        }

                        break;
                }
            }

            // Вывод в ListView

            lw_id.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, answers);

            lw_id.ItemClick += (s, e) => {
                var t = answers[e.Position];
                Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Long).Show();
            };
        }
    }
}