use Assignments

create function fn_calculateEmployeeBonus(@departmentNo int)
returns decimal(6,2)
as
begin
	declare @employeeCurrentSalary int
	declare @bonus decimal(6, 2)
	select @employeeCurrentSalary = salary from Employees where deptNo = @departmentNo

	-- calculating bonus
	if @departmentNo = 10
	begin
		set @bonus = @employeeCurrentSalary * 0.15
	end
	else if @departmentNo = 20
	begin
		set @bonus = @employeeCurrentSalary * 0.20
	end
	else
	begin
		set @bonus = @employeeCurrentSalary * 0.05
	end

	-- returning bonus
	return @bonus
end

select employeeNo, employeeName, salary, deptNo, dbo.fn_calculateEmployeeBonus(deptNo) as Bonus from Employees