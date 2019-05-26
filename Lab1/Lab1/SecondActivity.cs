using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Views.View;

namespace Lab1
{
    [Activity(Label = "SecondActivity")]
    class SecondActivity: Activity
    {
        private List<Element> names = null;
        private ListView Slist = null;
        private int Front = 999999;
        private Adapter1 AdapterForSecond = null;
        private System.Threading.Thread Worker = null;

        string[] nums_1_9 = "ноль один два три четыре пять шесть семь восемь девять".Split();
        string[] nums_10_19 = "десять одиннадцать двенадцать тринадцать четырнадцать пятнадцать шестнадцать семнадцать восемнадцать девятнадцать".Split();
        string[] nums_20_90 = "ноль десять двадцать тридцать сорок пятьдесят шестьдесят семьдесят восемьдесят девяносто".Split();
        string[] nums_100_900 = "ноль сто двести триста четыреста пятьсот шестьсот семьсот восемьсот девятьсот".Split();
        string[] razrad = @" тысяч миллион миллиард триллион квадриллион квинтиллион секстиллион септиллион октиллион нониллион дециллион андециллион дуодециллион тредециллион кваттордециллион квиндециллион сексдециллион септемдециллион октодециллион новемдециллион вигинтиллион анвигинтиллион дуовигинтиллион тревигинтиллион кватторвигинтиллион квинвигинтиллион сексвигинтиллион септемвигинтиллион октовигинтиллион новемвигинтиллион тригинтиллион антригинтиллион".Split();

        Random rnd = null;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            rnd = new Random(DateTime.Now.Millisecond);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SecondActivity);
            names = new List<Element>();
            for (int i = 0; i < 3000; i++)
                names.Add(GetNext());

            Slist = FindViewById<ListView>(Resource.Id.LVSec);
            AdapterForSecond = new Adapter1(this, names.ToList());
            Slist.Adapter = AdapterForSecond;
            List<Element> Full = AdapterForSecond.Numbers.ToList();

            Worker = new System.Threading.Thread(() => {
                for (int i = 0; i < 997000; i++)
                {
                    Full.Add(GetNext());
                }
                RunOnUiThread(()=>{
                    int i = Slist.SelectedItemPosition;
                    AdapterForSecond.Numbers = Full;
                    Slist.SetSelection(i);
                    AdapterForSecond.NotifyDataSetChanged();
                });
                GC.Collect();
            });
            Worker.Start();
        }


        


        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.VolumeDown && Slist.LastVisiblePosition + 20000 < 999999 && !Worker.IsAlive)
            {
                Slist.SetSelection(Slist.LastVisiblePosition + 20000);
            }

            if (keyCode == Keycode.VolumeUp && Slist.FirstVisiblePosition - 20000 > 0)
            {
                Slist.SetSelection(Slist.FirstVisiblePosition - 20000);
            }

            if (keyCode == Keycode.Back)
            {
                if (Worker.IsAlive)
                    Worker.Abort();
                Finish();
            }
                
            return true;
        }


        public Element GetNext()
        {
            string current = "";

            foreach (var s in solve(splitIntoCategories((Front+1).ToString())))
                current += s;

            int k = rnd.Next(4);
            int img = 0;
            if (k == 0)
                img = Resource.Drawable.carrot;
            if (k == 1)
                img = Resource.Drawable.cake;
            if (k == 2)
                img = Resource.Drawable.dino;
            if (k == 3)
                img = Resource.Drawable.humster;


            Element Returnable = null;
            if (Front % 2 != 0)
            {
                Returnable = new Element(current, Android.Graphics.Color.Rgb(204, 204, 204), img);
            }
            else
            {
                Returnable = new Element(current, Android.Graphics.Color.Rgb(255, 255, 255), img);
            }
            Front--;
            return Returnable;
        }
        



        //разбить на разряды
        IEnumerable<string> splitIntoCategories(string s)
        {
            s = s.PadLeft(s.Length + 3 - s.Length % 3, '0');
            return Enumerable.Range(0, s.Length / 3).Select(i => s.Substring(i * 3, 3));
        }
        //вывести название цифр в разряде
        IEnumerable<string> solve(IEnumerable<string> n)
        {
            var ii = 0;
            foreach (var s in n)
            {
                var countdown = n.Count() - ++ii;
                yield return
                    String.Format(@"{0} {1} {2} {3}",
                        s[0] == '0' ? "" : nums_100_900[getDigit(s[0])],
                        getE1(s[1], s[2]),
                        getE2(s[1], s[2], countdown),
                        s == "000" ? "" : getRankName(s, countdown)
                    );
            }

        }
        //вторая цифра разряда
        public string getE1(char p1, char p2)
        {
            if (p1 != '0')
            {
                if (p1 == '1')
                    return nums_10_19[getDigit(p2)];
                return nums_20_90[getDigit(p1)];
            }
            return "";
        }
        //третья цифра разряда
        public string getE2(char p1, char p2, int cd)
        {
            if (p1 != '1')
            {
                if (p2 == '0') return "";
                return (p2 == '2' && cd == 1) ? "две" : nums_1_9[getDigit(p2)];
            }
            return "";
        }

        public int getDigit(char p1)
        {
            return Int32.Parse(p1.ToString());
        }
        //вывести название разрядов
        public string getRankName(string s, int ii)
        {
            if (ii == 0) return " ";
            var r = razrad[ii];
            //10 11 ...
            if (s[1] == '1') return r + (ii == 1 ? " " : "ов");

            if (new[] { '2', '3', '4' }.Contains(s[2]))
            {
                return r + (ii == 1 ? "и" : "а");
            }
            else
                return r + (ii == 1 ? " " : "ов");
        }







    }
}