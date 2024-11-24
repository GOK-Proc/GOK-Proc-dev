using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public abstract class EncryptedScriptableObject : ScriptableObject
{
	private static readonly string _aesKey = "p3fj7Lg5Tq2B8Cx9vKwA1Ns6EdY4RmQV";
	private static readonly string _aesIv = "Z9X2y3Wr5T6Lp7Nq";

	[Tooltip("UnityEditor時に保存されるディレクトリ")]
	[SerializeField][HideInInspector] private string _saveDir;

	[Tooltip("保存されるファイル名")]
	[SerializeField][HideInInspector] private string _fileName;
	
	private string FilePath
	{
		get
		{
			if (String.IsNullOrWhiteSpace(_fileName))
			{
				throw new ArgumentException("fileName cannot be null, empty, or whitespace.", nameof(_fileName));
			}
			else
			{
#if UNITY_EDITOR
				return Path.Combine(Application.dataPath, _saveDir, _fileName);
#else
				return Path.Combine(Application.persistentDataPath, _fileName);
#endif
			}
		}
	}

	private void OnEnable()
	{
		Load();
	}

	public void Save()
	{
		var json = JsonUtility.ToJson(this);
		var encrypted = AesEncrypt(json);
		using (var writer = new StreamWriter(FilePath))
		{
			writer.Write(encrypted);
		}
	}

	public void Load()
	{
		if (!File.Exists(FilePath))
		{
			Save();
			return;
		}

		using (var reader = new StreamReader(FilePath))
		{
			var encrypted = reader.ReadToEnd();
			var json = AesDecrypt(encrypted);
			JsonUtility.FromJsonOverwrite(json, this);
		}
	}

	private static string AesEncrypt(string plain_text)
	{
		string encrypted_str;

		using (Aes aes = Aes.Create())
		{
			using ICryptoTransform encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(_aesKey), Encoding.UTF8.GetBytes(_aesIv));
			using MemoryStream out_stream = new();
			using (CryptoStream cs = new(out_stream, encryptor, CryptoStreamMode.Write))
			{
				using StreamWriter sw = new(cs);
				sw.Write(plain_text);
			}

			byte[] result = out_stream.ToArray();
			encrypted_str = Convert.ToBase64String(result);
		}

		return encrypted_str;
	}

	private static string AesDecrypt(string base64_text)
	{
		string plain_text;

		byte[] cipher = Convert.FromBase64String(base64_text);

		using (Aes aes = Aes.Create())
		{
			using ICryptoTransform decryptor = aes.CreateDecryptor(Encoding.UTF8.GetBytes(_aesKey), Encoding.UTF8.GetBytes(_aesIv));
			using MemoryStream in_stream = new(cipher);
			using CryptoStream cs = new(in_stream, decryptor, CryptoStreamMode.Read);
			using StreamReader sr = new(cs);
			plain_text = sr.ReadToEnd();
		}
		return plain_text;
	}

}
