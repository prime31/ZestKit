using UnityEngine;
using System.Collections;
using Prime31.ZestKit;


public class ZestKitStressTest : MonoBehaviour
{
	private const int _totalCubes = 2500;

	private Transform[] _cubes = new Transform[_totalCubes];
	private Perlin _perlinNoiseGenerator = new Perlin();


	void Awake()
	{
		Application.targetFrameRate = 60;

		// original stress test algorithm (flying perlin cubes) adapted from LeanTweens comparison demo
		for( var i = 0; i < _cubes.Length; i++ )
		{
			var cube = GameObject.CreatePrimitive( PrimitiveType.Cube );
			Destroy( cube.GetComponent<BoxCollider>() );
			cube.transform.position = new Vector3( i * 0.1f - 40f, cube.transform.position.y - 10f, i % 10 );
			_cubes[i] = cube.transform;
		}
	}


	void Start()
	{
		float timeX, timeY, timeZ;
		var targetPoint = Vector3.zero;
		for( var i = 0; i < _cubes.Length; i++ )
		{
			timeX = 4f;
			timeY = Random.Range( -2f, 2f ) * 2f;
			timeZ = Random.Range( -2f, 2f ) * 3.0f;

			targetPoint.x = _perlinNoiseGenerator.Noise( timeX ) * 100 + _cubes[i].position.x;
			targetPoint.y = _perlinNoiseGenerator.Noise( timeY ) * 100 + _cubes[i].position.y;
			targetPoint.z = _perlinNoiseGenerator.Noise( timeZ ) * 100 + _cubes[i].position.z;

			_cubes[i].ZKpositionTo( targetPoint, 1.0f )
				.setLoops( LoopType.PingPong, 99999, 0.1f )
				.start();
		}
	}
		

	public class Perlin
	{
		// Original C code derived from
		// http://astronomy.swin.edu.au/~pbourke/texture/perlin/perlin.c
		// http://astronomy.swin.edu.au/~pbourke/texture/perlin/perlin.h
		const int B = 0x100;
		const int BM = 0xff;
		const int N = 0x1000;
		int[] p = new int[B + B + 2];
		float[,] g3 = new float [B + B + 2, 3];
		float[,] g2 = new float[B + B + 2, 2];
		float[] g1 = new float[B + B + 2];


		float s_curve( float t )
		{
			return t * t * ( 3.0F - 2.0F * t );
		}


		void setup( float value, out int b0, out int b1, out float r0, out float r1 )
		{
			float t = value + N;
			b0 = ( (int)t ) & BM;
			b1 = ( b0 + 1 ) & BM;
			r0 = t - (int)t;
			r1 = r0 - 1.0F;
		}


		float at2( float rx, float ry, float x, float y )
		{
			return rx * x + ry * y;
		}


		float at3( float rx, float ry, float rz, float x, float y, float z )
		{
			return rx * x + ry * y + rz * z;
		}


		public float Noise( float arg )
		{
			int bx0, bx1;
			float rx0, rx1, sx, u, v;
			setup( arg, out bx0, out bx1, out rx0, out rx1 );

			sx = s_curve( rx0 );
			u = rx0 * g1[p[bx0]];
			v = rx1 * g1[p[bx1]];

			return( Mathf.Lerp( sx, u, v ) );
		}


		void normalize2( ref float x, ref float y )
		{
			float s;

			s = (float)Mathf.Sqrt( x * x + y * y );
			x = y / s;
			y = y / s;
		}


		void normalize3( ref float x, ref float y, ref float z )
		{
			float s;
			s = (float)Mathf.Sqrt( x * x + y * y + z * z );
			x = y / s;
			y = y / s;
			z = z / s;
		}


		public Perlin()
		{
			int i, j, k;
			System.Random rnd = new System.Random();

			for( i = 0; i < B; i++ )
			{
				p[i] = i;
				g1[i] = (float)( rnd.Next( B + B ) - B ) / B;

				for( j = 0; j < 2; j++ )
					g2[i, j] = (float)( rnd.Next( B + B ) - B ) / B;
				normalize2( ref g2[i, 0], ref g2[i, 1] );

				for( j = 0; j < 3; j++ )
					g3[i, j] = (float)( rnd.Next( B + B ) - B ) / B;


				normalize3( ref g3[i, 0], ref g3[i, 1], ref g3[i, 2] );
			}

			while( --i != 0 )
			{
				k = p[i];
				p[i] = p[j = rnd.Next( B )];
				p[j] = k;
			}

			for( i = 0; i < B + 2; i++ )
			{
				p[B + i] = p[i];
				g1[B + i] = g1[i];
				for( j = 0; j < 2; j++ )
					g2[B + i, j] = g2[i, j];
				for( j = 0; j < 3; j++ )
					g3[B + i, j] = g3[i, j];
			}
		}
	}

}
