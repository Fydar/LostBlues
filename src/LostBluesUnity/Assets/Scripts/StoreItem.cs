using UnityEngine;
using UnityEngine.UI;

namespace LostBluesUnity
{
    public class StoreItem : MonoBehaviour
    {
        public static StoreItem CurrentlySelected;

        [Header("General")]
        public int CostAmount = 10;
        public bool InisideTerrain;

        [Header("Setup")]
        public GameObject[] Prefab;
        public Graphic[] SelectedTinted;
        public Graphic[] AfforadbleTinted;
        public Text CostText;

        [Space]
        public Color DeselectColor;
        public Color SelectColor;

        private void Start()
        {
            if (CostText != null)
            {
                CostText.text = CostAmount.ToString();
            }

            Deselect();

            bool affordableNow = Game.Instance.Currency.Value >= CostAmount;
            foreach (var affordableTint in AfforadbleTinted)
            {
                affordableTint.color = affordableNow ? SelectColor : DeselectColor;
            }

            Game.Instance.Currency.onChanged += () =>
            {
                bool affordable = Game.Instance.Currency.Value >= CostAmount;
                foreach (var affordableTint in AfforadbleTinted)
                {
                    affordableTint.color = affordable ? SelectColor : DeselectColor;
                }
            };
        }

        private void Update()
        {
            if (CurrentlySelected == this)
            {
                if (Game.Instance.Currency.Value < CostAmount)
                {
                    CurrentlySelected.Deselect();
                }
            }
        }

        public void Select()
        {
            if (CurrentlySelected != null)
            {
                CurrentlySelected.Deselect();
            }

            foreach (var graphic in SelectedTinted)
            {
                graphic.color = SelectColor;
            }
            CurrentlySelected = this;
        }

        public void Deselect()
        {
            if (CurrentlySelected == this)
            {
                CurrentlySelected = null;
            }
            foreach (var graphic in SelectedTinted)
            {
                graphic.color = DeselectColor;
            }
        }

        public void UiClick()
        {
            if (CurrentlySelected == this)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }
    }
}
