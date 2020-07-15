using TMPro;
using UnityEngine;
using YUR.Fit.Core.Models;

public class Main : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textStart = null;
    [SerializeField]
    private TextMeshProUGUI textUpdate = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnYURStartup(){
         textStart.text = "aaaaa";
    }

    public void OnYURUpdate(OverlayStatusUpdate obj){
         textUpdate.text = "aa";
    }
}
