IF [%1] == [] (
	set GRUN_OPTION=-gui
) ELSE (
	set GRUN_OPTION=%1
)

echo GRUN_OPTION=%GRUN_OPTION%

grun MyLangV4 program %GRUN_OPTION% "D:\development\cvsout\MyLanguage\TestMyLanguageApp\Sample Programs\fibonacci.myl"