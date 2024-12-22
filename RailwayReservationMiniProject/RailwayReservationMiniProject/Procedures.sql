-- procedure to drop berth tables related to the deleted train
create procedure sp_DropBerthTablesOfDeletedTrain(@trainNo int, @dropped bit output)
as
begin
	declare @tableName varchar(10) = cast(@trainNo as char(5)) + '_A'
	declare @sqlCommand nvarchar(100)
	declare @berthCounter tinyint = 1
	while @berthCounter <= 3
	begin
		set @tableName = @tableName + CAST(@berthCounter as char(1))
		set @sqlCommand = N'drop table ' + QUOTENAME(@tableName)
		exec sp_executesql @sqlCommand
		set @berthCounter = @berthCounter + 1
		set @tableName = cast(@trainNo as char(5)) + '_A'
	end
	if @berthCounter = 4
	begin
		set @dropped = 1
	end
	else
	begin
		set @dropped = 0
	end
end

--drop procedure sp_DropBerthTablesOfDeletedTrain