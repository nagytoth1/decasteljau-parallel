using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _GraphicsDLL
{
    public class HermiteArc
    {
        public Vector2 p0, p1, t0, t1;
        public int[,] h_matrix;
        public float weight;

        //majd kiegészíteni a házi alapján
        public HermiteArc() : this(
            p0: new Vector2(0f, 0f), p1: new Vector2(0f, 0f), 
            t0: new Vector2(0f, 0f), t1: new Vector2(0f, 0f))
        {
        }

        //mivel struct, ezért átadhatom simán érték szerint őket
        public HermiteArc(Vector2 p0, Vector2 p1, Vector2 t0, Vector2 t1) : this(p0, p1, t0, t1, 0f, 
            new int[,]{
            { 2, -2, 1, 1   },
            { -3, 3, -2, -1  },
            { 0, 0, 1, 0    },
            { 1, 0, 0, 0    }}){ }

        //Megcsinálni a házi alapján
        public HermiteArc(Vector2 p0, Vector2 p1, Vector2 t0, Vector2 t1, float weight, int[,] matrix)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.t0 = t0;
            this.t1 = t1;
            this.weight = weight;
            this.h_matrix = matrix; //átadhatom érték szerint, mivel tömbről van szó
        }

        //házi: tetszőlegesen változtatható H-mátrix, tehát lehessen átadni paraméterben
        public double H_Calculate(float t, int column)
        {
            double sum = 0;
            int rows = h_matrix.GetLength(0);
            for (int i = 0; i < rows; i++)
            {
                if (h_matrix[i, column] == 0)
                    continue; //felesleges számítani, nullát adunk hozzá az összeghez
                sum += h_matrix[i, column] * Math.Pow(t, rows - i - 1); //t hatványai úgy csökkennek, ahogy a sorindexek növekednek
            }
            return sum;
        }

        /*
        public static double H0(float t)
        {
            return 2 * Math.Pow(t, 3) - 3 * Math.Pow(t, 2) + 1;
        }
        public static double H1(float t)
        {
            return -2 * Math.Pow(t, 3) + 3 * Math.Pow(t, 2);
        }
        public static double H2(float t)
        {
            return Math.Pow(t, 3) - 2 * Math.Pow(t, 2) + t;
        }
        public static double H3(float t)
        {
            return Math.Pow(t, 3) - 1 * Math.Pow(t, 2);
        }*/
    }
}
