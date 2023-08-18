using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class WatcherAttribute : PropertyAttribute
{
  public readonly string[] callback_names;


  public WatcherAttribute( params string[] callback_names )
  {
    this.callback_names = callback_names;
  }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(WatcherAttribute))]
public class WatcherDrawer : PropertyDrawer
{
  public override void OnGUI( Rect position, SerializedProperty prop, GUIContent label )
  {
    EditorGUI.BeginChangeCheck();
    EditorGUI.PropertyField( position, prop, label );
    if ( EditorGUI.EndChangeCheck() )
    {
      if ( prop.serializedObject.targetObject.GetType().IsSubclassOf( typeof(MonoBehaviour) ) )
      {
        MonoBehaviour mono = (MonoBehaviour)prop.serializedObject.targetObject;

        foreach ( string callback_name in watcherAttribute.callback_names )
          mono.Invoke( callback_name, 0 );
      }
    }
  }

  private WatcherAttribute watcherAttribute
  {
    get
    {
      return (WatcherAttribute)attribute;
    }
  }
}
#endif
