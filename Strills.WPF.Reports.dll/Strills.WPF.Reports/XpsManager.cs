using System;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;

namespace Strills.WPF.Reports;

public class XpsManager
{
	private const string C_Documents = "Documents";

	private string _contentDir;

	public XpsManager()
	{
		_contentDir = GetContentFolder(baseFolder: false);
	}

	private string GetContainerPathFromDialog()
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.Filter = "XPS Document files (*.xps)|*.xps";
		saveFileDialog.FilterIndex = 1;
		saveFileDialog.InitialDirectory = _contentDir;
		if (saveFileDialog.ShowDialog() == true)
		{
			return saveFileDialog.FileName;
		}
		return null;
	}

	public string GetContentFolder(bool baseFolder)
	{
		string text = Directory.GetCurrentDirectory();
		int length = text.Length;
		if (text.ToLower().EndsWith("\\bin\\debug"))
		{
			text = text.Remove(length - 10, 10);
		}
		else if (text.ToLower().EndsWith("\\bin\\release"))
		{
			text = text.Remove(length - 12, 12);
		}
		if (baseFolder)
		{
			if (Directory.Exists(text + "\\Documents"))
			{
				text += "\\Documents";
			}
		}
		else if (text.ToLower().EndsWith("\\" + "Documents".ToLower()))
		{
			text = text.Remove(length - 14, 14);
		}
		return text;
	}

	public void SaveXps(XpsDocument document)
	{
		string containerPathFromDialog = GetContainerPathFromDialog();
		if (containerPathFromDialog != null)
		{
			SaveHelper saveHelper = new SaveHelper(_contentDir);
			saveHelper.Save(containerPathFromDialog, document);
		}
	}

	public void SaveXps(XpsDocument document, string path, string name)
	{
		try
		{
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				path += Path.DirectorySeparatorChar;
			}
			if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Não foi possível criar directoria: \n\n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		try
		{
			path = (string.IsNullOrEmpty(path) ? _contentDir : path);
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				path += Path.DirectorySeparatorChar;
			}
			path += name;
			SaveHelper saveHelper = new SaveHelper(_contentDir);
			saveHelper.Save(path, document);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Não foi possível gravar ficheiro: \n\n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}
}
