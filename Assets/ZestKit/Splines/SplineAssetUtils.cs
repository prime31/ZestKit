using UnityEngine;
using System.Collections.Generic;
using System.IO;
#if ENABLE_UNITYWEBREQUEST
using UnityEngine.Networking;
#endif


namespace Prime31.ZestKit
{
	public static class SplineAssetUtils
	{
		/// <summary>
		/// helper to get a node list from an asset created with the visual editor
		/// </summary>
		public static List<Vector3> nodeListFromAsset( string pathAssetName )
		{
			
			#if !UNITY_5_4_OR_NEWER
			if( Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer )
			{
				Debug.LogError( "The Web Player does not support loading files from disk." );
				return null;
			}
			#endif

			var path = string.Empty;
			if( !pathAssetName.EndsWith( ".asset" ) )
				pathAssetName += ".asset";


			if( Application.platform == RuntimePlatform.Android )
			{
				path = Path.Combine( "jar:file://" + Application.dataPath + "!/assets/", pathAssetName );

#if ENABLE_UNITYWEBREQUEST
				UnityWebRequest loadAsset = UnityWebRequest.Get( path );
				loadAsset.SendWebRequest();
				while( !loadAsset.isDone ) { } // maybe make a safety check here

				return bytesToVector3List( loadAsset.downloadHandler.data );
#elif ENABLE_WWW
				WWW loadAsset = new WWW( path );
				while( !loadAsset.isDone ) { } // maybe make a safety check here

				return bytesToVector3List( loadAsset.bytes );
#else
				throw System.NotImplementedException();
#endif
			}
			else
			{
				// at runtime on other platforms
				path = Path.Combine( Application.streamingAssetsPath, pathAssetName );
			}

#if UNITY_WEBPLAYER || NETFX_CORE || UNITY_WP8
			// it isnt possible to get here but the compiler needs it to be here anyway
			return null;
#else
			var bytes = File.ReadAllBytes( path );
			return bytesToVector3List( bytes );
#endif
		}


		/// <summary>
		/// helper to get a node list from an asset created with the visual editor
		/// </summary>
		public static List<Vector3> bytesToVector3List( byte[] bytes )
		{
			var vecs = new List<Vector3>();
			for( var i = 0; i < bytes.Length; i += 12 )
			{
				var newVec = new Vector3( System.BitConverter.ToSingle( bytes, i ), System.BitConverter.ToSingle( bytes, i + 4 ), System.BitConverter.ToSingle( bytes, i + 8 ) );
				vecs.Add( newVec );
			}

			return vecs;
		}
	}
}
