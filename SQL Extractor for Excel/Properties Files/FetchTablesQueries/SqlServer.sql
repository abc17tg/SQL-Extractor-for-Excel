CREATE TABLE #AllTables (Database_Schema_Object NVARCHAR(MAX));
DECLARE @sql NVARCHAR(MAX) = N'';
DECLARE @dbName NVARCHAR(128);
DECLARE dbCursor CURSOR FOR 
    SELECT [name] FROM sys.databases 
    WHERE state = 0 AND [name] NOT IN ('master', 'tempdb', 'model', 'msdb');
OPEN dbCursor;
FETCH NEXT FROM dbCursor INTO @dbName;
WHILE @@FETCH_STATUS = 0 
BEGIN 
    SET @sql = N'USE [' + @dbName + ']; 
    INSERT INTO #AllTables 
    SELECT ''' + @dbName + '.'' + SCHEMA_NAME(schema_id) + ''.'' + [name] 
    FROM sys.tables t 
    WHERE EXISTS (SELECT 1 FROM ' + QUOTENAME(@dbName) + '.sys.partitions p 
                  WHERE p.object_id = t.object_id AND p.rows > 0)
    UNION ALL 
    SELECT ''' + @dbName + '.'' + SCHEMA_NAME(schema_id) + ''.'' + [name] 
    FROM sys.views v;';
    BEGIN TRY 
        EXEC sp_executesql @sql; 
    END TRY 
    BEGIN CATCH 
        PRINT 'Error accessing database ' + @dbName + ': ' + ERROR_MESSAGE(); 
    END CATCH;
    FETCH NEXT FROM dbCursor INTO @dbName; 
END 
CLOSE dbCursor;
DEALLOCATE dbCursor;
SELECT * FROM #AllTables ORDER BY Database_Schema_Object;
DROP TABLE #AllTables;