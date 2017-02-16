using UnityEngine;

// Based on http://answers.unity3d.com/questions/470876/off-axis-projection-problem.html

public class OffAxisCamera : MonoBehaviour
{
	private Matrix4x4 customProj;

	public float zeroParallaxDistance = 10.0f;
	public float eyeSeparation = 0.08f;

	public enum EyeMode
	{
		Left  = -1,
		Right = 1
	}	
	public EyeMode eye = EyeMode.Left;

	public static Matrix4x4 PerspectiveOffCenter(float left, float right,
	                                             float bottom, float top,
	                                             float near, float far)
	{
		float x = 2.0F * near / (right - left);
		float y = 2.0F * near / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0F * far * near) / (far - near);
		float e = -1.0F;
		
		Matrix4x4 m = new Matrix4x4();
		m[0, 0] = x; m[0, 1] = 0; m[0, 2] = a; m[0, 3] = 0;
		m[1, 0] = 0; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0;
		m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
		m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = e; m[3, 3] = 0;
		
		return m;
	}	

	// Use this for initialization
	Matrix4x4 calcOffAxisMatrix()
	{
		float near = GetComponent<Camera>().nearClipPlane;
		float far = GetComponent<Camera>().farClipPlane;
		float aspect = GetComponent<Camera>().aspect;
		float fov = GetComponent<Camera>().fieldOfView * Mathf.Deg2Rad;

		// Taken from OpenSG
		float left, right, bottom, top;

		float eyeoff = -eyeSeparation * .5f * (float)eye;

		top    = Mathf.Tan(fov / 2.0f) * near; 
		bottom = -top;
		
		/* Calculate left and right clipping planes */

		float gtan = Mathf.Tan(fov / 2.0f) * aspect;  		
		left  = (-gtan + eyeoff / zeroParallaxDistance) * near;
		right = ( gtan + eyeoff / zeroParallaxDistance) * near;

		Matrix4x4 proj = PerspectiveOffCenter(left, right, bottom, top, near, far);

		Vector3 t = new Vector3(eyeoff,0,0);
		Quaternion r = new Quaternion(0,0,0,1);
		Vector3 s = new Vector3(1,1,1);
		Matrix4x4 trans = Matrix4x4.TRS(t, r, s);

		return proj * trans;
	}
	
	// Use this for initialization
	void Start()
	{
		customProj = calcOffAxisMatrix();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		GetComponent<Camera>().projectionMatrix = customProj;
	}
}
