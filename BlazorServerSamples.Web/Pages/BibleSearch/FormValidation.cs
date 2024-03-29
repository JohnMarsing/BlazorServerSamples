﻿using System.ComponentModel.DataAnnotations;
using BlazorServerSamples.Web.Enums;

namespace BlazorServerSamples.Web.Pages.BibleSearch;

public class FormValidation
{
	[Required]
	[Display(Name = "Book")]
	public BibleBook SelectedBook { get; set; }

	public string MyHebrewBibleBookChapterUrl(int chapter)
	{
		return "https://myhebrewbible.com/BookChapter/" + SelectedBook.Title + "/" + chapter + "/slug";
	}

	public string MyHebrewBibleBookChapterTitle(int chapter)
	{
		return "MyHebrewBible.com/BookChapter/" + SelectedBook.Title + "/" + chapter;
	}

}

