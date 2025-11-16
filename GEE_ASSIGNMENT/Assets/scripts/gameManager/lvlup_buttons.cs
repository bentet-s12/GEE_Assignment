using UnityEngine;

public class lvlup_buttons : MonoBehaviour
{
    [SerializeField] private GameObject manager;
    [SerializeField] private levelling_logic lvlscript;
    [SerializeField] private lvlup_UI UI_control;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("gameManager");
        if (manager != null)
        {
            lvlscript = manager.GetComponent<levelling_logic>();
        }
        GameObject UI = GameObject.FindGameObjectWithTag("lvlupUI");
        if (UI != null)
        {
            UI_control = UI.GetComponent<lvlup_UI>();
        }
        }
    public void dmgup()
    {
        lvlscript.adddmg(10);
        UI_control.deactivateUI();
    }
    public void multishotup()
    {
        lvlscript.addmulti();
        UI_control.deactivateUI();
    }
    public void temphealthup()
    {
        lvlscript.addtemphealth(10);
        UI_control.deactivateUI();
    }
    public void speedup()
    {
        lvlscript.addtempspd(0.2f);
        UI_control.deactivateUI();
    }
    public void enableTP()
    {
        lvlscript.upgradeTP();
        UI_control.deactivateUI();
    }
    public void enableDJ()
    {
        lvlscript.upgradeDJ();
        UI_control.deactivateUI();
    }


}
