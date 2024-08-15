using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IGaugeDrawable
    {
        void DamagePlayer(float playerHitPoint, float playerHitPointMax);
        void DamageEnemy(float enemyHitPoint, float enemyHitPointMax);
    }
}