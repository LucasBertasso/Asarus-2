using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coelho : EnemyStats {

    // Use this for initialization
    void Start() {
        GetRefs();
        SetInitialValues();
        Timers[0] = Random.Range(1, 5);
        atualState = 0;

    }

    // Update is called once per frame
    void FixedUpdate() {
        EssencialsVoids();
        ShadowControl();
        ///
        Patrol();
        if (attackMode)
        {
            LookControlAttack();
            if (grounded)
            {
                directionX = transform.localScale.x;
            }
        }
        if (!grounded)
        {
            directionX = 0;
        }
    }
    new void LookControlAttack()
    {
        Vector2 scale = transform.localScale;
        if (scale.x < 0 && transform.position.x > PlayerRefs[CharacterChangeInformation.atual].transform.position.x && !attack)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
        if (scale.x > 0 && transform.position.x < PlayerRefs[CharacterChangeInformation.atual].transform.position.x && !attack)
        {
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
