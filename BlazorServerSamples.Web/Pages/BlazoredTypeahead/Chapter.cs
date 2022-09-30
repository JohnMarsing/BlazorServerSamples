namespace BlazorServerSamples.Web.Pages.BlazoredTypeahead;
using BlazorServerSamples.Web.Enums;

public class Chapter
{
	public Chapter() 	{ }

	public Chapter(BibleBook bibleBook)
	{
		BibleBook = bibleBook;
	}

	public BibleBook BibleBook { get; set; }
	public string MhbUrl { get => "https://myhebrewbible.com/BookChapter/" + BibleBook.Title + "/" + BibleBook.Value + "/slug"; }
}

/*

	https://myhebrewbible.com/BookChapter/Genesis/1/god-creates-heaven-earth-plants-animals-a-man-named-adom

, string name, string mhbUrl

		Name = name;
		MhbUrl = mhbUrl;

public int Number { get; set; } //Number = number; 
public string Name { get; set; }

 */



