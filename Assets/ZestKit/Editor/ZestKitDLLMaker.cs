#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;



public class ZestKitDLLMaker : Editor
{
	[MenuItem( "File/ZestKit/Create ZestKit.dll..." )]
	static void createDLL()
	{
		if( createDLL( false, "ZestKit.dll" ) )
			EditorUtility.DisplayDialog( "ZestKit", "ZestKit.dll should now be on your desktop.", "OK" );
	}


	[MenuItem( "File/ZestKit/Create ZestKit.Editor.dll..." )]
	static void createEditorDLL()
	{
		if( createDLL( true, "ZestKit.Editor.dll" ) )
			EditorUtility.DisplayDialog( "ZestKit", "ZestKit.Editor.dll should now be on your desktop. Stick it in an Editor folder in your Unity project to use it.", "OK" );
	}


	static bool createDLL( bool isEditorDLL, string DLLName )
	{
		var compileParams = new CompilerParameters();
		compileParams.OutputAssembly = Path.Combine( System.Environment.GetFolderPath( System.Environment.SpecialFolder.Desktop ), DLLName );
		compileParams.CompilerOptions = "/optimize";

		if( isEditorDLL )
			compileParams.CompilerOptions += " /define:UNITY_EDITOR";
		
		compileParams.ReferencedAssemblies.Add( Path.Combine( EditorApplication.applicationContentsPath, "Frameworks/Managed/UnityEngine.dll" ) );
		compileParams.ReferencedAssemblies.Add( Path.Combine( EditorApplication.applicationContentsPath, "UnityExtensions/Unity/GUISystem/UnityEngine.UI.dll" ) );

		if( isEditorDLL )
		{
			compileParams.ReferencedAssemblies.Add( Path.Combine( Application.dataPath.Replace( "Assets", string.Empty ), "Library/ScriptAssemblies/Assembly-CSharp.dll" ) );
			compileParams.ReferencedAssemblies.Add( Path.Combine( EditorApplication.applicationContentsPath, "Frameworks/Managed/UnityEditor.dll" ) );
		}


		var source = isEditorDLL ? getSourceForEditorDLL() : getSourceForDLL();

		Debug.Log( "source len: " + source.Length );

		var codeProvider = new CSharpCodeProvider( new Dictionary<string,string> { { "CompilerVersion", "v3.0" } } );
		var compilerResults = codeProvider.CompileAssemblyFromSource( compileParams, source );

		if( compilerResults.Errors.Count > 0 )
		{
			foreach( var error in compilerResults.Errors )
				Debug.LogError( error.ToString() );

			return false;
		}

		return true;
	}


	static string[] getSourceForDLL( string path = "Assets/ZestKit/" )
	{
		var source = new List<string>();

		foreach( var file in Directory.GetFiles( path, "*.cs", SearchOption.AllDirectories ) )
		{
			if( !file.Contains( "DummySplineEditor" ) && !file.Contains( "Demo" ) && !file.Contains( "/Editor" ) )
				source.Add( File.ReadAllText( file ) );
		}

		return source.ToArray();
	}


	static string[] getSourceForEditorDLL( string path = "Assets/ZestKit/" )
	{
		var source = new List<string>();

		foreach( var file in Directory.GetFiles( path, "DummySplineEditor.cs", SearchOption.AllDirectories ) )
		{
			source.Add( File.ReadAllText( file ) );
		}

		return source.ToArray();
	}

}
#endif
