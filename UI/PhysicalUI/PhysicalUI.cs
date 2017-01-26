
using UnityEngine;
using System.Collections;

public class PhysicalUI : MonoBehaviour
{
    [SerializeField] protected Transform UIElement;
    [SerializeField] protected Color elementColor;
    public new Transform transform
    {
        get
        {
            return UIElement;
        }
        set
        {
            UIElement = value;
        }
    }

    public Color color
    {
        get
        {
            return elementColor;
        }
        set
        {
            elementColor = value;
        }
    }

    public delegate void PhysicalUIEventHandler(object sender, EventArgs e);
    
	public class EventArgs
	{
		public object a;
		public object b;
		public object c;

	}

}
