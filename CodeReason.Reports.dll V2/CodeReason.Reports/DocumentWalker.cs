using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CodeReason.Reports;

public class DocumentWalker
{
	private object _tag = null;

	public object Tag
	{
		get
		{
			return _tag;
		}
		set
		{
			_tag = value;
		}
	}

	public event DocumentVisitedEventHandler VisualVisited;

	public List<Inline> Walk(FlowDocument fd)
	{
		return TraverseBlockCollection<Inline>(fd.Blocks);
	}

	public List<T> Walk<T>(FlowDocument fd) where T : class
	{
		return TraverseBlockCollection<T>(fd.Blocks);
	}

	public List<T> TraverseInlines<T>(InlineCollection inlines) where T : class
	{
		List<T> list = new List<T>();
		if (inlines != null && inlines.Count > 0)
		{
			Inline inline = inlines.FirstInline;
			while (inline != null)
			{
				if (inline is T)
				{
					list.Add(inline as T);
				}
				if (inline is Run visitedObject)
				{
					if (VisualVisited != null)
					{
						VisualVisited(this, visitedObject, start: true);
					}
					inline = inline.NextInline;
					continue;
				}
				if (inline is Span span)
				{
					if (VisualVisited != null)
					{
						VisualVisited(this, span, start: true);
					}
					list.AddRange(TraverseInlines<T>(span.Inlines));
					inline = inline.NextInline;
					continue;
				}
				if (inline is InlineUIContainer { Child: not null } inlineUIContainer)
				{
					if (VisualVisited != null)
					{
						VisualVisited(this, inlineUIContainer.Child, start: true);
					}
					if (inlineUIContainer.Child is T)
					{
						list.Add(inlineUIContainer.Child as T);
					}
					if (inlineUIContainer.Child is TextBlock textBlock)
					{
						list.AddRange(TraverseInlines<T>(textBlock.Inlines));
					}
					inline = inline.NextInline;
					continue;
				}
				if (inline is Figure figure)
				{
					if (VisualVisited != null)
					{
						VisualVisited(this, figure, start: true);
					}
					list.AddRange(TraverseBlockCollection<T>(figure.Blocks));
				}
				inline = inline.NextInline;
			}
		}
		return list;
	}

	public List<T> TraverseParagraph<T>(Paragraph p) where T : class
	{
		return TraverseInlines<T>(p.Inlines);
	}

	public List<T> TraverseBlockCollection<T>(IEnumerable<Block> blocks) where T : class
	{
		List<T> list = new List<T>();
		foreach (Block block in blocks)
		{
			if (block is T)
			{
				if (VisualVisited != null)
				{
					VisualVisited(this, block, start: true);
				}
				list.Add(block as T);
			}
			if (block is Paragraph paragraph)
			{
				if (VisualVisited != null)
				{
					VisualVisited(this, paragraph, start: true);
				}
				list.AddRange(TraverseParagraph<T>(paragraph));
			}
			else if (block is BlockUIContainer blockUIContainer)
			{
				if (VisualVisited != null)
				{
					VisualVisited(this, blockUIContainer.Child, start: true);
				}
			}
			else if (block is Section section)
			{
				if (VisualVisited != null)
				{
					VisualVisited(this, section, start: true);
				}
				list.AddRange(TraverseBlockCollection<T>(section.Blocks));
			}
			else
			{
				if (!(block is Table table))
				{
					continue;
				}
				if (VisualVisited != null)
				{
					VisualVisited(this, table, start: true);
				}
				foreach (TableRowGroup item in (IEnumerable<TableRowGroup>)table.RowGroups)
				{
					if (VisualVisited != null)
					{
						VisualVisited(this, item, start: true);
					}
					foreach (TableRow item2 in (IEnumerable<TableRow>)item.Rows)
					{
						if (VisualVisited != null)
						{
							VisualVisited(this, item2, start: true);
						}
						if (item2 is T)
						{
							list.Add(item2 as T);
						}
						foreach (TableCell item3 in (IEnumerable<TableCell>)item2.Cells)
						{
							if (VisualVisited != null)
							{
								VisualVisited(this, item3, start: true);
							}
							list.AddRange(TraverseBlockCollection<T>(item3.Blocks));
						}
					}
				}
			}
		}
		return list;
	}
}
