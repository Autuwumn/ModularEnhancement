using UnityEngine;

namespace ME.Monos
{
    public class DisplayIdentifier : MonoBehaviour
    {
        public ModularArmor owner;
        public CardInfo card;
        public Vector2 bounds = new Vector2(2.3f, 6.25f);
        public bool isOn = false;
    }
}