using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RollaBall.UI
{
    public class UIMenu : MonoBehaviour
    {
        [Header("First Selected Button")]
        [SerializeField] private Button firstSelected;

        protected virtual void OnEnable()
        {
            SetFirstSelected(firstSelected);
        }

        public void SetFirstSelected(Button firstSelectedButton)
        {
            firstSelectedButton.Select();
        }
    }
}