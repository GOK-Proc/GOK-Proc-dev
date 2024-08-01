using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IGaugeDrawer
    {
        void UpdateHitPointGauge(float playerHitPoint, float playerHitPointMax, float enemyHitPoint, float enemyHitPointMax);
    }
}