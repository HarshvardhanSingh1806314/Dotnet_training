-- trigger to change berth status to available when ticket it cancelled
--drop trigger tr_UpdateBerthStatusToAvailable
create trigger tr_UpdateBerthStatusToAvailable on Tickets after update
as
begin
	declare @executionStartTime datetime = getdate()
	declare @berthNo int
	declare @berthClass char(2)
	declare @trainNo int
	declare @triggerStatus int
	declare @sqlCommand nvarchar(500)
	declare @tableName varchar(10)

	-- getting the required columns value from inserted virtual table to update in berth table
	select @berthNo = berthNo, @berthClass = berthClass, @trainNo = trainNo from inserted

	-- updating the status of the berth to booked
	set @tableName = CAST(@trainNo as char(5)) + '_' + @berthClass
	set @sqlCommand = N'update ' + QUOTENAME(@tableName) + ' set berthStatus = @berthStatus where berthNo = @berthNo'
	exec sp_executesql @sqlCommand, N'@berthStatus nvarchar(450), @berthNo tinyint', @berthStatus = 'aAOhppPHkaYOi0vXTyysjsaET/TKcNFUr2Aee8EgPrY=', @berthNo = @berthNo

	-- checking if the berth status is updated, if successfully updated then triggerStatus = 1 otherwise 2
	declare @berthStatus nvarchar(450)
	set @sqlCommand = N'select @berthStatus = berthStatus from ' + QUOTENAME(@tableName) + ' where berthNo = @berthNo and trainNo = @trainNo'
	exec sp_executesql @sqlCommand, N'@berthStatus nvarchar(450) output, @berthNo tinyint, @trainNo int', @berthStatus output, @berthNo = @berthNo, @trainNo = @trainNo
	
	if @berthStatus = 'aAOhppPHkaYOi0vXTyysjsaET/TKcNFUr2Aee8EgPrY='
	begin
		declare @berthCountColumnName varchar(20)
		if @berthClass = 'A1'
		begin
			set @berthCountColumnName = 'availableSeatsInA1'
		end
		else if @berthClass = 'A2'
		begin
			set @berthCountColumnName = 'availableSeatsInA2'
		end
		else if @berthClass = 'A3'
		begin
			set @berthCountColumnName = 'availableSeatsInA3'
		end

		set @sqlCommand = N'update Trains set ' + QUOTENAME(@berthCountColumnName) + ' = ' + QUOTENAME(@berthCountColumnName) + ' + 1'
		exec sp_executesql @sqlCommand
		set @triggerStatus = 1
	end
	else
	begin
		set @triggerStatus = 2
	end

	-- logging trigger info into TriggerLogging Table
	insert into TriggerLogging values('tr_UpdateBerthStatusToAvailable', 1, 2, @executionStartTime, GETDATE(), @triggerStatus)
end

-- trigger to change the berth status to booked when ticket is booked
--drop trigger tr_UpdateBerthStatusToBooked
create trigger tr_UpdateBerthStatusToBooked on Tickets after insert
as
begin
	declare @executionStartTime datetime = getdate()
	declare @berthNo int
	declare @berthClass char(2)
	declare @trainNo int
	declare @triggerStatus int
	declare @sqlCommand nvarchar(500)
	declare @tableName varchar(10)

	-- getting the required columns value from inserted virtual table to update in berth table
	select @berthNo = berthNo, @berthClass = berthClass, @trainNo = trainNo from inserted

	-- updating the status of the berth to booked
	set @tableName = CAST(@trainNo as char(5)) + '_' + @berthClass
	set @sqlCommand = N'update ' + QUOTENAME(@tableName) + ' set berthStatus = @berthStatus where berthNo = @berthNo'
	exec sp_executesql @sqlCommand, N'@berthStatus nvarchar(450), @berthNo tinyint', @berthStatus = 'IxXkG3QsYx/6ihS2oP9OC+IwrptoRogVEv5RwAywApM=', @berthNo = @berthNo

	-- checking if the berth status is updated, if successfully updated then triggerStatus = 1 otherwise 2
	declare @berthStatus nvarchar(450)
	set @sqlCommand = N'select @berthStatus = berthStatus from ' + QUOTENAME(@tableName) + ' where berthNo = @berthNo and trainNo = @trainNo'
	exec sp_executesql @sqlCommand, N'@berthStatus nvarchar(450) output, @berthNo tinyint, @trainNo int', @berthStatus output, @berthNo = @berthNo, @trainNo = @trainNo
	
	if @berthStatus = 'IxXkG3QsYx/6ihS2oP9OC+IwrptoRogVEv5RwAywApM='
	begin
		declare @berthCountColumnName varchar(20)
		if @berthClass = 'A1'
		begin
			set @berthCountColumnName = 'availableSeatsInA1'
		end
		else if @berthClass = 'A2'
		begin
			set @berthCountColumnName = 'availableSeatsInA2'
		end
		else if @berthClass = 'A3'
		begin
			set @berthCountColumnName = 'availableSeatsInA3'
		end

		set @sqlCommand = N'update Trains set ' + QUOTENAME(@berthCountColumnName) + ' = ' + QUOTENAME(@berthCountColumnName) + ' - 1'
		exec sp_executesql @sqlCommand
		set @triggerStatus = 1
	end
	else
	begin
		set @triggerStatus = 2
	end

	-- logging trigger info into TriggerLogging Table
	insert into TriggerLogging values('tr_UpdateBerthStatusToBooked', 1, 1, @executionStartTime, GETDATE(), @triggerStatus)
end

-- trigger to create new berth tables for newly created train
--drop trigger tr_CreateBerthTablesForNewTrain
create trigger tr_CreateBerthTablesForNewTrain on Trains after insert
as
begin
	declare @executionStartTime datetime = getdate()
	declare @trainNo int
	declare @a1_inserted_count int
	declare @a2_inserted_count int
	declare @a3_inserted_count int
	declare @availableStatusId nvarchar(450)
	declare @tableName varchar(10)
	declare @sqlCommand nvarchar(1000)

	select @trainNo = trainNo, @a1_inserted_count = totalSeatsInA1, @a2_inserted_count = totalSeatsInA2, @a3_inserted_count = totalSeatsInA3 from inserted
	select @availableStatusId = id from BerthStatus where statusName = 'AVAILABLE'

	set @tableName = CAST(@trainNo as char(5)) + '_A'

	declare @berthCounter tinyint = 1
	while @berthCounter <= 3
	begin
		-- creating berth tables
		set @tableName = @tableName + CAST(@berthCounter as char(1))
		set @sqlCommand = N'
			create table ' + QUOTENAME(@tableName) + '(
			berthNo int primary key identity,
			trainNo int references Trains(trainNo) on update cascade on delete cascade,
			berthStatus nvarchar(450) references BerthStatus(id) on update cascade on delete cascade
		)';
		exec sp_executesql @sqlCommand

		-- inserting into berth tables
		declare @insertCounterLimit int
		if @berthCounter = 1
		begin
			set @insertCounterLimit = 10
		end
		else if @berthCounter = 2
		begin
			set @insertCounterLimit = 20
		end
		else if @berthCounter = 3
		begin
			set @insertCounterLimit = 40
		end

		set @sqlCommand = N'insert into ' + QUOTENAME(@tableName) + ' values(@trainNo, @availableStatusId)';
		while @insertCounterLimit > 0
		begin
			exec sp_executesql @sqlCommand, N'@trainNo int, @availableStatusId nvarchar(450)', @trainNo = @trainNo, @availableStatusId = @availableStatusId
			set @insertCounterLimit = @insertCounterLimit - 1
		end
		set @berthCounter = @berthCounter + 1
		set @tableName = CAST(@trainNo as char(5)) + '_A'
	end

	-- logging into TriggerLogging Table
	declare @a1_count int
	declare @a2_count int
	declare @a3_count int

	select @a1_count = totalSeatsInA1 from Trains where trainNo = @trainNo
	select @a2_count = totalSeatsInA2 from Trains where trainNo = @trainNo
	select @a3_count = totalSeatsInA3 from Trains where trainNo = @trainNo

	if @a1_count = @a1_inserted_count and @a2_count = @a2_inserted_count and @a3_count = @a3_inserted_count
	begin
		insert into TriggerLogging values ('tr_CreateBerthTablesForNewTrain', 1, 1, @executionStartTime, GETDATE(), 1)
	end
	else
	begin
		insert into TriggerLogging values ('tr_CreateBerthTablesForNewTrain', 1, 1, @executionStartTime, GETDATE(), 2)
	end
end
