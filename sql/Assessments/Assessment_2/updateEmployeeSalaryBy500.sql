use Assignments

-- table state before updating
select * from Employees where deptNo = (select departmentNo from Departments where departmentName = 'SALES') and salary < 1500

create or alter proc sp_updateSalaryBy500
as
begin
	update Employees
	set salary = salary + 500
	where deptNo = (select departmentNo from Departments where departmentName = 'SALES') and salary < 1500
end

exec sp_updateSalaryBy500

-- table state after updating
select * from Employees where deptNo = (select departmentNo from Departments where departmentName = 'SALES')


