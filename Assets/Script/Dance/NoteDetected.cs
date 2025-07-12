using System;
using UnityEngine;

namespace Script.Dance
{
    public class NoteDetected : MonoBehaviour
    {
        public bool canTouch,NoTouch;
        public string arrow;
        private GameObject _arrow;

        public void Update()
        {
            if(canTouch && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                NoTouch = true;
                if (touch.phase == TouchPhase.Began)
                {
                    
                    if (_arrow != null)
                    {
                        DanceController.Instance.GoodNote();
                        Destroy(_arrow);
                        _arrow = null;
                    }

                   
                    canTouch = false;
                }
            }
            else
            {
                NoTouch = false;
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(arrow))
            {
                canTouch = true;
                _arrow = other.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject == _arrow)
            {
                if (canTouch)
                {
                    DanceController.Instance.MissedNote();
                    
                }
                canTouch = false;
                _arrow = null;
                
            }
        }
    }
}