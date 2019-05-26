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

namespace Lab1
{
    class Element
    {
        public string Text { get; set; }
        public Android.Graphics.Color CurrentColor { get; set; }
        public int CBitmap { get; set; }

        public Element(string Text, Android.Graphics.Color color, int bmp)
        {
            this.Text = Text;
            CurrentColor = color;
            CBitmap = bmp;
        }
    }
}