using ME.Cards;
using ModdingUtils.MonoBehaviours;
using ModsPlus;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnboundLib.GameModes;
using UnboundLib;
using UnityEngine;
using ME.Extensions;
using System.Collections;

namespace ME.Monos
{
    public class ModularArmor : CardEffect
    {
        public List<int> mods = new List<int>();
        public List<int> savedMods = new List<int>();
        public List<GameObject> modObjs = new List<GameObject>();
        public bool editingMods = false;
        public int slots;
        public int boosts;
        public string[] statsS = new string[] { "Damage", "Health", "Movement Speed", "Jump-Height", "Block Duration" };
        public string[] statsE = new string[] { "%", "%", "%", "%", "s" };
        public float[] statsN = new float[] { 1, 1, 1, 1, 1 };
        public GameObject modularDisplay;
        protected override void OnDestroy()
        {
            GameModeManager.RemoveHook(GameModeHooks.HookBattleStart, PointStart);
            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, PointEnd);
        }
        protected override void Start()
        {
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, PointStart);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
            foreach (var c in player.data.currentCards)
            {
                if (c == ModularMK1.card || c == ModularMK2.card || c == ModularMK3.card)
                {
                    slots = int.Parse(c.cardStats[0].amount);
                    boosts = int.Parse(c.cardStats[1].amount);
                }
            }
            BuildModular();
        }
        public void BuildModular()
        {
            var size = slots + boosts;
            for (int i = 0; i < size; i++) mods.Add(0);
        }
        void HoverOverCard(CardInfo card)
        {
            print(card.cardName);
        }
        private void ShowModularDisplay()
        {
            editingMods = true;
            if (modularDisplay != null) { Destroy(modularDisplay); }
            modularDisplay = Instantiate(ME.ArtAssets.LoadAsset<GameObject>("CardDisplaySystem"));
            var a = Instantiate(ME.ArtAssets.LoadAsset<GameObject>("ModularGreyedout"), modularDisplay.transform);
            a.GetComponent<SpriteRenderer>().sortingLayerID = -1160774667;

            modularDisplay.transform.position = Vector3.zero;
            float modsAmt = mods.Count / 4;
            modsAmt = 0;
            for (int i = 0; i < modsAmt; i++)
            {
                var obj = Instantiate(ME.ArtAssets.LoadAsset<GameObject>("USBHub"), modularDisplay.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0));
                obj.transform.GetChild(0).GetComponent<Canvas>().sortingLayerID = -1160774667;
                //obj.transform.position = new Vector3((12.2f * (modsAmt - 1)) / 2f - (12.2f * i), 0, 0);
                modularDisplay = obj.transform.parent.gameObject;
                obj.SetActive(false);
            }
            List<CardInfo> modsPlayerHas = new List<CardInfo>();
            foreach (var c in player.data.currentCards)
            {
                if (c.categories.Contains(ME.Module))
                {
                    modsPlayerHas.Add(c);
                }
            }
            modObjs.Clear();
            var needed = new int[ME.Modules.Length];
            foreach (var m in mods)
            {
                if (m != 0)
                {
                    needed[m]++;
                }
            }
            for (int i = 0; i < modsPlayerHas.Count; i++)
            {
                var c = modsPlayerHas[i];
                Vector3 pos = new Vector3((-10f * (modsPlayerHas.Count - 1)) / 2f + (10f * i), -10, 0);
                var card = GenerateCard(c, modularDisplay.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0));
                modObjs.Add(card);
                card.gameObject.AddComponent<DisplayIdentifier>().owner = this;
                card.gameObject.GetComponent<DisplayIdentifier>().card = c;
                var num = ME.Modules.ToList().IndexOf(c);
                if (needed[num] > 0)
                {
                    card.GetComponent<DisplayIdentifier>().isOn = true;
                    needed[num]--;
                    ME.instance.ExecuteAfterFrames(5, () =>
                    {
                        card.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 0);
                    });
                }
                //card.transform.localPosition = pos;
            }
        }
        public override void OnTakeDamage(Vector2 damage, bool selfDamage)
        {
            if(damage.magnitude > player.data.health)
            {
                HideModularDisplay();
            }
        }
        private GameObject GenerateCard(CardInfo cardToMake, Transform daddy)
        {
            GameObject cardObj = GameObject.Instantiate<GameObject>(cardToMake.gameObject, daddy);
            cardObj.SetActive(true);
            cardObj.transform.Rotate(0, 0, 0);
            cardObj.GetComponentInChildren<CardVisuals>().firstValueToSet = true;
            Destroy(cardObj.transform.GetComponentInChildren<Collider2D>().gameObject);
            RectTransform rect = cardObj.GetOrAddComponent<RectTransform>();
            rect.localScale = Vector3.one * 20;
            rect.transform.localPosition /= 20;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);

            GameObject back = FindObjectInChildren(cardObj, "Back");
            try
            {
                GameObject.Destroy(back);
            }
            catch { }
            FindObjectInChildren(cardObj, "BlockFront")?.SetActive(false);

            var canvasGroups = cardObj.GetComponentsInChildren<CanvasGroup>();
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.alpha = 1;
            }

            this.ExecuteAfterSeconds(0.2f, () =>
            {
                var rarities = cardObj.GetComponentsInChildren<CardRarityColor>();

                foreach (var rarity in rarities)
                {
                    try
                    {
                        rarity.Toggle(true);
                    }
                    catch
                    {

                    }
                }
            });
            return cardObj;
        }
        private static GameObject FindObjectInChildren(GameObject gameObject, string gameObjectName)
        {
            Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
            return (from item in children where item.name == gameObjectName select item.gameObject).FirstOrDefault();
        }
        private void HideModularDisplay()
        {
            editingMods = false;
            modObjs.Clear();
            Destroy(modularDisplay);
        }
        private void Update()
        {
            if (editingMods)
            {
                var cursX = MainCam.instance.cam.ScreenToWorldPoint(Input.mousePosition).x;
                var cursY = MainCam.instance.cam.ScreenToWorldPoint(Input.mousePosition).y;
                var cursPos = new Vector3(cursX, cursY, player.transform.position.z);
                var bounds = new Vector3(6.25f, 45f, 0);
                foreach (var go in modObjs)
                {
                    var cur = Input.mousePosition;
                    var up = go.transform.position + new Vector3(148, 45, 0);
                    var down = go.transform.position - new Vector3(77, 45, 0);
                    if (cur.x > down.x && cur.x < up.x && cur.y > down.y && cur.y < up.y)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (go.GetComponent<DisplayIdentifier>().isOn == false)
                            {
                                if (mods.IndexOf(0) != -1)
                                {
                                    go.GetComponent<DisplayIdentifier>().isOn = true;
                                    var firstInstance = mods.IndexOf(0);
                                    var modulId = ME.Modules.ToList().IndexOf(go.GetComponent<DisplayIdentifier>().card);
                                    mods[firstInstance] = modulId;
                                    go.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 0);
                                }
                            }
                            else
                            {
                                go.GetComponent<DisplayIdentifier>().isOn = false;
                                var modulId = ME.Modules.ToList().IndexOf(go.GetComponent<DisplayIdentifier>().card);
                                var firstInstance = mods.IndexOf(modulId);
                                mods[firstInstance] = 0;
                                go.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color(0.6792f, 0.6792f, 0.6792f);
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.M) && player.data.view.IsMine)
            {
                if (!editingMods)
                {
                    ShowModularDisplay();
                }
                else
                {
                    HideModularDisplay();
                }
            }
            if (GameObject.Find("__MODLR__Modular Stats(Clone)") != null)
            {
                var ploy = 0;
                for (var i = 0; i < PlayerManager.instance.players.Count(); i++)
                {
                    var name = "Bar" + (i + 1);
                    var bar = GameObject.Find(name);
                    for (var j = 1; j < bar.transform.childCount; j++)
                    {
                        var c = bar.transform.GetChild(j).gameObject;
                        if (c.GetComponent<HoverEvent>().isHovered)
                        {
                            ploy = i;
                        }
                    }
                }
                var ma = PlayerManager.instance.players[ploy].transform.GetComponentInChildren<ModularArmor>();
                var newVal = ma.statsN;
                var hoveredCard = GameObject.Find("__MODLR__Modular Stats(Clone)");
                var statObj = hoveredCard.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(4).gameObject;
                var grid = hoveredCard.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;
                for (var i = grid.transform.childCount - 1; i > 0; i--) Destroy(grid.transform.GetChild(i).gameObject);
                for (var i = 0; i < newVal.Length; i++)
                {
                    float val = (int)((newVal[i] - 1) * 100);
                    float valNeg = (int)((newVal[i]) * 100);
                    switch (statsE[i])
                    {
                        case "s": val = newVal[i]-1;  break;
                    }
                    if (newVal[i] > 1)
                    {
                        var temp = Instantiate(statObj, grid.transform);
                        temp.SetActive(true);
                        temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + val + statsE[i];
                        temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = statsS[i];
                    }
                    if (newVal[i] < 1)
                    {
                        var temp = Instantiate(statObj, grid.transform);
                        temp.SetActive(true);
                        temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0.6981f, 0.326f, 0.326f);
                        temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + valNeg + statsE[i];
                        temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = statsS[i];
                    }
                }
            }
            if (savedMods != mods && player.GetComponentInChildren<ModularStats>() != null)
            {
                player.GetComponentInChildren<ModularStats>().DoStatUpdate(player.playerID, mods.ToArray());
                savedMods = mods;
            }
        }
        public void ListReadInt(List<int> list)
        {
            string thingy = "";
            foreach (int i in list)
            {
                thingy += i + ", ";
            }
            print(thingy);
        }
        IEnumerator PointStart(IGameModeHandler gm)
        {
            statsN = new float[] { 1, 1, 1, 1, 1 };
            float mult = 1;
            foreach (var m in mods)
            {
                if (m == 1) mult *= 1.20f;
            }
            foreach (var m in mods)
            {
                switch (m)
                {
                    case 2: statsN[0] *= 1.3f * mult; break;
                    case 3: statsN[1] *= 1.3f * mult; break;
                    case 4: statsN[2] *= 1.2f * mult; statsN[3] *= 1.2f * mult; break;
                    case 5: statsN[4] += 0.25f * mult; break;
                }
            }
            ReversibleEffect reversibleEffect = player.gameObject.AddComponent<ReversibleEffect>();
            reversibleEffect.gunStatModifier.damage_mult = statsN[0];
            reversibleEffect.characterDataModifier.maxHealth_mult = statsN[1];
            reversibleEffect.stats.movementSpeed = statsN[2];
            reversibleEffect.stats.jump = statsN[3];
            player.data.stats.GetAdditionalData().extraBlockTime += statsN[4] - 1;
            var ratio = 1f / ((0.3f + player.data.stats.GetComponent<CharacterStatModifiers>().GetAdditionalData().extraBlockTime) / 0.3f);
            var main = block.particle.main;
            main.simulationSpeed = ratio;
            reversibleEffect.SetLivesToEffect(player.data.stats.respawns + 1);
            yield break;
        }
        IEnumerator PointEnd(IGameModeHandler gm)
        {
            player.data.stats.GetAdditionalData().extraBlockTime -= statsN[4] - 1;
            var ratio = 1f / ((0.3f + player.data.stats.GetComponent<CharacterStatModifiers>().GetAdditionalData().extraBlockTime) / 0.3f);
            var main = block.particle.main;
            main.simulationSpeed = ratio;
            yield break;
        }
    }
}