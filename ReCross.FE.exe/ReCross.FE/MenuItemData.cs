namespace ReCross.FE;

public class MenuItemData
{
	public string Title { get; set; }

	public string ImageURL { get; set; }

	public MenuItemData(string title, string imageURL)
	{
		Title = title;
		ImageURL = imageURL;
	}
}
