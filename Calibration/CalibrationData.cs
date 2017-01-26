using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalibrationData
{
    public List<Vector3> samples;
    public List<Vector3> references;
    public float[] xs, ys, zs, xys, xzs, yzs, xr, yr, zr;
    public Vector3 ZeroPoint;

}

public class CalibrationMap
{
    public class Equation
    {
        public float[] e = { 0, 0, 0, 0, 0};
        public void SetZero()
        {
            for(int i = 0; i < 7; i++)
            {
                e[i] = 0;
            }
        }
    }
    public Vector3 MapTo(Vector3 Sample)
    {
        float x, y, z;
        x = Sample.x;
        y = Sample.y;
        z = Sample.z;
        var xr = x * x * equation_x.e[0] + x * equation_x.e[1] + y * equation_x.e[2] + z * equation_x.e[3] + equation_x.e[4];
        var yr = y * y * equation_y.e[0] + y * equation_y.e[1] + z * equation_y.e[2] + x * equation_y.e[3] + equation_y.e[4];
        var zr = z * z * equation_z.e[0] + z * equation_z.e[1] + x * equation_z.e[2] + y * equation_z.e[3] + equation_z.e[4]; 
        return new Vector3();
    }
    // equation real_x = ax2 + bx + cy + dz + i
    public Equation equation_x = new Equation();
    public Equation equation_y = new Equation();
    public Equation equation_z = new Equation();
    public Equation[] equations
    {
        get
        {
            var e = new Equation[3];
            e[0] = equation_x;
            e[1] = equation_y;
            e[2] = equation_z;
            return e;
        }
    }


}


