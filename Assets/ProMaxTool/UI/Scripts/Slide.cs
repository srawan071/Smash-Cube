using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMaxUtils
{
    public class Slide : MonoBehaviour
    {

        [SerializeField]
        float speed, StartPos, endPos;
        [SerializeField]
        AnimationCurve curve;
        public void OnButtonClick()
        {
            StartCoroutine(slide());
        }
        IEnumerator slide()
        {
            float t = 0;
            Vector3 startPos = Vector3.right*StartPos;
            Vector3 desiredPos = new Vector3(endPos,0,0);
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, Time.unscaledDeltaTime * speed);
                transform.localPosition = Vector3.LerpUnclamped(startPos, desiredPos, curve.Evaluate(t));
                yield return null;
            }
        }

    }
}