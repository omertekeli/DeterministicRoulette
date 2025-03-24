using System.Collections;
using Runtime.Utilies;
using UnityEngine;

namespace Runtime.Managers
{
   public class UIEffectManager : MonoBehaviour
   { 
        public void ScaleUI(RectTransform target, Vector2 targetSize, float duration)
        {
            StartCoroutine(ScaleAnimation(target, targetSize, duration));
        }

        public void MoveUI(RectTransform target, Vector2 targetPos, float duration)
        {
            StartCoroutine(MoveAnimation(target, targetPos, duration));
        }
        
        public void MoveAndBack(RectTransform  target, Vector2 targetPos, float duration)
        {
            StartCoroutine(MoveAndBackAnimation(target, targetPos, duration));
        }
        
        public void ScaleUpDownUI(RectTransform target, Vector2 targetSize, float duration)
        {
            StartCoroutine(ScaleUpDownAnimation(target, targetSize, duration));
        }
        
        private IEnumerator MoveAndBackAnimation(RectTransform  target, Vector2 targetPos, float duration)
        {
            Vector2 startPos = target.anchoredPosition;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseInOut(t); 
                target.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.anchoredPosition = targetPos;
            
            yield return new WaitForSeconds(1f);

            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseInOut(t); 
                target.anchoredPosition = Vector2.Lerp(targetPos, startPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            target.anchoredPosition = startPos;
        }

        private IEnumerator ScaleUpDownAnimation(RectTransform target, Vector2 targetSize, float duration)
        {
            Vector2 startSize = target.sizeDelta; 
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseInOut(t);
                target.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.sizeDelta = targetSize;

            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseOut(t);
                target.sizeDelta = Vector2.Lerp(targetSize, startSize, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.sizeDelta = startSize;
        }

        private IEnumerator ScaleAnimation(RectTransform target, Vector2 targetSize, float duration)
        {
            Vector2 startSize = target.sizeDelta;
            float elapsedTime = 0;
            
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseInOut(t);
                target.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.sizeDelta = targetSize;
        }

        private IEnumerator MoveAnimation(RectTransform target, Vector2 targetPos, float duration)
        {
            Vector2 startPos = target.anchoredPosition;
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                t = UIEaseUtility.EaseInOut(t); 
                target.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.anchoredPosition = targetPos;
        }
        
        public void ResetPosition(RectTransform target, Vector3 originalPosition, float duration)
        {
            StartCoroutine(MoveAnimation(target, originalPosition, duration));
        }
        
        public void ResetScale(RectTransform  target, Vector2 originalSize, float duration)
        {
            StartCoroutine(ScaleAnimation(target, originalSize, duration));
        }
   }
}

