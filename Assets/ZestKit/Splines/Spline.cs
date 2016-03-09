using UnityEngine;
using System.Collections.Generic;


namespace Prime31.ZestKit
{
	public enum SplineType
	{
		StraightLine, // 2 points
		QuadraticBezier, // 3 points
		CubicBezier, // 4 points
		CatmullRom, // 5+ points
		Bezier // 5+ points
	}


	public class Spline
	{
		public int currentSegment { get; private set; }
		public bool isClosed { get; private set; }
		public SplineType splineType { get; private set; }

		// used by the visual path editor
		public List<Vector3> nodes { get { return _solver.nodes; } }

		private bool _isReversed; // internal flag that lets us know if our nodes are reversed or not
		private AbstractSplineSolver _solver;

		public float pathLength
		{
			get
			{
				return _solver.pathLength;
			}
		}


		/// <summary>
		/// generates an arc from start to end with the arc axis perpendicular to start and end points
		/// </summary>
		/// <returns>The arc.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="curvature">how far away from the line from start to end the arc extends</param>
		public static Spline generateArc( Vector3 start, Vector3 end, float curvature )
		{
			return Spline.generateArc( start, end, curvature, Vector3.Cross( start, end ) );
		}


		/// <summary>
		/// generates an arc from start to end with the arc axis curvatureAxis
		/// </summary>
		/// <returns>The arc.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="curvature">how far away from the line from start to end the arc extends</param>
		/// <param name="curvatureAxis">the axis which the arc should extend into</param>
		public static Spline generateArc( Vector3 start, Vector3 end, float curvature, Vector3 curvatureAxis )
		{
			curvatureAxis.Normalize();
			return Spline.generateArc( start, end, curvature, curvatureAxis, curvatureAxis );
		}


		/// <summary>
		/// generates an arc from start to end with a separate axis for the start and and points
		/// </summary>
		/// <returns>The arc.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="curvature">how far away from the line from start to end the arc extends</param>
		/// <param name="startCurvatureAxis">Start curvature axis.</param>
		/// <param name="endCurvatureAxis">End curvature axis.</param>
		public static Spline generateArc( Vector3 start, Vector3 end, float curvature, Vector3 startCurvatureAxis, Vector3 endCurvatureAxis )
		{
			startCurvatureAxis.Normalize();
			endCurvatureAxis.Normalize();

			var nodes = new List<Vector3>()
			{
				start,
				start + startCurvatureAxis * curvature,
				end + endCurvatureAxis * curvature,
				end
			};

			return new Spline( nodes );
		}


		/// <summary>
		/// Default constructor. Creates and initializes a spline from a List of nodes
		/// </summary>
		/// <param name="nodes">Nodes.</param>
		/// <param name="useBezierIfPossible">If set to <c>true</c> use bezier if possible.</param>
		/// <param name="useStraightLines">If set to <c>true</c> use straight lines.</param>
		public Spline( List<Vector3> nodes, bool useBezierIfPossible = false, bool useStraightLines = false )
		{
			// determine spline type and solver based on number of nodes
			if( useStraightLines || nodes.Count == 2 )
			{
				splineType = SplineType.StraightLine;
				_solver = new StraightLineSplineSolver( nodes );
			}
			else if( nodes.Count == 3 )
			{
				splineType = SplineType.QuadraticBezier;
				_solver = new QuadraticBezierSplineSolver( nodes );
			}
			else if( nodes.Count == 4 )
			{
				splineType = SplineType.CubicBezier;
				_solver = new CubicBezierSplineSolver( nodes );
			}
			else
			{
				if( useBezierIfPossible )
				{
					splineType = SplineType.Bezier;
					_solver = new BezierSplineSolver( nodes );
				}
				else
				{
					splineType = SplineType.CatmullRom;
					_solver = new CatmullRomSplineSolver( nodes );
				}
			}
		}
			

		public Spline( string pathAssetName, bool useBezierIfPossible = false, bool useStraightLines = false ) : this( SplineAssetUtils.nodeListFromAsset( pathAssetName ), useBezierIfPossible, useStraightLines )
		{}


		public Spline( Vector3[] nodes, bool useBezierIfPossible = false, bool useStraightLines = false ) : this( new List<Vector3>( nodes ), useBezierIfPossible, useStraightLines )
		{}


		/// <summary>
		/// gets the last node. used to setup relative tweens
		/// </summary>
		public Vector3 getLastNode()
		{
			return _solver.nodes[_solver.nodes.Count];
		}


		/// <summary>
		/// responsible for calculating total length, segmentStartLocations and segmentDistances
		/// </summary>
		public void buildPath()
		{
			_solver.buildPath();
		}


		/// <summary>
		/// directly gets the point for the current spline type with no lookup table to adjust for constant speed
		/// </summary>
		private Vector3 getPoint( float t )
		{
			return _solver.getPoint( t );
		}


		/// <summary>
		/// returns the point that corresponds to the given t where t >= 0 and t <= 1 making sure that the
		/// path is traversed at a constant speed.
		/// </summary>
		public Vector3 getPointOnPath( float t )
		{
			// if the path is closed, we will allow t to wrap. if is not we need to clamp t
			if( t < 0f || t > 1f )
			{
				if( isClosed )
				{
					if( t < 0f )
						t += 1;
					else
						t -= 1;
				}
				else
				{
					t = Mathf.Clamp01( t );
				}
			}

			return _solver.getPointOnPath( t );
		}


		/// <summary>
		/// closes the path adding a new node at the end that is equal to the start node if it isn't already equal
		/// </summary>
		public void closePath()
		{
			// dont let this get closed twice!
			if( isClosed )
				return;

			isClosed = true;
			_solver.closePath();
		}


		/// <summary>
		/// reverses the order of the nodes
		/// </summary>
		public void reverseNodes()
		{
			if( !_isReversed )
			{
				_solver.reverseNodes();
				_isReversed = true;
			}
		}


		/// <summary>
		/// unreverses the order of the nodes if they were reversed
		/// </summary>
		public void unreverseNodes()
		{
			if( _isReversed )
			{
				_solver.reverseNodes();
				_isReversed = false;
			}
		}


		public void drawGizmos( float resolution, bool isInEditMode )
		{
			if( _solver.nodes.Count == 0 )
				return;

			if( isInEditMode )
				_solver.drawGizmos();
			
			var previousPoint = _solver.getPoint( 0 );

			resolution *= _solver.nodes.Count;
			for( var i = 1; i <= resolution; i++ )
			{
				var t = (float)i / resolution;
				var currentPoint = _solver.getPoint( t );
				Gizmos.DrawLine( currentPoint, previousPoint );
				previousPoint = currentPoint;
			}
		}


		/// <summary>
		/// helper for drawing gizmos in the editor
		/// </summary>
		public static void drawGizmos( Vector3[] nodes, float resolution = 50, bool isInEditMode = false )
		{
			// horribly inefficient but it only runs in the editor
			var spline = new Spline( new List<Vector3>( nodes ) );
			spline.drawGizmos( resolution, isInEditMode );
		}


        public int getTotalPointsBetweenPoints(float t, float t2) {
            return _solver.getTotalPointsBetweenPoints(t, t2);
        }
    }
}