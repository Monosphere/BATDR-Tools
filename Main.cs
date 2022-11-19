using HarmonyLib;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BendyTools
{
    public class Main : MelonMod
    {
        public ToolsHandler toolsHandler;
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            if(sceneName =="InitializeGame")
            {
                if (toolsHandler == null)
                {
                    Debug.Log("Slayification");
                    GameObject obj = new GameObject("ToolManager");
                    toolsHandler = obj.AddComponent<ToolsHandler>();
                }
            }
        }

    }
    public class ToolsHandler : MonoBehaviour
    {
        public Player player;
        float vclipStrength = 4;
        float walkStrength = 2;
        string pipeType;
        bool collectAllBooks = false;
        WeaponData data = new WeaponData();

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {

        }
        void Update()
        {
            if (player != null)
            {
                if (Input.GetKeyDown(KeyCode.V))
                {
                    player.gameObject.transform.position += player.GameCamera.transform.forward * vclipStrength;
                }
                Traverse.Create(player.PlayerMovement).Field("m_RunSpeed").SetValue(walkStrength*2);
                Traverse.Create(player.PlayerMovement).Field("m_MoveSpeed").SetValue(walkStrength);
                if (collectAllBooks)
                {
                    GameObject IllusionBook = GameObject.Find("IllusionContent_Default(Clone)");
                    if (IllusionBook != null && IllusionBook.GetComponent<InteractableProximity>() != null)
                    {
                        IllusionBook.GetComponent<InteractableProximity>().Interact(new Vector3(), new RaycastHit());
                    }
                }
            }
        }
        public void OnGUI()
        {
            if (GUI.Button(new Rect(20, 20, 200, 20), "Locate Player"))
            {
                if (MonoBehaviour.FindObjectOfType<Player>() != null)
                {
                    player = MonoBehaviour.FindObjectOfType<Player>();
                    BendyToolsExternal.Message("Located Player");
                }
            }
            if (player == null)
            {
                GUI.Box(new Rect(20, 50, 200, 30), "Locate Player To Begin");
            }
            else
            {
                GUI.Box(new Rect(20, 50, 200, 330), "Mono's BATDR Tools");
                if (GUI.Button(new Rect(25, 70, 190f, 20f), "Heal"))
                {
                    player.Heal(50);
                    WeaponData data = new WeaponData();
                    data.WeaponType = WeaponType.LEVEL_4;
                    player.EquipWeapon(data);
                }
                if (GUI.Button(new Rect(25, 100, 190f, 20f), "Obtain Gent Pipe 1-4"))
                {
                    switch(int.Parse(pipeType))
                    {
                        case 1:
                            data.WeaponType = WeaponType.LEVEL_1;
                            break;
                        case 2:
                            data.WeaponType = WeaponType.LEVEL_2;
                            break;
                        case 3:
                            data.WeaponType = WeaponType.LEVEL_3;
                            break;
                        case 4:
                            data.WeaponType = WeaponType.LEVEL_4;
                            break;
                    }
                    player.EquipWeapon(data);
                }
                pipeType = GUI.TextField(new Rect(25, 120, 190f, 20f), pipeType);
                GUI.Label(new Rect(25, 150, 150, 80), "VClip Strength");
                vclipStrength = GUI.HorizontalSlider(new Rect(25, 170, 190f, 20f), vclipStrength, 4, 15);
                GUI.Label(new Rect(25, 200, 200, 80), "Walk Speed");
                walkStrength = GUI.HorizontalSlider(new Rect(25, 220, 190f, 20f), walkStrength, 2, 10);
                collectAllBooks = GUI.Toggle(new Rect(25, 240, 190f, 20f),collectAllBooks,"Collect All Books");
            }
        }
    }
    static class BendyToolsExternal
    {
        public static void Message(string text)
        {
            MelonDebug.Msg("-Bendy Tools-\n" + text+"\n");
        }
        public static void Error(string text)
        {
            MelonDebug.Error("-Bendy Tools-\n" + text + "\n");
        }
    }
}
