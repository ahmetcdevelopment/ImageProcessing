using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoruntuIsleme.Extensions
{
    public static class BitMapExtensions
    {
        public static void copyTo(this Bitmap bmp, Bitmap hedef)
        {
            if (bmp == null || hedef == null)
                throw new ArgumentNullException();

            if (bmp.Size != hedef.Size)
                throw new ArgumentException("iki bitmap aynı boyutta olmalıdır.");

            using (Graphics g = Graphics.FromImage(hedef))
            {
                g.DrawImageUnscaled(bmp, 0, 0);
            }
        }
    }
}
