using UnityEngine;
using System.Collections;
using System.IO;

/*
 * Save file format:
 * gold;10
 * monster;monster;exp
 * monster;monster;exp
 * monster;monster;exp
 */

public static class PlayerSave
{
	private static string savePath = Application.persistentDataPath + "/savedGames.sav";
	private static bool loaded = false;

	static PlayerSave()
	{

	}

	public static void Load()
	{
		if (!File.Exists(savePath))
		{
			return;
		}

		string line;
		StreamReader file = new StreamReader(savePath);

		while((line = file.ReadLine()) != null)
		{
			string[] exp = line.Split(';');

			if (exp[0] == "gold")
			{
				PlayerCoin.Set(int.Parse(exp[1]));
			}
			else if (exp[0] == "monster")
			{
				PlayerMonster.Add(exp[1], int.Parse(exp[2]));
			}
			else if (exp[0] == "field")
			{
				if (exp[2] == "0")
					PlayerField.Add(int.Parse(exp[1]), FieldStatus.FIELD_FAILED);
				else if (exp[2] == "1")
					PlayerField.Add(int.Parse(exp[1]), FieldStatus.FIELD_1STAR);
				else if (exp[2] == "2")
					PlayerField.Add(int.Parse(exp[1]), FieldStatus.FIELD_2STAR);
				else if (exp[2] == "3")
					PlayerField.Add(int.Parse(exp[1]), FieldStatus.FIELD_3STAR);
				else
					PlayerField.Add(int.Parse(exp[1]), FieldStatus.FIELD_EMPTY);
			}

			PlayerMonster.Load();
		}

		file.Close();
	}

	public static void Save()
	{
		StreamWriter file = new StreamWriter(savePath);

		file.WriteLine("gold;" + PlayerCoin.Get());

		for (int i=0;i<PlayerMonster.Count;i++)
		{
			file.WriteLine("monster;" + PlayerMonster.Get(i).monsterName + ";"+PlayerMonster.Get(i).exp);
		}

		for (int i=0;PlayerField.Has(i);i++)
		{
			switch (PlayerField.Get(i))
			{
			case FieldStatus.FIELD_EMPTY:
				file.WriteLine("field;" + i.ToString() + ";-1");
				break;
			case FieldStatus.FIELD_FAILED:
				file.WriteLine("field;" + i.ToString() + ";0");
				break;
			case FieldStatus.FIELD_1STAR:
				file.WriteLine("field;" + i.ToString() + ";1");
				break;
			case FieldStatus.FIELD_2STAR:
				file.WriteLine("field;" + i.ToString() + ";2");
				break;
			case FieldStatus.FIELD_3STAR:
				file.WriteLine("field;" + i.ToString() + ";3");
				break;
			}
		}

		file.Close();
	}

	public static void Auto()
	{
		if (!loaded)
		{
			Load();
			loaded = true;
		}
			
		Save();
	}
}
