
using UnityEngine;

public class InspectorDataSession
{

	public static InspectorDataSession Instance;

	private static DataSession _dataSession;
	
	public static void Init()
	{
		if (_dataSession != null)
		{
			ScriptableObject.Destroy(_dataSession);
		}
		
//		_dataSession = 
	}
	
}
