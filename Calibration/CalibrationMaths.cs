using UnityEngine;
using System.Collections;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
/// <summary>
/// class that handles the actual computation
/// </summary>
public class CalibrationMaths
{

    //private int degree = 5;
    public List<Vector3> result;
    private CalibrationData cd = new CalibrationData();
    private CalibrationMap cm = new CalibrationMap();
    [SerializeField] private int max_iteration = 10000;
    private bool terminal_condition = false;
    private float alpha = 0.1f;
    public void Calibrate()
    {

        //setup correlation arrays
        cd.xs = cd.samples.Select(j => (float)j.x).ToArray<float>();
        cd.ys = cd.samples.Select(j => (float)j.y).ToArray<float>();
        cd.zs = cd.samples.Select(j => (float)j.z).ToArray<float>();
        cd.xr = cd.references.Select(j => (float)j.x).ToArray<float>();
        cd.yr = cd.references.Select(j => (float)j.y).ToArray<float>();
        cd.zr = cd.references.Select(j => (float)j.z).ToArray<float>();
        int iteration = 0;
        int i;
        CalibrationMap.Equation[] grads = new CalibrationMap.Equation[5];
        for (i = 0; i < 3; i++)
        {
            grads[i].SetZero();
        }
        float error;
        while (iteration++ < max_iteration && terminal_condition)
        {
            CalculateGradient(cm.equations, ref grads);
            for (i = 0; i < 5; i++)
            {
                cm.equation_x.e[i] -= alpha * grads[0].e[i];
                cm.equation_y.e[i] -= alpha * grads[1].e[i];
                cm.equation_z.e[i] -= alpha * grads[2].e[i];
            }
            error = CalculateError().magnitude;
            if(error < 0.1)
            {
                terminal_condition = true;
            }
            UnityEngine.Debug.Log("Error : " + error + "@ iter : " + iteration);

        }
    }
    public void Evaluate(List<Vector3> _samples, List<Vector3> _references)
    {

    }

    public Vector3 CalculateError()
    {
        Vector3 error = Vector3.zero;
        for (int i = 0; i < cd.samples.Count; i++)
        {
            error.x += Mathf.Abs(cd.samples[i].x - cd.references[i].x);
            error.y += Mathf.Abs(cd.samples[i].y - cd.references[i].y);
            error.z += Mathf.Abs(cd.samples[i].z - cd.references[i].z);
        }
        return error;
    }

    public Vector3 CalculateError(List<int> sample_index)
    {
        Vector3 error = Vector3.zero;
        for (int i = 0; i < sample_index.Count; i++)
        {
            error.x += Mathf.Abs(cd.samples[sample_index[i]].x - cd.references[sample_index[i]].x);
            error.y += Mathf.Abs(cd.samples[sample_index[i]].y - cd.references[sample_index[i]].y);
            error.z += Mathf.Abs(cd.samples[sample_index[i]].z - cd.references[sample_index[i]].z);
        }
        return error;
    }

    public void CalculateGradient(CalibrationMap.Equation[] equ, ref CalibrationMap.Equation[] grads, List<int> sample_index)
    {
        for (int i = 0; i < 3; i++)
        {
            grads[i].SetZero();
        }
        // equation real_x = ax2 + bx + cy + dz + i
        int sampleCount = sample_index.Count();
        for (int i = 0; i < sampleCount; i++)
        {
            float x = cd.samples[sample_index[i]].x, y = cd.samples[sample_index[i]].y, z = cd.samples[sample_index[i]].z;
            float error_x_root = cd.references[sample_index[i]].x - (equ[0].e[0] * (x * x) + equ[0].e[1] * x + equ[0].e[2] * y + equ[0].e[3] * z + equ[0].e[4]);
            float error_y_root = cd.references[sample_index[i]].y - (equ[1].e[0] * (y * y) + equ[1].e[1] * y + equ[1].e[2] * z + equ[1].e[3] * x + equ[1].e[4]);
            float error_z_root = cd.references[sample_index[i]].z - (equ[2].e[0] * (z * z) + equ[2].e[1] * z + equ[2].e[2] * x + equ[2].e[3] * y + equ[2].e[4]);
            grads[0].e[0] += error_x_root * (-x * x) * 2;
            grads[0].e[1] += error_x_root * (-x) * 2;
            grads[0].e[2] += error_x_root * (-y) * 2;
            grads[0].e[3] += error_x_root * (-z) * 2;
            grads[0].e[4] += error_x_root;
            grads[1].e[0] += error_y_root * (-y * y) * 2;
            grads[1].e[1] += error_y_root * (-y) * 2;
            grads[1].e[2] += error_y_root * (-z) * 2;
            grads[1].e[3] += error_y_root * (-x) * 2;
            grads[1].e[4] += error_y_root;
            grads[2].e[0] += error_z_root * (-z * z) * 2;
            grads[2].e[1] += error_z_root * (-z) * 2;
            grads[2].e[2] += error_z_root * (-x) * 2;
            grads[2].e[3] += error_z_root * (-y) * 2;
            grads[2].e[4] += error_z_root;
        }
    }

    public void CalculateGradient(CalibrationMap.Equation[] equ, ref CalibrationMap.Equation[] grads)
    {
        for (int i = 0; i < 3; i++)
        {
            grads[i].SetZero();
        }
        // equation real_x = ax2 + bx + cy + dz + i
        int sampleCount = cd.samples.Count();
        for (int i = 0; i < sampleCount; i++)
        {
            float x = cd.samples[i].x, y = cd.samples[i].y, z = cd.samples[i].z;
            float error_x_root = cd.references[i].x - (equ[0].e[0] * (x * x) + equ[0].e[1] * x + equ[0].e[2] * y + equ[0].e[3] * z + equ[0].e[4]);
            float error_y_root = cd.references[i].y - (equ[1].e[0] * (y * y) + equ[1].e[1] * y + equ[1].e[2] * z + equ[1].e[3] * x + equ[1].e[4]);
            float error_z_root = cd.references[i].z - (equ[2].e[0] * (z * z) + equ[2].e[1] * z + equ[2].e[2] * x + equ[2].e[3] * y + equ[2].e[4]);
            grads[0].e[0] += error_x_root * (-x * x) * 2;
            grads[0].e[1] += error_x_root * (-x) * 2;
            grads[0].e[2] += error_x_root * (-y) * 2;
            grads[0].e[3] += error_x_root * (-z) * 2;
            grads[0].e[4] += error_x_root;
            grads[1].e[0] += error_y_root * (-y * y) * 2;
            grads[1].e[1] += error_y_root * (-y) * 2;
            grads[1].e[2] += error_y_root * (-z) * 2;
            grads[1].e[3] += error_y_root * (-x) * 2;
            grads[1].e[4] += error_y_root;
            grads[2].e[0] += error_z_root * (-z * z) * 2;
            grads[2].e[1] += error_z_root * (-z) * 2;
            grads[2].e[2] += error_z_root * (-x) * 2;
            grads[2].e[3] += error_z_root * (-y) * 2;
            grads[2].e[4] += error_z_root;
        }
    }

    public void AddSample(Vector3 sample, Vector3 reference)
    {
        cd.samples.Add(sample);
        cd.references.Add(reference);
    }



    public void SetZeroPoint(Vector3 zp)
    {
        cd.ZeroPoint = zp;
    }

}
