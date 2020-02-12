using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour {

    public List<AgentController> Agents;
    public int pow = 10;
    public float acc = 0;
    public float frt = 0.5f;
    public float spd = 10;
    public float ppd = 20;
    public int skp = 0;


    private TextMeshProUGUI powCurText;
    private TextMeshProUGUI accCurText;
    private TextMeshProUGUI frtCurText;
    private TextMeshProUGUI spdCurText;
    private TextMeshProUGUI ppdCurText;
    private TextMeshProUGUI skpCurText;
    private TextMeshProUGUI cstCurText;
    private Button powUpBtn;
    private Button accUpBtn;
    private Button frtUpBtn;
    private Button spdUpBtn;
    private Button ppdUpBtn;
    private Button skpUpBtn;

    private int cost = 10;

    private void Awake() {
        //Agents = new List<AgentController>();
    }

    void Start()
    {
        powCurText = transform.Find("PowCur").GetComponent<TextMeshProUGUI>();
        accCurText = transform.Find("AccCur").GetComponent<TextMeshProUGUI>();
        frtCurText = transform.Find("FrtCur").GetComponent<TextMeshProUGUI>();
        spdCurText = transform.Find("SpdCur").GetComponent<TextMeshProUGUI>();
        ppdCurText = transform.Find("PpdCur").GetComponent<TextMeshProUGUI>();
        skpCurText = transform.Find("SkpCur").GetComponent<TextMeshProUGUI>();
        cstCurText = transform.Find("CstCur").GetComponent<TextMeshProUGUI>();
        powUpBtn = transform.Find("PowUpBtn").GetComponent<Button>();
        accUpBtn = transform.Find("AccUpBtn").GetComponent<Button>();
        frtUpBtn = transform.Find("FrtUpBtn").GetComponent<Button>();
        spdUpBtn = transform.Find("SpdUpBtn").GetComponent<Button>();
        ppdUpBtn = transform.Find("PpdUpBtn").GetComponent<Button>();
        skpUpBtn = transform.Find("SkpUpBtn").GetComponent<Button>();


        powCurText.text = pow.ToString();
        accCurText.text = acc.ToString();
        frtCurText.text = frt.ToString();
        spdCurText.text = spd.ToString();
        ppdCurText.text = ppd.ToString();
        skpCurText.text = skp.ToString();
        cstCurText.text = cost.ToString();
    }

    public void Sync(AgentController ac) {
        ac.power = pow;
        ac.shootAcc = 20 - acc;
        ac.shootCD = frt;
        ac.speed = spd;
        ac.projSpd = ppd;
        ac.speedLimit = spd * 1.5f;
    }

    public void OnPowUp() {
        pow += 2;
        cost += 2;
        powCurText.text = pow.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.power = pow;
        }
    }

    public void OnAccUp() {
        if (acc >= 20) {
            acc = 20;
            return;
        }

        acc += 2;
        cost += 2;
        accCurText.text = acc.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.shootAcc = 20 - acc;
        }
    }

    public void OnFrtUp() {
        if (frt <= 0.1f) {
            frt = 0.1f;
            return;
        }

        frt -= 0.1f;
        cost += 2;
        frtCurText.text = frt.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.shootCD = frt;
        }
    }

    public void OnSpdUp() {
        spd += 1;
        cost += 2;
        spdCurText.text = spd.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.speed = spd;
            ac.speedLimit = spd * 1.5f;
        }
    }

    public void OnPpdUp() {
        ppd += 2;
        cost += 2;
        ppdCurText.text = ppd.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.projSpd = ppd;
        }
    }

    public void OnSkpUp() {
        skp += 1;
        cost += 2;
        skpCurText.text = skp.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            GameMan.Instance.waveMan.startWave = skp;
        }
    }

}
