using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public GameObject shell;
    
    private void OnDestroy()
    {
        if (shell != null)
        {
            if (!gameObject.scene.isLoaded) return;
            Instantiate(shell, gameObject.transform.position - Vector3.up * 0.25f, Quaternion.identity);
        }
    }
}
