using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RPGSystem_Sample_GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryUI;
    private bool inventoryActive = false;

    [SerializeField]
    private GameObject[] inventorySlots = new GameObject[5];

    public Image[] inventoryImages = new Image[5];

    private Text[] inventoryNames = new Text[5];

    private GameObject[] inventoryItems;

    [SerializeField]
    private GameObject weightText;

    [SerializeField]
    private GameObject dpsText;

    [SerializeField]
    private GameObject player;
    private RPGSystem.RPGSystem_Character characterScript;

    [SerializeField]
    private GameObject sword;

    [SerializeField]
    private GameObject buySword;
    private bool swordBought = false;

    [SerializeField]
    private GameObject addXP;

    [SerializeField]
    private GameObject levelUpText;
    private bool levelUpStarted = false;


    [SerializeField]
    private GameObject levelScreenUI;
    private bool levelScreenActive = false;

    private Text levelsToSpend;
    private Text strengthLevelText;
    private Text constitutionLevelText;
    private Text dexterityLevelText;
    private Text willpowerLevelText;

    [SerializeField]
    private GameObject notEnoughLevels;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text mpText;

    [SerializeField]
    private GameObject deathText;

    [SerializeField]
    private GameObject shouldHealText;

    [SerializeField]
    private GameObject dontNeedHealsText;

    //set up link between inventory UI and player character inventory
    //also set up levels UI based on player character
    void Start()
    {
        inventoryUI.SetActive(inventoryActive);

        levelScreenUI.SetActive(levelScreenActive);

        characterScript = player.GetComponent<RPGSystem.RPGSystem_Character>();

        inventoryItems = new GameObject[5];

        for (int i = 0; i < 5; i++)
        {
            inventoryImages[i] = inventorySlots[i].GetComponent<Image>();
            inventoryNames[i] = inventorySlots[i].GetComponentInChildren<Text>();
        }

        //find and assign text objects to appropriate vars
        Text[] textInLevelsUI = levelScreenUI.GetComponentsInChildren<Text>();
        for (int i = 0; i < textInLevelsUI.Length; i++)
        {
            switch (textInLevelsUI[i].name)
            {
                case "LevelsToSpendText":
                    levelsToSpend = textInLevelsUI[i];
                    break;

                case "StrengthLevelText":
                    strengthLevelText = textInLevelsUI[i];
                    break;

                case "ConstitutionLevelText":
                    constitutionLevelText = textInLevelsUI[i];
                    break;

                case "DexterityLevelText":
                    dexterityLevelText = textInLevelsUI[i];
                    break;

                case "WillPowerLevelText":
                    willpowerLevelText = textInLevelsUI[i];
                    break;

                default:
                    break;
            }
        }

        GameObject potion = GameObject.Find("Healing Potion");
        characterScript.AddItem(1, potion.GetComponent<RPGSystem_Sample_HealingPotion>());
    }

    //handle all inputs except player and camera movement
    //also handle UI activation and deactivation
    void Update()
    {
        if (!characterScript.IsDead())
        {
            //Inventory screen
            if (Input.GetKeyDown("i"))
            {
                for (int i = 0; i < inventoryItems.Length; i++)
                {
                    if (characterScript.GetInventory().ContainsKey(i))
                    {
                        inventoryItems[i] = characterScript.GetInventory()[i].gameObject;
                        inventoryImages[i].sprite = inventoryItems[i].GetComponent<RPGSystem.RPGSystem_Item>().GetSprite();
                        inventoryNames[i].text = inventoryItems[i].GetComponent<RPGSystem.RPGSystem_Item>().GetName();
                    }
                }
                //workaround for always having unarmed item in character inventory dictionary (set at key: int.MaxValue)
                if (inventoryItems[0] == null)
                {
                    inventoryItems[0] = characterScript.GetInventory()[int.MaxValue].gameObject;
                    inventoryNames[0].text = inventoryItems[0].GetComponent<RPGSystem.RPGSystem_Item>().GetName();
                }
                inventoryActive = !inventoryActive;
                weightText.GetComponent<Text>().text = "Weight: " + (characterScript.m_carryCapacity - characterScript.GetCurrentInventoryWeight()) + " / " + characterScript.m_carryCapacity;
                dpsText.GetComponent<Text>().text = "DPS: " + characterScript.CalculateDPS();
                inventoryUI.SetActive(inventoryActive);
                if (levelScreenActive)
                {
                    levelScreenActive = false;
                    levelScreenUI.SetActive(levelScreenActive);
                }
            }

            //Levels screen
            if (Input.GetKeyDown("l"))
            {
                CheckAvailableLevels();
                levelScreenActive = !levelScreenActive;
                levelScreenUI.SetActive(levelScreenActive);
                if (inventoryActive)
                {
                    inventoryActive = false;
                    inventoryUI.SetActive(inventoryActive);
                }
            }

            //Handle arriving at store, and buying sword
            if (player.GetComponent<RPGSystem_Sample_PlayerMovement>().HasHitStore() && !swordBought)
            {
                buySword.SetActive(true);
                if (Input.GetKeyDown("y") && !swordBought)
                {
                    GameObject mySword = Instantiate(sword);
                    mySword.name = "Basic Sword";
                    mySword.GetComponent<RPGSystem.RPGSystem_Item>().SetName("Basic Sword");

                    GameObject hand = GameObject.Find("PlayerHand");
                    mySword.transform.position = new Vector3(hand.transform.position.x, hand.transform.position.y + 0.75f, hand.transform.position.z);
                    characterScript.AddItem(0, mySword.GetComponent<RPGSystem.RPGSystem_Item>());
                    swordBought = true;
                    characterScript.EquipItem(0);
                }

                else if (Input.GetKeyDown("y"))
                {
                    Debug.Log("SWORD HAS ALREADY BEEN BOUGHT!\nCHECK YOUR INVENTORY BY PRESSING 'I'");
                }

            }
            else
            {
                buySword.SetActive(false);
            }

            //handle arriving at XP point and adding XP
            if (player.GetComponent<RPGSystem_Sample_PlayerMovement>().HasHitXP())
            {
                addXP.SetActive(true);
                if (Input.GetKeyDown("x"))
                {
                    player.GetComponent<RPGSystem.RPGSystem_Character>().AddXP(100);

                    if (player.GetComponent<RPGSystem.RPGSystem_Character>().ShouldLevelUp() && !levelUpStarted)
                    {
                        GameObject levelUp = Instantiate(levelUpText);
                        StartCoroutine(LevelUpText(levelUp));
                    }
                }
            }
            else
            {
                addXP.SetActive(false);
            }

            //remove camera access to mouse when viewing a UI
            if (levelScreenActive || inventoryActive)
            {
                RPGSystem_Sample_CameraController cam = player.GetComponentInChildren<RPGSystem_Sample_CameraController>();
                cam.canUseMouse = false;
            }
            else
            {
                RPGSystem_Sample_CameraController cam = player.GetComponentInChildren<RPGSystem_Sample_CameraController>();
                cam.canUseMouse = true;
            }

            if (Input.GetMouseButton(0))
            {
                characterScript.GetEquippedItem().OnUse(characterScript);
            }
            healthText.text = characterScript.GetCurrentHP() + "/" + characterScript.GetMaxHP();

            if (Input.GetKeyDown("2"))
            {
                if (characterScript.GetCurrentHP() == characterScript.GetMaxHP() && characterScript.GetInventory().ContainsKey(1))
                {
                    StartCoroutine("DontNeedHeals");
                }
                else if (!characterScript.GetInventory().ContainsKey(1))
                {
                    dontNeedHealsText.GetComponentInChildren<Text>().text = "You don't have another potion!";
                    dontNeedHealsText.SetActive(true);
                }
                else
                {
                    characterScript.GetInventory()[1].OnUse(characterScript);
                    inventoryImages[1].sprite = null;
                    inventoryNames[1].text = "--EMPTY SLOT--";
                }
            }
            else
            {
                //dontNeedHealsText.SetActive(false);
            }

            if (characterScript.GetCurrentHP() <= (characterScript.GetMaxHP() / 2) && characterScript.GetInventory().ContainsKey(1))
            {
                shouldHealText.SetActive(true);
            }
            else
            {
                shouldHealText.SetActive(false);
            }
        }
        else
        {
            healthText.transform.parent.transform.parent.gameObject.SetActive(false);
            deathText.SetActive(true);
            if (Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene("RPGSystem_SampleScene");   
            }
        }   
        
        if (Input.GetKeyDown("escape"))
        {
            UnityEditor.EditorApplication.isPlaying = false;            
        }
    }

    //show no need to heal UI
    IEnumerator DontNeedHeals()
    {
        dontNeedHealsText.SetActive(true);
        yield return new WaitForSeconds(1);
        dontNeedHealsText.SetActive(false);
    }

    //show level up UI
    IEnumerator LevelUpText(GameObject levelUp)
    {
        levelUpStarted = true;
        yield return new WaitForSeconds(2);
        levelUpStarted = false;
        Destroy(levelUp);
    }

    //add a strength level through levels UI
    public void AddStrengthLevel()
    {
        int spareLevels = CheckAvailableLevels();
        if (spareLevels >= 1)
        {
            characterScript.StrengthLevelUp();
            CheckAvailableLevels();
        }
        else
        {
            GameObject gO = Instantiate(notEnoughLevels);
            StartCoroutine(NotEnoughLevels(gO));
        }
    }

    //add a constitution level through levels UI
    public void AddConstitutionLevel()
    {
        int spareLevels = CheckAvailableLevels();
        if (spareLevels >= 1)
        {
            characterScript.ConstitutionLevelUp();
            CheckAvailableLevels();
        }
        else
        {
            GameObject gO = Instantiate(notEnoughLevels);
            StartCoroutine(NotEnoughLevels(gO));
        }
    }

    //add a dexterity level through levels UI
    public void AddDexterityLevel()
    {
        int spareLevels = CheckAvailableLevels();
        if (spareLevels >= 1)
        {
            characterScript.DexterityLevelUp();
            CheckAvailableLevels();
        }
        else
        {
            GameObject gO = Instantiate(notEnoughLevels);
            StartCoroutine(NotEnoughLevels(gO));
        }
    }

    //add a willpower level through levels UI
    public void AddWillPowerLevel()
    {
        int spareLevels = CheckAvailableLevels();
        if (spareLevels >= 1)
        {
            characterScript.WillPowerLevelUp();
            CheckAvailableLevels();
        }
        else
        {
            GameObject gO = Instantiate(notEnoughLevels);
            StartCoroutine(NotEnoughLevels(gO));
        }
    }

    //show UI if attempting to add levels, when you have 0 available
    IEnumerator NotEnoughLevels(GameObject text)
    {
        yield return new WaitForSeconds(1);
        Destroy(text);
    }  
    
    //update levels UI based on player character
    private int CheckAvailableLevels()
    {
        //RPGSystem.RPGSystem_Character tempPlayer = player.GetComponent<RPGSystem.RPGSystem_Character>();
        int spareLevels = characterScript.GetCurrentLevel() - (characterScript.GetStrengthLevel() + characterScript.GetConstitutionLevel() + characterScript.GetDexterityLevel() + characterScript.GetWillPowerLevel());
        levelsToSpend.text = "Levels To Spend: " + spareLevels;

        strengthLevelText.text = "Level: " + characterScript.GetStrengthLevel();
        constitutionLevelText.text = "Level: " + characterScript.GetConstitutionLevel();
        dexterityLevelText.text = "Level: " + characterScript.GetDexterityLevel();
        willpowerLevelText.text = "Level: " + characterScript.GetWillPowerLevel();

        return spareLevels;
    }
}
