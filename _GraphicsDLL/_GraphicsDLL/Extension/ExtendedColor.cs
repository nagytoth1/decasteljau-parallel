using System.Drawing;

namespace _GraphicsDLL
{
    public static class ExtendedColor
    {
        public static bool Equals2(this Color c1, Color c2)
        {
            return c1.R == c2.R &&
                   c1.G == c2.G &&
                   c1.B == c2.B;
        }
    }
}
