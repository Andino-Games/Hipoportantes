using System;
using UnityEngine;

namespace Script.Dance
{
    public class NoteDetected : MonoBehaviour
    {
        public bool canTouch,NoTouch;
        public string arrow;
        private GameObject _arrow;
        public Collider2D col;

        public void Update()
        {
            if(canTouch)
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
                 canTouch = false;
                _arrow = null;
                
            }
        }

        public void ActivateColliders()
        {
            col.enabled = true;
        }

        public void DesactivateCollider()
        {
            col.enabled = false;
            
        }
    }
}