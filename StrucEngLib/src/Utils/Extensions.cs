using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Eto.Forms;

namespace StrucEngLib.Utils
{
    /// <summary>Utility class with class extensions</summary>
    public static class Extensions
    {
        
        public static IList<T> OrEmptyIfNull<T>(this IList<T> source)
        {
            return source ?? Enumerable.Empty<T>().ToList();
        }
        
        public static IndirectBinding<string> PropertyWithIntConvert<MODEL>(
            Expression<Func<MODEL, int>> f, int defVal = 0)
        {
            return Binding.Property(f).Convert(
                v => v.ToString(),
                s =>
                {
                    int i = defVal;
                    return int.TryParse(s, out i) ? i : defVal;
                });
        }

        public static IndirectBinding<string> PropertyWithDoubleConvert<MODEL>(
            Expression<Func<MODEL, double>> f, double defVal = 0)
        {
            return Binding.Property(f).Convert(
                v => v.ToString(),
                s =>
                {
                    double i = defVal;
                    return double.TryParse(s, out i) ? i : defVal;
                });
        }
    }
   
}