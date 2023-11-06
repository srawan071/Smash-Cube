using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMaxUtils {
    public class Pop_Up : MonoBehaviour
    {
       
        
        [SerializeField]
        float speed,StartScale,desiredScale;
        [SerializeField]
        AnimationCurve curve;
        public void OnButtonClick()
        {
            StartCoroutine(popup());
            
        }
        IEnumerator popup()
        {
            float t = 0;
            Vector3 startScale = Vector3.one*StartScale;
            Vector3 finalScale = Vector3.one * desiredScale;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, Time.unscaledDeltaTime * speed);
                transform.localScale = Vector3.LerpUnclamped(startScale,finalScale , curve.Evaluate(t));
              
                yield return null;
            }
          
        }

    }
}
