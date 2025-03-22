using System.Collections;
using Runtime.Utilies;
using UnityEngine;

namespace Runtime.Managers
{
   public class UIEffectManager : MonoBehaviour
   { 
        public void ScaleUI(Transform target, Vector3 targetScale, float duration)
        {
            StartCoroutine(ScaleAnimation(target, targetScale, duration));
        }

        public void MoveUI(Transform target, Vector3 targetPos, float duration)
        {
            StartCoroutine(MoveAnimation(target, targetPos, duration));
        }
        
        public void ScaleUpDownUI(Transform target, Vector3 targetScale, float duration)
        {
            StartCoroutine(ScaleUpDownAnimation(target, targetScale, duration));
        }

        private IEnumerator ScaleUpDownAnimation(Transform target, Vector3 targetScale, float duration)
        {
            Vector3 startScale = target.localScale; 
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseInOut(t);
                target.localScale = Vector3.Lerp(startScale, targetScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.localScale = targetScale;

            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseOut(t);
                target.localScale = Vector3.Lerp(targetScale, startScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.localScale = startScale;
        }

        private IEnumerator ScaleAnimation(Transform target, Vector3 targetScale, float duration)
        {
            Vector3 startScale = target.localScale;
            float elapsedTime = 0;
            
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseInOut(t);
                target.localScale = Vector3.Lerp(startScale, targetScale, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.localScale = targetScale;
        }

        private IEnumerator MoveAnimation(Transform target, Vector3 targetPos, float duration)
        {
            Vector3 startPos = target.position;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseInOut(t); 
                target.position = Vector3.Lerp(startPos, targetPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.position = targetPos;
        }
        
        public void ResetPosition(Transform target, Vector3 originalPosition, float duration)
        {
            StartCoroutine(MoveAnimation(target, originalPosition, duration));
        }
        
        public void ResetScale(Transform target, Vector3 originalScale, float duration)
        {
            StartCoroutine(ScaleAnimation(target, originalScale, duration));
        }
   }
}

