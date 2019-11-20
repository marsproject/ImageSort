using System;

namespace ImageSort
{
    public class SortingMethodAttribute : Attribute
    {
           public string Name { get; set; }

           public SortingMethodAttribute( string name ) => Name = name;
    }
}