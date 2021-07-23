using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorStatusUI : SingletonMonoBehavior<ActorStatusUI>
{
    Text status;
    Text nickname;
    Image icon;

    RectTransform mPBarGauge;
    RectTransform mPBar;
    RectTransform hPBarGauge;
    RectTransform hPBar;
    Image mPBarGaugeImage;
    Image hPBarGaugeImage;


    internal void Show(Actor actor)
    {
        base.Show();

        icon = transform.Find("Icon").GetComponent<Image>();
        icon.sprite = Resources.Load<Sprite>("Icon/" + actor.iconName);

        status = transform.Find("Status").GetComponent<Text>();
        nickname = transform.Find("Name").GetComponent<Text>();


        mPBarGauge = transform.Find("MPBar/mPBarGauge").GetComponent<RectTransform>();
        mPBar = transform.Find("MPBar/mPBar").GetComponent<RectTransform>();
        hPBarGauge = transform.Find("MPBar/hPBarGauge").GetComponent<RectTransform>();
        hPBar = transform.Find("MPBar/hPBar").GetComponent<RectTransform>();

        mPBarGaugeImage = mPBarGauge.GetComponent<Image>();
        hPBarGaugeImage = hPBarGauge.GetComponent<Image>();

        var size = mPBarGauge.sizeDelta;
        size.x = actor.maxMP;
        mPBarGauge.sizeDelta = size;
        mPBar.sizeDelta = size;

size = hPBarGauge.sizeDelta;
        size.x = actor.maxHP;
        hPBarGauge.sizeDelta = size;
        hPBar.sizeDelta = size;

        mPBarGaugeImage.fillAmount = actor.mp / actor.maxMp;
        hPBarGaugeImage.fillAmount = actor.hp / actor.maxHp;

        nickname.text = actor.nickname;
        status.text = actor.status.ToString();
    }

}
