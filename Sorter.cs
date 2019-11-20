using System;
using System.Linq;
using System.Reflection;

namespace ImageSort
{
    public class Sorter
    {
        private readonly Pixel[,] _pixels;
        private readonly int _width;
        private readonly int _height;

        public Sorter( Pixel[,] pixels )
        {
            _pixels = pixels;
            _width = _pixels.GetLength( 0 );
            _height = _pixels.GetLength( 1 );
        }

        public Pixel[,] SimpleSort()
        {
            Pixel[,] result = new Pixel[_width, _height];

            for (int u = 0; u < _width; u++)
            {
                Pixel[] column = new Pixel[_height];

                for (int v = 0; v < _height; v++)
                {
                    column[v] = _pixels[u, v];
                }

                column = column.OrderBy(p => p.Absolute).ToArray();
                for (int i = 0; i < column.Length; i++)
                    result[u, i] = column[i];
            }

            return result;
        }

        public Pixel[,] Sort( Func<Pixel, int> selector )
        {
            Pixel[,] result = new Pixel[_width, _height];

            for( int u = 0; u < _width; u++ )
            {
                Pixel[] column = new Pixel[_height];

                for( int v = 0; v < _height; v++ )
                {
                    column[v] = _pixels[u, v];
                }

                column = column.OrderBy( p => selector( p ) ).ToArray();
                for( int i = 0; i < column.Length; i++ )
                    result[u, i] = column[i];
            }

            return result;
        }

        public Pixel[,] Sort(MethodInfo field)
        {
            Pixel[,] result = new Pixel[_width, _height];

            for (int u = 0; u < _width; u++)
            {
                Pixel[] column = new Pixel[_height];

                for (int v = 0; v < _height; v++)
                {
                    column[v] = _pixels[u, v];
                }

                column = column.OrderBy(p => ( int )field.Invoke( p, null ) ).ToArray();
                for (int i = 0; i < column.Length; i++)
                    result[u, i] = column[i];
            }

            return result;
        }
    }
}