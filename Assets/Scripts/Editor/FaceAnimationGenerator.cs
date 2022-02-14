using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class FaceAnimationGenerator : EditorWindow
{
  public string CharacterName;

  public List<FacialExpression> faceSprites = new List<FacialExpression>();

  private SerializedObject so;

  [MenuItem( "Tools/Ink Dialog System/Face Animation Generator" )]
  public static void InitWindow()
  {
    // Get existing open window or if none, make a new one:
    FaceAnimationGenerator window = (FaceAnimationGenerator)EditorWindow.GetWindow( typeof( FaceAnimationGenerator ) );
    window.Show();
  }

  public void OnEnable()
  {
    // Create a reference to this object so
    // we can use a serialized version to grab
    // the faces array
    ScriptableObject target = this;
    so = new SerializedObject( target );
  }

  public void OnGUI()
  {
    SerializedProperty facesProperty = so.FindProperty( nameof( faceSprites ) );

    GUILayout.Label( "Face Information", EditorStyles.boldLabel );
    CharacterName = EditorGUILayout.TextField( "Character Name", CharacterName );
    GUILayout.Label( "Sprites", EditorStyles.label );
    EditorGUILayout.PropertyField( facesProperty, true );
    so.ApplyModifiedProperties(); // remember to update the real faces array
    EditorGUILayout.Space();
    if ( GUILayout.Button( "Create Animation Clips" ) && !String.IsNullOrWhiteSpace( CharacterName ) )
    {
      for ( int i = 0; i < faceSprites.Count; i++ )
      {
        CreateClip( faceSprites[i], CharacterName );
      }
    }
  }


  private void CreateClip(FacialExpression facialExpression, string CharacterName)
  {
    Sprite faceSprite = facialExpression.FacialSprite;
    string clipName = CharacterName.Replace( " ", "_" ).ToLower() + "_" + facialExpression.ExpressionName;
    string folderName = CharacterName.Replace( " ", "" );
    AnimationClip animClip = new AnimationClip();
    animClip.name = clipName;
    animClip.frameRate = 60;   // FPS
    EditorCurveBinding spriteBinding = new EditorCurveBinding();
    spriteBinding.type = typeof( SpriteRenderer );
    spriteBinding.path = "";
    spriteBinding.propertyName = "m_Sprite";
    ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[5];
    spriteKeyFrames[0] = new ObjectReferenceKeyframe();
    spriteKeyFrames[0].time = 0;
    spriteKeyFrames[0].value = faceSprite;
    AnimationUtility.SetObjectReferenceCurve( animClip, spriteBinding, spriteKeyFrames );
    if ( !AssetDatabase.IsValidFolder( "Assets/Animations/Portraits/" + folderName ) )
    {
      AssetDatabase.CreateFolder( "Assets/Animations/Portraits", folderName );
    }
    AssetDatabase.CreateAsset( animClip, "Assets/Animations/Portraits/" + folderName + "/" + clipName + ".anim" );
    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
    AnimatorController facialController = AssetDatabase.LoadAssetAtPath<AnimatorController>( "Assets/Animations/Portraits/PortraitAnimator.controller" );
    facialController.AddMotion( animClip );
    AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( animClip ) );
    AssetDatabase.Refresh();
  }
}
