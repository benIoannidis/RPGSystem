using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Mono;


namespace RPGSystem
{
    public class RPGMenuAdditions : Editor
    {
        [MenuItem("RPG System/Add Object/Add Character")]
        private static void AddCharacter()
        {
            GameObject NEW_RPGSystem_Character;
            NEW_RPGSystem_Character = new GameObject();

            NEW_RPGSystem_Character.AddComponent<RPGSystem_Character>();
            NEW_RPGSystem_Character.name = "NEW_RPGSystem_Character";
            Debug.Log("Added character");
        }

        [MenuItem("RPG System/Add Object/Add Item")]
        private static void AddItem()
        {
            Debug.Log("CREATING OBJECT WITH NEW CLASS");

            //Code for creating class from Unity Answers site
            //Author: APMIX
            //URL: https://answers.unity.com/questions/12599/editor-script-need-to-create-class-script-automati.html
            
            #region CreateClassScript
            string name = "NewItemClass";
            string filePath = "Assets/" + name + ".cs";
            int additionalFileNames = 1;
            while (File.Exists(filePath) == true)
            {
                name = "NewItemClass" + additionalFileNames.ToString();
                additionalFileNames++;

                filePath = "Assets/" + name + ".cs";
            }

            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.WriteLine("using System.Collections;");
                outfile.WriteLine("using System.Collections.Generic;");
                outfile.WriteLine("using UnityEngine;");

                outfile.WriteLine("public class " + name + " : RPGSystem.RPGSystem_Item");
                outfile.WriteLine("{");

                outfile.WriteLine("\t//Overriden from base item class");
                outfile.WriteLine("\t//Add code for when this item is used");

                outfile.WriteLine("\tpublic override bool OnUse(RPGSystem.RPGSystem_Character user)");
                outfile.WriteLine("\t{");
                outfile.WriteLine("\t\treturn false;");
                outfile.WriteLine("\t}");

                outfile.WriteLine("\t//Overriden from base item class");
                outfile.WriteLine("\t//Add code for when this item is used targeting another character");

                outfile.WriteLine("\tpublic override bool OnUse(RPGSystem.RPGSystem_Character user, RPGSystem.RPGSystem_Character target)");
                outfile.WriteLine("\t{");
                outfile.WriteLine("\t\treturn false;");
                outfile.WriteLine("\t}");

                outfile.WriteLine("}");
            }

            AssetDatabase.Refresh();
            #endregion


            Debug.Log("NEW CLASS HAS BEEN ADDED!\nCHECK THE 'ASSETS' FOLDER!");

            //while (EditorApplication.isCompiling)
            //{

            //}

            //GameObject newItem = new GameObject();
            //newItem.name = "NEW_RPG_ITEM";

            //newItem.AddComponent(Type.GetType(name));
        }
    }
}
