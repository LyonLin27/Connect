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

    private int cost = 5;
    private int deltaCost = 5;

    private void Awake() {
        //Agents = new List<AgentController>();
    }

    void Start()
    {
        powCurText = transform.Find("pow/Number/text").GetComponent<TextMeshProUGUI>();
        accCurText = transform.Find("acc/Number/text").GetComponent<TextMeshProUGUI>();
        frtCurText = transform.Find("frt/Number/text").GetComponent<TextMeshProUGUI>();
        spdCurText = transform.Find("spd/Number/text").GetComponent<TextMeshProUGUI>();
        ppdCurText = transform.Find("ppd/Number/text").GetComponent<TextMeshProUGUI>();
        skpCurText = transform.Find("skp/Number/text").GetComponent<TextMeshProUGUI>();
        cstCurText = transform.Find("cst/Number/text").GetComponent<TextMeshProUGUI>();
        powUpBtn = transform.Find("pow/BtnPanel/Btn").GetComponent<Button>();
        accUpBtn = transform.Find("acc/BtnPanel/Btn").GetComponent<Button>();
        frtUpBtn = transform.Find("frt/BtnPanel/Btn").GetComponent<Button>();
        spdUpBtn = transform.Find("spd/BtnPanel/Btn").GetComponent<Button>();
        ppdUpBtn = transform.Find("ppd/BtnPanel/Btn").GetComponent<Button>();
        skpUpBtn = transform.Find("skp/BtnPanel/Btn").GetComponent<Button>();

        powUpBtn.onClick.AddListener(OnPowUp);
        accUpBtn.onClick.AddListener(OnAccUp);
        frtUpBtn.onClick.AddListener(OnFrtUp);
        spdUpBtn.onClick.AddListener(OnSpdUp);
        ppdUpBtn.onClick.AddListener(OnPpdUp);
        skpUpBtn.onClick.AddListener(OnSkpUp);

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
        if (GameMan.Instance.money >= cost) {
            GameMan.Instance.SpendMoney(cost);
        }
        else {
            return;
        }

        pow = pow > 150 ? (int)(pow * 1.1f) : (int)(pow * 1.2f);
        cost += deltaCost;
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

        if (GameMan.Instance.money >= cost) {
            GameMan.Instance.SpendMoney(cost);
        }
        else {
            return;
        }

        acc += 2;
        cost += deltaCost;
        accCurText.text = acc.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.shootAcc = 20 - acc;
        }
    }

    public void OnFrtUp() {
        if (frt <= 0.1f) {
            frt = 0.1f;
            foreach (AgentController ac in Agents) {
                ac.shootCD = frt;
            }
            return;
        }

        if (GameMan.Instance.money >= cost) {
            GameMan.Instance.SpendMoney(cost);
        }
        else {
            return;
        }

        frt *= 0.8f;
        cost += deltaCost;
        frtCurText.text = string.Format("{0:0.00}", frt); // frt.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.shootCD = frt;
        }
    }

    public void OnSpdUp() {
        if (GameMan.Instance.money >= cost) {
            GameMan.Instance.SpendMoney(cost);
        }
        else {
            return;
        }

        spd += 1;
        cost += deltaCost;
        spdCurText.text = spd.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.speed = spd;
            ac.speedLimit = spd * 1.5f;
        }
    }

    public void OnPpdUp() {
        if (GameMan.Instance.money >= cost) {
            GameMan.Instance.SpendMoney(cost);
        }
        else {
            return;
        }

        ppd += 2;
        cost += deltaCost;
        ppdCurText.text = ppd.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            ac.projSpd = ppd;
        }
    }

    public void OnSkpUp() {
        if (skp >= 9) {
            skp = 9;
            return;
        }

        if (GameMan.Instance.money >= cost) {
            GameMan.Instance.SpendMoney(cost);
        }
        else {
            return;
        }

        skp += 1;
        cost += deltaCost;
        skpCurText.text = skp.ToString();
        cstCurText.text = cost.ToString();
        foreach (AgentController ac in Agents) {
            GameMan.Instance.waveMan.startWave = skp;
            GameMan.Instance.waveMan.currWave = skp;
        }
    }

}
