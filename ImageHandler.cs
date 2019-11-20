using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageSort
{
    public class ImageHandler
    {
        public Pixel[,] LoadImage( string imagePath )
        {
            Pixel[,] result;

            using ( Bitmap bmp = ( Bitmap )Image.FromFile( imagePath ) )
            {
                 result = new Pixel[bmp.Width, bmp.Height];

                var data = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadOnly, bmp.PixelFormat );
                var ptr = data.Scan0;

                for( int i = 0; i < bmp.Width * bmp.Height; i++ )
                {
                    //Console.WriteLine( $"x: {i % bmp.Width}, y: {i / bmp.Width}" );

                    if ( bmp.PixelFormat == PixelFormat.Format24bppRgb )
                    {
                        byte b = Marshal.ReadByte( ptr );
                        ptr = IntPtr.Add( ptr, sizeof( byte ) );
                        byte g = Marshal.ReadByte( ptr );
                        ptr = IntPtr.Add( ptr, sizeof( byte ) );
                        byte r = Marshal.ReadByte( ptr );
                        ptr = IntPtr.Add( ptr, sizeof( byte ) );

                        result[i % bmp.Width, i / bmp.Width] = new Pixel(r, g, b);
                    }
                    else
                    {
                        int val = Marshal.ReadInt32( ptr );
                        ptr = IntPtr.Add( ptr, sizeof( int ) );

                        result[ i % bmp.Width, i / bmp.Width ] = readRgb( val, bmp.PixelFormat );
                    }
                    
                }
                
            }

            return result;
        }

        public void WriteImage( Pixel[,] pixels, string imagePath )
        {
            using( Bitmap bmp = new Bitmap( pixels.GetLength( 0 ), pixels.GetLength( 1 ) ) )
            {
                for( int u = 0; u < bmp.Width; u++ )
                {
                    for( int v = 0; v < bmp.Height; v++ )
                    {
                        Pixel p = pixels[u, v];
                        bmp.SetPixel( u, v, System.Drawing.Color.FromArgb( p.R, p.G, p.B ) );
                    }
                }

                bmp.Save( imagePath );
            }
        }

        private Pixel readRgb( int val, PixelFormat format )
        {
            Pixel result = new Pixel();

            switch( format )
            {
                case PixelFormat.Format24bppRgb:
                    return new Pixel(
                        ( val & 0xFF0000 ) >> 16,
                        ( val & 0x00FF00 ) >> 8,
                        ( val & 0x0000FF )
                    );
                case PixelFormat.Format32bppArgb:
                    return new Pixel(
                        ( val & 0x00FF0000 ) >> 16,
                        ( val & 0x0000FF00 ) >> 8,
                        val & 0x000000FF
                    );

                default:
                    throw new Exception( $"Format {format.ToString()} not implemented." );
            }
        }
    }
}