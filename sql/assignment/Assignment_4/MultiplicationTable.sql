-- creating a procedure to print multiplication table
create or alter proc sp_multiplicationTable @number int
as
begin
	declare @multiplicator int = 1
	declare @multiplicationTable table (multiple int)
	while @multiplicator <= 10
	begin
		insert into @multiplicationTable values(@number * @multiplicator)
		set @multiplicator = @multiplicator + 1
	end
	select * from @multiplicationTable
end

exec sp_multiplicationTable 5