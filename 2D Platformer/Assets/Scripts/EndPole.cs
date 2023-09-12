using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPole : MonoBehaviour
{
    [SerializeField] private Transform lowestPoint;
    [SerializeField] private Transform highestpoint;

    public int GetHeightPoints(float _currentYPos)
    {
        float currentYPos = _currentYPos - lowestPoint.position.y;
        float yDis = highestpoint.position.y - lowestPoint.position.y;
        float normalizedPos = currentYPos / yDis;
        float pointMuliplier = Mathf.Lerp(0, 10, normalizedPos);

        FloatingScorePool.instance.GetFromPool(new Vector3(transform.position.x, _currentYPos, transform.position.z), (int)pointMuliplier * 1000);
        return (int)pointMuliplier * 1000;
    }
}
