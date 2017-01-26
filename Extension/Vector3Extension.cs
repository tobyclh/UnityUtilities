using UnityEngine;
using System.Collections;

public static class BasisExtension{


    /// <summary>
    /// 
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static Vector3 ChangeVelocity(this Vector3 velocity, Transform from, Transform to)
    {
        Quaternion Q = new Quaternion();
        Q.SetFromToRotation(to.eulerAngles, from.eulerAngles);
        return Q* velocity;
    }

    public static Vector3 ChangeRotation(this Vector3 velocity, Transform from, Transform to)
    {
        return ChangeVelocity(velocity, from,  to);
    }

    public static Vector3 RegularCoordinate(this Vector3 quantity, Transform from)
    {
        Quaternion Q = new Quaternion();
        Q.SetFromToRotation(Vector3.zero, from.eulerAngles);
        return Q * quantity;
    }


}
