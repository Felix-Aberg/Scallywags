using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ScallyWags {

public class SetObjective : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    [SerializeField] private StringVariable _tutorial1;
    [SerializeField] private StringVariable _tutorial2;
    [SerializeField] private StringVariable _tutorial3;
    [SerializeField] private StringVariable _tutorial4;
    [SerializeField] private StringVariable _protectTreasure;
    void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        _textMesh.text = "";
    }

    private void OnEnable()
    {
        EventManager.StartListening("Intro1", FirstTutorial);
        EventManager.StartListening("Intro2", SecondTutorial);
        EventManager.StartListening("Intro3", ThirdTutorial);
        EventManager.StartListening("KrakenIntro", FourthTutorial);
        EventManager.StartListening("protectTreasure", ProtectTreasure);
        EventManager.StartListening("IntroDone", ClearText);
    }
    private void OnDisable()
    {
        StopListeningForEvents();
    }
    
    private void ClearText(EventManager.EventMessage msg)
    {
        _textMesh.text = "";
    }

    private void FirstTutorial(EventManager.EventMessage msg)
    {
        _textMesh.text = _tutorial1.Value;
    }
    private void SecondTutorial(EventManager.EventMessage msg)
    {
        _textMesh.text = _tutorial2.Value;
    }

    private void ThirdTutorial(EventManager.EventMessage msg)
    {
        _textMesh.text = _tutorial3.Value;
    }
    private void FourthTutorial(EventManager.EventMessage msg)
    {
        _textMesh.text = _tutorial4.Value;
    }
    private void ProtectTreasure(EventManager.EventMessage arg0)
    {
        _textMesh.text = _protectTreasure.Value;
        StopListeningForEvents();
    }

    private void StopListeningForEvents()
    {
        EventManager.StopListening("Intro1", FirstTutorial);
        EventManager.StopListening("Intro2", SecondTutorial);
        EventManager.StopListening("Intro3", ThirdTutorial);
        EventManager.StopListening("KrakenIntro", FourthTutorial);
        EventManager.StopListening("protectTreasure", ProtectTreasure);
        EventManager.StopListening("IntroDone", ClearText);
    }
}
}