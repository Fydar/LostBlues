using System;
using UnityEngine;
using UnityEngine.UI;

namespace LostBluesUnity
{
    public class StoreUpgrade : MonoBehaviour
    {
        [Serializable]
        public struct Levels
        {
            public int NextCost;
            public int Damage;
        }

        public static StoreUpgrade Instance;

        public int CurrentLevel;

        [Header("General")]
        public Levels[] Upgrades;

        [Header("Setup")]
        public Graphic[] SelectedTinted;
        public Graphic[] AfforadbleTinted;
        public GameObject[] Hides;
        public GameObject[] Shows;
        public Text CostText;
        public Image Fill;

        [Space]
        public Color DeselectColor;
        public Color SelectColor;

        public bool IsAtMaxLevel => CurrentLevel == Upgrades.Length - 1;

        public int CurrentDamage => Upgrades[CurrentLevel].Damage;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!IsAtMaxLevel)
            {
                if (CostText != null)
                {
                    CostText.text = Upgrades[CurrentLevel].NextCost.ToString();
                }
            }

            foreach (var item in Hides)
            {
                item.SetActive(!IsAtMaxLevel);
            }
            foreach (var item in Shows)
            {
                item.SetActive(IsAtMaxLevel);
            }

            bool affordable = IsAtMaxLevel || Game.Instance.Currency.Value >= Upgrades[CurrentLevel].NextCost;
            foreach (var affordableTint in AfforadbleTinted)
            {
                affordableTint.color = affordable ? SelectColor : DeselectColor;
            }
        }

        public void UiClick()
        {
            if (IsAtMaxLevel)
            {
                return;
            }
            bool affordable = Game.Instance.Currency.Value >= Upgrades[CurrentLevel].NextCost;

            if (affordable)
            {
                Game.Instance.Currency.Value -= Upgrades[CurrentLevel].NextCost;
                CurrentLevel++;

                Fill.fillAmount = ((float)CurrentLevel) / (Upgrades.Length - 1);
            }
        }
    }
}
