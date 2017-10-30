using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleConnector : MonoBehaviour
{

    public string[] Tags;
    public bool IsDefault;

    private void OnDrawGizmos()
    {
        var scale = 1f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * scale);
        Gizmos.DrawLine(transform.position, transform.position - transform.right * scale);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * scale);
    }
}
