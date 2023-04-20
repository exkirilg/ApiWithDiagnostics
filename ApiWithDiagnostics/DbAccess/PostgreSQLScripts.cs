namespace ApiWithDiagnostics.DbAccess;

public static class PostgreSQLScripts
{
    public static string CreateQuotesTable => @"
		create table if not exists quotes (
			id serial primary key,
			text text not null,
			author text not null,
			language text not null
		)
	";

	public static string PopulateQuotes => @"
		do $$
		declare
			has_rows boolean;
		begin
			select exists(select 1 from quotes) into has_rows;
			if not has_rows then
				insert into quotes (text, author, language)
					values
						('The price of greatness is responsibility.', 'Sir Winston Churchill', 'eng'),
						('There is no sin except stupidity.', 'Oscar Wilde', 'eng'),
						('Do not go where the path may lead, go instead where there is no path and leave a trail.', 'Ralph Waldo Emerson', 'eng'),
						('Your time is limited, don''t waste it living someone else''s life.', 'Steve Jobs', 'eng');
			end if;
		end
		$$
	";

	public static string GetRandomQuote => @"select * from quotes order by random() limit 1";

	public static string InsertQuote => @"insert into quotes (text, author, language) values (@text, @author, @language)";
}
