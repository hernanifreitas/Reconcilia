using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;

namespace CodeReason.Reports;

public class ReportPaginatorDynamicCache
{
	private FlowDocument _flowDocument = null;

	private Dictionary<Type, ArrayList> _documentByType = new Dictionary<Type, ArrayList>();

	private Dictionary<Type, ArrayList> _documentByInterface = new Dictionary<Type, ArrayList>();

	public FlowDocument FlowDocument => _flowDocument;

	public ReportPaginatorDynamicCache(FlowDocument flowDocument)
	{
		_flowDocument = flowDocument;
		BuildCache();
	}

	private void BuildCache()
	{
		DocumentWalker documentWalker = new DocumentWalker();
		documentWalker.VisualVisited += walker_VisualVisited;
		documentWalker.Walk(_flowDocument);
	}

	private void walker_VisualVisited(object sender, object visitedObject, bool start)
	{
		if (visitedObject == null)
		{
			return;
		}
		Type type = visitedObject.GetType();
		if (!_documentByType.ContainsKey(type))
		{
			_documentByType[type] = new ArrayList();
		}
		_documentByType[type].Add(visitedObject);
		Type[] interfaces = type.GetInterfaces();
		foreach (Type key in interfaces)
		{
			if (!_documentByInterface.ContainsKey(key))
			{
				_documentByInterface[key] = new ArrayList();
			}
			_documentByInterface[key].Add(visitedObject);
		}
	}

	public ArrayList GetFlowDocumentVisualListByType(Type type)
	{
		if (type == null)
		{
			return new ArrayList();
		}
		if (!_documentByType.ContainsKey(type))
		{
			return new ArrayList();
		}
		return _documentByType[type];
	}

	public ArrayList GetFlowDocumentVisualListByInterface(Type type)
	{
		if (type == null)
		{
			return new ArrayList();
		}
		if (!type.IsInterface)
		{
			throw new ArgumentException("Specified type must be an interface");
		}
		if (!_documentByInterface.ContainsKey(type))
		{
			return new ArrayList();
		}
		return _documentByInterface[type];
	}
}
