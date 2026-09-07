using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace Strills.WPF.Reports;

internal class XPFContent
{
	private string _contentDir;

	private static string String1 = "No scene from prehistory is quite so vivid as that of the mortal struggles";

	private static string String2 = "of great beasts in the tar pits. In the mind's eye one sees dinosaurs,";

	private static string String3 = " mammoths, and sabertoothed tigers struggling against the grip of the tar.";

	private static string String4 = "The fiercer the struggle, the more entangling the tar, and no";

	private static string String5 = "beast is so strong or so skillful but that he ultimately sinks.";

	private static string _paragraphText = "The story which follows was first written out in Paris during the Peace Conference, from notes jotted daily on the march, strengthened by some reports sent to my chiefs in Cairo. Afterwards, in the autumn of 1919, this first draft and some of the notes were lost. It seemed to me historically needful to reproduce the tale, as perhaps no one but myself in Feisal's army had thought of writing down at the time what we felt, what we hoped, what we tried. So it was built again with heavy repugnance in London in the winter of 1919-20 from memory and my surviving notes. The record of events was not dulled in me and perhaps few actual mistakes crept in - except in details of dates or numbers - but the outlines and significance of things had lost edge in the haze of new interestz.";

	public XPFContent(string contentPath)
	{
		_contentDir = contentPath;
	}

	public Canvas CreateFirstVisual(bool shouldMeasure)
	{
		Canvas canvas = new Canvas();
		canvas.Width = 816.0;
		canvas.Height = 1056.0;
		TextBlock textBlock = new TextBlock();
		textBlock.Foreground = Brushes.DarkBlue;
		textBlock.FontFamily = new FontFamily("Arial");
		textBlock.FontSize = 36.0;
		textBlock.Text = "TopLeft";
		Canvas.SetTop(textBlock, 0.0);
		Canvas.SetLeft(textBlock, 0.0);
		canvas.Children.Add(textBlock);
		textBlock = new TextBlock();
		textBlock.Foreground = Brushes.Bisque;
		textBlock.Text = "BottomRight";
		textBlock.FontFamily = new FontFamily("Arial");
		textBlock.FontSize = 56.0;
		Canvas.SetTop(textBlock, 750.0);
		Canvas.SetLeft(textBlock, 520.0);
		canvas.Children.Add(textBlock);
		textBlock = new TextBlock();
		textBlock.Foreground = Brushes.BurlyWood;
		textBlock.Text = "TopRight";
		textBlock.FontFamily = new FontFamily("CASTELLAR");
		textBlock.FontSize = 32.0;
		Canvas.SetTop(textBlock, 0.0);
		Canvas.SetLeft(textBlock, 520.0);
		canvas.Children.Add(textBlock);
		textBlock = new TextBlock();
		textBlock.Foreground = Brushes.Cyan;
		textBlock.Text = "BottomLeft";
		textBlock.FontFamily = new FontFamily("Arial");
		textBlock.FontSize = 18.0;
		Canvas.SetTop(textBlock, 750.0);
		Canvas.SetLeft(textBlock, 0.0);
		canvas.Children.Add(textBlock);
		Rectangle rectangle = new Rectangle();
		rectangle.Fill = new SolidColorBrush(Colors.Red);
		Thickness margin = new Thickness
		{
			Left = 150.0,
			Top = 150.0
		};
		rectangle.Margin = margin;
		rectangle.Width = 300.0;
		rectangle.Height = 300.0;
		canvas.Children.Add(rectangle);
		Button button = new Button();
		button.Background = Brushes.LightYellow;
		button.BorderBrush = new SolidColorBrush(Colors.Black);
		button.BorderThickness = new Thickness(4.0);
		button.Content = "I am button 1...";
		button.FontSize = 16.0;
		margin.Left = 80.0;
		margin.Top = 250.0;
		button.Margin = margin;
		canvas.Children.Add(button);
		Ellipse ellipse = new Ellipse();
		SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.DarkCyan);
		solidColorBrush.Opacity = 0.7;
		ellipse.Fill = solidColorBrush;
		SetEllipse(ellipse, 500.0, 350.0, 120.0, 250.0);
		canvas.Children.Add(ellipse);
		Polygon polygon = new Polygon();
		polygon.Fill = Brushes.Bisque;
		polygon.Opacity = 0.2;
		PointCollection pointCollection = new PointCollection();
		pointCollection.Add(new Point(50.0, 0.0));
		pointCollection.Add(new Point(10.0, 30.0));
		pointCollection.Add(new Point(30.0, 170.0));
		pointCollection.Add(new Point(90.0, 40.0));
		pointCollection.Add(new Point(230.0, 180.0));
		pointCollection.Add(new Point(200.0, 60.0));
		pointCollection.Add(new Point(240.0, 10.0));
		pointCollection.Add(new Point(70.0, 130.0));
		polygon.Points = pointCollection;
		polygon.Stroke = Brushes.Navy;
		Canvas.SetTop(polygon, 300.0);
		Canvas.SetLeft(polygon, 160.0);
		canvas.Children.Add(polygon);
		if (shouldMeasure)
		{
			Size size = new Size(816.0, 1056.0);
			canvas.Measure(size);
			canvas.Arrange(new Rect(default(Point), size));
			canvas.UpdateLayout();
		}
		return canvas;
	}

	public Canvas CreateSecondVisual(bool shouldMeasure)
	{
		Canvas canvas = new Canvas();
		Ellipse ellipse = new Ellipse();
		ellipse.Fill = Brushes.LightSeaGreen;
		SetEllipse(ellipse, 130.0, 200.0, 100.0, 70.0);
		ellipse.Stroke = Brushes.Black;
		canvas.Children.Add(ellipse);
		Rectangle rectangle = new Rectangle();
		rectangle.Fill = Brushes.PowderBlue;
		rectangle.Opacity = 0.8;
		rectangle.RadiusX = 5.0;
		rectangle.RadiusY = 5.0;
		rectangle.Stroke = Brushes.Orange;
		rectangle.Height = 200.0;
		rectangle.Width = 350.0;
		Canvas.SetTop(rectangle, 50.0);
		Canvas.SetLeft(rectangle, 100.0);
		canvas.Children.Add(rectangle);
		Polygon polygon = new Polygon();
		polygon.Fill = Brushes.MediumVioletRed;
		polygon.Opacity = 0.7;
		PointCollection pointCollection = new PointCollection();
		pointCollection.Add(new Point(50.0, 0.0));
		pointCollection.Add(new Point(10.0, 30.0));
		pointCollection.Add(new Point(30.0, 170.0));
		pointCollection.Add(new Point(90.0, 40.0));
		pointCollection.Add(new Point(230.0, 180.0));
		pointCollection.Add(new Point(200.0, 60.0));
		pointCollection.Add(new Point(240.0, 10.0));
		pointCollection.Add(new Point(70.0, 130.0));
		polygon.Points = pointCollection;
		polygon.Stroke = Brushes.Navy;
		Canvas.SetTop(polygon, 150.0);
		Canvas.SetLeft(polygon, 250.0);
		canvas.Children.Add(polygon);
		TextBlock textBlock = new TextBlock();
		textBlock.Foreground = Brushes.Green;
		textBlock.FontFamily = new FontFamily("Courier New");
		textBlock.FontSize = 18.0;
		textBlock.Opacity = 0.5;
		Canvas.SetLeft(textBlock, 355.20000000000005);
		Canvas.SetTop(textBlock, 988.8000000000001);
		canvas.Children.Add(textBlock);
		TextBlock textBlock2 = new TextBlock();
		textBlock2.Text = "This is a piece of text content.";
		textBlock2.FontSize = 16.0;
		textBlock2.FontFamily = new FontFamily("Comic Sans MS");
		textBlock2.Foreground = Brushes.Orange;
		Canvas.SetTop(textBlock2, 576.0);
		Canvas.SetLeft(textBlock2, 15.0);
		canvas.Children.Add(textBlock2);
		textBlock2 = new TextBlock();
		textBlock2.Text = "This is the second piece of text content.";
		textBlock2.FontSize = 16.0;
		textBlock2.FontFamily = new FontFamily("Comic Sans MS");
		textBlock2.Foreground = Brushes.Blue;
		Canvas.SetTop(textBlock2, 691.2);
		Canvas.SetLeft(textBlock2, 15.0);
		canvas.Children.Add(textBlock2);
		textBlock2 = new TextBlock();
		textBlock2.Text = "This is the last text section.";
		textBlock2.FontSize = 16.0;
		textBlock2.FontFamily = new FontFamily("Comic Sans MS");
		textBlock2.Foreground = Brushes.Red;
		Canvas.SetTop(textBlock2, 806.4000000000001);
		Canvas.SetLeft(textBlock2, 15.0);
		canvas.Children.Add(textBlock2);
		if (shouldMeasure)
		{
			Size size = new Size(816.0, 1056.0);
			canvas.Measure(size);
			canvas.Arrange(new Rect(default(Point), size));
			canvas.UpdateLayout();
		}
		return canvas;
	}

	public Canvas CreateThirdVisual(bool shouldMeasure)
	{
		Canvas canvas = new Canvas();
		RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
		radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.0));
		radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
		radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
		radialGradientBrush.SpreadMethod = GradientSpreadMethod.Repeat;
		radialGradientBrush.Center = new Point(0.5, 0.5);
		radialGradientBrush.RadiusX = 0.2;
		radialGradientBrush.RadiusY = 0.2;
		radialGradientBrush.GradientOrigin = new Point(0.5, 0.5);
		Rectangle rectangle = new Rectangle();
		rectangle.Fill = radialGradientBrush;
		rectangle.Margin = new Thickness
		{
			Left = 100.0,
			Top = 100.0
		};
		rectangle.Width = 400.0;
		rectangle.Height = 400.0;
		canvas.Children.Add(rectangle);
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
		linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.0));
		linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 1.0));
		linearGradientBrush.SpreadMethod = GradientSpreadMethod.Reflect;
		linearGradientBrush.StartPoint = new Point(0.0, 0.0);
		linearGradientBrush.EndPoint = new Point(49.0 / 800.0, 0.0);
		linearGradientBrush.Opacity = 0.5;
		rectangle = new Rectangle();
		rectangle.Fill = linearGradientBrush;
		rectangle.Margin = new Thickness
		{
			Left = 200.0,
			Top = 200.0
		};
		rectangle.Width = 400.0;
		rectangle.Height = 400.0;
		canvas.Children.Add(rectangle);
		if (shouldMeasure)
		{
			Size size = new Size(816.0, 1056.0);
			canvas.Measure(size);
			canvas.Arrange(new Rect(default(Point), size));
			canvas.UpdateLayout();
		}
		return canvas;
	}

	public FixedDocument CreateFixedDocumentWithPages()
	{
		FixedDocument fixedDocument = CreateFixedDocument();
		PageContent newPageContent = CreateFirstPageContent();
		fixedDocument.Pages.Add(newPageContent);
		PageContent newPageContent2 = CreateSecondPageContent();
		fixedDocument.Pages.Add(newPageContent2);
		PageContent newPageContent3 = CreateThirdPageContent();
		fixedDocument.Pages.Add(newPageContent3);
		PageContent newPageContent4 = CreateFourthPageContent();
		fixedDocument.Pages.Add(newPageContent4);
		PageContent newPageContent5 = CreateFifthPageContent();
		fixedDocument.Pages.Add(newPageContent5);
		return fixedDocument;
	}

	public IDocumentPaginatorSource CreateFlowDocument()
	{
		FlowDocument flowDocument = new FlowDocument();
		for (int i = 0; i < 2; i++)
		{
			Paragraph item = new Paragraph(new Run(_paragraphText));
			flowDocument.Blocks.Add(item);
		}
		return flowDocument;
	}

	public FixedDocumentSequence LoadFixedDocumentSequenceFromDocument()
	{
		string path = _contentDir + "\\ViewFixedDocumentSequence.xps";
		XpsDocument xpsDocument = new XpsDocument(path, FileAccess.Read, CompressionOption.NotCompressed);
		return xpsDocument.GetFixedDocumentSequence();
	}

	private FixedDocument CreateFixedDocument()
	{
		FixedDocument fixedDocument = new FixedDocument();
		fixedDocument.DocumentPaginator.PageSize = new Size(816.0, 1056.0);
		return fixedDocument;
	}

	private PageContent CreateFirstPageContent()
	{
		PageContent pageContent = new PageContent();
		FixedPage value = CreateFirstFixedPage();
		((IAddChild)pageContent).AddChild((object)value);
		return pageContent;
	}

	private FixedPage CreateFirstFixedPage()
	{
		FixedPage fixedPage = new FixedPage();
		fixedPage.Background = Brushes.LightYellow;
		UIElement element = CreateFirstVisual(shouldMeasure: false);
		FixedPage.SetLeft(element, 0.0);
		FixedPage.SetTop(element, 0.0);
		double width = 816.0;
		double height = 1056.0;
		fixedPage.Width = width;
		fixedPage.Height = height;
		fixedPage.Children.Add(element);
		Size size = new Size(816.0, 1056.0);
		fixedPage.Measure(size);
		fixedPage.Arrange(new Rect(default(Point), size));
		fixedPage.UpdateLayout();
		return fixedPage;
	}

	private PageContent CreateSecondPageContent()
	{
		PageContent pageContent = new PageContent();
		FixedPage fixedPage = new FixedPage();
		fixedPage.Background = Brushes.LightGray;
		UIElement element = CreateSecondVisual(shouldMeasure: false);
		FixedPage.SetLeft(element, 0.0);
		FixedPage.SetTop(element, 0.0);
		double width = 816.0;
		double height = 1056.0;
		fixedPage.Width = width;
		fixedPage.Height = height;
		fixedPage.Children.Add(element);
		Size size = new Size(816.0, 1056.0);
		fixedPage.Measure(size);
		fixedPage.Arrange(new Rect(default(Point), size));
		fixedPage.UpdateLayout();
		((IAddChild)pageContent).AddChild((object)fixedPage);
		return pageContent;
	}

	public PageContent CreateThirdPageContent()
	{
		PageContent pageContent = new PageContent();
		FixedPage fixedPage = new FixedPage();
		fixedPage.Background = Brushes.White;
		Canvas canvas = new Canvas();
		canvas.Width = 816.0;
		canvas.Height = 1056.0;
		TextBlock textBlock = new TextBlock();
		textBlock.Foreground = Brushes.Black;
		textBlock.FontFamily = new FontFamily("Arial");
		textBlock.FontSize = 14.0;
		textBlock.Text = String1;
		Canvas.SetTop(textBlock, 0.0);
		Canvas.SetLeft(textBlock, 0.0);
		canvas.Children.Add(textBlock);
		textBlock = new TextBlock();
		textBlock.Foreground = Brushes.Black;
		textBlock.FontFamily = new FontFamily("Arial");
		textBlock.FontSize = 14.0;
		textBlock.Text = String2;
		Canvas.SetTop(textBlock, 20.0);
		Canvas.SetLeft(textBlock, 0.0);
		canvas.Children.Add(textBlock);
		textBlock = new TextBlock();
		textBlock.Foreground = Brushes.Black;
		textBlock.FontFamily = new FontFamily("Arial");
		textBlock.FontSize = 14.0;
		textBlock.Text = String3;
		Canvas.SetTop(textBlock, 40.0);
		Canvas.SetLeft(textBlock, 0.0);
		canvas.Children.Add(textBlock);
		textBlock = new TextBlock();
		textBlock.Foreground = Brushes.Black;
		textBlock.FontFamily = new FontFamily("Arial");
		textBlock.FontSize = 14.0;
		textBlock.Text = String4;
		Canvas.SetTop(textBlock, 60.0);
		Canvas.SetLeft(textBlock, 0.0);
		canvas.Children.Add(textBlock);
		textBlock = new TextBlock();
		textBlock.Foreground = Brushes.Black;
		textBlock.FontFamily = new FontFamily("Arial");
		textBlock.FontSize = 14.0;
		textBlock.Text = String5;
		Canvas.SetTop(textBlock, 80.0);
		Canvas.SetLeft(textBlock, 0.0);
		canvas.Children.Add(textBlock);
		fixedPage.Children.Add(canvas);
		double width = 816.0;
		double height = 1056.0;
		fixedPage.Width = width;
		fixedPage.Height = height;
		Size size = new Size(816.0, 1056.0);
		fixedPage.Measure(size);
		fixedPage.Arrange(new Rect(default(Point), size));
		fixedPage.UpdateLayout();
		((IAddChild)pageContent).AddChild((object)fixedPage);
		return pageContent;
	}

	private PageContent CreateFourthPageContent()
	{
		PageContent pageContent = new PageContent();
		FixedPage fixedPage = new FixedPage();
		fixedPage.Background = Brushes.BlanchedAlmond;
		BitmapImage source = new BitmapImage(new Uri(_contentDir + "\\tiger.jpg", UriKind.RelativeOrAbsolute));
		Image image = new Image();
		image.Source = source;
		Canvas.SetTop(image, 0.0);
		Canvas.SetLeft(image, 0.0);
		fixedPage.Children.Add(image);
		Image image2 = new Image();
		image2.Source = source;
		image2.Opacity = 0.3;
		FixedPage.SetTop(image2, 150.0);
		FixedPage.SetLeft(image2, 150.0);
		fixedPage.Children.Add(image2);
		((IAddChild)pageContent).AddChild((object)fixedPage);
		double width = 816.0;
		double height = 1056.0;
		fixedPage.Width = width;
		fixedPage.Height = height;
		Size size = new Size(816.0, 1056.0);
		fixedPage.Measure(size);
		fixedPage.Arrange(new Rect(default(Point), size));
		fixedPage.UpdateLayout();
		return pageContent;
	}

	private PageContent CreateFifthPageContent()
	{
		PageContent pageContent = new PageContent();
		FixedPage fixedPage = new FixedPage();
		UIElement element = CreateThirdVisual(shouldMeasure: false);
		FixedPage.SetLeft(element, 0.0);
		FixedPage.SetTop(element, 0.0);
		double width = 816.0;
		double height = 1056.0;
		fixedPage.Width = width;
		fixedPage.Height = height;
		fixedPage.Children.Add(element);
		Size size = new Size(816.0, 1056.0);
		fixedPage.Measure(size);
		fixedPage.Arrange(new Rect(default(Point), size));
		fixedPage.UpdateLayout();
		((IAddChild)pageContent).AddChild((object)fixedPage);
		return pageContent;
	}

	private void SetEllipse(Ellipse shape, double cx, double cy, double rx, double ry)
	{
		shape.Margin = new Thickness
		{
			Left = cx - rx,
			Top = cy - ry
		};
		shape.Width = rx * 2.0;
		shape.Height = ry * 2.0;
	}
}
