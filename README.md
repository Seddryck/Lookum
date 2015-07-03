# Lookum
Etl components for building Etl solutions with .Net

![project status](http://stillmaintained.com/Seddryck/Lookum.png)

## Continuous Integration ##
A continuous integration service is available on AppVeyor at https://ci.appveyor.com/project/CdricLCharlier/Lookum/ 
Note that all the tests are not executed on this environment due to limitations in the availability of some components (SSAS).

[![Build status](https://ci.appveyor.com/api/projects/status/ved2u21k9lwixcv3)](https://ci.appveyor.com/project/CdricLCharlier/Lookum)

## HelloWorld sample ##

````csharp
public class CountryLookup : DatabaseLookup<string, string>
{
    public CountryLookup()
		: base(true)
    {
		ConnectionString=ConnectionStringReader.GetSqlClient();
		CommandTimeOut = 300;
    }

    protected override IDbCommand BuildCommand()
    {
		var sql = "select [CategoryValue].[Key], [CategoryValueTranslation].[Value] from [CategoryType] inner join [CategoryValue] on [CategoryValue].[CategoryTypeId] = [CategoryType].[Id] inner join [CategoryValueTranslation] on [CategoryValueTranslation].[CategoryValueId] = [CategoryValue].[Id] inner join [IsoLanguage] on [IsoLanguage].[Id] = [CategoryValueTranslation].[IsoLanguageId] where [IsoLanguage].[IsDefault]=1 and [CategoryType].[Value]=@Category";
		var command = new SqlCommand();
		command.CommandText = sql;
		command.Parameters.Add(new SqlParameter("Category", "Country"));
		return command;
    }
}
````

