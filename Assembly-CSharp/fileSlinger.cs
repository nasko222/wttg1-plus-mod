using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class fileSlinger
{
	public void saveFile(string fileName = "defaultSave.gd")
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(Application.persistentDataPath + "/" + fileName);
		binaryFormatter.Serialize(fileStream, this.saveData);
		fileStream.Close();
	}

	public bool loadFile(string fileName = "defaultSave.gd")
	{
		if (File.Exists(Application.persistentDataPath + "/" + fileName))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);
			this.saveData = (GameData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			return true;
		}
		return false;
	}

	public void deleteFile(string fileName = "defaultSave.gd")
	{
		File.Delete(Application.persistentDataPath + "/" + fileName);
		this.saveData.resetData();
	}

	public void saveOptionFile()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(Application.persistentDataPath + "/wttgOptionData.gd");
		binaryFormatter.Serialize(fileStream, this.optData);
		fileStream.Close();
	}

	public bool loadOptionFile()
	{
		if (File.Exists(Application.persistentDataPath + "/wttgOptionData.gd"))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(Application.persistentDataPath + "/wttgOptionData.gd", FileMode.Open);
			this.optData = (OptionData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			return true;
		}
		return false;
	}

	public void wildSaveFile<T>(string fileName, T wildDataType)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(Application.persistentDataPath + "/" + fileName);
		binaryFormatter.Serialize(fileStream, wildDataType);
		fileStream.Close();
	}

	public bool wildLoadFile<T>(string fileName, out T returnWildData)
	{
		if (File.Exists(Application.persistentDataPath + "/" + fileName))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);
			returnWildData = (T)((object)binaryFormatter.Deserialize(fileStream));
			fileStream.Close();
			return true;
		}
		returnWildData = default(T);
		return false;
	}

	public void wildDeleteFile(string fileName = "defaultSave.gd")
	{
		File.Delete(Application.persistentDataPath + "/" + fileName);
	}

	public GameData saveData = new GameData();

	public OptionData optData = new OptionData();

	public bool deleteTheFile;
}
