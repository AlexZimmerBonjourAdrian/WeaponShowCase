//using UnityEngine;

//using UnityEditor;
// [CustomEditor(typeof(FindOfView))]
//public class FindOfViewEditor : Editor
//{
//  private void OnSceneGUI()
//    {
//        FindOfView fov = (FindOfView)target;
//        Handles.color = Color.white;
//        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

//        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
//        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

//        Handles.color = Color.yellow;
//        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
//        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);
    
//        if(fov.canSeePlayer)
//        {
//            Handles.color = Color.green;
//            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
//        }
//    }

//    private Vector3 DirectionFromAngle(float eulerY, float angleInDegress)
//    {
//        angleInDegress += eulerY;

//        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
//    }
//}
