using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionBtn : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler*/ {

    [SerializeField] OptionsManager optionManager;

    //public Image btnBackground;
    public GameObject optionScreen;

    private void Start() {
        //btnBackground = GetComponent<Image>();
        optionScreen.SetActive(false);
    }

    //public void OnPointerClick(PointerEventData eventData) {
    //    optionManager.OnButtonSelect(this);
    //}

    //public void OnPointerEnter(PointerEventData eventData) {
    //    optionManager.OnButtonEnter(this);
    //}

    //public void OnPointerExit(PointerEventData eventData) {
    //    optionManager.OnButtonExit(this);
    //}
}
