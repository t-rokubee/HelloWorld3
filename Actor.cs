using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BadState
{
    None,
    KnockedOut,
    Poison

}

public enum GoodState
{
    None,
    StrUp
}


// 敵・味方キャラクターの共通クラス
public class Actor : MonoBehaviour {

    // ID
    public int m_id;
    // 名前
    public string m_name;
    
    List<BadState> m_badStates = new List<BadState>();      // クラスに要変更
    List<GoodState> m_goodStates = new List<GoodState>();   // クラスに要変更


    // メッセージ関連
    public string RecoveryHpFormat = "{0}のHPが{1}回復した";
    public string RecoveryMpFormat = "{0}のMPが{1}回復した";
    public string DamageHpFormat = "  → {0}は{1}のダメージ！";
    public string DamageMpFormat = "  → {0}は{1}のMPダメージ！";

    // 基本ステータス
    public int Hp;
    public int Mp;

    int MaxHp;
    int MaxMp;
    int Str;
    int Vit;
    int Mag;
    int Mnd;
    int Dex;
    int Agi;

    // 基本ステータスと装備と付与効果により決められる値
    public int Speed;
    public int AttackVal;
    public int DefenseVal;
    public int Accuracy;
    public int Avoidance;
    public int MagicAttackVal;
    public int MagicDefenseVal;
    public int MagicAccuracy;
    public int MagicAvoidance;

    // 共通パラメータの設定
    public void SetupActor(int id)
    {
        // (暫定)
        if(id == 1)
        {
            m_id = 1;
            m_name = "八衛兵";
            MaxHp = 100;
            MaxMp = 20;
            Hp = 60;
            Mp = 15;
            Str = 12;
            Vit = 12;
            Mag = 10;
            Mnd = 9;
            Dex = 11;
            Agi = 9;
        }
    }


    // サブステータス更新
    public virtual void UpdateStatus()
    {
        Speed = Agi;
        AttackVal = Str;
        DefenseVal = Vit;
        Accuracy = 90 + Dex;
        Avoidance = Agi + Dex / 2;

        MagicAttackVal = Mag;
        MagicDefenseVal = Mag / 2;
        MagicAccuracy = 100 + Dex / 2;
        MagicAvoidance = Mnd + Agi;
    }

    public void RecoverHp(int amount)
    {
        Hp += amount;
        if (Hp > MaxHp) Hp = MaxHp;
        string message = System.String.Format(RecoveryHpFormat,
                                        m_name, amount);
        Debug.Log(message);
    }

    // ダメージ適用処理
    public void ApplyDamage(int damage)
    {
        Hp -= damage;

        string message = System.String.Format(DamageHpFormat,
                                                m_name,
                                                damage);
        // Animation

        Debug.Log(message);
        Debug.Log("残りHP=" + Hp);
        if (Hp <= 0)
        {
            Hp = 0;
            BeKockDown();
        }
    }


    // 戦闘不能処理
    public void BeKockDown()
    {
        string message = System.String.Format("{0}は倒れた・・", m_name);
        Debug.Log(message);

        // 状態を全部取り除いて、戦闘不能に
        m_goodStates.Clear();
        m_badStates.Clear();
        m_badStates.Add(BadState.KnockedOut);
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
