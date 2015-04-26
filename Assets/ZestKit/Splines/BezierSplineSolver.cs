using UnityEngine;
using System.Collections.Generic;


namespace Prime31.ZestKit
{
	public class BezierSplineSolver : AbstractSplineSolver
	{
		int _curveCount; //how many bezier curves in this path?


		public BezierSplineSolver( List<Vector3> nodes )
		{
			_nodes = nodes;
			_curveCount = ( _nodes.Count - 1 ) / 3;
		}


		// http://www.gamedev.net/topic/551455-length-of-a-generalized-quadratic-bezier-curve-in-3d/
		protected float quadBezierLength( Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint )
		{
			// ASSERT: all inputs are distinct points.
			var A = new Vector3[2];
			A[0] = controlPoint - startPoint;
			A[1] = startPoint - 2f * controlPoint + endPoint;

			float length;

			if( A[1] != Vector3.zero )
			{
				// Coefficients of f(t) = c*t^2 + b*t + a.
				float c = 4.0f * Vector3.Dot( A[1], A[1] ); // A[1].Dot(A[1]);  // c > 0 to be in this block of code
				float b = 8.0f * Vector3.Dot( A[0], A[1] ); // A[0].Dot(A[1]);
				float a = 4.0f * Vector3.Dot( A[0], A[0] ); // A[0].Dot(A[0]);  // a > 0 by assumption
				float q = 4.0f * a * c - b * b;  // = 16*|Cross(A0,A1)| >= 0

				float twoCpB = 2.0f * c + b;
				float sumCBA = c + b + a;
				float mult0 = 0.25f / c;
				float mult1 = q / ( 8.0f * Mathf.Pow( c, 1.5f ) );
				length = mult0 * ( twoCpB * Mathf.Sqrt( sumCBA ) - b * Mathf.Sqrt( a ) ) +
					mult1 * ( Mathf.Log( 2.0f * Mathf.Sqrt( c * sumCBA ) + twoCpB ) - Mathf.Log( 2.0f * Mathf.Sqrt( c * a ) + b ) );
			}
			else
			{
				length = 2.0f * A[0].magnitude;
			}

			return length;
		}


		/// <summary>
		/// Calculates a point on the path.
		/// </summary>
		/// <returns>The bezier point.</returns>
		/// <param name="curveIndex">The index of the curve that the point is on. For example, the second curve (index 1) is the curve
		/// with controlpoints 3, 4, 5, and 6.</param>
		/// <param name="t">indicates where on the curve the point is. 0 corresponds  to the "left" point, 1 corresponds to the "right"
		/// end point.</param>
		Vector3 getPoint( int curveIndex, float t )
		{
			var nodeIndex = curveIndex * 3;

			var p0 = _nodes[nodeIndex];
			var p1 = _nodes[nodeIndex + 1];
			var p2 = _nodes[nodeIndex + 2];
			var p3 = _nodes[nodeIndex + 3];

			return getPoint( t, p0, p1, p2, p3 );
		}


		/// <summary>
		/// Calculates a point on the Bezier curve represented with the four control points given.
		/// </summary>
		/// <returns>The bezier point.</returns>
		/// <param name="t">T.</param>
		/// <param name="p0">P0.</param>
		/// <param name="p1">P1.</param>
		/// <param name="p2">P2.</param>
		/// <param name="p3">P3.</param>
		Vector3 getPoint( float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3 )
		{
			var u = 1f - t;
			var tt = t * t;
			var uu = u * u;
			var uuu = uu * u;
			var ttt = tt * t;

			var p = uuu * p0; //first term
			p += 3f * uu * t * p1; //second term
			p += 3f * u * tt * p2; //third term
			p += ttt * p3; //fourth term

			return p;
		}


		#region AbstractSplineSolver

		public override void closePath()
		{
			// if the first and last node are not the same move them to the midpoint between them
			if( _nodes[0] != _nodes[_nodes.Count - 1] )
			{
				var midPoint = Vector3.Lerp( _nodes[0], _nodes[_nodes.Count - 1], 0.5f );
				var deltaMove = midPoint - _nodes[0];

				_nodes[0] = _nodes[_nodes.Count - 1] = midPoint;

				// shift the ctrl points
				_nodes[1] += deltaMove;
				_nodes[_nodes.Count - 2] -= deltaMove;
			}

			// normalize the ctrl points
			var firstCtrlPoint = _nodes[1];
			var middlePoint = _nodes[0];
			var enforcedTangent = middlePoint - firstCtrlPoint;
			_nodes[_nodes.Count - 2] = middlePoint + enforcedTangent;
		}


		/// <summary>
		/// calculates the point on the path with t representing the absolute position from 0 - 1 on the entire spline
		/// </summary>
		/// <returns>The bezier point.</returns>
		/// <param name="t">T.</param>
		public override Vector3 getPoint( float t )
		{
			// wrap t if it is over 1 or less than 0
			if( t > 1f )
				t = 1f - t;
			else if( t < 0f )
				t = 1f + t;

			int currentCurve;
			if( t == 1f )
			{
				t = 1f;
				currentCurve = _curveCount - 1;
			}
			else
			{
				// grab our curve than set t to the remainder along the current curve
				t = t * _curveCount;
				currentCurve = (int)t;
				t -= currentCurve;
			}

			return getPoint( currentCurve, t );
		}


		public override void drawGizmos()
		{
			// draw just the control points
			var originalColor = Gizmos.color;
			Gizmos.color = Color.red;

			for( var i = 0; i < _nodes.Count; i += 3 )
			{
				if( i > 0 )
					Gizmos.DrawLine( _nodes[i], _nodes[i - 1] );

				if( i < _nodes.Count - 2 )
					Gizmos.DrawLine( _nodes[i], _nodes[i + 1] );
			}

			Gizmos.color = originalColor;
		}

		#endregion
	}

}