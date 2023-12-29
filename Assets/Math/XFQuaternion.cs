using System;
using UnityEngine;
using XFixMath.NET;

public struct XFQuaternion {	
	//
	// Fields
	//
	public XFix64 x;
	
	public XFix64 y;
	
	public XFix64 z;
	
	public XFix64 w;

	//
	// Static Properties
	//
	public static XFQuaternion identity
	{
		get
		{
			return new XFQuaternion (0f, 0f, 0f, 1f);
		}
	}

	//
	// Properties
	//
	public Vector3 eulerAngles
	{
		get
		{
			return XFQuaternion.ToEuler (this);
		}
		set
		{
			this = XFQuaternion.Euler(value);
		}
	}

	//
	// Indexer
	//
	public XFix64 this [int index]
	{
		get
		{
			switch (index)
			{
			case 0:
				return this.x;
			case 1:
				return this.y;
			case 2:
				return this.z;
			case 3:
				return this.w;
			default:
				throw new IndexOutOfRangeException ("Invalid Quaternion index!");
			}
		}
		set
		{
			switch (index)
			{
			case 0:
				this.x = value;
				break;
			case 1:
				this.y = value;
				break;
			case 2:
				this.z = value;
				break;
			case 3:
				this.w = value;
				break;
			default:
				throw new IndexOutOfRangeException ("Invalid Quaternion index!");
			}
		}
	}

	//
	// Constructors
	//
	public XFQuaternion (XFix64 x, XFix64 y, XFix64 z, XFix64 w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public XFQuaternion (Quaternion q)
	{
		this.x = q.x;
		this.y = q.y;
		this.z = q.z;
		this.w = q.w;
	}

	//
	// Static Methods
	//

	public static XFQuaternion Normalize(XFQuaternion a){
		XFix64 m = XFQuaternion.Dot (a, a);

		if (m > 0) {
			m = 1/ Mathf.Sqrt(m);
			return new XFQuaternion(a.x * m, a.y * m,a.z * m,a.w * m);
		}
		return a;
	}

	public static XFix64 Angle (XFQuaternion a, XFQuaternion b)
	{
		XFix64 f = XFQuaternion.Dot (a, b);
		return Mathf.Acos (Mathf.Min (Mathf.Abs (f), 1f)) * 2f * 57.29578f;
	}

	public static XFQuaternion AngleAxis (XFix64 angle, Vector3 axis)
	{
		Vector3 normAxis = axis.normalized;
		angle = angle * 0.0174532924f * 0.5f; 
		XFix64 s = Mathf.Sin (angle); 
		XFix64 w = Mathf.Cos (angle);
		XFix64 x = normAxis.x * s;
		XFix64 y = normAxis.y * s;
		XFix64 z = normAxis.z * s;
		return new XFQuaternion (x, y, z, w);
	}

	public static XFix64 Dot (XFQuaternion a, XFQuaternion b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	}

	public static XFQuaternion Euler (Vector3 euler)
	{
		euler = euler * 0.0174532924f * 0.5f;

		XFix64 sinX = Mathf.Sin (euler.x);
		XFix64 cosX = Mathf.Cos (euler.x);
		XFix64 sinY = Mathf.Sin (euler.y);
		XFix64 cosY = Mathf.Cos (euler.y);
		XFix64 sinZ = Mathf.Sin (euler.z);
		XFix64 cosZ = Mathf.Cos (euler.z);

		XFQuaternion quat = XFQuaternion.identity;

		quat.w = cosY * cosX * cosZ + sinY * sinX * sinZ;
		quat.x = cosY * sinX * cosZ + sinY * cosX * sinZ;
		quat.y = sinY * cosX * cosZ - cosY * sinX * sinZ;
		quat.z = cosY * cosX * sinZ - sinY * sinX * cosZ;

		return quat;
	}

	public static XFQuaternion Euler (XFix64 x, XFix64 y, XFix64 z)
	{
		return XFQuaternion.Euler (new Vector3 (x, y, z));
	}

	public static void SanitizeEnler(Vector3 v){
		if (v.x < 0) {
			v.x += XFix64.PiTimes2;
		} else if (v.x > XFix64.PiTimes2) {
			v.x -= XFix64.PiTimes2;
		}

		if (v.y < 0) {
			v.y += XFix64.PiTimes2;
		} else if (v.y > XFix64.PiTimes2) {
			v.y -= XFix64.PiTimes2;
		}

		if (v.z < 0) {
			v.z += XFix64.PiTimes2;
		} else if (v.z > XFix64.PiTimes2) {
			v.z -= XFix64.PiTimes2;
		}


	}
	
	public static Vector3 ToEuler(XFQuaternion q){
		XFix64 check = 2 * (q.y * q.z - q.w * q.x);
		if (check < 1) {
			if(check > -1){
				Vector3 v = new Vector3(Mathf.Asin(check) * -1,
				                        Mathf.Atan2(2*(q.x*q.z + q.w*q.y),1 - 2*(q.x*q.x + q.y*q.y)),
				                        Mathf.Atan2(2*(q.x*q.y + q.w*q.z),1 - 2*(q.x*q.x + q.z*q.z)));
				SanitizeEnler(v);
				v = v * 57.29578f;
				return v;
			}else{
				Vector3 v = new Vector3(XFix64.PiOver2,
				                        Mathf.Atan2(2*(q.x*q.y - q.w*q.z),1 - 2*(q.y*q.y + q.z*q.z)),
				                        0);
				SanitizeEnler(v);
				v = v * 57.29578f;
				return v;
			}
		}else{
			Vector3 v = new Vector3(-XFix64.PiOver2,
			                        Mathf.Atan2(-2*(q.x*q.y - q.w*q.z),1 - 2*(q.y*q.y + q.z*q.z)),
			                        0);
			SanitizeEnler(v);
			v = v * 57.29578f;
			return v;
		}
	}

	public static XFQuaternion Matrix3X3ToQuaternion(XFix64[,] rot){
		XFix64 trace = rot [0,0] + rot [1, 1] + rot [2, 2];

		XFQuaternion quat = new XFQuaternion ();
		if (trace > 0) {
			XFix64 s = Mathf.Sqrt (trace + 1);
			quat.w = 0.5f * s;
			s = 0.5f / s;
			quat.x = (rot [2, 1] - rot [1, 2]) * s;
			quat.y = (rot [0, 2] - rot [2, 0]) * s;
			quat.z = (rot [1, 0] - rot [0, 1]) * s;
			return XFQuaternion.Normalize (quat);
		} else {
			int i = 0;
			XFix64[] q = new XFix64[3]{0,0,0};
			int[] _next = new int[3]{1,2,0};

			if(rot[1,1] > rot[0,0]){
				i = 1;
			}
			if(rot[2,2] > rot[i,i]){
				i = 2;
			}

			int j = _next[i];
			int k = _next[j];

			XFix64 t = rot[i,i] - rot[j,j] - rot[k,k] + 1;
			XFix64 s = 0.5f / Mathf.Sqrt(t);
			q[i] = s * t;

			XFix64 w = (rot[k,j] - rot[j,k]) * s;
			q[j] = (rot[j,i] + rot[i,j]) * s;
			q[k] = (rot[k,i] + rot[i,k]) * s;

			quat.Set(q[0],q[1],q[2],w);
			return XFQuaternion.Normalize(quat);

		}
	}

	public static XFQuaternion FromToRotation (Vector3 fromDirection, Vector3 toDirection)
	{
		Vector3 from = fromDirection.normalized;
		Vector3 to = toDirection.normalized;
		XFix64 e = Vector3.Dot (from, to);

		if (e > XFix64.One) {
			return XFQuaternion.identity;
		}
		else if (e < -XFix64.One) {
			XFix64[] left = new XFix64[3]{0,from.z,from.y};
			XFix64 mag = left[1] * left[1] + left[2] * left[2]; // + left[0] * left[0]
			if(mag.Equals(XFix64.Zero)){
				left[0] = -from.z;
				left[1] = 0;
				left[2] = from.x;
				mag = left[0] * left[0] + left[2] * left[2];
			}

            XFix64.Sqrt(mag, out var sqrt);
			XFix64 invlen = XFix64.One / sqrt;
			left[0] = left[0] * invlen;
			left[1] = left[1] * invlen;
			left[2] = left[2] * invlen;

			XFix64[] up = new XFix64[] { XFix64.Zero, XFix64.Zero, XFix64.Zero };
			up[0] = left[1] * from.z - left[2] * from.y;
			up[1] = left[2] * from.x - left[0] * from.z;
			up[2] = left[0] * from.y - left[1] * from.x;

			XFix64 fxx = -from.x * from.x;
			XFix64 fyy = -from.y * from.y;
			XFix64 fzz = -from.z * from.z;
					
			XFix64 fxy = -from.x * from.y;
			XFix64 fxz = -from.x * from.z;
			XFix64 fyz = -from.y * from.z;
					
			XFix64 uxx = up[0] * up[0];
			XFix64 uyy = up[1] * up[1];
			XFix64 uzz = up[2] * up[2];
			XFix64 uxy = up[0] * up[1];
			XFix64 uxz = up[0] * up[2];
			XFix64 uyz = up[1] * up[2];
					
			XFix64 lxx = -left[0] * left[0];
			XFix64 lyy = -left[1] * left[1];
			XFix64 lzz = -left[2] * left[2];
			XFix64 lxy = -left[0] * left[1];
			XFix64 lxz = -left[0] * left[2];
			XFix64 lyz = -left[1] * left[2];

			XFix64[,] rot = new XFix64[3,3]{
				{fxx + uxx + lxx, fxy + uxy + lxy, fxz + uxz + lxz},
				{fxy + uxy + lxy, fyy + uyy + lyy, fyz + uyz + lyz},
				{fxz + uxz + lxz, fyz + uyz + lyz, fzz + uzz + lzz}};

			return XFQuaternion.Matrix3X3ToQuaternion(rot);
		}else {
			Vector3 v = Vector3.Cross(from, to);
			XFix64 h = (1 - e) / Vector3.Dot(v, v); 
					
			XFix64 hx = h * v.x;
			XFix64 hz = h * v.z;
			XFix64 hxy = hx * v.y;
			XFix64 hxz = hx * v.z;
			XFix64 hyz = hz * v.y;
					
			XFix64[,] rot = new XFix64[3,3]{
				{e + hx*v.x, 	hxy - v.z, 		hxz + v.y},
				{hxy + v.z,  	e + h*v.y*v.y, 	hyz-v.x},
				{hxz - v.y,  	hyz + v.x,    	e + hz*v.z}};
			
			return XFQuaternion.Matrix3X3ToQuaternion(rot);
		}
	}

	public static XFQuaternion Inverse (XFQuaternion rotation)
	{
		return new XFQuaternion (-rotation.x, -rotation.y, -rotation.z, rotation.w);
	}

	public static XFQuaternion Lerp (XFQuaternion from, XFQuaternion to, XFix64 t)
	{
		XFQuaternion tmpQuat = XFQuaternion.identity;
		if (XFQuaternion.Dot (from, to) < 0) {
			tmpQuat.Set (from.x + t * (-to.x - from.x),
			            from.y + t * (-to.y - from.y),
			            from.z + t * (-to.z - from.z),
			            from.w + t * (-to.w - from.w));
		} else {
			tmpQuat.Set (from.x + t * (to.x - from.x),
			             from.y + t * (to.y - from.y),
			             from.z + t * (to.z - from.z),
			             from.w + t * (to.w - from.w));
		}
		return tmpQuat;
	}

	public static XFQuaternion LookRotation (Vector3 forward)
	{
		Vector3 up = Vector3.up;
		return XFQuaternion.LookRotation (forward, up);
	}

	//
	public static XFQuaternion LookRotation (Vector3 forward, Vector3 upwards)
	{
		XFix64 mag = forward.magnitude;
		if (mag == XFix64.Zero) {
			//XLog.LogError("XFQuaternion LookRotation Error...");
			return XFQuaternion.identity;
		}

		forward = forward / mag;
		Vector3 right = Vector3.Cross (upwards, forward);
		right.Normalize ();
		upwards = Vector3.Cross (forward, right);
		right = Vector3.Cross (upwards, forward);

		XFix64 t = right.x + upwards.y + forward.z;

		if (t > 0) {
			t += 1;
			XFix64 s = 0.5f / Mathf.Sqrt (t);
			XFix64 w = s * t;
			XFix64 x = (upwards.z - forward.y) * s;
			XFix64 y = (forward.x - right.z) * s;
			XFix64 z = (right.y - upwards.x) * s;

			XFQuaternion q = new XFQuaternion (x, y, z, w);
			return XFQuaternion.Normalize (q);
		} else {
			XFix64[,] rot = new XFix64[3,3]{
				{right.x,upwards.x,forward.x},
				{right.y,upwards.y,forward.y},
				{right.z,upwards.z,forward.z}};

			int[] _next = new int[3]{1,2,0};

			XFix64[] q = new XFix64[3]{0,0,0};
			int i = 0;

			if(upwards.y > right.x){
				i = 1;
			}
			if(forward.z > rot[i,i]){
				i = 2;
			}

			int j = _next[i];
			int k = _next[j];

			XFix64 t2 = rot[i,i] - rot[j,j] - rot[k,k] + 1;

			XFix64 s = 0.5f / Mathf.Sqrt(t2);
			q[i] = s * t;
			XFix64 w = (rot[k,j] - rot[j,k]) * s;
			q[j] = (rot[j,i] + rot[i,j]) * s;
			q[k] = (rot[k,i] + rot[i,k]) * s;

			XFQuaternion quat = new XFQuaternion (q[0], q[1], q[2], w);
			return XFQuaternion.Normalize (quat);

		}

	}
	
	public static XFQuaternion RotateTowards (XFQuaternion from, XFQuaternion to, XFix64 maxDegreesDelta)
	{
		XFix64 num = XFQuaternion.Angle (from, to);
		if (num == 0f)
		{
			return to;
		}
		XFix64 t = Mathf.Min (1f, maxDegreesDelta / num);
		return XFQuaternion.UnclampedSlerp (from, to, t);
	}
	
	public static XFQuaternion Slerp (XFQuaternion from, XFQuaternion to, XFix64 t)
	{
		t = Mathf.Clamp (t, 0, 1);
		return XFQuaternion.UnclampedSlerp (from, to, t);
	}

	public static XFQuaternion UnclampedSlerp(XFQuaternion from,XFQuaternion to,XFix64 t){
		XFix64 cosAngle = XFQuaternion.Dot (from, to);

		if (cosAngle < 0) {
			cosAngle = -cosAngle;
			to = new XFQuaternion(-to.x,-to.y,-to.z,-to.w);
		}

		if (cosAngle < 0.95f) {
			XFix64 angle = Mathf.Acos (cosAngle);
			XFix64 sinAngle = Mathf.Sin (angle);
			XFix64 invSinAngle = 1 / sinAngle;

			XFix64 t1 = Mathf.Sin ((1 - t) * angle) * invSinAngle;
			XFix64 t2 = Mathf.Sin (t * angle) * invSinAngle;

			return new XFQuaternion (from.x * t1 + to.x * t2, from.y * t1 + to.y * t2, from.z * t1 + to.z * t2, from.w * t1 + to.w * t2);
		} else {
			return XFQuaternion.Lerp(from,to,t);
		}

	}

	//
	// Methods
	//

	public Quaternion ToUnityQuaternion(){
		return new Quaternion (x, y, z, w);
	}

	public override bool Equals (object other)
	{
		if (!(other is XFQuaternion))
		{
			return false;
		}
		XFQuaternion quaternion = (XFQuaternion)other;
		return this.x.Equals (quaternion.x) && this.y.Equals (quaternion.y) && this.z.Equals (quaternion.z) && this.w.Equals (quaternion.w);
	}

	public override int GetHashCode ()
	{
		return this.x.GetHashCode () ^ this.y.GetHashCode () << 2 ^ this.z.GetHashCode () >> 2 ^ this.w.GetHashCode () >> 1;
	}

	public void Set (XFix64 new_x, XFix64 new_y, XFix64 new_z, XFix64 new_w)
	{
		this.x = new_x;
		this.y = new_y;
		this.z = new_z;
		this.w = new_w;
	}

	public void SetFromToRotation (Vector3 fromDirection, Vector3 toDirection)
	{
		this = XFQuaternion.FromToRotation (fromDirection, toDirection);
	}

	public void SetLookRotation (Vector3 view)
	{
		Vector3 up = Vector3.up;
		this.SetLookRotation (view, up);
	}
	
	public void SetLookRotation (Vector3 view, Vector3 up)
	{
		this = XFQuaternion.LookRotation (view, up);
	}
	
	public void ToAngleAxis (out XFix64 angle, out Vector3 axis)
	{
		angle = Mathf.Acos (w) * 2;
		XFix64.Abs(angle, out var abs);

        if (abs.Equals(XFix64.Zero)) {
			axis = new Vector3 (1, 0, 0);
		} else {
			XFix64.Sqrt(1 - w * w, out var sqrt);
            XFix64 div = XFix64.One/sqrt;
			axis = new Vector3(x*div,y*div,z*div);//??
		}
		//??
		angle *= 57.29578f;
	}

	public override string ToString ()
	{
		return string.Format ("XFQuaternion ({0:F1}, {1:F1}, {2:F1}, {3:F1})", new object[]
		                           {
			this.x,
			this.y,
			this.z,
			this.w
		});
	}

	//
	// Operators
	//
	public static bool operator == (XFQuaternion lhs, XFQuaternion rhs)
	{
		return XFQuaternion.Dot (lhs, rhs) > 0.999999f;
	}
	
	public static bool operator != (XFQuaternion lhs, XFQuaternion rhs)
	{
		return XFQuaternion.Dot (lhs, rhs) <= 0.999999f;
	}
	
	public static XFQuaternion operator * (XFQuaternion lhs, XFQuaternion rhs)
	{
		return new XFQuaternion (lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
	}
	
	public static Vector3 operator * (XFQuaternion rotation, Vector3 point)
	{
		XFix64 num = rotation.x * 2f;
		XFix64 num2 = rotation.y * 2f;
		XFix64 num3 = rotation.z * 2f;
		XFix64 num4 = rotation.x * num;
		XFix64 num5 = rotation.y * num2;
		XFix64 num6 = rotation.z * num3;
		XFix64 num7 = rotation.x * num2;
		XFix64 num8 = rotation.x * num3;
		XFix64 num9 = rotation.y * num3;
		XFix64 num10 = rotation.w * num;
		XFix64 num11 = rotation.w * num2;
		XFix64 num12 = rotation.w * num3;
		Vector3 result;
		result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
		result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
		result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
		return result;
	}
}
