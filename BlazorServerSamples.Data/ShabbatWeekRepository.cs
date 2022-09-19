using BlazorServerSamples.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace BlazorServerSamples.Data;

public interface IShabbatWeekRepository
{
	string BaseSqlDump { get; }
	Task<Parasha> GetCurrentParashaAndChildren();
	Task<Tuple<BibleBook, List<ParashaList>>> GetParashotByBookId(int bookId);
}

public class ShabbatWeekRepository : BaseRepositoryAsync, IShabbatWeekRepository
{
	public ShabbatWeekRepository(IConfiguration config, ILogger<ShabbatWeekRepository> logger) : base(config, logger)
	{
	}

	public string BaseSqlDump
	{
		get { return base.SqlDump; }
	}


	public async Task<Parasha> GetCurrentParashaAndChildren()
	{
		base.Sql = $@"
SELECT 
Id, TriNum, ShabbatDate
, PrevId, NextId, BookId
, TorahLong, Haftorah, Brit
, ParashaName, AhavtaURL, NameUrl
, BaseParashaUrl
-- , Name, Meaning, ShabbatWeekId
FROM Bible.vwCurrentParasha;

SELECT Id, Abrv, Title AS EnglishTitle, HebrewTitle, HebrewName 
FROM Bible.Book
--WHERE Id = @Id
";
		return await WithConnectionAsync(async connection =>
		{
			var multi = await connection.QueryMultipleAsync(sql: base.Sql);
			/*
			*** NOTE THE ORDER OF THE  `multi.ReadAsync<foo>` MATTERS AND MUST MATCH UP WITH `base.Sql` ***
			*/
			var Parasha = await multi.ReadSingleOrDefaultAsync<Parasha>();    // #1
			if (Parasha != null)
			{
				Parasha.BibleBook = (await multi.ReadAsync<BibleBook>())
					.Where(w => w.Id == Parasha.BookId).SingleOrDefault();   // #2
			}
			return Parasha;
		});

	}

	public async Task<Tuple<BibleBook,
					List<ParashaList>>> GetParashotByBookId(int bookId)
	{
		base.Parms = new DynamicParameters(new { BookId = bookId });
		base.Sql = $@"
--DECLARE @BookId int=1

SELECT
Id, Abrv, Title AS EnglishTitle, HebrewTitle, HebrewName 
FROM Bible.Book
WHERE Id = @BookId

SELECT
Id
, ROW_NUMBER() OVER(PARTITION BY BookId ORDER BY Id ) AS RowCntByBookId
, BookId, Torah AS TorahLong, Name, TriNum, ParashaName
, NameUrl, AhavtaURL, Meaning, IsNewBook, Haftorah, Brit
, ShabbatDate
, BaseParashaUrl, CurrentParashaUrl
FROM Bible.vwParasha
WHERE BookId = @BookId
ORDER BY Id
";

		return await WithConnectionAsync(async connection =>
		{
			var multi = await connection.QueryMultipleAsync(sql: base.Sql, param: base.Parms);
			var book = await multi.ReadAsync<BibleBook>();
			var parashot = await multi.ReadAsync<ParashaList>();
			return new Tuple<BibleBook, List<ParashaList>>(book.SingleOrDefault(), parashot.ToList());
		});

	}

}
