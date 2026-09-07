using System;
using System.IO;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Strills.WPF.Reports;

public class SaveHelper
{
	private int _batchProgress;

	private XPFContent _xpfContent;

	private VisualsToXpsDocument _activeVtoXPSD;

	private XpsDocument _xpsDocument;

	private XpsDocumentWriter _xpsdwActive;

	public SaveHelper(string contentPath)
	{
		_xpfContent = new XPFContent(contentPath);
	}

	private XpsDocumentWriter GetSaveXpsDocumentWriter(string containerName, XpsDocument document)
	{
		return XpsDocument.CreateXpsDocumentWriter(document);
	}

    private void SaveVisual(XpsDocumentWriter xpsdw, XpsDocument document)
    {
        try
        {
            Directory.CreateDirectory(@"C:\ReconciliaLogs");

            File.AppendAllText(
                @"C:\ReconciliaLogs\log.txt",
                DateTime.Now + " - Inicio SaveVisual\r\n");

            xpsdw.Write(document.GetFixedDocumentSequence());

            File.AppendAllText(
                @"C:\ReconciliaLogs\log.txt",
                DateTime.Now + " - Fim SaveVisual\r\n");
        }
        catch (Exception ex)
        {
            File.WriteAllText(
                @"C:\ReconciliaLogs\erro.txt",
                ex.ToString());

            throw;
        }
    }

    public void Save(string containerName, XpsDocument document)
	{
		File.Delete(containerName);
		_xpsDocument = new XpsDocument(containerName, FileAccess.ReadWrite);
		XpsDocumentWriter saveXpsDocumentWriter = GetSaveXpsDocumentWriter(containerName, _xpsDocument);
		SaveVisual(saveXpsDocumentWriter, document);
		_xpsDocument.Close();
	}
}
