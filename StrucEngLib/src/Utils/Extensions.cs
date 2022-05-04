using System;
using System.Linq.Expressions;
using Eto.Forms;

namespace StrucEngLib.Utils
{
    /// <summary></summary>
    public class Extensions
    {
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