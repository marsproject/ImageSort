using System;
using System.IO;
using System.Linq;

namespace ImageSort
{
    class Program
    {
        static void Main(string[] args)
        {
            while( true )
            {
                writeLine( "=== ImageSorter 0.2 ===", ConsoleColor.Green );
                writeLine( "\tby Marc Dorok" );

                var files = Directory.EnumerateFiles( Path.Combine( Environment.CurrentDirectory, "images" ) ).ToList();
                writeLine( String.Empty );
                writeLine( "Choose file: ", ConsoleColor.White );
                for ( int i = 0; i < files.Count(); i++ )
                {
                    Console.WriteLine( $"{i + 1}) {Path.GetFileName( files[i] )}" );
                }

                int n = -1;
                while( true )
                {
                    writeLine(String.Empty);
                    write("> ", ConsoleColor.Yellow);

                    string input = Console.ReadLine().ToLower();
                    if ( input == "quit" || input == "exit" )
                        Environment.Exit( 0 );
                    
                    if ( !Int32.TryParse( input, out n ) )
                    {
                        writeLine( "Unknown command or index.", ConsoleColor.Red );
                        continue;
                    }
                    if ( n < 1 || n > files.Count )
                    {
                        writeLine( "Unknown index.", ConsoleColor.Red );
                        continue;
                    }

                    break;
                }

                writeLine( String.Empty );

                string imagePath = files[n - 1];

                ImageHandler imageHandler = new ImageHandler();
                var pixels = imageHandler.LoadImage( Path.Combine( Environment.CurrentDirectory, imagePath ) );

                Sorter sorter = new Sorter( pixels );
                Pixel[,] sortedPixels = null;

                var t = pixels[0, 0].GetType();
                var methods = t.GetMethods().Where( f => f.GetCustomAttributes( false ).Any( v => v.GetType() == typeof( SortingMethodAttribute ) ) );

                writeLine( "Choose method:", ConsoleColor.White );

                for (int i = 0; i < methods.Count(); i++)
                {
                    writeLine($"{i + 1}) " +
                    (methods.ElementAt(i).GetCustomAttributes(typeof(SortingMethodAttribute), false).First() as SortingMethodAttribute).Name);
                }

                int m = -1;
                while( true )
                {
                    writeLine(String.Empty);
                    write("> ", ConsoleColor.Yellow);


                    string input = Console.ReadLine().ToLower();
                    if (input == "quit" || input == "exit")
                        Environment.Exit(0);

                    if (!Int32.TryParse(input, out m))
                    {
                        writeLine("Unknown command or index.", ConsoleColor.Red);
                        continue;
                    }
                    if (m < 1 || m > methods.Count())
                    {
                        writeLine("Unknown index.", ConsoleColor.Red);
                        continue;
                    }

                    break;
                }

                var chosenMethod = methods.ElementAt( m - 1 );

                writeLine( String.Empty );
                write( "Processing... ", ConsoleColor.White );

                
                sortedPixels = sorter.Sort( chosenMethod );

                imageHandler.WriteImage( sortedPixels, "result.png" );

                writeLine( "Done!", ConsoleColor.Green );
                writeLine( "Press any key to continue.", ConsoleColor.White );
                Console.ReadKey( true );
                Console.Clear();
            }
        }

        private static void write( string text ) => Console.Write( text );
        private static void write( string text, ConsoleColor color )
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write( text );
            Console.ForegroundColor = oldColor;
        }

        private static void writeLine( string text ) => write( text + Environment.NewLine );
        private static void writeLine( string text, ConsoleColor color ) => write( text + Environment.NewLine, color );
        
    }
}
