using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHelper : MonoBehaviour
{
    AIEnemyController aIEnemyController;
    void Start()
    {
        aIEnemyController = transform.parent.GetComponent<AIEnemyController>();
    }

    void AttackEvent()
    {
        aIEnemyController.AttackEvent();
    }
}
