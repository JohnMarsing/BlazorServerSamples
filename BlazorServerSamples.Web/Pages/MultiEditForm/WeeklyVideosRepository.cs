using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using BlazorServerSamples.Data;

namespace BlazorServerSamples.Web.Pages.MultiEditForm;

public interface IWeeklyVideosRepository
{
	string BaseSqlDump { get; }

	// Query 
	Task<List<ShabbatWeek>> GetShabbatWeekList(int top);
		Task<List<WeeklyVideoTable>> GetWeeklyVideoByShabbatWeekId(int shabbatWeekId);
	//Task<WeeklyVideoUpdateVM> GetWeeklyVideoById(int id);

	// Command
	Task<int> WeeklyVideoAdd(WeeklyVideoInsert dto);
	//Task<int> WeeklyVideoUpdate(WeeklyVideoUpdate dto);
	Task<int> WeeklyVideoDelete(int id);
}

public class WeeklyVideosRepository : BaseRepositoryAsync, IWeeklyVideosRepository
{
	public WeeklyVideosRepository(IConfiguration config, ILogger<WeeklyVideosRepository> logger) : base(config, logger)
	{

	}

	public string BaseSqlDump
	{
		get { return SqlDump; }
	}

#region Query
	public async Task<List<ShabbatWeek>> GetShabbatWeekList(int top = 3)
	{
		base.log.LogDebug(string.Format("Inside {0}, top={1}", nameof(WeeklyVideosRepository) + "!" + nameof(GetShabbatWeekList), top));
		Parms = new DynamicParameters(new { Top = top });
		Sql = $@"
-- DECLARE @Top int = 3
SELECT TOP (@Top) Id, ShabbatDate
FROM ShabbatWeek 
WHERE ShabbatDate <= dbo.udfGetNextShabbatDate()
ORDER BY ShabbatDate DESC
";
		return await WithConnectionAsync(async connection =>
		{
			var rows = await connection.QueryAsync<ShabbatWeek>(sql: Sql, param: Parms);
			base.log.LogDebug(string.Format("...Sql {0}", Sql));
			return rows.ToList();
		});
	}

	public async Task<List<WeeklyVideoTable>> GetWeeklyVideoByShabbatWeekId(int shabbatWeekId)
	{
		base.log.LogDebug(string.Format("Inside {0}, top={1}"
			, nameof(WeeklyVideosRepository) + "!" + nameof(GetWeeklyVideoByShabbatWeekId), shabbatWeekId));
		Parms = new DynamicParameters(new { ShabbatWeekId = shabbatWeekId });

		Sql = $@"
-- DECLARE @Top int = 3
SELECT 
 wv.Id
, Descr AS WeeklyVideoTypeDescr
, ShabbatDate
, wv.ShabbatWeekId,	wv.WeeklyVideoTypeId
,	wv.YouTubeId
, wv.Title
--, LAG(wv.ShabbatWeekId, 1, 0) OVER (ORDER BY ShabbatDate DESC, tvf.WeeklyVideoTypeId) AS PrevShabbatWeekId
FROM tvfShabbatWeekCrossWeeklyVideoTypeByTop(9) tvf
LEFT OUTER JOIN WeeklyVideo wv 
	ON tvf.ShabbatWeekId = wv.ShabbatWeekId AND
	   tvf.WeeklyVideoTypeId = wv.WeeklyVideoTypeId
WHERE wv.Id IS NOT NULL AND wv.ShabbatWeekId = @ShabbatWeekId
ORDER BY ShabbatDate DESC, tvf.WeeklyVideoTypeId
";
		return await WithConnectionAsync(async connection =>
		{
			var rows = await connection.QueryAsync<WeeklyVideoTable>(sql: Sql, param: Parms);
			return rows.ToList();
		});
	}


	#endregion

	#region Command


	public async Task<int> WeeklyVideoAdd(WeeklyVideoInsert dto)
	{
		base.log.LogDebug(string.Format("Inside {0}", nameof(WeeklyVideosRepository) + "!" + nameof(WeeklyVideoAdd)));
		Parms = new DynamicParameters(new
		{
			dto.ShabbatWeekId,
			dto.WeeklyVideoTypeId,
			dto.YouTubeId,
			dto.Title,
			dto.Book,
			dto.Chapter
		});
		//dto.GraphicFileRoot,dto.NotesFileRoot, ... GraphicFile, NotesFile ... , @GraphicFile, @NotesFile
		Sql = $@"
INSERT INTO WeeklyVideo
(ShabbatWeekId, WeeklyVideoTypeId, YouTubeId, Title, Book, Chapter)
VALUES (@ShabbatWeekId, @WeeklyVideoTypeId, @YouTubeId, @Title, @Book, @Chapter)
; SELECT CAST(SCOPE_IDENTITY() as int)
";
		int newId;

		return await WithConnectionAsync(async connection =>
		{
			newId = await connection.ExecuteScalarAsync<int>(Sql, Parms);
			//base.log.LogDebug(string.Format("...newId={0}, Sql {1}", newId, SqlDump));
			return newId;
		});


	}


	public async Task<int> WeeklyVideoDelete(int id)
	{
		base.log.LogDebug(string.Format("Inside {0}, id:{1}"
			, nameof(WeeklyVideosRepository) + "!" + nameof(WeeklyVideoDelete), id));

		base.Sql = "DELETE FROM WeeklyVideo WHERE Id = @id";
		base.Parms = new DynamicParameters(new { Id = id });
		return await WithConnectionAsync(async connection =>
		{
			var affectedrows = await connection.ExecuteAsync(sql: base.Sql, param: base.Parms);
			return affectedrows;
		});
	}
	#endregion

}

