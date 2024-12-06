use Assignments

create trigger tr_checkForPermission 
on Employees 
after insert, update, delete
as
begin
	declare @holidayName varchar(20)
	declare @currentDate date = getdate()

	select @holidayName = holidayName from Holidays where holidayDate = @currentDate
	if @holidayName is not null
	begin
		raiserror('Due to %s, you cannot manipulate data.', 16, 1, @holidayName)
		rollback
	end
end

-- creating holiday table
create table Holidays (
	holidayDate date,
	holidayName varchar(20)
)

-- inserting into holidays table
insert into Holidays values('2024-08-15', 'Independence Day'),
('2024-10-01', 'Gandhi Jayanti'),
('2024-10-23', 'Diwali'),
('2024-12-25', 'Christmas Day');