using System.Drawing;

namespace ImageSort
{
    public class Pixel
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public int Absolute
        {
            [SortingMethod( "Absolute" )]
            get => R + G + B;
        }

        public int Average
        {
            [SortingMethod( "Average" )]
            get => ( int )( ( R + G + B ) / 3f + .5f );
        }

        public int Luma
        {
            [SortingMethod( "Luma" )]
            get => ( int )( .2126f * R + .7152f * G + .0722f * B + .5f );
        }

        public int Hue
        {
            [SortingMethod( "Hue" )]
            get => ( int )( Color.FromArgb( R, G, B ).GetHue() + .5f );
        }

        public int Saturation
        {
            [SortingMethod( "Saturation" )]
            get => ( int )( Color.FromArgb( R, G, B ).GetSaturation() * 1000 + .5f );
        }

        public int Brightness
        {
            [SortingMethod( "Brightness" )]
            get => ( int )( Color.FromArgb( R, G, B ).GetBrightness() * 1000 + .5f );
        }

        public Pixel() {}

        public Pixel( int r, int g, int b )
        {
            R = r;
            G = g;
            B = b;
        }

        public override string ToString()
        {
            return $"{R.ToString()}, {G.ToString()}, {B.ToString()}";
        }
    }
}