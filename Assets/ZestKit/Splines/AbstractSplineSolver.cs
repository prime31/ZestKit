﻿using UnityEngine;
using System.Collections.Generic;


namespace Prime31.ZestKit
{
	public abstract class AbstractSplineSolver
	{
		protected List<Vector3> _nodes;
		public List<Vector3> nodes { get { return _nodes; } }

        protected List<Vector3> _nodesWithDivisions;
        public List<Vector3> nodesWithDivisions { get { return _nodesWithDivisions; } }


        protected float _pathLength;

		public float pathLength
		{
			get
			{
				return _pathLength;
			}
		}

        public float allPathLength 
        {
            get 
            {
                return _nodesWithDivisions.Count;
            }
        }

        //get the distance from the beginning to a point
        public float getPathLength(int endPoint) 
        {
            float length = 0f;
            var totalSudivisions = _nodes.Count * totalSubdivisionsPerNodeForLookupTable;
            float timePerSlice = 1f / totalSudivisions;
            Vector3 lastPoint = getPoint(0);

            for (int i = 1; i < endPoint + 1; i++) 
            {
                float currentTime = timePerSlice * i;
                var currentPoint = getPoint(currentTime);
                length += Vector3.Distance(currentPoint, lastPoint);
                lastPoint = currentPoint;
            }

            return length;
        }

        //get the distance from the initpoint to the endpoint we want
        //this points are NOT the waypoints or nodes, these are the subdivisions
        public float getPathLength(int initPoint, int endPoint, float normalizedInitPos) 
        {
            float length = 0f;
            var totalSudivisions = _nodes.Count * totalSubdivisionsPerNodeForLookupTable;
            float timePerSlice = 1f / totalSudivisions;
            //get the initial point to work with
            Vector3 lastPoint = getPoint(normalizedInitPos);

            for (int i = initPoint+1; i < endPoint + 1; i++) 
            {
                float currentTime = timePerSlice * i;
                var currentPoint = getPoint(currentTime);
                length += Vector3.Distance(currentPoint, lastPoint);
                lastPoint = currentPoint;
            }

            return length;
        }


        // how many subdivisions should we divide each segment into? higher values take longer to build and lookup but
        // result in closer to actual constant velocity
        protected int totalSubdivisionsPerNodeForLookupTable = 5;
		protected Dictionary<float, float> _segmentTimeForDistance; // holds data in the form [time:distance] as a lookup table


		// the default implementation breaks the spline down into segments and approximates distance by adding up
		// the length of each segment
		public virtual void buildPath()
		{
			var totalSudivisions = _nodes.Count * totalSubdivisionsPerNodeForLookupTable;
			_pathLength = 0;
			float timePerSlice = 1f / totalSudivisions;

			// we dont care about the first node for distances because they are always t:0 and len:0
			_segmentTimeForDistance = new Dictionary<float, float>( totalSudivisions );

            _nodesWithDivisions = new List<Vector3>();

			var lastPoint = getPoint( 0 );

			// skip the first node and wrap 1 extra node
			for( var i = 1; i < totalSudivisions + 1; i++ )
			{
				// what is the current time along the path?
				float currentTime = timePerSlice * i;

				var currentPoint = getPoint( currentTime );
				_pathLength += Vector3.Distance( currentPoint, lastPoint );
				lastPoint = currentPoint;

                _nodesWithDivisions.Add(currentPoint);
				_segmentTimeForDistance.Add( currentTime, _pathLength );
			}
		}


		public abstract void closePath();


		// gets the raw point not taking into account constant speed. used for drawing gizmos
		public abstract Vector3 getPoint( float t );


		// gets the point taking in to account constant speed. the default implementation approximates the length of the spline
		// by walking it and calculating the distance between each node
		public virtual Vector3 getPointOnPath( float t )
		{
			// we know exactly how far along the path we want to be from the passed in t
			var targetDistance = _pathLength * t;

			// store the previous and next nodes in our lookup table
			var previousNodeTime = 0f;
			var previousNodeLength = 0f;
			var nextNodeTime = 0f;
			var nextNodeLength = 0f;

			float[] keysSegmentTimeForDistance = new float[_segmentTimeForDistance.Keys.Count];
			_segmentTimeForDistance.Keys.CopyTo( keysSegmentTimeForDistance, 0 );

			// loop through all the values in our lookup table and find the two nodes our targetDistance falls between
			for( int k = 0; k < keysSegmentTimeForDistance.Length; ++k )
			{
				float key = keysSegmentTimeForDistance[k];
				float value = _segmentTimeForDistance[key];

				// have we passed our targetDistance yet?
				if( value >= targetDistance )
				{
					nextNodeTime = key;
					nextNodeLength = value;

					if( previousNodeTime > 0 )
						previousNodeLength = _segmentTimeForDistance[previousNodeTime];

					break;
				}
				previousNodeTime = key;
			}

			// translate the values from the lookup table estimating the arc length between our known nodes from the lookup table
			var segmentTime = nextNodeTime - previousNodeTime;
			var segmentLength = nextNodeLength - previousNodeLength;
			var distanceIntoSegment = targetDistance - previousNodeLength;

			t = previousNodeTime + ( distanceIntoSegment / segmentLength ) * segmentTime;

			return getPoint( t );
		}



		public void reverseNodes()
		{
			_nodes.Reverse();
		}


		public virtual void drawGizmos()
		{ }


		public virtual int getTotalPointsBetweenPoints( float t, float t2 )
		{
			int totalPoints = 0;

			// we know exactly how far along the path we want to be from the passed in t
			var targetDistance = _pathLength * t;
			var targetDistance2 = _pathLength * t2;

			// store the previous and next nodes in our lookup table
			var nextNodeLength = 0f;

			float[] keysSegmentTimeForDistance = new float[_segmentTimeForDistance.Keys.Count];
			_segmentTimeForDistance.Keys.CopyTo( keysSegmentTimeForDistance, 0 );

			// loop through all the values in our lookup table and find the two nodes our targetDistance falls between
			for( int k = 0; k < keysSegmentTimeForDistance.Length; ++k )
			{
				float key = keysSegmentTimeForDistance[k];
				float value = _segmentTimeForDistance[key];

				// have we passed our targetDistance yet?
				if( value >= targetDistance )
				{
					nextNodeLength = value;
					break;
				}
			}


			// store the previous and next nodes in our lookup table
			var previousNodeTime = 0f;
			var previousNodeLength = 0f;

			// loop through all the values in our lookup table and find the two nodes our targetDistance falls between
			for( int k = 0; k < keysSegmentTimeForDistance.Length; ++k )
			{
				float key = keysSegmentTimeForDistance[k];
				float value = _segmentTimeForDistance[key];

				// have we passed our targetDistance yet?
				if( value >= targetDistance2 )
				{
					if( previousNodeTime > 0 )
						previousNodeLength = _segmentTimeForDistance[previousNodeTime];

					break;
				}
				previousNodeTime = key;
			}

			//round the float values, we just want an approximation to the amount of nodes
			//not a very strict and real value
			totalPoints = (int)( nextNodeLength + 0.5f ) + (int)( previousNodeLength + 0.5f );

			return totalPoints;
		}
	}
}