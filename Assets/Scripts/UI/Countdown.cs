using HexUN.Events;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField]
        public VoidReliableEvent _onCounted;

        [SerializeField]
        TextMeshProUGUI _text;

        [SerializeField]
        int countFrom;

        [SerializeField]
        string message;

        [SerializeField]
        bool countOnStart = false;

        public void Start()
        {
            if(countOnStart) DoCountdown();
        }

        public void DoCountdown()
        {
            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            int count = countFrom;
            _text.enabled = true;

            while(count > 0)
            {
                _text.text = count.ToString();
                count--;
                yield return new WaitForSeconds(1f);
            }

            _text.text = message;
            yield return new WaitForSeconds(1f);

            _text.enabled = false;
            _onCounted.Invoke();
        }
    }
}