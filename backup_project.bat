
rem from VS 2017     CALL c:\install\backup\backup.bat

set fileName="TJ_MsSqlScripter_%date:~-4,4%%date:~-7,2%%date:~-10,2%_%time:~0,2%%time:~3,2%%time:~6,2%"

e:\install\_archiver\7za\7za a e:\backup\TJ_Ms_Sql_Scripter\%fileName%  c:\projects\Project_TJMsSqlScripter\*.*  -r -mx1  -x!*.dll -x!*.nupkg -x!*.exe -x!*.xml -x!*.pdb -x!*.lock -x!*.cache -x!*.licenses -x!*.resources -x!*.suo -x!desktop.ini -x!storage.id* -xr!bin -xr!obj -xr!Rubbish -xr!.vs
 
copy e:\backup\TJ_Ms_Sql_Scripter\%fileName%.* z:\backup\project\TJ_Ms_Sql_Scripter\*.*

@echo off



